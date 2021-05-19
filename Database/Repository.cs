using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SystemShutdown.Database
{
    public class Repository : IRepository
    {
        private readonly IDatabaseProvider provider;
        private readonly IMapper mapper;
        private IDbConnection connection;

        public Repository(IDatabaseProvider provider, IMapper mapper)
        {
            this.provider = provider;
            this.mapper = mapper;
        }

        private void CreateDatabaseTables()
        {
            var cmd = new SQLiteCommand($"PRAGMA foreign_keys = ON;", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS Player (PlayerID INTEGER PRIMARY KEY, Scrap INTEGER, UNIQUE(PlayerId));", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS Mods (ModID INTEGER PRIMARY KEY, Name VARCHAR(50), Effect INTEGER, UNIQUE(Name);", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }

        public Mods FindMods(string name)
        {
            var cmd = new SQLiteCommand($"SELECT * from Mods WHERE name = '{name}'", (SQLiteConnection)connection);
            var reader = cmd.ExecuteReader();

            var result = mapper.MapModsFromReader(reader).First();
            return result;
        }

        public void AddMods(string name, int effect)
        {
            var cmd = new SQLiteCommand($"INSERT OR IGNORE INTO Fish (Name,Effect) VALUES ('{name}',{effect})", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }

        public void Open()
        {
            if (connection == null)
            {
                connection = provider.CreateConnection();
            }
            connection.Open();

            CreateDatabaseTables();
        }

        public void Close()
        {
            connection.Close();
        }
    }
}