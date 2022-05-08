# About

Variable binding times:

- in-place binding (the earliest and the worst):

~~~  
titleBar.color = 255;
~~~

- compiling binding:

~~~
private static final int COLOR_WHITE = 255;

...

titleBar.color = COLOR_WHITE;
~~~

- binding while the program is running (optimal way):

~~~
titleBar.color = ReadTitleBarColor(); // reading from config file
~~~

---

~~~
// example format

// (X)
// <binding time>
// why: <something>
code usage
~~~

---

### [LinesConnector class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableBindingTimes/LinesConnector.cs)

~~~
// (1)
// in-place binding
// why: variable describes tolerance value, which is always constant
const double offsetTolerance = 0.01;

bool isLeadOffsetFromSecond = Math.Abs(leadingEndPoint.Z - secondEndPoint.Z) > offsetTolerance;

if (_isRightAngle && isLeadOffsetFromSecond)
    (width, height) = (height, width);
~~~

### [Application class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableBindingTimes/AutoSave/Application.cs)

~~~
// (2)
// compiling binding
// why: config file has only one relative path, which perfectly stored in a constant, 
// but it also easy to maintain if you need to move file somewhere else
private const string ConfigPath = "../../config.txt";

...

// (3)
// binding while the program is running
// why: every application user has his own specific settings, which could change behavior of the executing program during the process
public Result OnStartup(UIControlledApplication application)
{
    // ...
    // business logic removed
    // ...

    UserConfig config = GetConfigFile();

    FileWatcher fileWatcher = new FileWatcher(config.AutoSaveInterval, config.AutoSyncInterval);
    fileWatcher.Run();

    return Result.Succeeded;
}
~~~
