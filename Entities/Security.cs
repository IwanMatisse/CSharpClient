using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class Security: Entity
    {
        string _Isin;
        int _Id;
        decimal _Bid;
        decimal _Ask;
        int _BidVolume;
        int _AskVolume;
        decimal _LastPrice;

        public string Name { get => Isin == "" ? Id.ToString() : Isin; }

        public string Isin
        {
            get => _Isin;
            set
            {
                if (_Isin != value)
                {
                    _Isin = value;
                    NotifyPropertyChanged("Isin");
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public int Id
        {
            get => _Id;
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }


        public int AskVolume
        {
            get => _AskVolume;
            set
            {
                if (_AskVolume != value)
                {
                    _AskVolume = value;
                    NotifyPropertyChanged("AskVolume");
                }
            }
        }

        public int BidVolume
        {
            get => _BidVolume;
            set
            {
                if (_BidVolume != value)
                {
                    _BidVolume = value;
                    NotifyPropertyChanged("BidVolume");
                }
            }
        }

        public decimal Bid
        {
            get => _Bid;
            set
            {
                if (_Bid != value)
                {
                    _Bid = value;
                    NotifyPropertyChanged("Bid");
                }
            }
        }

        public decimal Ask
        {
            get => _Ask;
            set
            {
                if (_Ask != value)
                {
                    _Ask = value;
                    NotifyPropertyChanged("Ask");
                }
            }
        }

        public decimal LastPrice
        {
            get => _LastPrice;
            set
            {
                if (_LastPrice != value)
                {
                    _LastPrice = value;
                    NotifyPropertyChanged("LastPrice");
                }
            }
        }

        public Security()
        {
            Isin = "";
        }

        public override string ToString()
        {
            return Isin == "" ? Id.ToString() : Isin;
        }

        public bool Update(Security source)
        {
            bool itChanged = false;
            itChanged = (Bid != source.Bid) || (Ask != source.Ask) || (Isin != source.Isin);

            Isin = source.Isin;
            Id = source.Id;
            Bid = source.Bid;
            Ask = source.Ask;
            BidVolume = source.BidVolume;
            AskVolume = source.AskVolume;
            LastPrice = source.LastPrice;
            return itChanged;

        }

        public static Security Parse(BinaryReader data)
        {
            return new Security
            {
                Id = data.ReadInt32(),
                Bid = (decimal)data.ReadDouble(),
                BidVolume = data.ReadInt32(),
                Ask = (decimal)data.ReadDouble(),
                AskVolume = data.ReadInt32(),
                LastPrice = (decimal)data.ReadDouble(),
                Isin = data.ReadChars(26).TakeWhile(c => c != 0).Take(26).ToString()
            };                        
        }
    }
}
