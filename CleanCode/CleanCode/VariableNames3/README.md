# About

Renamed bad variable names at 6 classes according:

- 7.1. best practices for boolean variables => 5 examples

- 7.2. standard boolean names (done, success, found, etc.) => 2 examples

- 7.3. best practices for index cycle variables => 1 examples

- 7.4. pair of antonyms (locked/unlocked) => 3 examples

- 7.5. best practices for temp/local variables => 8 examples

~~~
// example format

// 7.X (Y)
// <old name> - <new name>
code usage
~~~

### [Matrix class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames3/Matrix.cs)

~~~
// 7.1 (1) 
// sorted - isMatrixSorted
bool isMatrixSorted = false;
while (!isMatrixSorted) {...}
~~~

### [EmailManager class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames3/EmailManager.cs)

~~~
// 7.1 (2) 
// skipped - isSkippedClasses
bool isSkippedClasses = false;
...
return isSkippedClasses;
~~~

### [SmsManager class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames3/SmsManager.cs)

~~~
// 7.1 (3) 
// lessAverage - isBelowAverage
bool isBelowAverage = false;
...
return isBelowAverage;
~~~

### [Crud class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames3/DataAccess/Crud.cs)

~~~
public bool Delete(T record)
{
    // 7.1 (4) 
    // result - isDeleted
    var isDeleted = false;
    
    try
    {
        // 7.5 (2) [variable table is useless]
        // var table = _context.Set<T>();
        // table.Remove(record);
        _context.Set<T>().Remove(record);
        
        ...
    }
    
    ...
    
    return isDeleted;
}

public List<T> Read()
{
    // 7.5 (1) 
    // result - retrievedData 
    
    var retrievedData = new List<T>();
    
    ...
    
    return retrievedData;
}

public List<T> Including(params string[] properties)
{
    try
    {
        // 7.5 (3) set - uniqueProperties
        var uniqueProperties = new HashSet<string>();
    
        // 7.5 (4) prop - property
        foreach (var property in properties)
            uniqueProperties.Add(property);
    
        // 7.5 (5) [variable output is useless]
        // string output = string.Join('.', properties);
        // return _context.Set<T>().Include(output).ToList();
    
        return _context.Set<T>().Include(string.Join('.', properties)).ToList();
    }
    
    ...
}
~~~

### [Player class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames3/ArcadeGame/Player.cs)

~~~
// 7.1 (5) 
// directionX - isDirectionX
bool isDirectionX = true;
...
        if (obstacle.DoesIntersect(this))
        {
            if (isDirectionX)
                PositionX = oldPosition;
                    else
                PositionY = oldPosition;
                    return;
        }
...

// 7.2 (2)
// result - success
public bool TryPick(Bonus bonus)
{
    var success = false;  
    
    ...
    
    return success;
}
~~~

### [AdventOfCode class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames3/AdventOfCode.cs)

~~~
// 7.2 (1) 
// ok - processingCompleted
bool processingCompleted = false;

...

if (!processingCompleted)
{
    invalidNum = allNums[pieceOfPuzzle];
    break;
}

...

// 7.3 (1) 
// i - pieceOfPuzzle
for (int pieceOfPuzzle = 0; pieceOfPuzzle < puzzle.Length; pieceOfPuzzle++)
{
    ...
}

// 7.4 (1) 
// (firstPiece, lastPiece)
var firstPiece = pieceOfPuzzle - preamble;
var lastPiece = pieceOfPuzzle - 1;

...

// 7.5 (6) 
// sum - sumOfPuzzleNumbers
int sumOfPuzzleNumbers = 0;

...

// 7.4 (2) 
// (minNumber, maxNumber)
int minNumber = -1;
int maxNumber = -1;

...

// 7.5 (7) 
// num - number
Int32.TryParse(puzzle[pieceOfPuzzle], out var number);
contiguousSet.Add(number);

// 7.5 (8)
// [variable result is useless]
// int result = min + max;
// Console.WriteLine("Day 09: " + result);
Console.WriteLine("Day 09: " + minNumber + maxNumber);
~~~