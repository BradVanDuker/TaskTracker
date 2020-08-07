﻿using DataStore.misc;
using System;
using System.Linq;
using TaskTracker;
using TaskTracker.DataStore;

namespace DataStore
{
    using TT = TaskTracker;
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                var dataStore = new SQLite3Store();
                //PostCreation.InitializeDb();
                Console.WriteLine("Done!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void QuickTestGetAllTasks(IDataStore dataStore)
        {
            var tasks = dataStore.GetAllTasks();
            Console.WriteLine(tasks.Count());
        }

        private static void QuickTestGetUsers(IDataStore dataStore)
        {
            var users = dataStore.GetAllUsers();
            Console.WriteLine($"Length = {users.Count()}");
            var me = dataStore.GetUser(1);
            Console.WriteLine(me.Name);
            me = dataStore.GetUser("Brad");
            Console.WriteLine(me.Name);
        }

        private static void QuickTestAddUser(IDataStore dataStore)
        {
            var user = new User("Fred");
            var newUser = dataStore.AddUser(user);
            Console.WriteLine($"{newUser.Name}  {newUser.Id}");
        }

        private static void QuickTestDeleteUser(IDataStore dataStore)
        {
            var fred = dataStore.GetUser("Fred");
            dataStore.DeleteUser(fred);
            try
            {
                var notFred =  dataStore.GetUser("Fred");
            }
            catch (Exception)
            {
                Console.WriteLine("Deleted Fred");
            }
        }
    }
}