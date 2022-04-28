# About

Renamed bad method names (12 examples) at 6 classes according best practices:

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

### [RabbitConnection class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ClassNames/RabbitConnection.cs)

~~~

~~~