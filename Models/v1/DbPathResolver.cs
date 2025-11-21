using System;
using System.IO;

namespace MarcoPortefolioServer.Models.v1
{
    public static class DbPathResolver
    {
        public static string GetDatabasePath()
        {
            string baseDir = AppContext.BaseDirectory;

            string folder = Path.Combine(baseDir, "Repository", "v1");
            Directory.CreateDirectory(folder);

            string dbPath = Path.Combine(folder, "MarcoPortefolioServer.db");
            return dbPath;
        }
    }
}