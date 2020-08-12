using DataStore.SQLiteDataManagers;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

using TaskTracker;

namespace Controllers
{
    
    public class MainController : IController
    {
        

        bool isRunning = true;

        public event EventHandler QuitEventHandler;
        readonly private TaskManager taskManager;
        readonly private UserManager userManager;

        public MainController(TaskManager taskManager, UserManager userManager)
        {
            this.taskManager = taskManager;
            this.userManager = userManager;
            QuitEventHandler += OnQuit; 
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

        public void AddTask(FauxTask task)
        {

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

        public void SendRequestToController()
        {
            throw new NotImplementedException();
        }


        // This listens for QuitEvent then does something in response.
        public void OnQuit(object sender, EventArgs e)
        {
            isRunning = false;
        }

        // When you want to raise a QuitEvent, call this method
        public void RaiseQuitEvent(object sender, EventArgs args)
        {
            QuitEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
