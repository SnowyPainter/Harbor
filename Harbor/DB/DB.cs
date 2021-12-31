using Microsoft.Data.Sqlite;
using System;

namespace Harbor.DB
{
    public class DB
    {
        public static string ConnectionString = "";
        public SqliteConnection Connection
        {
            get {
                return new SqliteConnection(ConnectionString);
            }
        }
        public SqliteCommand Command
        {
            get
            {
                var conn = Connection;
                conn.Open();
                return Connection.CreateCommand();
            }
        }
    }
}
