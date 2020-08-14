using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controllers;
using DataStore.SQLiteDataManagers;
using SolutionInterfaceLibrary;
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
                Run();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        async static void Run()
        {
            var userManager = new UserManager();
            var taskManager = new TaskManager(userManager);
            var eventHub = EventHub.GetInstance();
            var controller = new MainController(taskManager, userManager, eventHub);
            var ui = new CommandLineInterface(controller, eventHub);

            ui.Run();
        }
    }
}
