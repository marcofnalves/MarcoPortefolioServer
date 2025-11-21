using Microsoft.Data.Sqlite;

namespace MarcoPortefolioServer.Models.v1
{
    public static class SQLLiteConnectionModel
    {
        public static SqliteConnection GetConnection()
        {
            string dbPath = DbPathResolver.GetDatabasePath();
            return new SqliteConnection($"Data Source={dbPath}");
        }
    }
}