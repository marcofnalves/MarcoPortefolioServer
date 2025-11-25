using System;
using System.IO;
using Microsoft.Data.Sqlite;
using MarcoPortefolioServer.Models.v1;

namespace MarcoPortefolioServer.Functions.v1.lib.server
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            string dbPath = DbPathResolver.GetDatabasePath();
            Console.WriteLine($"📌 DB Path: {dbPath}");

            if (!File.Exists(dbPath))
            {
                Console.WriteLine("➡ A criar ficheiro da base de dados...");
                using (File.Create(dbPath)) { }
            }

            EnsureSchema();
        }

        private static void EnsureSchema()
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS info (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    dateOfBirth TEXT
                );

                CREATE TABLE IF NOT EXISTS tokens (
                    id_token INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    type TEXT NOT NULL,
                    token TEXT NOT NULL,
                    conta TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS versions (
                    id_version INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    description TEXT,
                    version TEXT
                );

                CREATE TABLE IF NOT EXISTS whitelist (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    license TEXT,
                    license2 TEXT,
                    steamid TEXT,
                    discord TEXT,
                    whitelisted INTEGER,
                    banned INTEGER
                );

                CREATE TABLE IF NOT EXISTS language (
                    id_lang INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT
                );

                CREATE TABLE IF NOT EXISTS ide (
                    id_ide INTEGER PRIMARY KEY AUTOINCREMENT,
                    nome TEXT,
                    usage INTEGER
                );

                CREATE TABLE IF NOT EXISTS projects (
                    id_project INTEGER PRIMARY KEY AUTOINCREMENT,
                    id_ide INTEGER REFERENCES ide(id_ide) ON DELETE CASCADE ON UPDATE CASCADE,
                    id_lang INTEGER REFERENCES language(id_lang) ON DELETE CASCADE ON UPDATE CASCADE,
                    name TEXT,
                    icon TEXT,
                    url TEXT
                );

                CREATE TABLE IF NOT EXISTS work_experiences (
                    id_empresa INTEGER PRIMARY KEY AUTOINCREMENT,
                    nome_empresa TEXT,
                    nome_cargo TEXT,
                    descricao TEXT,
                    data_entrada TEXT,
                    data_saida TEXT
                );

                CREATE TABLE IF NOT EXISTS curriculo (
                    nome_empresa TEXT,
                    resumo TEXT,
                    telemovel TEXT,
                    email TEXT
                );

                CREATE TABLE IF NOT EXISTS srv_data (
                    id_data INTEGER PRIMARY KEY AUTOINCREMENT,
                    dkey TEXT,
                    dvalue TEXT
                );

                CREATE TABLE IF NOT EXISTS user_data (
                    id_data INTEGER PRIMARY KEY AUTOINCREMENT,
                    dkey TEXT,
                    dvalue TEXT
                );
            ";
            cmd.ExecuteNonQuery();
        }
    }
}