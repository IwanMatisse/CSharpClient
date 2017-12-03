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
        
            DataContext = mainView;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
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
