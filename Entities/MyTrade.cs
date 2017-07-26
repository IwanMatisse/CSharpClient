using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.Entities
{
    public class MyTrade
    {

        public DateTime Time { get; set; }
        public long TradeId { get; set; }
        public Security Security { get; set; }
        public decimal Price { get; set; }
        public int Volume { get; set; }

        public string Algo { get; set; }
        public Direction Direction { get; set; }
        public int SecurityId { get; set; }

        public static MyTrade Parse(BinaryReader data, Security sec)
        {
            MyTrade deal = new MyTrade();          
            deal.Security = sec;
            deal.Volume = data.ReadInt32();
            deal.Price = (decimal)data.ReadDouble();
            deal.Direction = (Direction)data.ReadInt32();
            deal.TradeId = data.ReadInt32();

            int y = data.ReadInt32();
            int m = data.ReadInt32();
            int d = data.ReadInt32();
            int hh = data.ReadInt32();
            int mm = data.ReadInt32();
            int ss = data.ReadInt32();
            int ms = data.ReadInt32();
            if (y != 0)
                deal.Time = new DateTime(y, m, d, hh, mm, ss, ms);
            //else
            //    deal.Time = DateTime.Now;
            //deal.Time = new DateTime(data.ReadInt32(), data.ReadInt32(), data.ReadInt32(), data.ReadInt32(), data.ReadInt32(), data.ReadInt32(), data.ReadInt32());
            return deal;
        }

    }
}
