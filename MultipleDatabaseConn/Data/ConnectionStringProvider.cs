namespace MultipleDatabaseConn.Data
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private Dictionary<string, string>? _connectionStrings;

        public void SetConnectionStrings(Dictionary<string, string> connectionStrings)
        {
            this._connectionStrings = connectionStrings;
        }

        public string GetConnectionString(string serverName, string databaseName)
        {
            if(this._connectionStrings is null)
            {
                throw new InvalidOperationException("Connection strings are not set");
            }

            if (this._connectionStrings.TryGetValue(serverName, out var connectionString) == false)
            {
                throw new ArgumentException("Unknown database identifier");
            }

            connectionString = $"{connectionString};Database={serverName}";

            return connectionString;
        }
    }
}
