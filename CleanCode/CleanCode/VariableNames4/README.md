# About

Renamed bad variable names at 2 classes according best practices:

- avoid similar names with different meaning

- avoid numerical or symbols inside names

- avoid key-words (sum, max, int, var, result, value, etc.)

- avoid uninformative names

- don't use names with hidden meaning (which is "obvious")

- use specifiers (prefix / postsuffix)

- use specifiers (suffix)


~~~
// example format

// (X)
// <old name> - <new name>
code usage
~~~

### [AocDay1 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames4/AocDay1.cs)

~~~
// (1)
// input - rawInput
string rawInput = File.ReadAllText("input.txt");

// (2)
// allNums - allNumbers
string[] allNumbers = rawInput.Split('\n');

// (3)
// firstNum - numberFirst
string numberFirst = "";

// (4)
// secondNum - numberSecond
string numberSecond = "";

// (5)
// thirdNum - numberThird
string numberThird = "";

// (6)
// item - number
foreach (string number in allNumbers)
{
    ...
    
    // (7)
    // elem - numberNext
    foreach (string numberNext in allNumbers)
    {
        ...
        
        // (8)
        // sum - numberAndNumberNextSum
        var numberAndNumberNextSum = Convert.ToInt32(number) + Convert.ToInt32(numberNext);
        
        // (9)
        // el - numberNextNext
        foreach (string numberNextNext in allNumbers)
        {
            if (numberNextNext != numberFirst && numberNextNext != numberSecond && Convert.ToInt32(numberNextNext) + numberAndNumberNextSum == 2020)
            {
                numberThird = numberNextNext;
                break;
            }
        }
    }
}

...
~~~

### [AocDay2 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames4/AocDay2.cs)

~~~
// (10)
// pass - currentPassword
string currentPassword = " "; // current password

// (11)
// passwords - validPasswordsCount
int validPasswordsCount = 0;

...

// (12)
// array - splitPolicyAndPassword
string[] splitPolicyAndPassword = policyAndPassword.Split(' ');

...
~~~