using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    public class MoneyInfo
    {
        public decimal All { get; set; }
        public decimal MoneyOnBegin { get; set; }
        public decimal Free { get; set; }
        public decimal Blocked { get; set; }
        public decimal Fee { get; set; }
        public decimal CoefGO { get; set; }
        public string Name { get; set; }

        public decimal VM { get; set; }
        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            sb.Append("Всего: "); sb.AppendLine(All.ToString("#,0.00"));
            sb.Append("Позиции: "); sb.AppendLine(Blocked.ToString("#,0.00"));
            sb.Append("Свободно: "); sb.AppendLine(Free.ToString("#,0.00"));
            sb.Append("Сборы: "); sb.AppendLine(Fee.ToString("#,0.00"));
            sb.Append("Коэф. ГО: "); sb.AppendLine(CoefGO.ToString("#,0.00"));
            sb.Append("Вар.маржа: "); sb.AppendLine(VM.ToString("#,0.00"));

            return sb.ToString();
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

        public bool Parse(BinaryReader data)
        {
            /*double coeffGo = 0.0;
          double all = 0.0;
          double free = 0.0;
          double blocked = 0.0;
          double fee = 0.0;*/

            CoefGO = (decimal)data.ReadDouble();
            All = (decimal)data.ReadDouble();
            Free = (decimal)data.ReadDouble();
            Blocked = (decimal)data.ReadDouble();
            Fee = (decimal)data.ReadDouble();  
            return true;

        }

       /* private decimal ConvertAndCompare(string source, decimal old_value, ref bool itChanged)
        {
            var temp = 0;// Utils.ConvertToDecimal(source);
            if (temp != old_value)
                itChanged = true;
            return temp;
        }*/
    }
}
