# About

Renamed bad class and method names at 6 classes according best practices:

3.1. class names must be nouns or it's combinations (avoid names: Manager, Processor, Data, Info, etc.) => 5 examples

3.2. don't mix up equal method names in different classes (eg. fetch / retrieve / get) - choose the only one for the abstract concept and use it everywhere => 7 examples


~~~
// example format

// 3.X (Y)
// <old name> - <new name>
code usage
~~~

### [RabbitConnection class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ClassNames/RabbitConnection.cs)

~~~
// 3.1 (1)
// RabbitManager - RabbitConnection
public class RabbitConnection : IDisposable
{
    ...
}
~~~

### [HttpRestClient class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ClassNames/HttpRestClient.cs)

~~~
// 3.1 (2)
// RequestHandler - HttpRestClient
public class HttpRestClient : IDisposable
{
    ...
    
    // 3.2 (1)
    // GetRequest - Get
    public async Task<string> Get(string apiUrl, string endpoint)
    {
        ...
    }
    
    // 3.2 (2)
    // PostRequest - Post
    public async Task<T> Post<T>(string apiUrl, object newObject, string endpoint)
    {
        ...
    }
    
    ...
}
~~~

### [MediaPost class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ClassNames/MediaPost.cs)

~~~
// 3.1 (3)
// PostCreate - MediaPost
public class MediaPost
{  
    ...
}
~~~

### [EmailClient class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ClassNames/EmailClient.cs)

~~~
// 3.2 (3)
// IsSkippedClassesMoreThan - CheckForSkippedClasses
public bool CheckForSkippedClasses(Student student, int numberOfSkippedClasses)
{
    ...
}

...

// 3.2 (4)
// SendToLecturerAndStudent - NotifyLecturerAndStudent        
private void NotifyLecturerAndStudent(Lecturer lecturer, Student student)
{
    _logger.LogInformation(@$"Student {student.Name} has skipped more than 3 lessons
        .Sending email to lecturer ({lecturer.Email}) and student({student.Email}).");
}
~~~

### [SmsClient class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ClassNames/SmsClient.cs)

~~~
// 3.2 (5)
// IsAverageMarkLowThan - CheckForLowAverageMark
public bool CheckForLowAverageMark(Student student, int lowerMark)
{
    ...
}

// 3.2 (6)
// SendToStudent - NotifyStudent
private void NotifyStudent(Student student, Discipline discipline)
{
    _logger.LogInformation(@$"Student {student.Name} has average mark below 4 in {discipline.Name}.
        Sending sms to student ({student.Telephone}).");
}
~~~

### [RetrievingData class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ClassNames/RetrievingData.cs)

~~~
// 3.2 (7)
// GetStudentJournalFromVisits - GetHomeworksJournal
public static Dictionary<Discipline, List<Homework>> GetHomeworksJournal(HashSet<Lecture> visits)
{
    ...
}
~~~