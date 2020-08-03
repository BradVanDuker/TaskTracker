using System.Collections.Generic;

namespace TaskTracker.DataStore
{
    public interface IDataStore
    {
        public IEnumerable<Task> GetAllTasks();

        public Task AddTask(Task task);

        public void DeleteTask(Task task);

        public void DeleteTask(int id);

        public Task GetTask(int id);

        public IEnumerable<User> GetAllUsers();

        public User GetUser(int id);

        public User GetUser(string name);

        public void DeleteUser(User user);

        public User AddUser(User user);

    }
}