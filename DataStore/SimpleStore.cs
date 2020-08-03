using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskTracker.DataStore
{
    public class SimpleStore : IDataStore
    {
        private IList<Task> Tasks;

        private IList<User> Users;

        public Task AddTask(Task task)
        {
            throw new NotImplementedException();
            this.Tasks.Add(task);
            return task;
        }

        public void DeleteTask(Task task)
        {
            this.Tasks.Remove(task);
        }

        public void DeleteTask(int id)
        {
            Task task = this.GetTask(id);
            this.DeleteTask(task);
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return this.Tasks;
        }

        public Task GetTask(int id)
        {
            var values = this.Tasks.Where(x => x.Id == id);
            var iterator = values.GetEnumerator();
            var hasSomething = iterator.MoveNext();
            if (hasSomething)
            {
                return iterator.Current;
            }
            else
            {
                throw new Exception("Task id not found.");
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return this.Users;
        }

        public User GetUser(int id)
        {
            return this.Users.First(user => user.Id == id);
        }

        public User GetUser(string name)
        {
            return this.Users.First(user => user.Name == name);
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public User AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public SimpleStore()
        {
            this.Users = new List<User>();

            User me = new User("Brad", 1);

            this.Users.Add(me);
            this.Tasks = new List<Task>();

            DateTime July16 = new DateTime(2020, 7, 16);
            DateTime July17 = new DateTime(2020, 7, 17);
            DateTime July21 = new DateTime(2020, 7, 21);
            DateTime July22 = new DateTime(2020, 7, 22);
            DateTime July23 = new DateTime(2020, 7, 23);

            Tasks.Add(new Task(
                title: "Basic Controler",
                description: "Create a simple super controller to tie everyting together",
                assignedTo: me,
                source: me,
                dateCreated: July17)
            {
                DateAssigned = July17
            }
            );

            Tasks.Add(new Task(

                title: "Invalid Menu Options",
                description: "Handle errors for invalid inputs for the command line menu",
                assignedTo: me,
                source: me,
                dateCreated: July17)
            {
                DateAssigned = July17
            }
            );

            Tasks.Add(new Task(

                title: "Testing",
                description: "Learn to use unit testing features of VS.",
                assignedTo: me,
                source: me,
                dateCreated: July21)
            {
                DateAssigned = July21
            }
            );

            Tasks.Add(new Task(

                title: "Command Objects",
                description: "The menu needs to handle user inputs with 0+ parameters and various return types",
                assignedTo: me,
                source: me,
                dateCreated: July21)
            {
                DateAssigned = July21
            }
            );

            Tasks.Add(new Task(

                title: "Utilities lib",
                description: "Add more utility functions to my library",
                assignedTo: me,
                source: me,
                dateCreated: July21)
            {
                DateAssigned = July21
            }
            );

            var interfaceAbstractionTask = new Task(

                title: "Abstract away commands from the interface.",
                description: "Implement some sort of command o-o pattern",
                assignedTo: me,
                source: me,
                dateCreated: July21)
            {
                DateAssigned = July21,
                DateCompleted = July23
            };
            Tasks.Add(interfaceAbstractionTask);

            var asynchInterfaceTask = new Task(

                title: "Asynch Interface",
                description: "The interface should run on a seperate thread than the program.  " +
                              "This will be good practice for asyn programming and events.  It's probably a best practice, anyway.  " +
                              "This will also help prep for a web-server system should I ever implement one.",
                assignedTo: me,
                source: me,
                dateCreated: July22)
            {
                DateAssigned = July22
            };
            Tasks.Add(asynchInterfaceTask);

            var shellInterfaceTask = new Task(

                title: "Shell Interface",
                description: "Complete an interface that can be used interactivley from the command prompt.  " +
                "This should run as an asynchronus componenet.",
                assignedTo: me,
                source: me,
                dateCreated: July22)
            {
                DateAssigned = July22,
                DateCompleted = July23
            };

            shellInterfaceTask.RelatedTasks = new List<Task> { asynchInterfaceTask, interfaceAbstractionTask, };
            Tasks.Add(shellInterfaceTask);

            var recursionTask = new Task(

                title: "Handle Recursion",
                description: "A Task's RelatedTask property can contain a cycle.  " +
                "Find places where this might cause endless recursion and fix them.",
                assignedTo: me,
                source: me,
                dateCreated: July22)
            {
                DateAssigned = July22,

                DateCompleted = July23
            };
            Tasks.Add(recursionTask);

            Tasks.Add(new Task(

                title: "DefaultDict",
                description: "Add to my utilities some equivlent to python's defaultDict class.",
                assignedTo: me,
                source: me,
                dateCreated: July16)
            {
                DateAssigned = July16,
                DateCompleted = July16
            });

            /**** COMPLETED TASKS ****/

            Tasks.Add(new Task(

                title: "Create a User Object",
                description: "Create an object that will represent users",
                assignedTo: me,
                source: me,
                dateCreated: July16)
            {
                DateAssigned = July16,
                DateCompleted = July16
            });

            Tasks.Add(new Task(

                title: "Create Basic Data Store",

                description: "Implemented something that might look like a store but doesn't really do anything",
                assignedTo: me,
                source: me,
                dateCreated: July16)
            {
                DateAssigned = July16,
                DateCompleted = July16
            }
            );

            Tasks.Add(new Task(

                title: "Create a Task Object",
                description: "Create a task object",
                assignedTo: me,
                source: me,
                dateCreated: July16)
            {
                DateAssigned = July16,
                DateCompleted = July16
            }
            );

            Tasks.Add(new Task(

                title: "Add task id",
                description: "Add a unique identifier to tasks",
                assignedTo: me,
                source: me,
                dateCreated: July16
                )
            {
                DateAssigned = July16,

                DateCompleted = July16
            }
            );

            Tasks.Add(new Task(

                title: "Basic User Interface",
                description: "Create a super simple command-line interface.",
                assignedTo: me,
                source: me,
                dateCreated: July17)
            {
                DateAssigned = July16,
                DateCompleted = July16
            }
            );

            Tasks.Add(new Task(

                title: "Task Status",
                description: "Add a Status state property to Task object",
                assignedTo: me,
                source: me,
                dateCreated: July16)
            {
                DateAssigned = July16,
                DateCompleted = July21,
                Notes = "Feature not needed at this time."
            }
            );

            Tasks.Add(new Task(

                title: "Add to Repo",
                description: "Learn how to use VS built-in repo management.  Commit this build.",
                assignedTo: me,
                source: me,
                dateCreated: July16)
            {
                DateAssigned = July16,
                DateCompleted = July21,
                Notes = "Some java error keeps git from working.  I will need to manually add to github."
            }
            );

            Tasks.Add(new Task(

                title: "Add basic functionality to data store",
                description: "Add ability to get all tasks from store, get one task, and delete one task.",
                assignedTo: me,
                source: me,
                dateCreated: July21)
            {
                DateAssigned = July21,
                DateCompleted = July21
            }
            );

            Tasks.Add(new Task(

                title: "Add basic handlers to main program.",
                description: "Add ability to get all tasks from store, get one task, and delete one task.",
                assignedTo: me,
                source: me,
                dateCreated: July21)
            {
                DateAssigned = July21,
                DateCompleted = July21
            }
            );

            Tasks.Add(new Task(

                title: "Subtasks",
                description: "A task should be able to be broken down into any number sub-tasks.",
                assignedTo: me,
                source: me,
                dateCreated: July21)
            {
                DateAssigned = July21,
                DateCompleted = July22
            }
            );
        }
    }
}