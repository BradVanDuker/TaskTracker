using System;
using System.Collections.Generic;

namespace TaskTracker
{
    public class Task
    {
        //static private int currentId = 0;

        public Task(string title, string description, User assignedTo, User source, DateTime? dateCreated = null, int id = 0)
        {
            this.RelatedTasks = new List<Task>();
            this.Title = title;
            this.Description = description;
            this.AssignedTo = assignedTo;
            this.Source = source;
            this.DateCreated = dateCreated ?? DateTime.Now;
            this.Id = id;
        }


        public int Id { get;}

        public string Title { get; set; }

        public string Description { get; set; }

        public User AssignedTo { get; set; }

        public User Source { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateAssigned { get; set; }

        public DateTime? DateCompleted { get; set; }

        public IList<Task> RelatedTasks { get; set; }

        public string Notes { get; set; }

        override public string ToString()
        {
            return $"Id:  {Id};  Title:  {Title}";
        }
    }
}