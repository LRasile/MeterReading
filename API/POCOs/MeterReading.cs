using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCOs
{
    public class MeterReading
    {
        public long AccountId { get; set; }
        public DateTime  MeterReadingDateTime { get; set; }
        public int  MeterReadValue { get; set; }
        public string  FormatterMeterReadValue => MeterReadValue.ToString("D5");
    }
}
