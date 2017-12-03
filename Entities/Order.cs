using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.Entities
{
    public enum OrderStatus { NONE = 0, ACTIVE = 1, DONE = 2, FAILED = 3 }

    public class Order : Entity
    {
        OrderStatus _Status;
        DateTime _Time;
        int _Balance;
        string _Algo;

        int _OrderId;
        Security _Security;
        decimal _Price;
        int _Volume;
        Direction _Direction;

        public string PriceString
        {
            get => Price.ToString("F4");
        }

        public OrderStatus Status
        {
            get => _Status;
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        public Direction Direction
        {
            get => _Direction;
            set
            {
                if (_Direction != value)
                {
                    _Direction = value;
                    NotifyPropertyChanged("Direction");
                }
            }
        }

        public string Algo
        {
            get => _Algo;
            set
            {
                if (_Algo != value)
                {
                    _Algo = value;
                    NotifyPropertyChanged("Algo");
                }
            }
        }

        public int Volume
        {
            get => _Volume;
            set
            {
                if (_Volume != value)
                {
                    _Volume = value;
                    NotifyPropertyChanged("Volume");
                }
            }
        }

        public int Balance
        {
            get => _Balance;
            set
            {
                if (_Balance != value)
                {
                    _Balance = value;
                    NotifyPropertyChanged("Balance");
                }
            }
        }

        public decimal Price
        {
            get => _Price;
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    NotifyPropertyChanged("Price");
                    NotifyPropertyChanged("PriceString");
                }
            }
        }

        public Security Security
        {
            get => _Security;
            set
            {
                if (_Security != value)
                {
                    _Security = value;
                    NotifyPropertyChanged("Security");
                }
            }
        }

        public int OrderId
        {
            get => _OrderId;
            set
            {
                if (_OrderId != value)
                {
                    _OrderId = value;
                    NotifyPropertyChanged("OrderId");
                }
            }
        }

        public DateTime Time
        {
            get => _Time;
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        /*public void Update(Order src)
        {            
            Time = src.Time;
            OrderId = src.OrderId;
            Security = src.Security;
            Price = src.Price;
            Volume = src.Volume;
            Balance = src.Balance;
            Direction = src.Direction;
            Status = src.Status;
            Time = src.Time;
        }*/

        public static Order Parse(BinaryReader data, Security sec)
        {
            Order ord = new Order
            {
                Security = sec,
                Volume = data.ReadInt32(),
                Price = (decimal)data.ReadDouble(),
                Direction = (Direction)data.ReadInt32(),
                OrderId = data.ReadInt32(),
                Status = (OrderStatus)data.ReadInt32(),
                Balance = data.ReadInt32()
            };
            int y = data.ReadInt32();
            int m = data.ReadInt32();
            int d = data.ReadInt32();
            int hh = data.ReadInt32();
            int mm = data.ReadInt32();
            int ss = data.ReadInt32();
            int ms = data.ReadInt32();
            if (y != 0)
                ord.Time = new DateTime(y, m, d, hh, mm, ss, ms);
            //else
            //    ord.Time = DateTime.Now;
            return ord;
        }
                
    }
}
