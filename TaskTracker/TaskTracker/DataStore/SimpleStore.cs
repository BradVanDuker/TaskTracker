using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.DataStore
{
    class SimpleStore
    {
        public IList<Task> Tasks { get; set; }

        public IList<User> Users { get; set; }

        public SimpleStore()
        {
            this.Users = new List<User>();

            User me = new User("Brad");
            this.Users.Add(me);
            this.Tasks = new List<Task>();

            DateTime July16 = new DateTime(2020, 7, 16);
            DateTime July17 = new DateTime(2020, 7, 17);
            DateTime July21 = new DateTime(2020, 7, 21);
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
                Title = "Task Status",
                Description = "Add a Status state property to Task object",
                AssignedTo = me,
                Source = me,
                DateCreated = July16,
                DateAssigned = July16,
                DateCompleted = July21,
                Notes = "Feature not needed at this time."
            }) ;

            Tasks.Add(new Task()
            {
                Title = "Add to Repo",
                Description = "Learn how to use VS built-in repo management.  Commit this build.",
                AssignedTo = me,
                Source = me,
                DateCreated = July16,
                DateAssigned = July16,
            });

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
        }
    }
}