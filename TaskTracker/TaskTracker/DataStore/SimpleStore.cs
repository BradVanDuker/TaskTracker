using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskTracker.DataStore
{
    class SimpleStore
    {
        public IList<Task> Tasks { get; set; }

        public IList<User> Users { get; set; }

        public void AddTask(Task task)
        {
            this.Tasks.Add(task);
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
            return this.Tasks.Select(x => x);
        }

        
        public Task GetTask(int id)
        {
            var values = this.Tasks.Where(x => x.Id == id);
            var iterator = values.GetEnumerator();
            var hasSomething = iterator.MoveNext();
            if(hasSomething)
            {
                return iterator.Current;
            }
            else
            {
                throw new Exception("Task id not found.");
            }
        }


        public SimpleStore()
        {
            this.Users = new List<User>();

            User me = new User("Brad");
            this.Users.Add(me);
            this.Tasks = new List<Task>();

            DateTime July16 = new DateTime(2020, 7, 16);
            DateTime July17 = new DateTime(2020, 7, 17);
            DateTime July21 = new DateTime(2020, 7, 21);
            DateTime July22 = new DateTime(2020, 7, 22);
            DateTime July23 = new DateTime(2020, 7, 23);
            
            Tasks.Add(new Task()
            {
                Title = "Basic Controler",
                Description = "Create a simple super controller to tie everyting together",
                AssignedTo = me,
                Source = me,
                DateCreated = July17,
                DateAssigned = July17,
            });

            Tasks.Add(new Task()
            {
                Title = "Invalid Menu Options",
                Description = "Handle errors for invalid inputs for the command line menu",
                AssignedTo = me,
                Source = me,
                DateCreated = July17,
                DateAssigned = July17,
            });


            Tasks.Add(new Task()
            {
                Title = "Testing",
                Description = "Learn to use unit testing features of VS.",
                AssignedTo = me,
                Source = me,
                DateCreated = July21,
                DateAssigned = July21,
            });

            

            Tasks.Add(new Task()
            {
                Title = "Command Objects",
                Description = "The menu needs to handle user inputs with 0+ parameters and various return types",
                AssignedTo = me,
                Source = me,
                DateCreated = July21,
                DateAssigned = July21,
            });

            Tasks.Add(new Task()
            {
                Title = "Utilities lib",
                Description = "Add more utility functions to my library",
                AssignedTo = me,
                Source = me,
                DateCreated = July21,
                DateAssigned = July21,
            });

            var interfaceAbstractionTask = new Task()
            {
                Title = "Abstract away commands from the interface.",
                Description = "Implement some sort of command o-o pattern",
                AssignedTo = me,
                Source = me,
                DateCreated = July21,
                DateAssigned = July21,

                DateCompleted = July23
            };
            Tasks.Add(interfaceAbstractionTask);


            var asynchInterfaceTask = new Task()
            {
                Title = "Asynch Interface",
                Description = "The interface should run on a seperate thread than the program.  " +
                              "This will be good practice for asyn programming and events.  It's probably a best practice, anyway.  " +
                              "This will also help prep for a web-server system should I ever implement one.",
                AssignedTo = me,
                Source = me,
                DateCreated = July22,
                DateAssigned = July22,
            };
            Tasks.Add(asynchInterfaceTask);

            var shellInterfaceTask = new Task()
            {
                Title = "Shell Interface",
                Description = "Complete an interface that can be used interactivley from the command prompt.  " +
                "This should run as an asynchronus componenet.",
                AssignedTo = me,
                Source = me,
                DateCreated = July22,
                DateAssigned = July22,

                DateCompleted = July23
            };
            shellInterfaceTask.RelatedTasks = new List<Task> { asynchInterfaceTask, interfaceAbstractionTask, };
            Tasks.Add(shellInterfaceTask);

            var recursionTask = new Task()
            {
                Title = "Handle Recursion",
                Description = "A Task's RelatedTask property can contain a cycle.  " +
                "Find places where this might cause endless recursion and fix them.",
                AssignedTo = me,
                Source = me,
                DateCreated = July22,
                DateAssigned = July22,

                DateCompleted = July23
            };
            Tasks.Add(recursionTask);

            Tasks.Add(new Task()
            {
                Title = "DefaultDict",
                Description = "Add to my utilities some equivlent to python's defaultDict class.",
                AssignedTo = me,
                Source = me,
                DateCreated = July16,
                DateAssigned = July16,

                DateCompleted = July16
            });



            /**** COMPLETED TASKS ****/

            Tasks.Add(new Task()
            {
                Title = "Create a User Object",
                Description = "Create an object that will represent users",
                AssignedTo = me,
                Source = me,
                DateCreated = July16,
                DateAssigned = July16,

                DateCompleted = July16
            });

            Tasks.Add(new Task()
            {
                AssignedTo = me,
                Title = "Create Basic Data Store",
                Source = me,
                Description = "Implemented something that might look like a store but doesn't really do anything",
                DateCreated = July16,
                DateAssigned = July16,

                DateCompleted = July16,
            });

            Tasks.Add(new Task()
            {
                AssignedTo = me,
                Title = "Create a Task Object",
                Source = me,
                Description = "Create a task object",
                DateCreated = July16,
                DateAssigned = July16,

                DateCompleted = July16
            });

            Tasks.Add(new Task()
            {
                Title = "Add task id",
                Description = "Add a unique identifier to tasks",
                AssignedTo = me,
                Source = me,
                DateCreated = July16,
                DateAssigned = July16,

                DateCompleted = July16,
            });

            Tasks.Add(new Task()
            {
                Title = "Basic User Interface",
                Description = "Create a super simple command-line interface.",
                AssignedTo = me,
                Source = me,
                DateCreated = July17,
                DateAssigned = July17,

                DateCompleted = July17
            });

            Tasks.Add(new Task()
            {
                Title = "Task Status",
                Description = "Add a Status state property to Task object",
                AssignedTo = me,
                Source = me,
                DateCreated = July16,
                DateAssigned = July16,
                DateCompleted = July21,
                Notes = "Feature not needed at this time."
            });

            Tasks.Add(new Task()
            {
                Title = "Add to Repo",
                Description = "Learn how to use VS built-in repo management.  Commit this build.",
                AssignedTo = me,
                Source = me,
                DateCreated = July16,
                DateAssigned = July16,
                DateCompleted = July21,
                Notes = "Some java error keeps git from working.  I will need to manually add to github."
            });

            Tasks.Add(new Task()
            {
                Title = "Add basic functionality to data store",
                Description = "Add ability to get all tasks from store, get one task, and delete one task.",
                AssignedTo = me,
                Source = me,
                DateCreated = July21,
                DateAssigned = July21,

                DateCompleted = July21
            });

            Tasks.Add(new Task()
            {
                Title = "Add basic handlers to main program.",
                Description = "Add ability to get all tasks from store, get one task, and delete one task.",
                AssignedTo = me,
                Source = me,
                DateCreated = July21,
                DateAssigned = July21,
                Notes = "See task about adding basic functionality to data store",

                DateCompleted = July22
            });

            Tasks.Add(new Task()
            {
                Title = "Subtasks",
                Description = "A task should be able to be broken down into any number sub-tasks.",
                AssignedTo = me,
                Source = me,
                DateCreated = July21,
                DateAssigned = July21,

                DateCompleted = July22
            });
        }
    }
}