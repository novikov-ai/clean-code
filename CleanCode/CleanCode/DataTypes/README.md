# About

Refactor data types according best practices:

1. General
   
    1.1. avoid type conversions
   
    1.2. avoid comparing values of different types
   
    1.3. pay attention to compiler warnings
   
    1.4. always keep in mind division by zero error and handle all the possible occurrence of that
   

2. Integer numbers

    2.1. check integer division (use suitable operations)
   
    2.2. check possible overflow of integer numbers
   
    2.3. check possible overflow of intermediate results of calculations inside expressions
   

3. Real numbers

    3.1. avoid addition and subtraction of too different in value numbers
   
    3.2. avoid comparing for equality
   
    3.3. pay attention and handle all the possible errors of rounding
   
    3.4. change real variable type to type with bigger accuracy (eg.: 0.0 -> 0.00; 0.00 -> 0.000)
   
    3.5. if it's possible try to change float numbers to integers
   
    3.6. check special symbols support at language and additional libraries


4. Strings and chars

    4.1. avoid magical strings and chars - use constants
    
    4.2. use Unicode format

    4.3. strategize string messages localization at the very beginning of your program


5. Boolean variables

    5.1. use boolean variables actively for increasing readable of your code

    5.2. use boolean variables for simplification of complicating condition expressions

~~~
// example format

// (X)
// prev: <old code>
// improved: <something>
code usage
~~~

### [AocDay8 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/DataTypes/AocDay8.cs)

~~~
// (1)
// prev: if (lineOfPuzzle[1].StartsWith('+'))
// improved: created a boolean variable
bool lineStartedWithPlus = lineOfPuzzle[1].StartsWith('+');
if (lineStartedWithPlus)
{
   signedNumbersCount = 1;
   numberCurrent = lineOfPuzzle[1].TrimStart('+');
}
...

// (2)
// prev: case "acc":
// improved: created a constant instead of magical string
case AccumulatorShortName:
{
   accumulatorValue += numberBeforeAction * signedNumbersCount;
   i++;
   break;
}
                    
// (3)
// prev: case "jmp":
// improved: created a constant instead of magical string
case JumpsShortName:
{
   i += numberBeforeAction * signedNumbersCount;
   break;
}
~~~

### [ConnectLinesLogic class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/DataTypes/ConnectLinesLogic.cs)

~~~
// (4)
// prev: public static XYZ GetIntersectionPoint(..., double angle, ...)
// improved: replaced data type from double to int
public static XYZ GetIntersectionPoint(Line leadingLine, Line secondLine, int angle, XYZ leadingEndPoint, XYZ secondEndPoint, XYZ secondEndPointFarAway)
{
   ...

   // (5)
   // prev: var angleRad = angle * Math.PI / 180;
   // improved: created constants instead of magic numbers
   const double multiplierRadiansToDegrees = Math.PI / 180;
   
   var angleRad = angle * multiplierRadiansToDegrees;
   
   ...
   
   // (6)
   // prev: var rotateAngleRad = rotateAngle * Math.PI / 180;
   // improved: created constants instead of magic numbers
   var rotateAngleRad = rotateAngle * multiplierRadiansToDegrees;
   
   ...
   
   // (7)
   // prev: rotatedCurveFirst.MakeBound(-1500, 1500);
   // improved: created constants instead of magic numbers
   const int boundValue = 1500;
   const int boundValueNegative = -1500;
   rotatedCurveFirst.MakeBound(boundValueNegative, boundValue);
   rotatedCurveSecond.MakeBound(boundValueNegative, boundValue);
   
   ...
   
   foreach (Line line in listLines)
   {
      ...
      
      // (8)
      // prev: if (resultArray != null && result == SetComparisonResult.Overlap)
      // improved: created a boolean variable
      bool linesOverlapped = resultArray != null && result == SetComparisonResult.Overlap;
      
      if (linesOverlapped)
      {
         ...
      
         // (9)
         // prev: if (resultLine is null || resultLine.Length > cacheLine.Length)
         // improved: created a boolean variable
         bool resultLineBiggerThanCacheLine = resultLine is null || resultLine.Length > cacheLine.Length;
         if (resultLineBiggerThanCacheLine)
         {
            resultLine = cacheLine;
            intersectionPoint = point;
         }
      }
   }
   
   return intersectionPoint;
}
~~~

### [ConnectingLinesCodeBehind class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/DataTypes/ConnectingLinesCodeBehind.cs)

~~~
// (10)
// prev: bool isLeadOffsetFromSecond = Math.Abs(leadingEndPoint.Z - secondEndPoint.Z) > 0.01;
// improved: created a double constant variable instead of magical number
const double offsetTolerance = 0.01;

bool isLeadOffsetFromSecond = Math.Abs(leadingEndPoint.Z - secondEndPoint.Z) > offsetTolerance;
if (_isRightAngle && isLeadOffsetFromSecond)
   (width, height) = (height, width);
~~~

### [PlacingFamiliesCodeBehind class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/DataTypes/PlacingFamiliesCodeBehind.cs)

~~~
// (11)
// prev: 
// if (parameter.Definition != null && !parameter.IsReadOnly &&
//     parameter.Definition.Name == "Offset from Host" ||
//     parameter.Definition.Name == "Смещение от главной модели")
// improved: created a boolean variable
bool isParameterModifiable = parameter.Definition != null && !parameter.IsReadOnly;
bool isParameterNameOffset = parameter.Definition.Name == "Offset from Host" || 
                             parameter.Definition.Name == "Смещение от главной модели";

if (isParameterModifiable && isParameterNameOffset)
{
    // (12)
    // prev: parameter.Set(_heightFromFloor / 304.8);
    // improved: created a double constant variable instead of magical number
    const double multiplierFtToMm = 1 / 304.8;
    
    parameter.Set(_heightFromFloor * multiplierFtToMm);
    break;
}
~~~