using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{

    public interface IDbConnectionFactory
    {
        SqlConnection CreateSqlExpressConnection();
    }

    public class SqlConnectionFactory : IDbConnectionFactory, IDisposable
    {

        private readonly IConfiguration _configuration;
        private IDictionary<string, SqlConnection> _connectionDictionary;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionDictionary = new Dictionary<string, SqlConnection>();
        }


        public SqlConnection CreateSqlExpressConnection()
        {
            return CreateConnection("SqlExpress");
        }

        private SqlConnection CreateConnection(string key)
        {
            var connectionString = _configuration.GetConnectionString(key);

            if (!_connectionDictionary.ContainsKey(key))
            {                
                var connection = new SqlConnection { ConnectionString = connectionString };
                connection.Open();
                _connectionDictionary.Add(key, connection);
            }

            return _connectionDictionary[key];
        }

        public void Dispose()
        {
            foreach (var item in _connectionDictionary)
            {
                System.Diagnostics.Debug.WriteLine("Disposing: " + item.Key);
                item.Value.Close();
                item.Value.Dispose();
            }
            _connectionDictionary = new Dictionary<string, SqlConnection>();
        }
    }
}
