using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanCode.MethodNames.ParsingAndValidation
{
    public class ArgumentsParser
    {
        public ValidArguments ValidatedArguments;

        // (4)
        // TryParse - TryParseUserInputAndSetUpArguments
        public bool TryParseUserInputAndSetUpArguments(string arguments, out string error)
        {
            error = null;

            if (String.IsNullOrEmpty(arguments))
                return false;

            string[] input =
                arguments.Split(' ').Where(item => !String.IsNullOrWhiteSpace(item)).ToArray();

            if (!TryValidateAndSetUpArguments(input, out error))
                return false;

            return true;
        }

        // (5)
        // ValidateInput - TryValidateAndSetUpArguments
        private bool TryValidateAndSetUpArguments(IEnumerable<string> input, out string error)
        {
            error = null;

            ValidArguments validArguments = new ValidArguments();

            IEnumerator enumerator = input.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string flag = enumerator.Current.ToString();

                if (enumerator.MoveNext())
                {
                    SetUpArguments(flag, enumerator, validArguments, out error);
                    if (error != null)
                        return false;
                }
                else
                {
                    error = "Input is incorrect.";
                    return false;
                }
            }

            ValidatedArguments = validArguments;
            return true;
        }

        // (6)
        // TryParseArgumentAndGetError - SetUpArguments
        private void SetUpArguments(string flag, IEnumerator enumerator, ValidArguments validArguments,
            out string error)
        {
            error = null;
            
            string value = enumerator.Current.ToString();
            switch (flag)
            {
                case Constants.Arg.NameArg:
                case Constants.Arg.TestArg:
                {
                    SetUpArgumentsNameTest(flag, value, validArguments);
                    break;
                }

                case Constants.Arg.MinMarkArg:
                case Constants.Arg.MaxMarkArg:
                {
                    error = SetArgumentsMinMaxMarks(flag, value, validArguments);
                    break;
                }

                case Constants.Arg.DateFromArg:
                case Constants.Arg.DateToArg:
                {
                    error = SetArgumentsDateFromDateTo(flag, value, validArguments);
                    break;
                }

                case Constants.Arg.SortArg:
                {
                    error = SetArgumentsSortingField(value, enumerator, validArguments);
                    break;
                }

                default:
                {
                    error = "Input is incorrect.";
                    break;
                }
            }
        }

        // (7)
        // SetValueAccordingToFlagNameOrTest - SetUpArgumentsNameTest
        private void SetUpArgumentsNameTest(string flag, string value, ValidArguments arguments)
        {
            if (flag == Constants.Arg.NameArg)
                arguments.Name = value;
            else
                arguments.Test = value;
        }

        // (8)
        // SetValueAccordingToFlagMinmarkOrMaxmark - SetArgumentsMinMaxMarks
        private string SetArgumentsMinMaxMarks(string flag, string value,
            ValidArguments arguments)
        {
            if (Int32.TryParse(value, out int mark))
            {
                if (flag == Constants.Arg.MinMarkArg)
                    arguments.MinMark = mark;
                else
                    arguments.MaxMark = mark;
            }
            else
                return $"{Constants.Prop.Mark} must be an integer";

            if (arguments.MinMark > arguments.MaxMark)
                return $"{Constants.Arg.MinMarkArg} can not be bigger than {Constants.Arg.MaxMarkArg}";

            return null;
        }

        // (9)
        // SetValueAccordingToFlagDatefromOrDateto - SetArgumentsDateFromDateTo
        private string SetArgumentsDateFromDateTo(string flag, string value,
            ValidArguments arguments)
        {
            if (DateTime.TryParse(value, out DateTime validDateTime))
            {
                if (flag == Constants.Arg.DateFromArg)
                    arguments.DateFrom = validDateTime;
                else
                    arguments.DateTo = validDateTime;

                if (arguments.DateFrom > arguments.DateTo)
                    return
                        $"{Constants.Arg.DateFromArg} can not be bigger than {Constants.Arg.DateToArg}";

                return null;
            }

            return @"Date must have ISO 8601 format: year-month-day. Example: 1900-01-01
Year: 1-9999; Month: 1-12; Day: 1-(max days of the month)";
        }

        // (10)
        // SetValueAccordingToFlagSortAndArguments - SetArgumentsSortingField
        private string SetArgumentsSortingField(string value,
            IEnumerator enumerator, ValidArguments arguments)
        {
            if (enumerator.MoveNext())
            {
                string sortOrder = enumerator.Current.ToString();

                if (ValidArguments.AllowedField.Contains(value) &&
                    (Constants.Arg.AllowedAscendingSortArg.Contains(sortOrder) ||
                     Constants.Arg.AllowedDescendingSortArg.Contains(sortOrder)))
                {
                    arguments.Sorting = true;
                    arguments.SortingField = value;

                    arguments.Ascending = Constants.Arg.AllowedAscendingSortArg.Contains(sortOrder);
                    return null;
                }
            }

            return $@"Flag {Constants.Arg.SortArg} takes 2 arguments! 
Allowed fields: {Constants.Prop.Name}, {Constants.Prop.Test}, {Constants.Prop.Date}, {Constants.Prop.Mark}. 
Allowed order: {String.Join(", ", Constants.Arg.AllowedAscendingSortArg)}, {String.Join(", ", Constants.Arg.AllowedDescendingSortArg)}. 
Example: {Constants.Arg.SortArg} {Constants.Prop.Name} {Constants.Arg.AllowedAscendingSortArg.First()}";
        }
    }
}