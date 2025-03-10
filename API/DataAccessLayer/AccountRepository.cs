using Microsoft.Data.SqlClient;
using POCOs;
using System.Data;

namespace DataAccessLayer
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts();
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AccountRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            var connection = _dbConnectionFactory.CreateSqlExpressConnection();

            var accounts = new List<Account>();

            using (var command = new SqlCommand("GetAllAccounts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        accounts.Add(new Account
                        {
                            AccountId = reader.GetInt64(reader.GetOrdinal("AccountId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName"))
                        });
                    }
                }
            }

            return accounts;
        }
    }
}
