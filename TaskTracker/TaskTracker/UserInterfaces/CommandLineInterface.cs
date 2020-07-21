using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.UserInterfaces
{
    class CommandLineInterface
    {
        public void DisplayMenu(List<CommandOption> options)
        {
            var numbers = Enumerable.Range(1, options.Count).ToList();
            var optionNumberPairs = options.Zip(numbers).ToList();

            foreach(var (option, number) in optionNumberPairs)
            {
                Console.WriteLine($"{number}:  {option.name}");
            }

            var userSelection = Int32.Parse(Console.ReadLine());

            var command = (from pair in optionNumberPairs
                           where pair.Second == userSelection
                           select pair.First)
                           .First();

            // add checking for invalid inputs

            command.action();
           
        }
    }
}
