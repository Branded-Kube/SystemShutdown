using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace SystemShutdown.Database
{
    // Lead author: Lau
    // Contributor: Søren
    public class Repository : IRepository
    {
        #region Fields
        private readonly IDatabaseProvider provider;
        private readonly IMapper mapper;
        private IDbConnection connection;

        private SQLiteDataReader reader;

        public SQLiteDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }
        #endregion

        #region Constructor
        public Repository(IDatabaseProvider provider, IMapper mapper)
        {
            this.provider = provider;
            this.mapper = mapper;
        }
        #endregion

        #region Methods
        private void CreateDatabaseTables()
        {
            var cmd = new SQLiteCommand($"PRAGMA foreign_keys = ON;", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS Mods (ModID INTEGER PRIMARY KEY, Name VARCHAR(50), UNIQUE(Name));", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS Effects (EffectID INTEGER PRIMARY KEY, Effect INTEGER, EffectName VARCHAR(50), ModFK INTEGER REFERENCES Mods(ModID), UNIQUE(EffectName));", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS Highscores (PlayerId INTEGER PRIMARY KEY, PlayerName VARCHAR(50) ,Kills INTEGER, DaysSurvived INTEGER ,UNIQUE(PlayerId));", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }



        public void AddMods(string name)
        {
            var cmd = new SQLiteCommand($"INSERT OR IGNORE INTO Mods (Name) VALUES ('{name}')", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }

        public Mods FindMods(string name)
        {
            var cmd = new SQLiteCommand($"SELECT * from Mods WHERE Name = '{name}'", (SQLiteConnection)connection);
            var reader = cmd.ExecuteReader();

            var result = mapper.MapModsFromReader(reader).First();
            return result;
        }

        public List<Effects> FindEffects(int modfk)
        {
            var cmd = new SQLiteCommand($"SELECT * from Effects WHERE ModFK = '{modfk}'", (SQLiteConnection)connection);
            var reader = cmd.ExecuteReader();

            var result = mapper.MapEffectsFromReader(reader);
            return result;
        }

        public void AddEffects(int effect, string effectname, int modfk)
        {
            var cmd = new SQLiteCommand($"INSERT OR IGNORE INTO Effects (Effect, EffectName, ModFK) VALUES ({effect}, '{effectname}', {modfk})", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }

        //Søren
        public void SaveScore(string name, int kills, int daysSurvived)
        {
            var cmd = new SQLiteCommand($"INSERT OR IGNORE INTO Highscores (PlayerName, Kills, DaysSurvived) VALUES ('{name}', {kills}, {daysSurvived})", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }
        //Søren
        public void ScoreHandler()
        {
            string sql = "SELECT * FROM Highscores ORDER BY Kills DESC";

            SQLiteCommand cmd = new SQLiteCommand(sql, (SQLiteConnection)connection);

            reader = cmd.ExecuteReader();

        }
        public void RemoveTables()
        {
            var cmd = new SQLiteCommand($"DROP TABLE IF EXISTS Effects;", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand($"DROP TABLE IF EXISTS Mods;", (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            Debug.WriteLine("tables dropped.");

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
        #endregion
    }
}