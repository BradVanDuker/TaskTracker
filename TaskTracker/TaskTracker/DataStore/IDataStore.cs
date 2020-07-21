using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.DataStore
{
    interface IDataStore : System.Collections.IList
    {
        public void StoreTask(Task task);

        public IList<Task> GetAllTasks();

    }
}
