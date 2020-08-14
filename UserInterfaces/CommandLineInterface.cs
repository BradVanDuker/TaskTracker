using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker;
using TaskTracker.UserInterfaces;
using Models;
using System.Collections;
using SolutionInterfaceLibrary;
using System.Reflection;

namespace UserInterfaces
{
    public class CommandLineInterface : IUserInterface
    {
        readonly IController controller;
        readonly EventHub eventHub;
        private bool isRunning = true;
        static List<MenuOption> menuOptions = new List<MenuOption>();

        #region General Functions
        public CommandLineInterface(IController controller, EventHub hub)
        {
            this.controller = controller;
            this.eventHub = hub;
            eventHub.QuitEvent += QuitEventResponse;
        }


        public void Run()
        {
            while(isRunning)
            {
                MainMenu();
            }
        }

        protected string GetResponseFromUser(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        protected void OnQuit()
        {
            eventHub.QuitEvent(this, EventArgs.Empty);
        }

        protected void QuitEventResponse(object sender, EventArgs args)
        {
            isRunning = false;
        }

        private IEnumerable<Task> GetAllTasks()
        {
            return controller.GetTasks();
        }

        private Task GetTask(int id)
        {
            return GetAllTasks().First(t => t.Id == id);
        }

        #endregion

        #region Menu Options
        protected IEnumerable<MenuOption> GetMainMenuOptions()
        {
            static void AddNewMenuOption(string prompt, Action action)
            {
                menuOptions.Add(new MenuOption(prompt, action));
            }

            if (menuOptions.Count == 0)
            {
                AddNewMenuOption("Quit", OnQuit);
                AddNewMenuOption("Display All Tasks", DisplayAllTasks);
                AddNewMenuOption("Display Task Detais", DisplayTaskDetails);
                AddNewMenuOption("Add New Task", AddNewTask);
                AddNewMenuOption("Delete Task", DeleteTask);
                AddNewMenuOption("Update a Task", UpdateTask);
            }

            return menuOptions;
        }

        protected void MainMenu()
        {
            Console.WriteLine("\n** Main Menu **");

            var options = GetMainMenuOptions();
            foreach(var option in options)
            {
                Console.WriteLine($"{option.Id}:  {option.Text}");
            }
            string response = GetResponseFromUser("Enter an option number...");
            int choiceNumber = -1;
            try
            {
                choiceNumber = Int32.Parse(response);
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid choice.");
                return;
            }
            if (choiceNumber == -1)
            {
                Console.WriteLine("Invalid choice");
                return;
            }

            try
            {
                var choice = options.First(o => o.Id == choiceNumber);
                choice.Do();
            }
            catch(InvalidOperationException)
            {
                Console.WriteLine("Invalid Selection");
            }
        }

        protected void DisplayTaskDetails()
        {
            var response = GetResponseFromUser("Enter the id of a task...");
            Task task;
            try
            {
                var id = Int32.Parse(response);
                task = GetTask(id);
            }
            catch(Exception)
            {
                Console.WriteLine("An invalid id was entered.");
                return;
            }
            var props = typeof(Task).GetProperties();
            foreach( var p in props)
            {
                Console.WriteLine($"{p.Name}:  {p.GetValue(task) ?? "None"}");
            }
        }

        protected void DisplayAllTasks()
        {
            var tasks = controller.GetTasks();
            tasks = tasks.OrderBy(t => t.Id);
            foreach(var task in tasks)
            {
                Console.WriteLine($"{task.Id}:  {task.Title}");
            }
        }

        protected delegate void ProcessInputHandler(PropertyInfo prop, string input, Task task);
        void AddNewTask()
        {
            var GenericPrompt = new Func<string, string>(propName => $"Please enter this task's {propName}");
            var ProcessString = new ProcessInputHandler((prop, input, task) => prop.SetValue(task, input));
            //var ProcessDate = new ProcessInputHandler((prop, input, task) => prop.SetValue(task, DateTime.Parse(input)));
            var ProcessUser = new ProcessInputHandler((prop, input, task) =>
            {
                var user = controller.GetUsers().First(u => u.Name == input);
                prop.SetValue(task, user);
            });

            var groups = new List<(string, string, ProcessInputHandler)>();
            groups.Add(("Title", GenericPrompt("Title"), ProcessString));
            groups.Add(("Description", GenericPrompt("Description"), ProcessString));
            groups.Add(("AssignedTo", "Please enter the user's name who is assigned to this task.", ProcessUser));
            groups.Add(("Source", "Please enter the user's name who is assigning this task.", ProcessUser));
            groups.Add(("Notes", "Please enter any additional notes for this task.", ProcessString));

            var dummyTask = new Task("", "", new User(""), new User(""));

            var back = "BACK";
            var quit = "QUIT";
            Console.WriteLine($"Creating a new task. Enter \"{back}\" at any time to go back a step, or \"{quit}\" to quit.");

            int i = 0;
            while (-1 < i && i < groups.Count)
            {
                (var name, var prompt, var proc) = groups[i];
                var prop = typeof(Task).GetProperty(name);
                var promptForUser = prompt;
                if (promptForUser == "")
                {
                    promptForUser = $"Enter the task's {name}";
                }
                var input = GetResponseFromUser(promptForUser);

                if (input == back)
                { i -= 1; continue; }
                if (input == quit)
                { return; }

                try
                {
                    proc(prop, input, dummyTask);
                }
                catch (Exception)
                {
                    GetResponseFromUser("Oops!  Something went wrong!");
                }

                i++;
            }

            if (i < 0) { return; }

            controller.AddTask(new Task(dummyTask.Title, dummyTask.Description,
                dummyTask.AssignedTo, dummyTask.Source, notes: dummyTask.Notes));
        }

        protected void DeleteTask()
        {
            var response = GetResponseFromUser("Enter the id of the task to delete");
            int id;
            try
            {
                id = Int32.Parse(response);
                controller.DeleteTask(id);
                Console.WriteLine($"Task {id} deleted");
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid id");
                return;
            }
        }

        protected void  UpdateTask()
        {
            var idResponse = GetResponseFromUser("Enter the id of the task to update");
            Task task;
            try
            {
                task = GetTask(Int32.Parse(idResponse)); 
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid Id");
                return;
            }
            var propResponse = GetResponseFromUser("Enter the name of the property to change");
            PropertyInfo prop;
            try
            {
                prop = typeof(Task).GetProperty(propResponse);
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid property name.");
                return;
            }
            var valueResponse = GetResponseFromUser("Enter the new value");
            object newValue = null;
            try
            {
                Type newType = prop.PropertyType;
                if(prop.PropertyType == typeof(User))
                {
                    newValue = controller.GetUsers().First(u => u.Name == valueResponse);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Could not parse your entry");
                return;
            }
            try
            {
                controller.UpdateTask(task.Id, prop.Name, newValue);
            }
            catch(Exception)
            {
                Console.WriteLine("Update didn't go through");
                return;
            }
            
        }

    }


    public class MenuOption
    {
        private static int currentId = 0;

        private int id;
        public int Id {
            get { return id; }
        }

        public string Text { get; set; }

        public Action Do { get; set; }

        override public string ToString()
        {
            return Text;
        }

        public MenuOption()
        {
            MakeId();
        }

        public MenuOption(string text, Action action)
        {
            Text = text;
            Do = action;
            MakeId();
        }   

        private void MakeId()
        {
            id = currentId++;
        }
    }
    #endregion
}
