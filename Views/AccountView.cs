using SimpleClient.Entities;
using SimpleClient.Utils;
using SimpleClient.Views;
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
    public class AccountView : INotifyPropertyChanged
    {

        private string _name;
        
        public MoneyInfoView MoneyInfoView
        {
            get { return _moneyInfo; }
            set
            {
                if (_moneyInfo != value)
                {
                    _moneyInfo = value;
                    NotifyPropertyChanged("MoneyInfoView");
                }
            }
        }

        public ObservableCollection<SecurityView> SecurityViews { get; set; }
        public ObservableCollection<PositionView> PositionViews { get; set; }

        public ObservableCollection<MyTradeView> MyTradeViews { get; set; }
        public ObservableCollection<StrategyView> StrategyViews { get; set; }

        public ObservableCollection<OrderView> OrderViews { get; set; }
        private Dictionary<int, OrderView> _ordDic = new Dictionary<int, OrderView>();
        private MoneyInfoView _moneyInfo;
       
        public Account Account { get; set; }

        private readonly ICommand _ConnectCommand;
        private readonly ICommand _DisconnectCommand;

        public AccountView(Account account)
        {
            _name = account.Settings.Name;
            Account = account;
            // Settings = settings;

          
            PositionViews = new ObservableCollection<PositionView>();
            SecurityViews = new ObservableCollection<SecurityView>();
            MyTradeViews = new ObservableCollection<MyTradeView>();        
            OrderViews = new ObservableCollection<OrderView>();
            StrategyViews = new ObservableCollection<StrategyView>();

            _ConnectCommand = new RelayCommand(arg => ConnectMethod());
            _DisconnectCommand = new RelayCommand(arg => DisconnectMethod());

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
            Account.SendCommand(new ClientRequest(RequestType.kConnect,0,0));           
        }

        public void DisconnectMethod()
        {
            Account.SendCommand(new ClientRequest(RequestType.kDisconnect, 0, 0));
        }
        public void ClearAll()
        {
            MyTradeViews.Clear();
            OrderViews.Clear();
            PositionViews.Clear();
            SecurityViews.Clear();
        }
        public OrderView GetOrderByID(int _id)
        {
            if (_ordDic.ContainsKey(_id))
                return _ordDic[_id];
            return null;
        }
        public void AddOrder(Order ord)
        {
            var orderView = new OrderView(ord);
            _ordDic.Add(ord.OrderId, orderView);
            OrderViews.Add(orderView);
        }

        public string Name
        {
            get { return _name; }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
