# About

Renamed bad constant names according best practices:

- use capital letters (according programming language guide styles - eg. C#: const string ConstantExample = "example")

- avoid magical numbers in your code (eg. for i=0; i < 100; i ++) { ... } - "100" - magical number, it's better to create a constant)

- you could only use directly numbers: 1, 0; otherwise - please create a constant or variable

~~~
// example format

// (X)
// prev: <old code>
// improved: <something>
code usage
~~~

### [AocDay4 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ConstantNames/AocDay4.cs)

~~~
// (1)
// prev: List<string> passportCodes = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };  
// improved: created a readonly static field instead of variable
if (PassportCodes.Contains(keys[0])) { ... }
...

// (2)
// prev: if (correct == 7)
// improved: created a constant instead of number
if (correct == PassportValidOccurrences)
    valid++;
...

// (3)
// prev: case "byr":
// improved: created a constant instead of string
case BirthYearCode:
{

    // (4)
    // prev: if (keys[1].Length == 4)
    // improved: created a constant instead of number
    if (keys[1].Length == DateLength)
    {
        Int32.TryParse(keys[1], out var res);
        
        // (5)
        // prev: if (res >= 1920 && res <= 2002)
        // improved: created two constants instead of numbers
        if (res >= BirthYearMin && res <= BirthYearMax)
            correct++;
        }

    break;
}

// (6)
// prev: case "iyr":
// improved: created a constant instead of string
case IssueYearCode:
{
    if (keys[1].Length == DateLength)
        {
            Int32.TryParse(keys[1], out var res);
            
            // (7)
            // improved: created two constants instead of numbers
            // prev: if (res >= 2010 && res <= 2020)
            if (res >= IssueYearMin && res <= IssueYearMax)
                correct++;
        }

    break;
}
...

// (8)
// if (res >= 2020 && res <= 2030)
// improved: created two constants instead of numbers
if (res >= ExpirationYearMin && res <= ExpirationYearMax)
    correct++;
...

// (9)
// prev: case "hgt":
// improved: created a constant instead of string
case HeightCode:
{
    // (10)
    // prev: if (keys[1].EndsWith("cm"))
    // improved: created a constant instead of string
    if (keys[1].EndsWith(HeightUnitMetric))
    {
        string heightNum = keys[1].Replace(HeightUnitMetric, "");
        Int32.TryParse(heightNum, out var res);
        if (res >= HeightMetricMin && res <= HeightMetricMax)
            correct++;
    }
    ...
 }
 ...
 
// (11)
// prev: case "hcl":
// improved: created a constant instead of string
case HairColorCode: { ... }
...

// (12)
// prev: List<string> Colors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
// improved: created a readonly static field instead of variable
    if (Colors.Contains(keys[1]))
        correct++;
...
~~~