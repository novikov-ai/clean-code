using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace CleanCode.VariablesLifeTime
{
    [Transaction(TransactionMode.Manual)]
    public class ImportDelete : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;
            
            try
            {
                ICollection<Element> allImportElements = new FilteredElementCollector(doc)
                    .OfClass(typeof(ImportInstance))
                    .WhereElementIsNotElementType().ToElements();

                // (7)
                // prev:
                // int count = 0;

                if (allImportElements.Any())
                {
                    bool removeLinked = true;

                    // ...
                    // business logic removed
                    // ...

                    // (7)
                    // improved: grouped variable declaration and it's usage
                    int count = 0;
                    
                    using (Transaction tx = new Transaction(doc, "Удаление импортированных экземпляров САПР"))
                    {
                        tx.Start();

                        foreach (Element item in allImportElements)
                        {
                            if (removeLinked)
                            {
                                doc.Delete(item.Id);
                                count++;
                            }
                        }
                        tx.Commit();
                    }

                    // ...
                    // business logic removed
                    // ...
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
    }
}