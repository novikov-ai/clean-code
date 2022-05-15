using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using View = Autodesk.Revit.DB.View;

namespace CleanCode.CommentsClassification.Engineering
{
    [Transaction(TransactionMode.Manual)]
    public class CropViews : BasicCommand
    {
        // ...
        // business logic removed
        // ...

        protected void RunFunc(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var allowedViews = new List<ViewType>
            {
                ViewType.FloorPlan,
                ViewType.CeilingPlan,
                ViewType.EngineeringPlan,
                ViewType.AreaPlan,
                ViewType.Elevation,
                ViewType.Section,
                ViewType.Detail
            };

            View activeView = doc.ActiveView;
            if (!allowedViews.Contains(activeView.ViewType))
            {
                throw new WarningException("Open: plan or section.");
            }

            // ...
            // business logic removed
            // ...

            bool isCropModeCopy = false;

            // ...
            // business logic removed
            // ...

            CurveLoop curveLoop = null;

            if (isCropModeCopy)
            {
                ViewCropRegionShapeManager manager = activeView.GetCropRegionShapeManager();
                curveLoop = manager.GetCropShape().FirstOrDefault();
            }
            else
            {
                var selectedCropRegion = uiDoc.Selection.PickBox(PickBoxStyle.Crossing);
                curveLoop = GetCurveLoopFromMinMax(selectedCropRegion.Min, selectedCropRegion.Max);
            }

            List<View> allViews = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views)
                .WhereElementIsNotElementType()
                .ToElements()
                .Cast<View>()
                .Where(v => v.ViewType == activeView.ViewType &&
                            !v.IsTemplate &&
                            IsRightDirectionMatch(v.RightDirection, activeView.RightDirection)).ToList();

            if (isCropModeCopy)
            {
                var viewInList = allViews.FirstOrDefault(v => v.Id == activeView.Id);
                allViews.Remove(viewInList);
            }

            if (!allViews.Any())
                allViews.Add(activeView);

            // ...
            // business logic removed
            // ...

            IEnumerable<View> views = null;

            using (Transaction tx = new Transaction(doc, "Crop selecting views"))
            {
                tx.Start();

                foreach (View view in views)
                {
                    try
                    {
                        view.CropBoxActive = true;

                        var viewCropManager = view.GetCropRegionShapeManager();
                        viewCropManager.SetCropShape(curveLoop);

                        view.CropBoxVisible = false;
                    }

                    // 8
                    // TODO comments
                    // add other shapes support (not only rectangle) to the next release
                    catch (Autodesk.Revit.Exceptions.InvalidOperationException e)
                    {
                        throw new WarningException(
                            "You can crop only rectangle shape.");
                    }
                }

                tx.Commit();
            }
        }

        private CurveLoop GetCurveLoopFromMinMax(XYZ min, XYZ max)
        {
            // ...
            // business logic removed
            // ...

            return null;
        }

        // 9
        // informative comments
        // check if two views have the same orientation
        private bool IsRightDirectionMatch(XYZ rightDirection, XYZ benchmark)
        {
            if (Math.Abs(rightDirection.X * benchmark.X) == 1)
                return true;
            if (Math.Abs(rightDirection.Y * benchmark.Y) == 1)
                return true;
            if (Math.Abs(rightDirection.Z * benchmark.Z) == 1)
                return true;

            return false;
        }
    }
}