using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleClient
{
    public class Position : Entity
    {
        Security _Security;
        int _Volume;
        decimal _Price;
        int _BeginVolume;
        int _BuyVolume;
        int _SellVolume;
        string _Account;
        int _SecurityId;
        decimal _VM;


        public decimal Price
        {
            get => _Price;
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        public string Account
        {
            get => _Account;
            set
            {
                if (_Account != value)
                {
                    _Account = value;
                    NotifyPropertyChanged("Account");
                }
            }
        }

        public decimal VM
        {
            get => _VM;
            set
            {
                if (_VM != value)
                {
                    _VM = value;
                    NotifyPropertyChanged("VM");
                }
            }
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

        public int SellVolume
        {
            get => _SellVolume;
            set
            {
                if (_SellVolume != value)
                {
                    _SellVolume = value;
                    NotifyPropertyChanged("SellVolume");
                }
            }
        }
        public int BuyVolume
        {
            get => _BuyVolume;
            set
            {
                if (_BuyVolume != value)
                {
                    _BuyVolume = value;
                    NotifyPropertyChanged("BuyVolume");
                }
            }
        }

        public int BeginVolume
        {
            get => _BeginVolume;
            set
            {
                if (_BeginVolume != value)
                {
                    _BeginVolume = value;
                    NotifyPropertyChanged("BeginVolume");
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


        public SolidColorBrush GetLineBackground
        {
            get
            {
                if (Volume < 0)
                    return new SolidColorBrush(Colors.LightPink);
                if (Volume > 0)
                    return new SolidColorBrush(Colors.LightGreen);

                return new SolidColorBrush(Colors.White);
            }
        }


        /// <summary>
        /// return TRUE if source is different, changes was made
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool Update(Position source)
        {
            bool itChanged = false;
            itChanged = (Volume != source.Volume) || (VM != source.VM) || (Price != source.Price) 
                || (BuyVolume != source.BuyVolume) || (SellVolume != source.SellVolume);

            Security = source.Security;
            Volume = source.Volume;
            Price = source.Price;
            BeginVolume = source.BeginVolume;
            BuyVolume = source.BuyVolume;
            SellVolume = source.SellVolume;
            Account = source.Account;
            VM = source.VM;
            SecurityId = source.SecurityId;
            return itChanged;
        }

        public static Position Parse(BinaryReader data)
        {
            return new Position
            {
                SecurityId = data.ReadInt32(),
                BeginVolume = data.ReadInt32(),
                BuyVolume = data.ReadInt32(),
                SellVolume = data.ReadInt32(),
                Volume = data.ReadInt32(),
                Price = (decimal)data.ReadDouble(),
                VM = (decimal)data.ReadDouble()
            };
            
        }


    }
}
