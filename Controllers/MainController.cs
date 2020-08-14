using DataStore.SQLiteDataManagers;
using Models;
using SolutionInterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using TaskTracker;

namespace Controllers
{
    
    public class MainController : IController
    {
        

        bool isRunning = true;

        //public event EventHandler QuitEventHandler;
        readonly private TaskManager taskManager;
        readonly private UserManager userManager;
        readonly private EventHub eventHub;
        

        public MainController(TaskManager taskManager, UserManager userManager, EventHub eventHub)
        {
            this.taskManager = taskManager;
            this.userManager = userManager;
            this.eventHub = eventHub;
            this.eventHub.QuitEvent += QuitEventResponse;
        }

        public void Run()
        {
            while (isRunning)
            {

            }
        }

        public void AddTask(Task task)
        {
            taskManager.Insert(task);
        }

        public void DeleteTask(int id)
        {
            var task = taskManager.Get(id);
            taskManager.Delete(task);
        }


        public IEnumerable<Task> GetTasks()
        {
            return taskManager.GetAll();
        }

        public IEnumerable<User> GetUsers()
        {
            return userManager.GetAll();
        }

        public Task GetTask(int id)
        {
            return GetTasks().First(t => t.Id == id);
        }


        // This listens for QuitEvent then does something in response.
        public void QuitEventResponse(object sender, EventArgs e)
        {
            isRunning = false;
        }

        public void Quit()
        {
            eventHub.QuitEvent(this, EventArgs.Empty);
        }

        public void UpdateTask(int id, string propertyName, object newValue)
        {
            var prop = typeof(Task).GetProperty(propertyName);
            taskManager.Update(id, prop, newValue);
            //var task = GetTask(id);
            //prop.SetValue(task, newValue);
            //taskManager.Update(task);
        }
    }

}
