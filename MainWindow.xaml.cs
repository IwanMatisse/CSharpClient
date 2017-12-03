using SimpleClient.Entities;
using SimpleClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Account testAccount;
        AccountView mainView { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            AccountSettings settings = new AccountSettings();
            settings.Name = "TEST";
            settings.AddressStreamData = "tcp://127.0.0.1:5001";
            settings.AddressRequestData = "tcp://127.0.0.1:5002";
            testAccount = new Account(settings);

            mainView = testAccount.AccountView;
            testAccount.Data.SecurityChanged += OnSecurityChanged;
            testAccount.Data.MoneyInfoChanged += OnMoneyInfoChanged;
            testAccount.Data.PositionAdded += OnPositionAdded;
            testAccount.Data.PositionChanged += OnPositionChanged;
            testAccount.Data.MyTradeAdded += OnMyTradeAdded;
            testAccount.Data.OrderAdded += OnOrderAdded;
            testAccount.Data.OrderChanged += OnOrderChanged;
            testAccount.Data.ClearAll += OnClearAll;
            testAccount.Data.StrategyChanged += OnStrategyChanged;
            DataContext = mainView;

            testAccount.Data.SubscriberStart();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            testAccount.Data.SecurityChanged -= OnSecurityChanged;
            testAccount.Data.MoneyInfoChanged -= OnMoneyInfoChanged;
            testAccount.Data.PositionAdded -= OnPositionAdded;
            testAccount.Data.PositionChanged -= OnPositionChanged;
            testAccount.Data.MyTradeAdded -= OnMyTradeAdded;
            testAccount.Data.OrderAdded -= OnOrderAdded;
            testAccount.Data.OrderChanged -= OnOrderChanged;
            testAccount.Data.ClearAll -= OnClearAll;
            testAccount.Data.StrategyChanged -= OnStrategyChanged;
            testAccount.Data.SubscriberStop();
        }

        public void OnSecurityChanged(string account, Security Changed)
        {
           testAccount.SecurityViewChange(Changed);
        }
        public void OnStrategyChanged(string account, Strategy Changed)
        {
            testAccount.StrategyViewChange(Changed);
        }
        public void OnMyTradeAdded(string account, MyTrade trade)
        {
            testAccount.AccountView.MyTradeViews.Add(new MyTradeView(trade));
        }

        public void OnMoneyInfoChanged(string account, MoneyInfo Changed)
        {
            testAccount.MoneyInfoViewChange(Changed);   
        }

        public void OnPositionAdded(string account, Position pos)
        {     
            testAccount.UpdateVM();
            testAccount.AddPositionView(pos);
        }
        /// <summary>
        /// Handler for update changed position data  
        /// </summary>
        /// <param name="Changed"></param>
        public void OnPositionChanged(string account, Position Changed)
        {
            testAccount.PositionViewChange(Changed);
        }

        public void OnOrderAdded(string account, Order Changed)
        {
            testAccount.OrderViewChange(Changed);
        }

        public void OnOrderChanged(string account, Order Changed)
        {
            testAccount.OrderViewChange(Changed);
        }

        public void OnClearAll(string account)
        {
            testAccount.AccountView.ClearAll();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                //chart.Title = (e.AddedItems[0] as SecurityView).Name;
                //BidChart.DataContext = (e.AddedItems[0] as SecurityView).Bids;
                //AskChart.DataContext = (e.AddedItems[0] as SecurityView).Asks;
            }
        }
    }
}
