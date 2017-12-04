using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.Entities
{
    public class Strategy : INotifyPropertyChanged
    {
        int _Id;
        string _Name;
        int _Position;
        decimal _Price;
        int _State;
        bool _Started;

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

        public bool Started
        {
            get => _Started;
            set
            {
                if (_Started != value)
                {
                    _Started = value;
                    NotifyPropertyChanged("Started");
                }
            }
        }

        public int State {
            get => _State;
            set
            {
                if (_State != value)
                {
                    _State = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        public int Position
        {
            get => _Position;
            set
            {
                if (_Position != value)
                {
                    _Position = value;
                    NotifyPropertyChanged("Position");
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
                }
            }
        }




        /// <summary>
        /// return TRUE if source is different, changes was made
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool Update(Strategy source)
        {
            bool itChanged = false;
            itChanged = (Name != source.Name) || (Position != source.Position)
                || (Price != source.Price) || (State != source.State) 
                || (Started != source.Started);

            Name = source.Name;
            Position = source.Position;
            Price = source.Price;
            State = source.State;
            Started = source.Started;
            Id = source.Id;

            return itChanged;
        }

        public static Strategy Parse(BinaryReader data)
        {           
            return new Strategy
            {
                Id = data.ReadInt32(),
                Price = (decimal)data.ReadDouble(),
                Position = data.ReadInt32(),
                State = data.ReadByte(),
                Started = data.ReadBoolean(),
                Name = data.ReadChars(10).TakeWhile(c => c != 0).Take(10).ToString()
            };           
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
