# About

Renamed bad method names (12 examples) at 4 classes according best practices:

- method names must be verbs or it's combinations

- good method-name describes the result and all side effects (try to create clean methods without side effects)

- use verb with object, which is being affected, for procedures (methods, which return void); if procedure exists inside class, then you don't need object name for procedure naming

- clean methods should only do 1 action and have to do it well


~~~
// example format

// (X)
// <old name> - <new name>
code usage
~~~

### [UserInteraction class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/MethodNames/UserInteraction.cs)

~~~
// (1)
// EnterCommand - SetUpInputDialog
public void SetUpInputDialog()
{
    Console.WriteLine(@"Please input search criteria using a search string as an input parameter.");
    ...
}
~~~

### [StudentsTools class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/MethodNames/StudentsTools.cs)

~~~
// (2)
// RunQuery - GetValidatedStudents
public static IEnumerable<Student> GetValidatedStudents(ValidArguments validatedResult, List<Student> students)
{
    if (validatedResult is null || students is null)
        return null;
            
    var result = students.Where(student =>
                ... ;

    if (validatedResult.Sorting)
        result = GetSortedStudents(result, validatedResult.SortingField, validatedResult.Ascending);

        return result;
}

// (3)
// Sort - GetSortedStudents
private static IEnumerable<Student> GetSortedStudents(IEnumerable<Student> students, string field, bool ascending)
{
    ...
    
    Func<Student, IComparable> comparer = field switch
    {
        ...
    };
    
    ...
    
    if (comparer is null)
        return null;

    return ascending ? students.OrderBy(comparer) : students.OrderByDescending(comparer);
}
~~~

### [ArgumentsParser class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/MethodNames/ParsingAndValidation/ArgumentsParser.cs)

~~~
// (4)
// TryParse - TryParseUserInputAndSetUpArguments
public bool TryParseUserInputAndSetUpArguments(string arguments, out string error)
{
    ...
    
    if (!TryValidateAndSetUpArguments(input, out error))
        return false;
    
    ...
}

// (5)
// ValidateInput - TryValidateAndSetUpArguments
private bool TryValidateAndSetUpArguments(IEnumerable<string> input, out string error)
{
    ...
    
    while (enumerator.MoveNext())
    {
        ...

        if (enumerator.MoveNext())
        {
            SetUpArguments(flag, enumerator, validArguments, out error);
            
            ...
        }   
        
        ...
    }
    
    ...
}

// (6)
// TryParseArgumentAndGetError - SetUpArguments
private void SetUpArguments(string flag, IEnumerator enumerator, ValidArguments validArguments, out string error)
{
    ...

    switch (flag)
    {
        case Constants.Arg.NameArg:
        case Constants.Arg.TestArg:
        {
            SetUpArgumentsNameTest(flag, value, validArguments);
            break;
        }
        
        ...
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
private string SetArgumentsMinMaxMarks(string flag, string value, ValidArguments arguments)
{
    ...
}

// (9)
// SetValueAccordingToFlagDatefromOrDateto - SetArgumentsDateFromDateTo
private string SetArgumentsDateFromDateTo(string flag, string value, ValidArguments arguments)
{
    ...
}

// (10)
// SetValueAccordingToFlagSortAndArguments - SetArgumentsSortingField
private string SetArgumentsSortingField(string value, IEnumerator enumerator, ValidArguments arguments)
{
    ...
}
~~~

### [DataAccessProvider class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/MethodNames/DataBaseRepository/DataAccessProvider.cs)

~~~
// (11)
// ChangeDataBase - ChangeDataFile
public void ChangeDataFile(string jsonFile)
{
    _jsonDataFile = jsonFile;
}
       
// (12)
// GetStudentsListFromJson - GetStudents
public List<Student> GetStudents()
{
    if (string.IsNullOrEmpty(_jsonDataFile))
        return null;

    return JsonSerializer.Deserialize<List<Student>>(File.ReadAllText(_jsonDataFile));
}
~~~