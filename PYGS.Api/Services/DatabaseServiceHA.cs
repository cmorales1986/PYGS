using System.Data.Odbc;
using System.Data;

namespace PYGS.Api.Services
{
    public class DatabaseServiceHA
    {
        private readonly string _connectionString;

        public DatabaseServiceHA(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyHanaConnection");
        }

        public async Task<List<Dictionary<string, object>>> GetDataAsync(string query, CancellationToken cancellationToken = default)
        {
            using (var connection = new OdbcConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new OdbcCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        var results = new List<Dictionary<string, object>>();
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            var row = new Dictionary<string, object>();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            results.Add(row);
                        }
                        return results;
                    }
                }
            }
        }


    }
}
