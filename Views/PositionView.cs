using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleClient
{
    public class PositionView : INotifyPropertyChanged
    {
        private Position position;

        public string Name
        {
            get
            {
                return position.Security.ToString();
            }
        }

        [Browsable(false)]
        public SolidColorBrush GetLineBackground
        {
            get
            {
                if (position.Volume < 0)
                    return new SolidColorBrush(Colors.LightPink);
                if (position.Volume > 0)
                    return new SolidColorBrush(Colors.LightGreen);

                return new SolidColorBrush(Colors.White);
            }
        }

        public Position Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    NotifyPropertyChanged("Position");
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public PositionView(Position posSource)
        {
            position = posSource;
        }
        public void UpdateData(Position source)
        {
            position.Update(source);
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Position");
            NotifyPropertyChanged("GetLineBackground");
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
