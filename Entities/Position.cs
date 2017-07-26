using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class Position
    {
        public Security Security { get; set; }
        public int Volume { get; set; }
        public decimal Price { get; set; }
        public int BeginVolume { get; set; }
        public int BuyVolume { get; set; }
        public int SellVolume { get; set; }
        public string Account { get; set; }
        public int SecurityId { get; set; }
        public decimal VM { get; set; }

        /// <summary>
        /// return TRUE if source is different, changes was made
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool Update(Position source)
        {
            bool itChanged = false;
            itChanged = (Volume != source.Volume) || (VM != source.VM) || (Price != source.Price) || (BuyVolume != source.BuyVolume) || (SellVolume != source.SellVolume);

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
            Position pos = new Position();
            pos.SecurityId = data.ReadInt32();
            pos.BeginVolume = data.ReadInt32();
            pos.BuyVolume = data.ReadInt32();
            pos.SellVolume = data.ReadInt32();
            pos.Volume = data.ReadInt32();
            pos.Price = (decimal) data.ReadDouble();
            pos.VM = (decimal)data.ReadDouble();           

            return pos;
        }


    }
}
