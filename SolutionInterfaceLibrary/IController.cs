using Models;
using System;
using System.Collections.Generic;
using System.Text;
using TaskTracker;

namespace TaskTracker
{
    public delegate void QuitHandler();
    public interface IController
    {
        event EventHandler QuitEventHandler;

        void RaiseQuitEvent(object sender, EventArgs args);

        IEnumerable<Task> GetTasks();

    }
}
