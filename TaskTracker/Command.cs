using System;
using System.Collections.Generic;

namespace TaskTracker
{
    public class Command
    {
    }

    public class MenuOption
    {
        public int Id { get; }
        public string Name { get; }
        public string Category { get; }

        public Action RunCommand { get; set; }

        public MenuOption(int id, string name, Action runCommand, string category = "")
        {
            this.Id = id;
            this.Name = name;
            this.RunCommand = runCommand;
            this.Category = category;
        }
    }

    public class Request
    {
        public int Id { get; set; }
        public Dictionary<string, object> parameters { get; set; }
    }
}