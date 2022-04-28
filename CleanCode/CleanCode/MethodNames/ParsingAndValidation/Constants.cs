using System.Collections.Generic;
namespace CleanCode.MethodNames.ParsingAndValidation
{
    public static class Constants
    {
        public static class Prop
        {
            public const string Name = "name";
            public const string Test = "test";
            public const string Date = "date";
            public const string Mark = "mark";
            public const string Sort = "sort";
        }

        public static class Arg
        {
            private const string Prefix = "-";
            public const string NameArg = Prefix + Prop.Name;
            public const string TestArg = Prefix + Prop.Test;
            public const string DateFromArg = Prefix + Prop.Date + "from";
            public const string DateToArg = Prefix + Prop.Date + "to";
            public const string MinMarkArg = Prefix + "min" + Prop.Mark;
            public const string MaxMarkArg = Prefix + "max" + Prop.Mark;
            public const string SortArg = Prefix + Prop.Sort;
            
            public static readonly IReadOnlyCollection<string> AllowedAscendingSortArg = new[] {"asc", "ascending"};
            public static readonly IReadOnlyCollection<string> AllowedDescendingSortArg = new[] {"desc", "descending"};
        }
    }
}