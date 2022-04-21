# About

Renamed bad variable names at 2 classes according:

- 7.1. best practices for boolean variables (done, success, found) => 5 examples

- 7.2. standard boolean names => ? examples

- 7.3. best practices for index cycle variables => ? examples

- 7.4. pair of antonyms (locked/unlocked) => 3 examples

- 7.5. best practices for temp/local variables => ? examples

~~~
// example format

// 7.X (Y)
// <old name> - <new name>
code usage
~~~

### [Repository class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames/Repository.cs)

~~~
cmd - spCommandGetUsers
// sql-stored procedure for getting all users
~~~

### [SqlLogger class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames/SqlLogger.cs)

~~~
cmd - spCommandLogInfo
// sql-stored procedure for logging info event
~~~

### [SqlTools class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames/Tools.cs)

~~~
cmd - spCommand
// sql-stored procedure for basic command
~~~