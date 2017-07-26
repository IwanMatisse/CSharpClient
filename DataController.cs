
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
        private string ConnectionStringStream = "";
        private string ConnectionStringRequest = "";
        private string Account = "";
        private int MessageRecieved = 0;
        private int MessageOnServer = 0;
       
        public MoneyInfo MoneyInfo = new MoneyInfo();     
        private ConcurrentDictionary<int, Security> Securities = new ConcurrentDictionary<int, Security>();
        private ConcurrentDictionary<int, Strategy> Strategies = new ConcurrentDictionary<int, Strategy>();
        public List<Position> Positions = new List<Position>();
        private Dictionary<int, Order> Orders = new Dictionary<int, Order>();
        private ConcurrentQueue<ClientRequest> Commands = new ConcurrentQueue<ClientRequest>();

        private bool bExit = false;
        
        public DataController(string AccountName, string AddressStreamData, string AddressRequestData)
        {
            MoneyInfo.Name = Account;
            Account = AccountName;
            ConnectionStringStream = AddressStreamData;
            ConnectionStringRequest = AddressRequestData;
        }

        public void AddCommand(ClientRequest req)
        {
            Commands.Enqueue(req);
        }
                
        /// <summary>
        /// Run process of recieve data in new stream
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
                                ClientRequest req = new ClientRequest(RequestType.kMessage, MessageRecieved + 1,0 );

                                SendRequest(req, client);
                                //get answer
                                var message = client.ReceiveMessage();
                                if (ProcessMessage(message) != DataType.kServer) 
                                    MessageRecieved++;

                            }
                            if (Commands.Count > 0)
                            {
                                ClientRequest req;
                                var result = Commands.TryDequeue(out req);
                                if (result)
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
                    Positions.Clear();
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
                                       (Action)(() =>
                                       {
                                           if (SecurityChanged != null)
                                               SecurityChanged(Account, oldSec);
                                       }));
        }

        private void ProcessStrategyData(BinaryReader data)
        {
            var newStr = Strategy.Parse(data);
            var oldStr = GetStrategyById(newStr.Id);
            if (oldStr.Update(newStr))
                Application.Current.Dispatcher.BeginInvoke(
                                       (Action)(() =>
                                       {
                                           if (StrategyChanged != null)
                                               StrategyChanged(Account, oldStr);
                                       }));
        }

        private void ProcessMoneyData(BinaryReader data)
        {
            var changed = MoneyInfo.Parse(data);           
            if (changed)
                Application.Current.Dispatcher.BeginInvoke(
                                       (Action)(() =>
                                       {
                                           if (MoneyInfoChanged != null)
                                               MoneyInfoChanged(Account, MoneyInfo);
                                       }));
        }

        private void ProcessTradeData(BinaryReader data)
        {          
            Security sec = null;
            int symbolid = data.ReadInt32();
            Securities.TryGetValue(symbolid, out sec);
            MyTrade deal = MyTrade.Parse(data, sec);                        

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                (Action)(() =>
                                {
                                    if (MyTradeAdded != null)
                                        MyTradeAdded(Account, deal);
                                }));   
        }

        private void ProcessPositionData(BinaryReader data)
        {
            var pos = Position.Parse(data);
            bool _exist = false;
            //update position
            foreach (var p in Positions)
                if (p.Security.Id == pos.SecurityId)
                {
                    if (pos.Security == null)
                        pos.Security = GetSecurityById(pos.SecurityId);
                    if (p.Update(pos))
                        Application.Current.Dispatcher.BeginInvoke(
                                               (Action)(() =>
                                               {
                                                   if (PositionChanged != null)
                                                       PositionChanged(Account, p);
                                               }));
                    _exist = true;
                    break;
                }
            //it's new one
            if (!_exist)
            {
                if (pos.Security == null)
                    pos.Security = GetSecurityById(pos.SecurityId);
                Positions.Add(pos);
                Application.Current.Dispatcher.BeginInvoke(
                                       (Action)(() =>
                                       {
                                           if (PositionAdded != null)
                                               PositionAdded(Account, pos);
                                       }));
            }
        }

        private void ProcessOrderData(BinaryReader data)
        {     
            Security sec = null;
            int symbolid = data.ReadInt32();
            Securities.TryGetValue(symbolid, out sec);
            Order ord = Order.Parse(data, sec);           

            if (Orders.ContainsKey(ord.OrderId))
            {
                Orders[ord.OrderId].Update(ord);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                (Action)(() =>
                                {
                                    if (OrderChanged != null)
                                        OrderChanged(Account, ord);
                                }));
            }
            else
            {

                Orders.Add(ord.OrderId, ord);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                (Action)(() =>
                                {
                                    if (OrderAdded != null)
                                        OrderAdded(Account, ord);
                                }));
            }               
            
        }
        private void ClearData()
        {
            Securities.Clear();
            Positions.Clear();
            Orders.Clear();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                        (Action)(() =>
                                        {
                                            if (ClearAll != null)
                                                ClearAll(Account);
                                        }));
        }

        public Security GetSecurityById(int isinid)
        {
            if (Securities.ContainsKey(isinid))
                return Securities[isinid];
            else
            {
                Security sec = new Security();
                sec.Id = isinid;
                Securities[isinid] = sec;
                return sec;
            }
        }

        public Strategy GetStrategyById(int id)
        {
            if (Strategies.ContainsKey(id))
                return Strategies[id];
            else
            {
                Strategy str = new Strategy();
                str.Id = id;
                Strategies[id] = str;
                return str;
            }
        }

        public event Action<string, MoneyInfo> MoneyInfoChanged;
        public event Action<string, Security> SecurityChanged;
        public event Action<string, Strategy> StrategyChanged;
        public event Action<string, Position> PositionChanged;
        public event Action<string, Position> PositionAdded;
        public event Action<string, MyTrade> MyTradeAdded;

        public event Action<string, Order> OrderAdded;
        public event Action<string, Order> OrderChanged;
        public event Action<string> ClearAll;

        ~DataController()
        {
        }

    }
}
