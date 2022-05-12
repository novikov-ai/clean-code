using System;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace CleanCode.Comments.Engineering
{
    [Transaction(TransactionMode.Manual)]
    public class OpeningsArea : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                int calculatedOpenings = 0;
                Guid paramOpeningArea = new Guid();

                var allWins = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Windows)
                    .WhereElementIsNotElementType().ToElements();

                // ...
                // business logic removed
                // ...

                if (allWins.Count() > 0)
                {
                    foreach (Element item in allWins)
                    {
                        double width = 0, height = 0;

                        if (item.Parameters != null)
                        {
                            foreach (Parameter param in item.Parameters)
                            {
                                if (param.Definition != null)
                                {
                                    switch (param.Definition.Name)
                                    {
                                        case "Ширина":
                                        {
                                            // 3.2 (1)
                                            // prev:
                                            // width = param.AsDouble() / 0.00328084; // перевод в мм из футов
                                            width = ConvertFtToMm(param.AsDouble());
                                            break;
                                        }

                                        case "Высота":
                                        {
                                            height = ConvertFtToMm(param.AsDouble());
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (width * height == 0)
                        {
                            // ...
                            // business logic removed
                            // ...
                        }

                        if (width * height > 0)
                        {
                            using (Transaction tx = new Transaction(doc, "Подсчет площади проема"))
                            {
                                tx.Start();
                                try
                                {
                                    // 3.2 (2)
                                    // prev:
                                    // if (item.get_Parameter(paramOpeningArea)
                                    // .Set(width * height / 0.092903 / Math.Pow(10, 6))) // перевод в метры из ft2 в mm2
                                    // calculatedOpenings++;
                                    
                                    if (item.get_Parameter(paramOpeningArea)
                                        .Set(ConvertFtSqrToMmSqr(width * height)))
                                        calculatedOpenings++;
                                }
                                catch (NullReferenceException)
                                {
                                    // ...
                                    // business logic removed
                                    // ...
                                }

                                tx.Commit();
                            }
                        }
                    }

                    if (calculatedOpenings > 0)
                    {
                        // ...
                        // business logic removed
                        // ...
                    }
                }
                else
                {
                    // ...
                    // business logic removed
                    // ...
                }

                return Result.Succeeded;
            }

            // ...
            // business logic removed
            // ...

            catch (Exception ex)
            {
                // ...
                // business logic removed
                // ...

                return Result.Failed;
            }
        }

        private double ConvertFtToMm(double feet)
        {
            return feet / 0.00328084;
        }

        private double ConvertFtSqrToMmSqr(double feetSqr)
        {
            return feetSqr / 0.092903 / Math.Pow(10, 6);
        }
    }
}