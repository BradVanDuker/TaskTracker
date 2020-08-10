using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskTracker.UserInterfaces
{
    internal class CommandLineInterface
    /// Used to output information when commands are given as arguments to Main
    {
        private const string genericPrompt = "Enter the number for your selection:  ";

        public void DisplayTasks(IEnumerable<Task> tasks, string preface = "\nHere's a list of tasks", string postface = "")
        {
            SendMessageToUser(preface);

            foreach (var task in tasks)
            {
                string completionString = task.DateCompleted != null ? $"**Completed**" : "";
                SendMessageToUser($"{task} {completionString}");
            }
            SendMessageToUser(postface);
        }

        public void DisplayMenuOptions(IEnumerable<MenuOption> menuOptions, string preface = "\nMenu Options", string postface = "")
        {
            SendMessageToUser(preface);
            foreach (var option in menuOptions)
            {
                SendMessageToUser($"{option.Id}:  {option.Name}");
            }
            SendMessageToUser(postface);
        }

        public void DisplayTaskDetails(Task task, string preface = "\nTask Details", string postface = "")
        {
            SendMessageToUser(preface);
            SendMessageToUser("Id:  " + task.Id);
            SendMessageToUser("Title:  " + task.Title);
            SendMessageToUser("Description:  " + task.Description);
            SendMessageToUser("Assigned To:  " + task.AssignedTo?.Name.ToString() ?? "Not Yet Assigned");
            SendMessageToUser("Creation Date:  " + task.DateCreated);
            SendMessageToUser("Details:  ");
            SendMessageToUser(task.DateCompleted?.ToString() ?? "Not Yet Completed.");
            if (task.RelatedTasks.Count > 0)
            {
                foreach (var subtask in task.RelatedTasks)
                {
                    DisplayTaskDetails(subtask, preface = $"---Task related to {task}---");
                }
            }
            SendMessageToUser(postface);
        }

        public string GetUserInput(string prompt = "")
        {
            SendMessageToUser(prompt);
            return Console.ReadLine();
        }

        public void SendMessageToUser(string msg)
        {
            if (msg.Length > 0)
            {
                Console.WriteLine(msg);
            }
        }

        public MenuOption GetUserMenuSelection(IEnumerable<MenuOption> menuOptions, string preface = "\nEnter the number for your selection:  ")
        {
            var userInput = GetUserInput(preface);
            try
            {
                var id = Int32.Parse(userInput);
                return menuOptions.First(x => x.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("Invalid choice");
            }
        }

        public void DisplayEnumeratedList(IEnumerable<object> stuff, string preface = "", string postface = "")
        {
            Console.WriteLine(preface);
            var number = 0;
            foreach(var thing in stuff)
            {
                Console.WriteLine($"{number}:  {thing}");
                number++;
            }
            Console.WriteLine(postface);
        }
    }
}