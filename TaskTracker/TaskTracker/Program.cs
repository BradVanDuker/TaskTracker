using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaskTracker.UserInterfaces;

namespace TaskTracker
{
    class Program
    {
        static private bool isRunning = true;
        static void Main(string[] args)
        {
            try
            {
                while (isRunning)
                {
                    //RunTests();
                    Console.WriteLine("Hello World!");

                    var ui = new CommandLineInterface();
                    ui.DisplayMenu(GenerateMenuOptions());
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void RunTests()
        {
            // Test for auto Id
            Task a = new Task();
            Task b = new Task();
            bool isUniqueIdOk = true;
            isUniqueIdOk = isUniqueIdOk && a.Id == 0;
            isUniqueIdOk = isUniqueIdOk && b.Id == a.Id + 1;
            string resultString = isUniqueIdOk ? "Pass" : "Fail";
            Console.WriteLine($"Unique Id test:  {resultString}");

            Console.WriteLine("\nTests Complete\n");
            isRunning = false;
            
        }

        List<Task> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        static List<CommandOption> GenerateMenuOptions()
        {
            List<CommandOption> commandList = new List<CommandOption>();
            commandList.Add(new CommandOption("Quit",
                            new Action(() => isRunning = false))
            );

            return commandList;
        }
    }

    class CommandOption
    {
        public string name { get; }
        public Action action { get; }

        public CommandOption(string name, Action action)
        {
            this.name = name;
            this.action = action;
        }
    }
}
