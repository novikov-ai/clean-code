using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanCode.Arrays.StringFormatting
{
    public static class Words
    {
        public static double GetAverageWordLength(string input)
        {
            if (input is null)
                throw new ArgumentNullException("Input string is null.");

            foreach (char symbol in input)
            {
                if (!Char.IsLetter(symbol))
                    input = input.Replace(symbol, ' ');
            }

            // (4)
            // prev:
            // string[] array = input.Split(' ');
            // what: array data structure replaced by generic List<string>
            List<string> array = input.Split(' ').ToList();
            var words = new List<string>();

            double wordsLength = 0;
            foreach (string word in array)
            {
                if (word.Length == 0)
                    continue;
                
                words.Add(word);
                wordsLength += word.Length;
            }

            return wordsLength / words.Count;
        }

        public static string ReverseWords(string input)
        {
            if (input is null)
                throw new ArgumentNullException("Input string is null.");

            // (5)
            // prev:
            // string[] words = input.Split(' ');
            // what: array data structure replaced by generic List<string>
            
            List<string> words = input.Split(' ').ToList();
            
            words.Reverse();
            
            return string.Join(" ", words);

            // (5)
            // prev:
            // string result = "";
            // for (int i = words.Length - 1; i >= 0; i--)
            // {
            //     if (result.Length > 0)
            //         result += " ";
            //     result += words[i];
            // }
            //
            // return result;
        }
    }
}