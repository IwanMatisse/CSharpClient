
using NetMQ;
using NetMQ.Sockets;
using SimpleClient.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SimpleClient
{
    /// <summary>
    /// see MessageStructure.h
    /// </summary>
    public enum DataType {
        kSecurity = 1,
        kPosition = 2,
        kTrade = 3,
        kOrder = 4,
        kMoney = 5,
        kStrategy = 6,
        kServer = 7,
        kPositionClear = 8,
        kRequest = 9 };

    public enum RequestType {
        kMessage = 101,
        kConnect = 102,
        kDisconnect = 103,
        kRunStrategy = 104,
        kStopStrategy = 105 };

    public class ClientRequest
    {
        public int type;
        public int req_type;
        public int value;
        public int id;
        public ClientRequest(RequestType reqtype, int val_, int id_)
        {
            type = (int)DataType.kRequest;
            req_type = (int)reqtype;
            value = val_;
            id = id_;
        }
    };

    public class DataController
    {
        string ConnectionStringStream = "";
        string ConnectionStringRequest = "";
        string Account = "";
        int MessageRecieved = 0;
        int MessageOnServer = 0;
       
        MoneyInfo _MoneyInfo = new MoneyInfo();     
        ConcurrentDictionary<int, Security> _Securities = new ConcurrentDictionary<int, Security>();
        ConcurrentDictionary<int, Strategy> _Strategies = new ConcurrentDictionary<int, Strategy>();
        List<Position> _Positions = new List<Position>();
        Dictionary<int, Order> _Orders = new Dictionary<int, Order>();
        ConcurrentQueue<ClientRequest> _Commands = new ConcurrentQueue<ClientRequest>();

        bool bExit = false;
        
        public DataController(string AccountName, string AddressStreamData, string AddressRequestData)
        {
            _MoneyInfo.Name = Account;
            Account = AccountName;
            ConnectionStringStream = AddressStreamData;
            ConnectionStringRequest = AddressRequestData;
        }

        public void AddCommand(ClientRequest req)
        {
            _Commands.Enqueue(req);
        }
                
        /// <summary>
        /// Run processes of receiving data in new threads
        /// </summary>
        public void SubscriberStart()
        {
            bExit = false;           
            Task.Factory.StartNew(StreamProcess, TaskCreationOptions.AttachedToParent);
            Task.Factory.StartNew(RequestProcess, TaskCreationOptions.AttachedToParent);        
        }

        public void SubscriberStop()
        {
            bExit = true;
        }

        private void SendRequest(ClientRequest req, RequestSocket client)
        {
            //convert to binary array
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(req.type);
            writer.Write(req.req_type);
            writer.Write(req.value);
            writer.Write(req.id);

            client.Send(output.ToArray(), (int)output.Length);
        }

        /// <summary>
        /// getting messages by request them and sending user's commands 
        /// </summary>
        public void RequestProcess()
        {
            using (var context = NetMQContext.Create())
            {
                using (var client = context.CreateRequestSocket())
                {
                    client.Connect(ConnectionStringRequest);

                    while (!bExit)
                    {
                        try
                        {
                            if (MessageRecieved < MessageOnServer)
                            {           
                                //fill the request message                     
                                ClientRequest req = new ClientRequest(RequestType.kMessage, MessageRecieved + 1, 0);

                                SendRequest(req, client);
                                //get answer
                                var message = client.ReceiveMessage();
                                if (ProcessMessage(message) != DataType.kServer) 
                                    MessageRecieved++;

                            }
                            if (_Commands.Count > 0)
                            {
                                if (_Commands.TryDequeue(out ClientRequest req))                               
                                {
                                    SendRequest(req, client);
                                    var message = client.ReceiveMessage();
                                }
                            }

                            if (MessageRecieved < MessageOnServer)
                                System.Threading.Thread.Sleep(1);
                            else
                                System.Threading.Thread.Sleep(10);
                        }
                        catch
                        {
                            //Utils.Log("Common process: " + ex.Message, LogType.Error);                           
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// recieve streaming data
        /// </summary>
        public void StreamProcess()
        {
            using (var context = NetMQContext.Create())
            {
                using (var subscriber = context.CreateSubscriberSocket())
                {
                    subscriber.Connect(ConnectionStringStream);
                    subscriber.Subscribe("");
                    while (!bExit)
                    {
                        var message = subscriber.ReceiveMessage();
                        ProcessMessage(message);
                        
                    }
                }
            }

        }

        /// <summary>
        /// convert the message data from binary format to certain class
        ///  
        /// </summary>
        /// <param name="message"></param>
        /// <returns>type of message</returns>
        private DataType ProcessMessage(NetMQMessage message)
        {
            var data = new BinaryReader(new MemoryStream(message.First.ToByteArray()));
            var type = (DataType)data.ReadInt32();
            switch (type)
            {
                case DataType.kServer: // server data
                    MessageOnServer = data.ReadInt32();
                    if (MessageOnServer < MessageRecieved)
                    {
                        MessageRecieved = 0;
                        ClearData();
                    }
                    break;
                case DataType.kSecurity: //security updates                                   
                    ProcessSecurityData(data);
                    break;
                case DataType.kMoney: //money&limits updates
                    ProcessMoneyData(data);
                    break;
                case DataType.kPosition: 
                    ProcessPositionData(data);
                    break;
                case DataType.kPositionClear:
                    _Positions.Clear();
                    break;
                case DataType.kTrade:
                    ProcessTradeData(data);
                    break;
                case DataType.kStrategy:
                    ProcessStrategyData(data);
                    break;
                case DataType.kOrder:
                    ProcessOrderData(data);
                    break;
                default:
                    break;
            }
            return type;
        }
        
        private void ProcessSecurityData(BinaryReader data)
        {
            var newSec = Security.Parse(data);
            var oldSec = GetSecurityById(newSec.Id);
            if (oldSec.Update(newSec))
                Application.Current.Dispatcher.BeginInvoke(
                                       (Action)(() => SecurityChanged?.Invoke(Account, oldSec)));
        }

        private void ProcessStrategyData(BinaryReader data)
        {
            var newStr = Strategy.Parse(data);
            var oldStr = GetStrategyById(newStr.Id);
            if (oldStr.Update(newStr))
                Application.Current.Dispatcher.BeginInvoke(
                                       (Action)(() => StrategyChanged?.Invoke(Account, oldStr)));
        }

        private void ProcessMoneyData(BinaryReader data)
        {                 
            if (_MoneyInfo.Update(MoneyInfo.Parse(data)))
                Application.Current.Dispatcher.BeginInvoke(
                                       (Action)(() => MoneyInfoChanged?.Invoke(Account, _MoneyInfo)));
        }

        private void ProcessTradeData(BinaryReader data)
        {          
            Security sec = null;
            int symbolid = data.ReadInt32();
            _Securities.TryGetValue(symbolid, out sec);
            MyTrade deal = MyTrade.Parse(data, sec);                        

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                (Action)(() => MyTradeAdded?.Invoke(Account, deal)));   
        }

        private void ProcessPositionData(BinaryReader data)
        {
            var pos = Position.Parse(data);
            bool _exist = false;
            //update position
            foreach (var p in _Positions)
                if (p.Security.Id == pos.SecurityId)
                {
                    if (pos.Security == null)
                        pos.Security = GetSecurityById(pos.SecurityId);
                    if (p.Update(pos))
                        Application.Current.Dispatcher.BeginInvoke(
                                               (Action)(() =>PositionChanged?.Invoke(Account, p)));
                    _exist = true;
                    break;
                }
            //it's new one
            if (!_exist)
            {
                if (pos.Security == null)
                    pos.Security = GetSecurityById(pos.SecurityId);
                _Positions.Add(pos);
                Application.Current.Dispatcher.BeginInvoke(
                                       (Action)(() => PositionAdded?.Invoke(Account, pos)));
            }
        }

        private void ProcessOrderData(BinaryReader data)
        {     
            Security sec = null;
            int symbolid = data.ReadInt32();
            _Securities.TryGetValue(symbolid, out sec);
            Order ord = Order.Parse(data, sec);           

            if (_Orders.ContainsKey(ord.OrderId))
            {
                _Orders[ord.OrderId].Update(ord);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                (Action)(() =>                            
                                        OrderChanged?.Invoke(Account, ord)
                                ));
            }
            else
            {

                _Orders.Add(ord.OrderId, ord);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                (Action)(() => OrderAdded?.Invoke(Account, ord)));
            }               
            
        }
        private void ClearData()
        {
            _Securities.Clear();
            _Positions.Clear();
            _Orders.Clear();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                        (Action)(() => ClearAll?.Invoke(Account)));
        }

        public Security GetSecurityById(int isinid)
        {
            if (_Securities.ContainsKey(isinid))
                return _Securities[isinid];
            else
            {
                Security sec = new Security();
                sec.Id = isinid;
                _Securities[isinid] = sec;
                return sec;
            }
        }

        public Strategy GetStrategyById(int id)
        {
            if (_Strategies.ContainsKey(id))
                return _Strategies[id];
            else
            {
                Strategy str = new Strategy();
                str.Id = id;
                _Strategies[id] = str;
                return str;
            }
        }

        public event Action<MoneyInfo> MoneyInfoChanged;
        public event Action<Security> SecurityChanged;
        public event Action<Strategy> StrategyChanged;
        public event Action<Position> PositionChanged;
        public event Action<Position> PositionAdded;
        public event Action<MyTrade> MyTradeAdded;

        public event Action<Order> OrderAdded;
        public event Action<Order> OrderChanged;
        public event Action ClearAll;

        ~DataController()
        {
        }

    }
}
