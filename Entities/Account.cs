using SimpleClient.Entities;
using SimpleClient.Views;
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
            AccountView = new AccountView(this);
            Data = new DataController(Settings.Name, Settings.AddressStreamData, Settings.AddressRequestData);
        }

        
        public void UpdateVM()
        {
            if (AccountView.MoneyInfoView != null)
                AccountView.MoneyInfoView.UpdateVMData(Data.Positions.Sum(_pos => _pos.VM));
        }

        public void AddPositionView(Position pos)
        {
            AccountView.PositionViews.Add(new PositionView(pos));
        }

        public void MoneyInfoViewChange(MoneyInfo info)
        {
            if (AccountView.MoneyInfoView == null)
                AccountView.MoneyInfoView = new MoneyInfoView(info);
            else
                AccountView.MoneyInfoView.UpdateData(info);
        }

        public void PositionViewChange(Position pos)
        {
            var view = AccountView;
            if (view.MoneyInfoView != null)
                view.MoneyInfoView.UpdateVMData(Data.Positions.Sum(_pos => _pos.VM));
            foreach (var _pos in view.PositionViews)
                if (_pos.Position.SecurityId == pos.SecurityId)
                {
                    _pos.UpdateData(pos);
                    return;
                }
            view.PositionViews.Add(new PositionView(pos));
        }

        public void SecurityViewChange(Security Changed)
        {
            var sec = AccountView.SecurityViews.FirstOrDefault(_sec => _sec.Security.Id == Changed.Id);
            if (sec != null)
                sec.UpdateData(Changed);
            else
            {
                sec = new SecurityView(Changed);
                AccountView.SecurityViews.Add(sec);
            }
            if (Changed.BidVolume != 0 && Changed.AskVolume != 0)
            {
                sec.AddBidAsk( Changed.Bid, Changed.Ask);
            }
        }

        public void StrategyViewChange(Strategy Changed)
        {
            var sec = AccountView.StrategyViews.FirstOrDefault(_sec => _sec.Strategy.Id == Changed.Id);
            if (sec != null)
                sec.UpdateData(Changed);
            else
            {
                sec = new StrategyView(Changed);
                AccountView.StrategyViews.Add(sec);
            }
        }

        public void OrderViewChange(Order Changed)
        {
            var ord = AccountView.GetOrderByID(Changed.OrderId);
            if (ord == null)
            {
                AccountView.AddOrder(Changed);
            }
            else
                ord.UpdateData(Changed);
        }

        public void SendCommand(ClientRequest req)
        {
            Data.AddCommand(req);
        }
    }
}
