using SimpleClient.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleClient.Views
{
    public class MyTradeView : INotifyPropertyChanged
    {
        private MyTrade trade;


        [Browsable(false)]
        public SolidColorBrush GetLineBackground
        {
            get
            {
                if (trade.Direction == Direction.SELL)
                    return new SolidColorBrush(Colors.LightPink);
                if (trade.Direction == Direction.BUY)
                    return new SolidColorBrush(Colors.LightGreen);

                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public MyTrade Trade
        {
            get { return trade; }
            set
            {
                if (trade != value)
                {
                    trade = value;
                    NotifyPropertyChanged("Trade");
                }
            }
        }

        public MyTradeView(MyTrade source)
        {
            trade = source;
        }

        public string PriceString
        {
            get
            {
                if (trade.Security != null)
                    return trade.Price.ToString("F4" );
                else
                    return "-";
            }
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
