# About

Array treatment using best practice:

- always pay attention to possible index out of range exception

- don't intersect inserted cycles indexes (CatArray\[ j ] or CatArray\[ i ])

- always check the correct order of multidimensional arrays indexes

- always check the boundary points

- try to use other serial containers (stacks, queues, etc.) instead of arrays structure


~~~
// example format

// (X)
// what: <something>
code usage

OR

// (X)
// prev: <old code>
// what: <something>
~~~

### [AocDay1 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Arrays/AocDay1.cs)

~~~
// (1)
// what: enumeration through the array using 'foreach' instead of 'for' (you don't need explicit indexing)
foreach (string item in allNums)
{
    if (thirdNum.Length > 0)
        break;
        
    ...
}
~~~

### [App class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Arrays/ConsoleApp/App.cs)

~~~
// (2)
// prev:
// string[] splitInput = Console.ReadLine().Split(';');
// int[] input = new int[splitInput.Length];
// for (int i = 0; i < userInput.Length; i++)
// {
//     input[i] = GetInt32(userInput[i]);
// }
// what: array data structure replaced by generic List<T>

List<string> splitInput = userInput
    .Split(';').ToList();

List<int> inputNumbers = splitInput
    .Select(s => GetInt32(s))
    .ToList();
~~~

### [BigNumbers class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Arrays/StringFormatting/BigNumbers.cs)

~~~
// (3)
// prev:
// char[] num1Reversed = CheckInputAndReverse(num1);
// char[] num2Reversed = CheckInputAndReverse(num2);
//
// char[] majorNum = num1Reversed.Length > num2Reversed.Length ? num1Reversed : num2Reversed;
// char[] minorNum = num1Reversed.Length > num2Reversed.Length ? num2Reversed : num1Reversed;
// what: array data structure replaced by generic List<char>

List<char> num1Reversed = CheckInputAndReverse(num1);
List<char> num2Reversed = CheckInputAndReverse(num2);

List<char> majorNum = num1Reversed.Count > num2Reversed.Count ? num1Reversed : num2Reversed;
List<char> minorNum = num1Reversed.Count > num2Reversed.Count ? num2Reversed : num1Reversed;

var resultReversed = new StringBuilder();

int sum;
int carry = 0;

// (3)
// prev:
// for (int i = 0; i < majorNum.Length; i++)
// {
//     sum = Int32.Parse(Char.ToString(majorNum[i])) + carry;
//
//     if (i < minorNum.Length)
//         sum += Int32.Parse(Char.ToString(minorNum[i]));
//
//     if (sum > 9)
//     {
//         carry = sum / 10;
//         resultReversed.Append(sum % 10);
//     }
//     else
//     {
//         carry = 0;
//         resultReversed.Append(sum);
//     }
// }

foreach (var letter in majorNum)
{
    sum = Int32.Parse(Char.ToString(letter)) + carry;

    var letterIndex = majorNum.IndexOf(letter);
    if (letterIndex < minorNum.Count)
        sum += Int32.Parse(Char.ToString(minorNum[letterIndex]));
    
    if (sum > 9)
    {
        carry = sum / 10;
        resultReversed.Append(sum % 10);
    }
    else
    {
        carry = 0;
        resultReversed.Append(sum);
    }
}
~~~

### [Words class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Arrays/StringFormatting/Words.cs)

~~~
public static double GetAverageWordLength(string input)
{
    ...

    // (4)
    // prev:
    // string[] array = input.Split(' ');
    // what: array data structure replaced by generic List<string>
    List<string> array = input.Split(' ').ToList();
    var words = new List<string>();

    double wordsLength = 0;
    foreach (string word in array)
    {
        if (word.Length == 0)
            continue;
        
        words.Add(word);
        wordsLength += word.Length;
    }

    return wordsLength / words.Count;
}

public static string ReverseWords(string input)
{
    ...

    // (5)
    // prev:
    // string[] words = input.Split(' ');
    // what: array data structure replaced by generic List<string>
    
    List<string> words = input.Split(' ').ToList();
    
    words.Reverse();
    
    return string.Join(" ", words);

    // (5)
    // prev:
    // string result = "";
    // for (int i = words.Length - 1; i >= 0; i--)
    // {
    //     if (result.Length > 0)
    //         result += " ";
    //     result += words[i];
    // }
    //
    // return result;
}
~~~