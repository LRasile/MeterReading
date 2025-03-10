using Microsoft.Data.SqlClient;
using POCOs;
using System.Data;

namespace DataAccessLayer
{
    public interface IMeterReadingRepository
    {
        Task<int> SaveReadings(IEnumerable<MeterReading> readings);
    }

    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MeterReadingRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> SaveReadings(IEnumerable<MeterReading> readings)
        {
            SqlConnection connection = _dbConnectionFactory.CreateSqlExpressConnection();

            using (var command = new SqlCommand("InsertMeterReadings", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                var table = new DataTable();
                table.Columns.Add("AccountId", typeof(long));
                table.Columns.Add("MeterReadingDateTime", typeof(DateTime));
                table.Columns.Add("MeterReadValue", typeof(string));

                foreach (var reading in readings)
                {
                    table.Rows.Add(reading.AccountId, reading.MeterReadingDateTime, reading.FormatterMeterReadValue);
                }

                var parameter = command.Parameters.AddWithValue("@MeterReadings", table);
                parameter.SqlDbType = SqlDbType.Structured;

                var insertedCount = (int)(await command.ExecuteScalarAsync())!;
                return insertedCount;
            }
        }
    }
}
