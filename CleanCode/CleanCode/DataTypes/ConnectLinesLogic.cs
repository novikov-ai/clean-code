using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace CleanCode.DataTypes
{
    public class ConnectLinesLogic
    {
        // (4)
        // prev: public static XYZ GetIntersectionPoint(..., double angle, ...)
        // improved: replaced data type from double to int
        public static XYZ GetIntersectionPoint(Line leadingLine, Line secondLine,
            int angle, XYZ leadingEndPoint, XYZ secondEndPoint, XYZ secondEndPointFarAway)
        {
            var plane = Plane.CreateByThreePoints(leadingLine.GetEndPoint(0),
                leadingLine.GetEndPoint(1), secondEndPointFarAway);

            var axis = plane.Normal;

            // (5)
            // prev: var angleRad = angle * Math.PI / 180;
            // improved: created constants instead of magic numbers
            const double multiplierRadiansToDegrees = Math.PI / 180;

            var angleRad = angle * multiplierRadiansToDegrees;

            var rotateAngle = angle == 90 ? angle : 360 - 2 * angle;

            // (6)
            // prev: var rotateAngleRad = rotateAngle * Math.PI / 180;
            // improved: created constants instead of magic numbers
            var rotateAngleRad = rotateAngle * multiplierRadiansToDegrees;

            var transformFirst = Transform.CreateRotationAtPoint(axis, angleRad, leadingEndPoint);
            var transformSecond = Transform.CreateRotationAtPoint(axis, angleRad + rotateAngleRad, leadingEndPoint);

            var rotatedCurveFirst = Line.CreateUnbound(leadingEndPoint, leadingLine.Direction)
                .CreateTransformed(transformFirst);
            var rotatedCurveSecond = Line.CreateUnbound(leadingEndPoint, leadingLine.Direction)
                .CreateTransformed(transformSecond);

            // (7)
            // prev: rotatedCurveFirst.MakeBound(-1500, 1500);
            // improved: created constants instead of magic numbers
            const int boundValue = 1500;
            const int boundValueNegative = -1500;
            rotatedCurveFirst.MakeBound(boundValueNegative, boundValue);
            rotatedCurveSecond.MakeBound(boundValueNegative, boundValue);

            var listLines = new List<Line>();
            listLines.Add(rotatedCurveFirst as Line);
            listLines.Add(rotatedCurveSecond as Line);

            var unboundSecond = Line.CreateUnbound(secondEndPoint, secondLine.Direction);
            unboundSecond.MakeBound(boundValueNegative, boundValue);

            XYZ intersectionPoint = null;

            Line resultLine = null;

            foreach (Line line in listLines)
            {
                IntersectionResultArray resultArray = null;
                var result = unboundSecond.Intersect(line, out resultArray);

                // (8)
                // prev: if (resultArray != null && result == SetComparisonResult.Overlap)
                // improved: created a boolean variable
                bool linesOverlapped = resultArray != null && result == SetComparisonResult.Overlap;
                if (linesOverlapped)
                {
                    var point = resultArray.get_Item(0).XYZPoint;
                    Line cacheLine;
                    try
                    {
                        cacheLine = Line.CreateBound(secondEndPoint, point);
                    }
                    catch
                    {
                        intersectionPoint = point;
                        break;
                    }

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

        public static (Connector, Connector) GetLeadAndSecondConnectors(ConnectorSet leadSet,
            ConnectorSet secondSet)
        {
            (Connector, Connector) leadSecondConnectors = (null, null);

            double minDist = Double.MaxValue;
            foreach (Connector leadConnector in leadSet)
            {
                foreach (Connector secondConnector in secondSet)
                {
                    double distanceFromLeadToSecond = leadConnector.Origin.DistanceTo(secondConnector.Origin);
                    if (distanceFromLeadToSecond < minDist)
                    {
                        minDist = distanceFromLeadToSecond;
                        leadSecondConnectors = (leadConnector, secondConnector);
                    }
                }
            }

            return leadSecondConnectors;
        }
    }
}