using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleClient.Entities
{
    public class MyTrade : Entity
    {

        DateTime _Time;
        long _TradeId;
        Security _Security;
        decimal _Price;
        int _Volume;

        string _Algo;
        Direction _Direction;
        int _SecurityId;

        public string PriceString
        {
            get => Price.ToString("F4");            
        }

        public int SecurityId
        {
            get => _SecurityId;
            set
            {
                if (_SecurityId != value)
                {
                    _SecurityId = value;
                    NotifyPropertyChanged("SecurityId");
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

        public long TradeId
        {
            get => _TradeId;
            set
            {
                if (_TradeId != value)
                {
                    _TradeId = value;
                    NotifyPropertyChanged("TradeId");
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

        public SolidColorBrush GetLineBackground
        {
            get
            {
                if (Direction == Direction.SELL)
                    return new SolidColorBrush(Colors.LightPink);
                if (Direction == Direction.BUY)
                    return new SolidColorBrush(Colors.LightGreen);

                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public static MyTrade Parse(BinaryReader data, Security sec)
        {
            MyTrade deal = new MyTrade
            {
                Security = sec,
                Volume = data.ReadInt32(),
                Price = (decimal)data.ReadDouble(),
                Direction = (Direction)data.ReadInt32(),
                TradeId = data.ReadInt32()
            };

            int y = data.ReadInt32();
            int m = data.ReadInt32();
            int d = data.ReadInt32();
            int hh = data.ReadInt32();
            int mm = data.ReadInt32();
            int ss = data.ReadInt32();
            int ms = data.ReadInt32();
            if (y != 0)
                deal.Time = new DateTime(y, m, d, hh, mm, ss, ms);
   
            return deal;
        }


        public void Update(MyTrade src)
        {
            Time = src.Time;
            TradeId = src.TradeId;
            Security = src.Security;
            Price = src.Price;
            Volume = src.Volume;          
            Direction = src.Direction;          
            Time = src.Time;
            Algo = src.Algo;
            SecurityId = src.SecurityId;
        }
    }
}
