using System;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using CleanCode.VariableValues.AutoViews;

namespace CleanCode.VariablesLifeTime.Views
{
    public class AutoViewsWindow
    {
        private Document _document;
        private Element _selectedElement;
        
        // ...
        // business logic removed
        // ...

        private ViewSection GetHorizontalSectionColumn(BoundingBoxXYZ elementBoundingBox, string viewSectionName)
        {
            // (13)
            // prev:
            // var elBoundBoxWidthX = elementBoundingBox.Max.X - elementBoundingBox.Min.X;
            // var elBoundBoxHeightZ = elementBoundingBox.Max.Z - elementBoundingBox.Min.Z;

            var elementBoundingBoxSumMinMaxHalf = 0.5 * (elementBoundingBox.Min + elementBoundingBox.Max);

            var midPoint = new XYZ(
                elementBoundingBoxSumMinMaxHalf.X,
                elementBoundingBoxSumMinMaxHalf.Y - SectionSettings.ProjectionOffset,
                elementBoundingBoxSumMinMaxHalf.Z);

            // (13)
            // improved: grouped variable declaration and it's usage
            var elBoundBoxWidthX = elementBoundingBox.Max.X - elementBoundingBox.Min.X;
            var elBoundBoxHeightZ = elementBoundingBox.Max.Z - elementBoundingBox.Min.Z;
            
            var sectionBox = GetSectionBoundingBoxHoriz(elBoundBoxWidthX, elBoundBoxHeightZ,
                SectionSettings.SideOffset, SectionSettings.BottomOffsetColumns, SectionSettings.TopOffsetColumns);

            return GetSectionBase(sectionBox, midPoint, false, SectionSettings.Scale, viewSectionName);
        }

        private ViewSection GetHorizontalSectionSlab(BoundingBoxXYZ elementBoundingBox, string viewSectionName)
        {
            // (14)
            // prev:
            // var elBoundBoxHeightZ = elementBoundingBox.Max.Z - elementBoundingBox.Min.Z;

            // var midPoint = new XYZ(elementBoundingBox.Max.X,
            //     elementBoundingBox.Max.Y - (elementBoundingBox.Max.Y - elementBoundingBox.Min.Y) / 2,
            //     elementBoundingBox.Max.Z - (elementBoundingBox.Max.Z - elementBoundingBox.Min.Z) / 2);

            // (14)
            // improved: grouped variable declaration and it's usage
            var elBoundBoxHeightZ = elementBoundingBox.Max.Z - elementBoundingBox.Min.Z;
            var sectionBox = GetSectionBoundingBoxHoriz(2 * SectionSettings.RightOffsetSlabs, elBoundBoxHeightZ,
                offsetBottom: SectionSettings.TopOffsetSlabs, offsetTop: SectionSettings.TopOffsetSlabs);

            // (14)
            // improved: grouped variable declaration and it's usage
            var midPoint = new XYZ(elementBoundingBox.Max.X,
                elementBoundingBox.Max.Y - (elementBoundingBox.Max.Y - elementBoundingBox.Min.Y) / 2,
                elementBoundingBox.Max.Z - (elementBoundingBox.Max.Z - elementBoundingBox.Min.Z) / 2);
            
            return GetSectionBase(sectionBox, midPoint, false, SectionSettings.Scale, viewSectionName);
        }

        private BoundingBoxXYZ GetSectionBoundingBoxHoriz(double elBoundBoxWidthX, double elBoundBoxHeightZ,
            double offsetSide = 0, double offsetBottom = 0, double offsetTop = 0)
        {
            return new BoundingBoxXYZ()
            {
                Min = new XYZ(-elBoundBoxWidthX / 2 - offsetSide, -elBoundBoxHeightZ / 2 - offsetBottom, 
                    -SectionSettings.ProjectionOffset),
                Max = new XYZ(elBoundBoxWidthX / 2 + offsetSide, elBoundBoxHeightZ / 2 + offsetTop, 0)
            };
        }

        // ...
        // business logic removed
        // ...

        private ViewSection GetSectionBase(BoundingBoxXYZ viewSectionBox, XYZ boxOrigin, bool isVertical,
            int scale, string sectionName)
        {
            // ...
            // business logic removed
            // ...

            return null;
        }

        private bool ReinforceViewPlan(ViewPlan viewPlan, BoundingBoxXYZ boundingBoxXyz)
        {
            // ...
            // business logic removed
            // ...

            return true;
        }

        private bool ReinforceViewSection(ViewSection viewSection, BoundingBoxXYZ boundingBoxXyz)
        {
            // ...
            // business logic removed
            // ...

            return true;
        }

        private Line GetViewSectionVerticalLine(ViewSection viewSection, BoundingBoxXYZ boundingBoxXyz,
            double sideOffset = 0)
        {
            var rightDirectionX = viewSection.RightDirection.X;
            var offset = rightDirectionX switch
            {
                -1 => boundingBoxXyz.Max.X - boundingBoxXyz.Min.X - sideOffset,
                0 => boundingBoxXyz.Max.Y - boundingBoxXyz.Min.Y - sideOffset,
                _ => throw new Exception("Not handled ViewSection case")
            };
            
            return rightDirectionX switch
            {
                -1 => Line.CreateBound(
                    new XYZ(boundingBoxXyz.Min.X + offset, viewSection.Origin.Y, boundingBoxXyz.Min.Z),
                    new XYZ(boundingBoxXyz.Min.X + offset, viewSection.Origin.Y, boundingBoxXyz.Max.Z)),
                0 => Line.CreateBound(
                    new XYZ(viewSection.Origin.X, boundingBoxXyz.Min.Y + offset, boundingBoxXyz.Min.Z),
                    new XYZ(viewSection.Origin.X, boundingBoxXyz.Min.Y + offset, boundingBoxXyz.Max.Z)),
                _ => throw new Exception("Not handled ViewSection case")
            };
        }

        // ...
        // business logic removed
        // ...

        private AutoColumnBlockedParams GetBlockedParams()
        {
            if (_selectedElement is null)
                return null;

            double length, width;
            int concreteClass = 0;

            if (_selectedElement is Wall wall)
            {
                var wallType = wall.WallType;

                length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                width = wallType.Width;

                // (15)
                // prev:

                // var structureMaterialName = wallType.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM)
                //     .AsValueString();
                //
                // var match = Regex.Match(structureMaterialName, Constants.ConcreteClassPattern);
                //
                // if (match.Success)
                //     int.TryParse(match.Value.Substring(1), out concreteClass);

                // (15)
                // improved: created a separate method
                concreteClass = GetConcreteClass(wallType);
            }
            else
            {
                var elementType = _document.GetElement(_selectedElement.GetTypeId());

                var columnDiameter = elementType.get_Parameter(Constants.GuidFamilySymbolDiameter);
                if (columnDiameter is null)
                {
                    length = elementType.get_Parameter(Constants.GuidFamilyInstanceLength).AsDouble();
                    width = elementType.get_Parameter(Constants.GuidFamilyInstanceWidth).AsDouble();
                }
                else
                {
                    length = width = columnDiameter.AsDouble();
                }
            }

            return new AutoColumnBlockedParams(Math.Round(width), Math.Round(length), concreteClass);
        }

        private int GetConcreteClass(WallType wallType)
        {
            int concreteClass = -1;

            var structureMaterialName = wallType.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM)
                .AsValueString();

            var match = Regex.Match(structureMaterialName, Constants.ConcreteClassPattern);

            if (match.Success)
                int.TryParse(match.Value.Substring(1), out concreteClass);

            return concreteClass;
        }
    }
}