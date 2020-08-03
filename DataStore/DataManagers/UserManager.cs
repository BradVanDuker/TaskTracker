﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using DataStore.misc;
using System.Data;
using TaskTracker;
using System.Linq;

namespace DataStore.DataManagers
{
    public class UserManager : DataManager<User>
    {
        readonly SqliteConnection connection;

        public UserManager(SqliteConnection connection)
        {
            Console.WriteLine(connection.DataSource.ToString());
            this.connection = connection;
        }

        override public IEnumerable<User> GetAll()
        {
            using (var connection = GetConnection())
            {
                string sql = "SELECT Id, UserName FROM User;";

                connection.Open();
                var allUsers = new List<User>();
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        allUsers.Add(new User(
                            reader.GetFieldValue<string>("UserName"),
                            reader.GetFieldValue<int>("Id")));
                    }
                }
                return allUsers;
            }
        }

        // Note to self:  "Premature optimzation is the root of all evil." -- Donald Knuth
        override public User Get(int id)
        {
            return GetAll().First(u => u.Id == id);
        }

        public User Get(string name)
        {
            return GetAll().First(u => u.Name == name);
        }

        override public int Insert(User user)
        {
            var sql = "INSERT INTO User (UserName) VALUES(@userName);";
            var param = new SqliteParameter("userName", user.Name);
            return InsertAndGetId(sql, param);
        }

        override public void Delete(User user)
        {
            var sql = "DELETE FROM User WHERE Id == @id AND UserName == @name";

            var nameParam = new SqliteParameter("@name", user.Name);
            var idParam = new SqliteParameter("@id", user.Id);
            ExecuteNonQuery(sql, nameParam, idParam);
            //var myParams = new SqliteParameter[] { nameParam, idParam };
            //ExecuteNonQuery(sql, myParams);
        }

        public override void Update(User user)
        {
            var sql = "UPDATE User SET UserName = @name WHERE Id = @id";
            var nameParam = new SqliteParameter("name", user.Name);
            var idParam = new SqliteParameter("id", user.Id);
            ExecuteNonQuery(sql, nameParam, idParam);
        }
    }
}
