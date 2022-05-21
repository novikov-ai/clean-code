using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanCode.Arrays.ConsoleApp
{
    class App
    {
        public static void Run()
        {
            while (true)
            {
                Console.Write(
                    "Hello there! Would you like to try ArrayHelper or RectangleHelper?\nOptions:\na - ArrayHelper\nr - RectangleHelper\nany key - Quit the App\n");

                switch (Console.ReadLine())
                {
                    case "a":
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("ArrayHelper:");

                        Console.WriteLine("Enter the integer numbers of your array. Example: 15;32;-5;0;43");

                        try
                        {
                            string userInput = Console.ReadLine();
                            if (string.IsNullOrEmpty(userInput))
                                throw new ArgumentException("User input is null");
                            
                            // (2)
                            // prev:
                            // string[] splitInput = Console.ReadLine().Split(';');
                            // int[] input = new int[splitInput.Length];
                            // for (int i = 0; i < userInput.Length; i++)
                            // {
                            //     input[i] = GetInt32(userInput[i]);
                            // }
                            // what: array data structure replaced by generic List<T>
                            
                            List<string> splitInput = userInput
                                .Split(';').ToList();

                            List<int> inputNumbers = splitInput
                                .Select(s => GetInt32(s))
                                .ToList();
                            
                            Console.WriteLine(
                                "Would you like to ascending or descending array with bubble-sort?\nOptions:\na - Ascending\nd - Descending");

                            string result = "Array was sorted: ";

                            switch (Console.ReadLine())
                            {
                                case "a":
                                {
                                    ArrayHelper.SortAsc(inputNumbers);
                                    break;
                                }
                                case "d":
                                {
                                    ArrayHelper.SortDesc(inputNumbers);
                                    break;
                                }
                                default:
                                {
                                    result = "Array wasn't sorted: ";
                                    break;
                                }
                            }

                            // ...
                            // business logic removed
                            // ...
                            
                        }
                        catch
                            (FormatException e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(e.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }

                        break;
                    }
                    case "r":
                    {
                        // ...
                        // business logic removed
                        // ...

                        break;
                    }
                    default:
                    {
                        return;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine("Again? Options:\ny - yes\nany key - Quit the App");
                switch (Console.ReadLine())
                {
                    case "y":
                    {
                        continue;
                    }
                    default:
                    {
                        return;
                    }
                }
            }
        }

        private static int GetInt32(string input)
        {
            int value;

            try
            {
                value = Convert.ToInt32(input);
            }
            catch (FormatException)
            {
                throw new FormatException("Invalid input: integer only");
            }

            return value;
        }
    }
}