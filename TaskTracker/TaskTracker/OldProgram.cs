using System;
using System.Collections.Generic;
using System.Linq;
//using TaskTracker.UserInterfaces;
using DataStore.DataManagers;
using DataStore.SQLiteDataManagers;
using System.Reflection;



namespace TaskTracker
{
    internal class OldProgram
    {
        static private bool isRunning = true;
        //static private DataStore.IDataStore dataStore = new SimpleStore();
        //static private IDataStore dataStore = new SQLite3Store();
        static private DataManager<Task> taskManager;
        static private DataManager<User> userManager;

        //static private OldCommandLineInterface ui;
        static private 

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                var controller = new MainController();
                
                

                //var controller = new MainController();



                //ui = new OldCommandLineInterface(new MainController());
                //var menuOptions = GenerateMenuOptions();

                //userManager = SQLiteManagerFactory.GetUserManager();
                //taskManager = SQLiteManagerFactory.GetTaskManager();

                //while (isRunning)
                //{
                //    try
                //    {
                //        ui.DisplayMenuOptions(menuOptions);
                //        var userSelection = ui.GetUserMenuSelection(menuOptions);
                //        userSelection.RunCommand();
                //        //TestCommandLineInteraction(ui, dataStore.GetAllTasks());
                //        //RunTests();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e);
                //    }
                //}
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

        protected static void TestCommandLineInteraction(SolutionInterfaceLibrary.IUserInterface someInterface, IEnumerable<Task> allTasks)
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

            var viewTaskDetailsOption = new Action(() =>
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
            options.Add(new MenuOption(id++, "View Task Details", viewTaskDetailsOption));

            var viewAllUsersOption = new MenuOption(id++, "View All Users", 
                new Action(() =>
                {
                    var users = userManager.GetAll().Select(u => u.Name);
                    ui.DisplayEnumeratedList(users);
                }));
            options.Add(viewAllUsersOption);

            options.Add(new MenuOption(id++, "Add New Task", AddNewTask));

            var deleteTask = new Action(() =>
            {
                int taskId = Int32.Parse(ui.GetUserInput("Select a task id:  "));
                var selectedTask = taskManager.Get(taskId);
                var answer = ui.GetUserInput($"Do you really want to delete {selectedTask.Title}?  Y/N:");
                if (answer == "Y")
                {
                    taskManager.Delete(selectedTask);
                    ui.SendMessageToUser("Task deleted");
                }
                else
                {
                    ui.SendMessageToUser("Canceled...");
                }

            });
            options.Add(new MenuOption(id++, "Delete Task", deleteTask));



            return options;
        }

        delegate void ProcessInputHandler(PropertyInfo prop, string input, Task task);
        static void AddNewTask()
        {
            var GenericPrompt = new Func<string, string>(propName => $"Please enter this task's {propName}");
            var ProcessString = new ProcessInputHandler((prop, input, task) => prop.SetValue(task, input));
            var ProcessUser = new ProcessInputHandler((prop, input, task) =>
            {
                var user = userManager.GetAll().First(u => u.Name == input);
                prop.SetValue(task, user);
            });

            var groups = new List<(string, string, ProcessInputHandler)>();
            groups.Add( ("Title", GenericPrompt("Title"), ProcessString) );
            groups.Add( ("Description", GenericPrompt("Description"), ProcessString) );
            groups.Add( ("AssignedTo", "Please enter the user's name who is assigned to this task.", ProcessUser) );
            groups.Add( ("Source", "Please enter the user's name who is assigning this task.", ProcessUser) );
            groups.Add( ("Notes", "Please enter any additional notes for this task.", ProcessString) );

            var dummyTask = new Task("", "", new User(""), new User(""));

            var back = "BACK";
            var quit = "QUIT";
            Console.WriteLine($"Creating a new task. Enter \"{back}\" at any time to go back a step, or \"{quit}\" to quit.");

            int i = 0;
            while(-1 < i && i < groups.Count)
            {
                (var name, var prompt, var proc) = groups[i];
                var prop = typeof(Task).GetProperty(name);
                var promptForUser = prompt;
                if (promptForUser == "")
                {
                    if(name == "Description")
                    {
                        Console.WriteLine();
                    }
                    promptForUser = $"Enter the task's {name}";
                }
                var input = ui.GetUserInput(promptForUser);
                
                if( input == back ) 
                    { i -= 1; continue; }
                if(input == quit) 
                    { return; }

                try
                {
                    proc(prop, input, dummyTask);
                }
                catch(Exception)
                {
                    ui.SendMessageToUser("Oops!  Something went wrong!");
                }
                i++;
            }
            if (i < 0) { return; }

            var newTask = new Task(dummyTask.Title, dummyTask.Description,
                dummyTask.AssignedTo, dummyTask.Source, notes: dummyTask.Notes);
            taskManager.Insert( newTask);

        }

    }
}