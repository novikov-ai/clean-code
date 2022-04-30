# About

Renamed bad constant names according best practices:

- use capital letters (according programming language guide styles - eg. C#: const string ConstantExample = "example")

- avoid magical numbers in your code (eg. for i=0; i < 100; i ++) { ... } - "100" - magical number, it's better to create a constant)

- you could only use directly numbers: 1, 0; otherwise - please create a constant or variable

~~~
// example format

// (X)
// prev: <old code>
code usage
~~~

### [AocDay4 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/ConstantNames/AocDay4.cs)

~~~
// (1)
// prev: List<string> passportCodes = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };  
if (PassportCodes.Contains(keys[0])) { ... }
...

// (2)
// prev: if (correct == 7)
if (correct == PassportValidOccurrences)
    valid++;
...

// (3)
// prev: case "byr":
case BirthYearCode:
{

    // (4)
    // prev: if (keys[1].Length == 4)
    if (keys[1].Length == DateLength)
    {
        Int32.TryParse(keys[1], out var res);
        
        // (5)
        // prev: if (res >= 1920 && res <= 2002)
        if (res >= BirthYearMin && res <= BirthYearMax)
            correct++;
        }

    break;
}

// (6)
// prev: case "iyr":
case IssueYearCode:
{
    if (keys[1].Length == DateLength)
        {
            Int32.TryParse(keys[1], out var res);
            
            // (7)
            // prev: if (res >= 2010 && res <= 2020)
            if (res >= IssueYearMin && res <= IssueYearMax)
                correct++;
        }

    break;
}
...

// (8)
// if (res >= 2020 && res <= 2030)
if (res >= ExpirationYearMin && res <= ExpirationYearMax)
    correct++;
...

// (9)
// prev: case "hgt":
case HeightCode:
{
    // (10)
    // prev: if (keys[1].EndsWith("cm"))
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
case HairColorCode: { ... }
...

// (12)
// prev: List<string> Colors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
    if (Colors.Contains(keys[1]))
        correct++;
...
~~~