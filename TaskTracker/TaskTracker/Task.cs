using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker
{
    class Task
    {
        static private int currentId = 0;

        public Task()
        {
            this.Id = currentId++;
        }

        public int Id { get; }

        public string Title { get; set; }

        public string Description { get; set; }

        public User AssignedTo { get; set; }

        public User Source { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateAssigned { get; set; }

        public DateTime? DateCompleted { get; set; }

        public IList<Task> RelatedTasks { get; set; }
    }
}
