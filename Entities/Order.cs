using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient.Entities
{
    public enum OrderStatus { NONE = 0, ACTIVE = 1, DONE = 2, FAILED = 3 }
    public class Order
    {
        public OrderStatus Status { get; set; }
        public DateTime Time { get; set; }
        public int Balance { get; set; }
        public string Algo { get; set; }

        public int OrderId { get; set; }
        public Security Security { get; set; }
        public decimal Price { get; set; }

        public int Volume { get; set; }
        public Direction Direction { get; set; }
 
        public void Update(Order src)
        {            
            Time = src.Time;
            OrderId = src.OrderId;
            Security = src.Security;
            Price = src.Price;
            Volume = src.Volume;
            Balance = src.Balance;
            Direction = src.Direction;
            Status = src.Status;
            Time = src.Time;
        }

        public static Order Parse(BinaryReader data, Security sec)
        {
            Order ord = new Order();
                        
            ord.Security = sec;
            ord.Volume = data.ReadInt32();
            ord.Price = (decimal)data.ReadDouble();
            ord.Direction = (Direction)data.ReadInt32();
            ord.OrderId = data.ReadInt32();
            ord.Status = (OrderStatus)data.ReadInt32();
            ord.Balance = data.ReadInt32();
            int y = data.ReadInt32();
            int m = data.ReadInt32();
            int d = data.ReadInt32();
            int hh = data.ReadInt32();
            int mm = data.ReadInt32();
            int ss = data.ReadInt32();
            int ms = data.ReadInt32();
            if (y != 0)
                ord.Time = new DateTime(y, m, d, hh, mm, ss, ms);
            //else
            //    ord.Time = DateTime.Now;
            return ord;
        }

    }
}
