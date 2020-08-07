using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker.UserInterfaces;
using DataStore.DataManagers;
using DataStore.SQLiteDataManagers;


namespace TaskTracker
{
    internal class Program
    {
        static private bool isRunning = true;
        //static private DataStore.IDataStore dataStore = new SimpleStore();
        //static private IDataStore dataStore = new SQLite3Store();
        static private DataManager<Task> taskManager;
        static private DataManager<User> userManager;

        static private CommandLineInterface ui;

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                ui = new CommandLineInterface();
                var menuOptions = GenerateMenuOptions();

                userManager = SQLiteManagerFactory.GetUserManager();
                taskManager = SQLiteManagerFactory.GetTaskManager();

                while (isRunning)
                {
                    try
                    {
                        ui.DisplayMenuOptions(menuOptions);
                        var userSelection = ui.GetUserMenuSelection(menuOptions);
                        userSelection.RunCommand();
                        //TestCommandLineInteraction(ui, dataStore.GetAllTasks());
                        //RunTests();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //protected private void RunTests()
        //{
        //    // Test for auto Id
        //    Task a = new Task();
        //    Task b = new Task();
        //    bool isUniqueIdOk = true;
        //    isUniqueIdOk = isUniqueIdOk && a.Id == 0;
        //    isUniqueIdOk = isUniqueIdOk && b.Id == a.Id + 1;
        //    string resultString = isUniqueIdOk ? "Pass" : "Fail";
        //    Console.WriteLine($"Unique Id test:  {resultString}");

        //    Console.WriteLine("\nTests Complete\n");
        //    isRunning = false;
        //}

        protected static IEnumerable<Task> GetAllTasks()
        {
            return taskManager.GetAll();
            //throw new NotImplementedException();
        }

        protected static Task GetTask(int id)
        {
            return taskManager.Get(id);
        }

        protected static void DeleteTask(Task task)
        {
            taskManager.Delete(task);
        }

        protected static void Quit()
        {
            isRunning = false;
        }

        protected static void TestCommandLineInteraction(CommandLineInterface someInterface, IEnumerable<Task> allTasks)
        {
            var options = GenerateMenuOptions();

            someInterface.DisplayMenuOptions(options);
            Console.WriteLine();

            someInterface.DisplayTasks(allTasks);
            Console.WriteLine();

            var interfaceTask = allTasks.First(x => x.Title == "Shell Interface");

            someInterface.DisplayTaskDetails(interfaceTask);
            Console.WriteLine();

            Quit();
        }


        protected static IEnumerable<MenuOption> GenerateMenuOptions()
        {
            var options = new List<MenuOption>();
            int id = 0;

            var quitOption = new MenuOption(id++, "Quit", new Action(() => isRunning = false));
            options.Add(quitOption);

            var viewAllTasksOption = new MenuOption(id++, "View All Tasks", new Action(() => ui.DisplayTasks(taskManager.GetAll())));
            options.Add(viewAllTasksOption);

            var viewTaskDetails = new Action(() =>
            {
                try
                {
                    int taskId = Int32.Parse(ui.GetUserInput("Select a task id:  "));
                    Task selectedTask = taskManager.GetAll().First(task => task.Id == taskId);
                    ui.DisplayTaskDetails(selectedTask);
                }
                catch
                {
                    ui.SendMessageToUser("Invalid Task id.");
                }
            });
            options.Add(new MenuOption(id++, "View Task Details", viewTaskDetails));

            return options;
        }

    }
}