using SimpleClient.Entities;
using SimpleClient.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleClient
{
    public class CommandEventArgs: EventArgs
    {
        public ClientRequest Request = null;
    }

    public class AccountView : Entity
    {

        private string _Name;
        MoneyInfo _MoneyInfo;


        public MoneyInfo MoneyInfo
        {
            get { return _MoneyInfo; }
            set
            {
                if (_MoneyInfo != value)
                {
                    _MoneyInfo = value;
                    NotifyPropertyChanged("MoneyInfo");
                }
            }
        }

        public ObservableCollection<Security> Securities { get; set; }
        public ObservableCollection<Position> Positions { get; set; }

        public ObservableCollection<MyTrade> MyTrades { get; set; }
        public ObservableCollection<Strategy> Strategies { get; set; }

        public ObservableCollection<Order> Orders { get; set; }
        private Dictionary<int, Order> _ordDic = new Dictionary<int, Order>();
                      

        private ICommand _ConnectCommand;
        private ICommand _DisconnectCommand;

        public event EventHandler<CommandEventArgs> SendCommand;

        private void _Init()
        {
            MoneyInfo = new MoneyInfo();
            Positions = new ObservableCollection<Position>();
            Securities = new ObservableCollection<Security>();
            MyTrades = new ObservableCollection<MyTrade>();
            Orders = new ObservableCollection<Order>();
            Strategies = new ObservableCollection<Strategy>();

            _ConnectCommand = new RelayCommand(arg => ConnectMethod());
            _DisconnectCommand = new RelayCommand(arg => DisconnectMethod());
        }

        public AccountView()
        {
            _Name = "";
            _Init();
        }

        public AccountView(string accountName)
        {
            _Name = accountName ?? "";
            _Init();
        }

        public ICommand ConnectCommand
        {
            get { return _ConnectCommand; }
        }
        public ICommand DisconnectCommand
        {
            get { return _DisconnectCommand; }
        }

        public void ConnectMethod()
        {
            SendCommand?.Invoke(this,new CommandEventArgs() { Request = new ClientRequest(RequestType.kConnect, 0, 0) });           
        }

        public void DisconnectMethod()
        {
            SendCommand?.Invoke(this, new CommandEventArgs() { Request = new ClientRequest(RequestType.kDisconnect, 0, 0) });
        }
        public void ClearAll()
        {
            MyTrades.Clear();
            Orders.Clear();
            Positions.Clear();
            Securities.Clear();
        }

        Order GetOrderByID(int _id)
        {
            if (_ordDic.ContainsKey(_id))
                return _ordDic[_id];
            return null;
        }

        void AddOrder(Order ord)
        {
            var order = new Order();
            order.Update(ord);
            _ordDic.Add(ord.OrderId, order);
            Orders.Add(order);
        }

        public string Name
        {
            get { return _Name; }
        }


        public void UpdateVM()
        {
            if (MoneyInfo != null)
                MoneyInfo.UpdateVMData(Positions.Sum(_pos => _pos.VM));
        }

        public void AddPositionView(Position pos)
        {
            var npos = new Position();
            npos.Update(pos);
            Positions.Add(npos);
        }

        public void AddMyTradeView(MyTrade trade)
        {
            var tr = new MyTrade();
            tr.Update(trade);
            MyTrades.Add(tr);
        }
        public void MoneyInfoViewChange(MoneyInfo info)
        {
            MoneyInfo.Update(info);
        }

        public void PositionViewChange(Position pos)
        {
           
            if (MoneyInfo != null)
                MoneyInfo.UpdateVMData(Positions.Sum(_pos => _pos.VM));
            foreach (var _pos in Positions)
                if (_pos.SecurityId == pos.SecurityId)
                {
                    _pos.Update(pos);
                    return;
                }
            var npos = new Position();
            npos.Update(pos);
            Positions.Add(npos);
        }

        public void SecurityViewChange(Security Changed)
        {
            var sec = Securities.FirstOrDefault(_sec => _sec.Id == Changed.Id);
            if (sec != null)
                sec.Update(Changed);
            else
            {
                sec = new Security();
                sec.Update(Changed);
                Securities.Add(sec);
            }
            if (Changed.BidVolume != 0 && Changed.AskVolume != 0)
            {
                sec.AddBidAsk(Changed.Bid, Changed.Ask);
            }
        }

        public void StrategyViewChange(Strategy Changed)
        {
            var sec = Strategies.FirstOrDefault(_sec => _sec.Id == Changed.Id);
            if (sec != null)
                sec.Update(Changed);
            else
            {
                sec = new Strategy();
                sec.Update(Changed);
                Strategies.Add(sec);
            }
        }

        public void OrderViewChange(Order Changed)
        {
            var ord = GetOrderByID(Changed.OrderId);
            if (ord == null)
            {
                AddOrder(Changed);
            }
            else
                ord.Update(Changed);
        }


    }
}
