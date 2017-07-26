using SimpleClient.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.Views
{
    public class StrategyView : INotifyPropertyChanged
    {
        private Strategy strategy;

        public string Name
        {
            get
            {
                return strategy.Name;
            }
        }
        public Strategy Strategy
        {
            get { return strategy; }
            set
            {
                if (strategy != value)
                {
                    strategy = value;
                    NotifyPropertyChanged("Strategy");
                }
            }
        }

        public StrategyView(Strategy strSource)
        {
            strategy = strSource;
        }
        public void UpdateData(Strategy source)
        {
            strategy.Update(source);
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Started");
            NotifyPropertyChanged("Position");
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
