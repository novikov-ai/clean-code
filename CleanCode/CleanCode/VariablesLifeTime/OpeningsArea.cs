using System;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace CleanCode.VariablesLifeTime
{
    [Transaction(TransactionMode.Manual)]
    public class OpeningsArea : IExternalCommand
    {
        private const double MmToFtCoef = 0.00328084;

        private const string WidthName = "Ширина";
        private const string HeightName = "Высота";

        private Document _document;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;

            _document = uiDoc.Document;
            try
            {
                int calculatedOpenings = 0;
                Guid paramOpeningArea = new Guid();

                var windows = new FilteredElementCollector(_document)
                    .OfCategory(BuiltInCategory.OST_Windows)
                    .WhereElementIsNotElementType().ToElements();

                // ...
                // business logic removed
                // ...

                if (windows.Any())
                {
                    foreach (Element item in windows)
                    {
                        (double width, double height) = (0, 0);

                        if (item.Parameters != null)
                        {
                            // (7)
                            // prev:
                            // foreach (Parameter param in item.Parameters)
                            // {
                            //     if (param.Definition != null)
                            //     {
                            //         switch (param.Definition.Name)
                            //         {
                            //             case "Ширина":
                            //             {
                            //                 width = param.AsDouble() / 0.00328084; // перевод в мм из футов
                            //                 break;
                            //             }
                            //
                            //             case "Высота":
                            //             {
                            //                 height = param.AsDouble() / 0.00328084; // перевод в мм из футов
                            //                 break;
                            //             }
                            //         }
                            //     }
                            // }
                            // improved: created a separate method

                            (width, height) = GetWidthHeightByDefault(item);
                        }

                        if (width * height == 0)
                        {
                            // (8)
                            // prev:
                            // if (item.GetTypeId() != null)
                            // {
                            //     Element itemTypeElement = doc.GetElement(item.GetTypeId());
                            //
                            //     if (itemTypeElement.Parameters != null)
                            //     {
                            //         foreach (Parameter param in itemTypeElement.Parameters)
                            //         {
                            //             if (param.Definition != null)
                            //             {
                            //                 switch (param.Definition.Name)
                            //                 {
                            //                     case "Ширина":
                            //                     {
                            //                         width = param.AsDouble() / 0.00328084; // перевод из ft в mm
                            //                         break;
                            //                     }
                            //
                            //                     case "Высота":
                            //                     {
                            //                         height = param.AsDouble() / 0.00328084; // перевод из ft в mm
                            //                         break;
                            //                     }
                            //                 }
                            //             }
                            //         }
                            //     }
                            // }
                            // improved: created a separate method

                            (width, height) = GetWidthHeightFromType(item);
                        }

                        if (width * height > 0)
                        {
                            using (Transaction tx = new Transaction(_document, "Подсчет площади проема"))
                            {
                                tx.Start();
                                try
                                {
                                    double convertedArea = width * height / 0.092903 / Math.Pow(10, 6); // from ft2 -> mm2
                                    bool isAreaSet = item.get_Parameter(paramOpeningArea).Set(convertedArea);
                                    
                                    if (isAreaSet) 
                                        calculatedOpenings++;
                                }
                                catch (NullReferenceException)
                                {
                                    throw;
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
                
                // ...
                // business logic removed
                // ...

                return Result.Succeeded;
            }

            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }

            catch (Exception ex)
            {
                return Result.Failed;
            }
        }

        private (double, double) GetWidthHeightByDefault(Element element)
        {
            (double width, double height) = (0, 0);

            foreach (Parameter param in element.Parameters)
            {
                if (param.Definition != null)
                {
                    switch (param.Definition.Name)
                    {
                        case WidthName:
                        {
                            width = param.AsDouble() / MmToFtCoef;
                            break;
                        }

                        case HeightName:
                        {
                            height = param.AsDouble() / MmToFtCoef;
                            break;
                        }
                    }
                }
            }

            return (width, height);
        }

        private (double, double) GetWidthHeightFromType(Element element)
        {
            (double width, double height) = (0, 0);

            if (element.GetTypeId() is null)
                return (width, height);

            Element itemTypeElement = _document.GetElement(element.GetTypeId());

            if (itemTypeElement.Parameters is null)
                return (width, height);

            return GetWidthHeightByDefault(itemTypeElement);
        }
    }
}