using CsvHelper.Configuration;
using POCOs;

namespace BusinessLayer
{
    public class MeterReadingCsvMap : ClassMap<MeterReading>
    {
        public MeterReadingCsvMap()
        {
            Map(m => m.AccountId);
            Map(m => m.MeterReadingDateTime).TypeConverterOption.Format("dd/MM/yyyy HH:mm");
            Map(m => m.MeterReadValue);
        }
    }
}
