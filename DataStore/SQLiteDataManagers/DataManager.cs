using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using DataStore.misc;
using System.Data;
using System.Reflection;

namespace DataStore.SQLiteDataManagers
{
    public abstract class DataManager<T>
    {
        public abstract IEnumerable<T> GetAll();

        public abstract T Get(int id);

        public abstract int Insert(T thing);

        public abstract void Delete(T thing);

        public abstract void Update(T thing);

        public abstract void Update(object taskId, PropertyInfo property, object newValue);



        protected SqliteConnection GetConnection()
        {
            return DbUtilityHelper.GetConnection();
        }

        protected SqliteCommand GetCommand(string sql, SqliteConnection connection)
        {
            var cnn = connection.CreateCommand();
            cnn.CommandText = sql;
            cnn.CommandType = CommandType.Text;
            return cnn;
        }

        protected void ExecuteNonQuery(string sql, params SqliteParameter[] args)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = GetCommand(sql, connection))
                {
                    command.Parameters.AddRange(args);
                    command.ExecuteNonQuery();
                }
            }
        }

        protected int InsertAndGetId(string insertSQL, params SqliteParameter[] args)
        {
            int id;
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = GetCommand(insertSQL, connection))
                    {
                        command.Parameters.AddRange(args);
                        command.ExecuteNonQuery();
                    }
                    var idSql = "SELECT last_insert_rowid();)";
                    using (var scaler = GetCommand(idSql, connection))
                    {
                        id = Int32.Parse(scaler.ExecuteScalar().ToString());
                    }
                    transaction.Commit();
                }
            }
            return id;
        }
    }
}
