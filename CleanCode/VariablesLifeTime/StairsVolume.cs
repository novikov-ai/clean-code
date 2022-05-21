using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace CleanCode.VariablesLifeTime
{
    [Transaction(TransactionMode.Manual)]
    public class StairsVolume : IExternalCommand
    {
        private static readonly Guid ParamVolumeGuid = new Guid();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                var allStairsRuns = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_StairsRuns)
                    .WhereElementIsNotElementType().ToElements();

                // (10)
                // prev:
                // var allStairsLandings = new FilteredElementCollector(doc)
                //      .OfCategory(BuiltInCategory.OST_StairsLandings)
                //      .WhereElementIsNotElementType().ToElements();

                // ...
                // business logic removed
                // ...

                int calculated = 0;

                if (allStairsRuns.Count > 0)
                {
                    using (Transaction tx = new Transaction(doc, "Заполнением параметра \"Объем\""))
                    {
                        tx.Start();
                        foreach (Element stairs in allStairsRuns)
                        {
                            // (11)
                            // prev:
                            // Options optns = new Options();
                            // GeometryElement geomElem = stairs.get_Geometry(optns);
                            // double volume = 0;
                            // foreach (var geom in geomElem)
                            // {
                            //     if (geom is Solid)
                            //     {
                            //         Solid stairSolid = geom as Solid;
                            //         volume = stairSolid.Volume;
                            //     }
                            // }
                            // improved: created a separate method

                            var volume = GetElementVolume(stairs);

                            // (12)
                            // prev:
                            // Parameter p = stairs.get_Parameter(ParamVolumeGuid);
                            // if (p != null)
                            // {
                            //     if (p.Set(volume))
                            //         calculated++;
                            // }
                            // improved: created a separate method

                            if (TrySetParameterVolume(stairs, volume))
                                calculated++;
                        }

                        if (calculated > 0)
                        {
                            // ...
                            // business logic removed
                            // ...
                        }
                        else
                        {
                            // ...
                            // business logic removed
                            // ...
                        }

                        tx.Commit();
                    }
                }

                // (10)
                // improved: grouped variable declaration and it's usage
                var allStairsLandings = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_StairsLandings)
                    .WhereElementIsNotElementType().ToElements();

                if (allStairsLandings.Count > 0)
                {
                    using (Transaction tx = new Transaction(doc, "Заполнением параметра \"Объем\""))
                    {
                        tx.Start();

                        foreach (Element landings in allStairsLandings)
                        {
                            var volume = GetElementVolume(landings);

                            if (TrySetParameterVolume(landings, volume))
                                calculated++;
                        }
                        
                        tx.Commit();
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

        private double GetElementVolume(Element element)
        {
            Options optns = new Options();
            GeometryElement geomElem = element.get_Geometry(optns);

            double volume = 0;

            foreach (var geom in geomElem)
            {
                if (geom is Solid)
                {
                    Solid stairSolid = geom as Solid;
                    volume = stairSolid.Volume;
                }
            }

            return volume;
        }

        private bool TrySetParameterVolume(Element element, double value)
        {
            Parameter p = element.get_Parameter(ParamVolumeGuid);
            
            return p is not null && p.Set(value);
        }
    }
}