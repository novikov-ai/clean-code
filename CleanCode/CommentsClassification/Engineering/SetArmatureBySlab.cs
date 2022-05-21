using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;

namespace CleanCode.CommentsClassification.Engineering
{
    [Transaction(TransactionMode.Manual)]
    public class SetArmatureBySlab : BasicCommand
    {
        // ...
        // business logic removed
        // ...

        protected void RunFunc(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // ...
            // business logic removed
            // ...

            Element selectedSlab = null;
            while (selectedSlab is Floor == false)
            {
                selectedSlab = doc.GetElement(
                    uiDoc.Selection.PickObject(ObjectType.Element).ElementId);
            }

            // 6
            // representation of intentions
            // we're deleting selected slab, because it's the only way to get it's sketch
            // after we got sketch, the slab is recovery (RollBack operation)
            ICollection<ElementId> slabElementIds;
            using (Transaction tx = new Transaction(doc, $"Temporary removed floor id {selectedSlab.Id}"))
            {
                tx.Start();
                slabElementIds = doc.Delete(selectedSlab.Id);
                tx.RollBack();
            }

            Sketch floorSketch = null;
            Plane plane = null;

            int count = 2;
            foreach (var elementId in slabElementIds)
            {
                if (count == 0)
                    break;

                var element = doc.GetElement(elementId);
                if (element is Sketch sketch)
                {
                    floorSketch = sketch;
                    count--;
                }
                else if (element is SketchPlane sketchPlane)
                {
                    plane = sketchPlane.GetPlane();
                    count--;
                }
            }

            var profArray = floorSketch.Profile;

            double maxPerimeter = 0;
            CurveArray externalCurveArray = null;

            foreach (CurveArray array in profArray)
            {
                double perimeter = 0;
                foreach (Curve curve in array)
                    perimeter += curve.Length;

                if (perimeter > maxPerimeter)
                {
                    maxPerimeter = perimeter;
                    externalCurveArray = array;
                }
            }

            // ...
            // business logic removed
            // ...

            using (Transaction tx = new Transaction(doc, $"Reinforcing along floor id {selectedSlab.Id}"))
            {
                tx.Start();

                Family selectedFamily = null;

                // ...
                // business logic removed
                // ...

                FamilySymbol familySymbol = null;

                foreach (var symbolId in selectedFamily.GetFamilySymbolIds())
                {
                    familySymbol = doc.GetElement(symbolId) as FamilySymbol;
                    break;
                }

                if (!familySymbol.IsActive)
                    familySymbol.Activate();

                foreach (Curve curve in externalCurveArray)
                {
                    XYZ curvePoint;

                    curvePoint = curve.Evaluate(0.5, true);

                    var newFamInstance = doc.Create.NewFamilyInstance(
                        curvePoint,
                        familySymbol,
                        null);

                    // ...
                    // business logic removed
                    // ...

                    Location location = newFamInstance.Location;
                    var locationPointStart = curvePoint;

                    // 7
                    // representation of intentions
                    // for creating rotation along axis Z we shift by 10 coordinate Z
                    var locationPointEnd = new XYZ(locationPointStart.X, locationPointStart.Y,
                        locationPointStart.Z + 10);

                    var axis = Autodesk.Revit.DB.Line.CreateBound(locationPointStart, locationPointEnd);

                    var curveLine = curve as Autodesk.Revit.DB.Line;

                    if (curveLine is null)
                        continue;

                    // ...
                    // business logic removed
                    // ...
                }

                tx.Commit();
            }
        }
    }
}