using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class Security
    {
        public string Isin { get; set; }
        public int Id { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public int BidVolume { get; set; }
        public int AskVolume { get; set; }
        public decimal LastPrice { get; set; }
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
            Security sec = new Security();
            
            sec.Id = data.ReadInt32();
            sec.Bid =(decimal) data.ReadDouble();
            sec.BidVolume = data.ReadInt32();
            sec.Ask = (decimal)data.ReadDouble();
            sec.AskVolume = data.ReadInt32();
            sec.LastPrice = (decimal)data.ReadDouble();
            var chars = data.ReadChars(26);
            int len = 0;
            while (chars[len] != 0 && len<26)
                len++;
            sec.Isin = new string(chars,0,len);
            /*                                
            int id = 0;
            double bid = 0.0;
            int bid_vol = 0;
            double ask = 0.0;
            int ask_vol = 0;
            double last_price = 0.0; 
            char symbol[26];*/

            return sec;
        }
    }
}
