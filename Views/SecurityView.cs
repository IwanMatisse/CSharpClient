using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class SecurityView : INotifyPropertyChanged
    {
        private Security security;
        private ObservableCollection<KeyValuePair<int, decimal>> bids = new ObservableCollection<KeyValuePair<int, decimal>>();
        private ObservableCollection<KeyValuePair<int, decimal>> asks = new ObservableCollection<KeyValuePair<int, decimal>>();
        private int TicksCount= 0;
        public string Name
        {
            get
            {
                return security.ToString();
            }
        }
        public Security Security
        {
            get { return security; }
            set
            {
                if (security != value)
                {
                    security = value;
                    NotifyPropertyChanged("Security");
                }
            }
        }

        public ObservableCollection<KeyValuePair<int, decimal>> Bids
        {
            get { return bids; }            
        }
        public ObservableCollection<KeyValuePair<int, decimal>> Asks
        {
            get { return asks; }
        }

        public void AddBidAsk(decimal bid, decimal ask)
        {
            if (bids.Count >= 50)
                bids.RemoveAt(0);
            if (asks.Count >= 50)
                asks.RemoveAt(0);
            TicksCount++;
            bids.Add(new KeyValuePair<int, decimal>(TicksCount, bid));
            asks.Add(new KeyValuePair<int, decimal>(TicksCount, ask));
            NotifyPropertyChanged("Bids");
            NotifyPropertyChanged("Asks");
        }
       
        public SecurityView(Security secSource)
        {
            security = secSource;
        }
        public void UpdateData(Security source)
        {
            security.Update(source);
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Security");
            NotifyPropertyChanged("LastPrice");
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
