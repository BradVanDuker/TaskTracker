using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Data;
using TaskTracker;
using System.IO;
using DataStore.DataManagers;

namespace Scratchpaper
{

    internal class Program
    {
        
        private static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            Console.WriteLine(directory);
            var dbName = "TestDB.db";
            var path = Path.Join(directory, dbName);
            var connStrBuilder = new SqliteConnectionStringBuilder();
            connStrBuilder.DataSource = path;
            SqliteConnection connection = new SqliteConnection(connStrBuilder.ToString());

            TryReflectionConverter.Go(connection);
        }
    }
}