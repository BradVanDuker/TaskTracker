using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Controllers;
using DataStore.SQLiteDataManagers;
using UserInterfaces;

namespace TaskTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello, World!");
                var userManager = new UserManager();
                var taskManager = new TaskManager(userManager);

                var controller = new MainController(taskManager, userManager);
                var ui = new CommandLineInterface(controller);
                ui.Run();
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
