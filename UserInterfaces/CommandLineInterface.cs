using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker;
using TaskTracker.UserInterfaces;
using Models;

namespace UserInterfaces
{
    public class CommandLineInterface : IUserInterface
    {
        readonly IController controller;
        private bool isRunning = true;

        public CommandLineInterface(IController controller)
        {
            this.controller = controller;
            controller.QuitEventHandler += QuitEventResponse;
        }

        public bool SendRequestToInterface()
        {
            throw new NotImplementedException();
        }


        public void Run()
        {
            while(isRunning)
            {
                MainMenu();
            }
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

        protected void EnumerateAndDisplayItems()
        {

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

        delegate T CastTo<T>(string str);
        protected void AddTask()
        {
            var BACK = "BACK";
            var QUIT = "QUIT";

            Console.WriteLine($"Type {BACK} to go back a step or {QUIT} to go back to the main menu.");
            int index = 0;

            CastTo<string> handleString = str => str;
            CastTo<DateTime> handleDT = str => DateTime.Parse(str);
            //var CastToInt = new Func<string, int>(str => )

            var props = typeof(Task).GetProperties().ToList();
            var fauxTask = new FauxTask();

            //fauxTask.Title = handleString("foo");
            //var foo = 
            while(-1 < index && index < props.Count)
            {
                var prop = props[index];
                var response = GetResponseFromUser($"Enter the task's {prop.Name}...");
                var propType = prop.PropertyType;
            }
        }

        protected void DeleteTask()
        {
            throw new NotImplementedException();
        }

        protected string GetResponseFromUser(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        protected void OnQuit()
        {
            controller.RaiseQuitEvent(this, EventArgs.Empty);
        }

        protected void QuitEventResponse(object sender, EventArgs args)
        {
            isRunning = false;
        }

        static List<MenuOption> menuOptions = new List<MenuOption>();
        protected IEnumerable<MenuOption> GetMainMenuOptions()
        {
            if (menuOptions.Count == 0)
            {
                menuOptions.Add(new MenuOption("Quit", OnQuit));
                menuOptions.Add(new MenuOption("Display All Tasks", DisplayAllTasks));
                menuOptions.Add(new MenuOption("Display Task Detais", DisplayTaskDetails));
                //menuOptions.Add()
            }
            
            return menuOptions;
        }

        private IEnumerable<Task> GetAllTasks()
        {
            return controller.GetTasks();
        }

        private Task GetTask(int id)
        {
            return GetAllTasks().First(t => t.Id == id);
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
}
