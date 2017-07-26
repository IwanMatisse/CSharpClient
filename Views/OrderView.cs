using SimpleClient.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.Views
{
    public class OrderView : INotifyPropertyChanged
    {
        private Order order;
        
        public Order Order
        {
            get { return order; }
            set
            {
                if (order != value)
                {
                    order = value;
                    NotifyPropertyChanged("Order");
                }
            }
        }

        public OrderView(Order orderSource)
        {
            order = orderSource;
        }
        public void UpdateData(Order source)
        {
            order.Update(source);
            NotifyPropertyChanged("Order");
        }

        public string PriceString
        {
            get
            {
                if (order.Security != null)
                    return order.Price.ToString("F4");
                else
                    return "-";
            }
        }
        
        public bool IsPartiallyMatched
        {
            get { return order.Volume != order.Balance; }
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
