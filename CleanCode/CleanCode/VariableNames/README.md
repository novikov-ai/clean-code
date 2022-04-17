# About

Renamed bad variable names at 3 classes. 
E.g. 12 of them:
~~~
// example format

<old name> - <new name>
// context
~~~

### [Repository class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames/Repository.cs)

~~~
cmd - spCommandGetUsers
// sql-stored procedure for getting all users

list - users
// all users list

loggerCmd - spCommandLogger
// sql-stored procedure for events logging

newId - createdUserId
// id of created user

pId - parameterUserId
// sql-parameter user id

result - isUserUpdated
// is user updated result

result - isUserDeleted
// is user deleted result
~~~

### [SqlLogger class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames/SqlLogger.cs)

~~~
cmd - spCommandLogInfo
// sql-stored procedure for logging info event

cmd - spCommandLogError
// sql-stored procedure for logging error event

pMessage - parameterLogMessage
// sql-parameter logging message

pStackTrace - parameterLogStackTrace
// sql-parameter logging stack trace
~~~

### [SqlTools class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames/Tools.cs)

~~~
cmd - spCommand
// sql-stored procedure for basic command
~~~