using System;
using System.Collections.Generic;
using System.Linq;
using CleanCode.MethodNames.DataBaseRepository;
using CleanCode.MethodNames.ParsingAndValidation;

namespace CleanCode.MethodNames
{
    public class UserInteraction
    {
        public string UserCommand { get; private set; }
        private ConsoleColor ConsoleDefaultColor;
        
        public UserInteraction()
        {
            ConsoleDefaultColor = Console.ForegroundColor;
        }
        
        // (1)
        // EnterCommand - SetUpInputDialog
        public void SetUpInputDialog()
        {
            Console.WriteLine(@"Please input search criteria using a search string as an input parameter.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Input search criteria: ");
            Console.ForegroundColor = ConsoleDefaultColor;
            UserCommand = Console.ReadLine()?.Trim();
        }

        public void DisplayStudents(IEnumerable<Student> students)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string line = new string('-', 35);
            Console.WriteLine($@"Students list
{line}
Student | Test | Date | Mark
{line}");
            foreach (var student in students)
            {
                Console.Write(
                    $"{student.Name} | {student.Test} | {student.Date:MM/dd/yyyy} | {student.Mark}");
                Console.WriteLine("");
            }

            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleDefaultColor;
        }
        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleDefaultColor;
        }
        public void DisplayHelp()
        {
            Console.WriteLine($@"You can use next flags for searching:
{Constants.Arg.NameArg} -> by student name
{Constants.Arg.TestArg} -> by passed test
{Constants.Arg.MinMarkArg} -> by minimal mark of the test
{Constants.Arg.MaxMarkArg} -> by maximum mark of the test
{Constants.Arg.DateFromArg} -> by minimal passed date
{Constants.Arg.DateToArg} -> by maximal passed date
{Constants.Arg.SortArg} -> by any field ascending (asc) or descending (desc)

example: {Constants.Arg.NameArg} Ivan {Constants.Arg.TestArg} Maths {Constants.Arg.MinMarkArg} 3 {Constants.Arg.MaxMarkArg} 5 
{Constants.Arg.DateFromArg} 2012-01-31 {Constants.Arg.DateToArg} 2013-02-02 
{Constants.Arg.SortArg} {Constants.Prop.Name} {Constants.Arg.AllowedAscendingSortArg.First()}");
        }
    }
}