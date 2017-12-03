using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class MoneyInfo : Entity
    {
        decimal _All;
        decimal _MoneyOnBegin;
        decimal _Free;
        decimal _Blocked;
        decimal _Fee;
        decimal _CoefGO;
        string _Name;

        decimal _VM;

        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public decimal All
        {
            get => _All;
            set
            {
                if (_All != value)
                {
                    _All = value;
                    NotifyPropertyChanged("All");
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

        public decimal CoefGO
        {
            get => _CoefGO;
            set
            {
                if (_CoefGO != value)
                {
                    _CoefGO = value;
                    NotifyPropertyChanged("CoefGO");
                }
            }
        }

        public decimal Fee
        {
            get => _Fee;
            set
            {
                if (_Fee != value)
                {
                    _Fee = value;
                    NotifyPropertyChanged("Fee");
                }
            }
        }

        public decimal Blocked
        {
            get => _Blocked;
            set
            {
                if (_Blocked != value)
                {
                    _Blocked = value;
                    NotifyPropertyChanged("Blocked");
                }
            }
        }

        public decimal Free
        {
            get => _Free;
            set
            {
                if (_Free != value)
                {
                    _Free = value;
                    NotifyPropertyChanged("Free");
                }
            }
        }

        public decimal MoneyOnBegin
        {
            get => _MoneyOnBegin;
            set
            {
                if (_MoneyOnBegin != value)
                {
                    _MoneyOnBegin = value;
                    NotifyPropertyChanged("MoneyOnBegin");
                }
            }
        }
        

        public void Update(MoneyInfo source)
        {
            All = source.All;
            MoneyOnBegin = source.MoneyOnBegin;
            Free = source.Free;
            Blocked = source.Blocked;
            Fee = source.Fee;
            CoefGO = source.CoefGO;
            // VM = source.VM;
        }

        public void Parse(BinaryReader data)
        {
            CoefGO = (decimal)data.ReadDouble();
            All = (decimal)data.ReadDouble();
            Free = (decimal)data.ReadDouble();
            Blocked = (decimal)data.ReadDouble();
            Fee = (decimal)data.ReadDouble();              
        }            
               
    }
}
