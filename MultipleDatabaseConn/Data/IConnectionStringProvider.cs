namespace MultipleDatabaseConn.Data
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string serverName, string databaseName);

        void SetConnectionStrings(Dictionary<string, string> connectionStrings);
    }
}
