using SimpleClient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class Account
    {
        public readonly AccountSettings Settings;
        public DataController Data;        
      
        public AccountView AccountView;

        public Account(AccountSettings settings)
        {
            Settings = settings;            
            AccountView = new AccountView(Settings.Name);
            AccountView.SendCommand += SendCommand;
            Data = new DataController(Settings.Name, Settings.AddressStreamData, Settings.AddressRequestData);

            Data.SecurityChanged += AccountView.SecurityViewChange;
            Data.MoneyInfoChanged += AccountView.MoneyInfoViewChange;
            Data.PositionAdded += AccountView.AddPositionView;
            Data.PositionChanged += AccountView.PositionViewChange;
            Data.MyTradeAdded += AccountView.AddMyTradeView;
            Data.OrderAdded += AccountView.OrderViewChange;
            Data.OrderChanged += AccountView.OrderViewChange;
            Data.ClearAll += AccountView.ClearAll;
            Data.StrategyChanged += AccountView.StrategyViewChange;
            Data.SubscriberStart();
        }

        ~Account()
        {
            Data.SubscriberStop();
        }


        public void SendCommand(object sender, CommandEventArgs args)
        {
            Data.AddCommand(args.Request);
        }
    }
}
