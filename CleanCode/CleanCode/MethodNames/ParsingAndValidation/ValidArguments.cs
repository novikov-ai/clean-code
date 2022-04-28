using System;
using System.Collections.Generic;

namespace CleanCode.MethodNames.ParsingAndValidation
{
    public class ValidArguments
    {
        public string Name = "";
        public string Test = "";

        public int MinMark;
        public int MaxMark = Int32.MaxValue;

        public DateTime DateFrom;
        public DateTime DateTo = DateTime.MaxValue;

        public bool Sorting;
        public string SortingField;

        public static readonly IReadOnlyCollection<string> AllowedField = new[] {Constants.Prop.Name, Constants.Prop.Test, Constants.Prop.Date, Constants.Prop.Mark};

        public bool Ascending;
    }
}