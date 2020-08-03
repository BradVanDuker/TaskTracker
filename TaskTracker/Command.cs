using System;

namespace TaskTracker
{
    internal class Command
    {
    }

    internal class MenuOption
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
}