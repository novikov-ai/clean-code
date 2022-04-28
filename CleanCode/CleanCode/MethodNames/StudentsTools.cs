using System;
using System.Collections.Generic;
using System.Linq;
using CleanCode.MethodNames.DataBaseRepository;
using CleanCode.MethodNames.ParsingAndValidation;

namespace CleanCode.MethodNames
{
    public static class StudentsTools
    {
        // (2)
        // RunQuery - GetValidatedStudents
        public static IEnumerable<Student> GetValidatedStudents(ValidArguments validatedResult, List<Student> students)
        {
            if (validatedResult is null || students is null)
                return null;
            
            var result = students.Where(student =>
                student.Name.Contains(validatedResult.Name) &&
                student.Test.Contains(validatedResult.Test) &&
                student.Mark >= validatedResult.MinMark &&
                student.Mark <= validatedResult.MaxMark &&
                student.Date >= validatedResult.DateFrom &&
                student.Date <= validatedResult.DateTo);

            if (validatedResult.Sorting)
                result = GetSortedStudents(result, validatedResult.SortingField, validatedResult.Ascending);

            return result;
        }
        
        // (3)
        // Sort - GetSortedStudents
        private static IEnumerable<Student> GetSortedStudents(IEnumerable<Student> students, 
            string field, bool ascending)
        {
            if (students is null)
                return null;
            
            Func<Student, IComparable> comparer = field switch
            {
                Constants.Prop.Name => student => student.Name,
                Constants.Prop.Test => student => student.Test,
                Constants.Prop.Date => student => student.Date,
                Constants.Prop.Mark => student => student.Mark,
                _ => null
            };

            if (comparer is null)
                return null;

            return ascending ? students.OrderBy(comparer) : students.OrderByDescending(comparer);
        }
    }
}