using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.Entities
{
    public class Strategy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public decimal Price { get; set; }
        public int State { get; set; }
        public bool Started { get; set; }


        /// <summary>
        /// return TRUE if source is different, changes was made
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool Update(Strategy source)
        {
            bool itChanged = false;
            itChanged = (Name != source.Name) || (Position != source.Position) || (Price != source.Price) || (State != source.State) || (Started != source.Started);

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
            /*int id = 0;
		double price = 0.0;
		int volume = 0;
		char state = 0;
		bool enabled = false;
		char name[10];*/
            Strategy str = new Strategy();
            str.Id = data.ReadInt32();
            str.Price = (decimal)data.ReadDouble();
            str.Position = data.ReadInt32();
            str.State = data.ReadByte();
            str.Started = data.ReadBoolean();
           
            var chars = data.ReadChars(10);
            int len = 0;
            while (chars[len] != 0 && len < 10)
                len++;
            str.Name = new string(chars, 0, len);

            return str;
        }


    }
}
