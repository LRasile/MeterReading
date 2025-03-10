using CsvHelper;
using DataAccessLayer;
using POCOs;
using System.Globalization;

namespace BusinessLayer
{
    public interface IMeterReadingService
    {
        Task<UploadResult> Upload(Stream stream);
    }

    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRespository;
        private readonly IAccountRepository _accountRepository;

        public MeterReadingService(IMeterReadingRepository meterReadingRespository, IAccountRepository accountRepository)
        {
            _meterReadingRespository = meterReadingRespository;
            _accountRepository = accountRepository;
        }

        public async Task<UploadResult> Upload(Stream stream)
        {
            using(var csvReader = new CsvReader(new StreamReader(stream), CultureInfo.InvariantCulture))
            {
                csvReader.Context.RegisterClassMap<MeterReadingCsvMap>();
                var readings = csvReader.GetRecords<MeterReading>().ToList();                 
                var totalReadings = readings.Count();

                var readingsWithAccounts = await GetReadingsWithValidAccounts(readings);
                var readingWithNoDuplicates = RemoveDuplicateReadings(readingsWithAccounts);
                var readingWithValidMeterReadValues = ExcludeInvalidMeterReadValues(readingWithNoDuplicates);

                int successfulRecords = await _meterReadingRespository.SaveReadings(readingWithValidMeterReadValues);
                int failedRecords = totalReadings - successfulRecords;

                return new UploadResult(successfulRecords, failedRecords);
            }   
        }

        private async Task<IEnumerable<MeterReading>> GetReadingsWithValidAccounts(IEnumerable<MeterReading> readings)
        {
            var accounts = await _accountRepository.GetAccounts();
            var accountIds = accounts.Select(a => a.AccountId);
            return readings.Where(r => accountIds.Contains(r.AccountId));
        }

        private IEnumerable<MeterReading> RemoveDuplicateReadings(IEnumerable<MeterReading> readings)
        {
            var dictionary = new Dictionary<(long, DateTime), MeterReading>();
            foreach (var reading in readings)
            {
                var key = (reading.AccountId, reading.MeterReadingDateTime);
                if (!dictionary.ContainsKey(key) || dictionary[key].MeterReadValue < reading.MeterReadValue)
                {
                    dictionary[key] = reading;
                }
            }

            return dictionary.Values;
        }

        private IEnumerable<MeterReading> ExcludeInvalidMeterReadValues(IEnumerable<MeterReading> readings)
        { 
            return readings.Where(r => r.MeterReadValue >= 0 && r.MeterReadValue <= 99999);
        }
    }
}
