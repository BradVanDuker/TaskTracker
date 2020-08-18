using DataStore.SQLiteDataManagers;
using DataStore.misc;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskTracker;

namespace DataStore.SQLiteDataManagers
{
    abstract public class SQLiteManagerFactory
    {
        private static UserManager userManager = null;
        private static TaskManager taskManager = null;
        
        private static SqliteConnection GetConnection()
        {
            return DbUtilityHelper.GetConnection();
        }

        public static UserManager GetUserManager()
        {
            if(userManager == null)
            {
                var connection = GetConnection();
                userManager = new UserManager();
            }
            return userManager;
        }

        public static TaskManager GetTaskManager()
        {
            if(taskManager == null)
            {
                var um = GetUserManager();
                taskManager = new TaskManager(um);
            }
            return taskManager;
        }
    }
}
