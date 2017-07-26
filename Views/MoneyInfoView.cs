using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class MoneyInfoView : INotifyPropertyChanged
    {
        private MoneyInfo moneyInfo;

        public MoneyInfoView(MoneyInfo source)
        {
            moneyInfo = source;
        }

        public void UpdateData(MoneyInfo source)
        {
            moneyInfo.Update(source);
            NotifyPropertyChanged("MoneyInfo");
            NotifyPropertyChanged("All");
            NotifyPropertyChanged("Blocked");
            NotifyPropertyChanged("Free");
        }
        public void UpdateVMData(decimal vm)
        {
            moneyInfo.VM = vm;
            NotifyPropertyChanged("VM");
        }

        public string All
        {
            get
            {
                return moneyInfo.All.ToString("#,0.00");
            }
        }
        public string Blocked
        {
            get
            {
                return moneyInfo.Blocked.ToString("#,0.00");
            }
        }
        public string Free
        {
            get
            {
                return moneyInfo.Free.ToString("#,0.00");
            }
        }
        public string VM
        {
            get
            {
                return moneyInfo.VM.ToString("#,0.00");
            }
        }

        public MoneyInfo MoneyInfo
        {
            get { return moneyInfo; }
            set
            {
                if (moneyInfo != value)
                {
                    moneyInfo = value;
                    NotifyPropertyChanged("MoneyInfo");
                    NotifyPropertyChanged("All");
                    NotifyPropertyChanged("Blocked");
                    NotifyPropertyChanged("Free");
                    NotifyPropertyChanged("VM");
                }
            }
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(moneyInfo.Name);
            sb.Append("Всего: "); sb.AppendLine(moneyInfo.All.ToString("#,0.00"));
            sb.Append("Позиции: "); sb.AppendLine(moneyInfo.Blocked.ToString("#,0.00"));
            sb.Append("Свободно: "); sb.AppendLine(moneyInfo.Free.ToString("#,0.00"));
            sb.Append("Сборы: "); sb.AppendLine(moneyInfo.Fee.ToString("#,0.00"));
            sb.Append("Коэф. ГО: "); sb.AppendLine(moneyInfo.CoefGO.ToString("#,0.00"));


            return sb.ToString();
        }

        public string Name { get { return moneyInfo.Name; } }
               

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
