using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace CleanCode.VariablesLifeTime
{
    [Transaction(TransactionMode.Manual)]
    public class RightOffset : IExternalCommand
    {
        // (1)
        // improved: changed scope of local variable usage
        private Document _document;
        
        // (2)
        // improved: changed scope of local variable usage
        const double MultiplierMmToFt = 0.00328084;
        const double OffsetValueMin = 3500 * MultiplierMmToFt;
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            
            // (1)
            // prev:
            // Document doc = uiDoc.Document;
            _document = uiDoc.Document;

            try
            {
                // (2)
                // prev:
                // const double mmToFt = 0.00328084;
                // const double offsetValue = 3500 * mmToFt;
                
                if (_document.ActiveView is View3D == false)
                {
                    return Result.Failed;
                }

                ICollection<ElementId> allLevels = new FilteredElementCollector(_document)
                    .OfClass(typeof(Level))
                    .WhereElementIsNotElementType().ToElementIds();

                // (3)
                // prev:
                // ICollection<Element> allElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
                //     .WhereElementIsNotElementType().Excluding(allLevels).ToElements();

                Dictionary<ElementId, double> levels = new Dictionary<ElementId, double>();

                // (4)
                // prev:
                // List<ElementId> wrongOffsetElems = new List<ElementId>();

                if (allLevels.Count == 0)
                {
                    return Result.Failed;
                }

                // (5)
                // prev:
                // foreach (ElementId lvlId in allLevels)
                // {
                //     Element lvl = doc.GetElement(lvlId);
                //     Level level = lvl as Level;
                //     if (level == null)
                //         continue;
                //
                //     levels.Add(level.Id, level.Elevation);
                // }
                // improved: created a separate method
                
                levels = GetLevelsMapWithElevations(allLevels);
                
                // (3)
                // improved: grouped variable declaration and it's usage
                ICollection<Element> allElems = new FilteredElementCollector(_document, _document.ActiveView.Id)
                    .WhereElementIsNotElementType().Excluding(allLevels).ToElements();
                
                // (4)
                // improved: grouped variable declaration and it's usage
                List<ElementId> wrongOffsetElems = new List<ElementId>();
                
                foreach (Element item in allElems)
                {
                    // (6)
                    // prev:
                    // ParameterSet parameterSet = item.Parameters;
                    // double offsetFromLevel = 0;
                    // foreach (Parameter param in parameterSet)
                    // {
                    //     if (param.Definition == null)
                    //         continue;
                    //
                    //     const string offsets = "Смещение;Смещение от уровня;Смещение снизу;Высота нижнего бруса";
                    //     List<string> offsetList = new List<string>(offsets.Split(';'));
                    //
                    //     if (!offsetList.Contains(param.Definition.Name))
                    //         continue;
                    //
                    //     string valueString = param.AsValueString();
                    //
                    //     if (valueString.Length == 0)
                    //         valueString = param.AsString();
                    //
                    //     offsetFromLevel = Convert.ToDouble(valueString) * MultiplierMmToFt; // mm to ft * 0,00328084
                    //     break;
                    // }
                    // improved: created a separate method

                    double offsetFromLevel = GetOffsetFromLevel(item);
                    
                    if (Math.Abs(offsetFromLevel) > OffsetValueMin)
                        wrongOffsetElems.Add(item.Id);
                }

                if (wrongOffsetElems.Count > 0)
                {
                    using (Transaction tx = new Transaction(_document, "Изолирование элементов с некорректным смещением"))
                    {
                        tx.Start();
                        uiDoc.ActiveView.IsolateElementsTemporary(wrongOffsetElems);
                        tx.Commit();
                    }
                }
                
                // ...
                // business logic removed
                // ...
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }

            catch (Exception ex)
            {
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private Dictionary<ElementId, double> GetLevelsMapWithElevations(ICollection<ElementId> levels)
        {
            var levelsMapWithElevations = new Dictionary<ElementId, double>();
            foreach (ElementId lvlId in levels)
            {
                Element lvl = _document.GetElement(lvlId);
                Level level = lvl as Level;
                if (level == null)
                    continue;

                levelsMapWithElevations.Add(level.Id, level.Elevation);
            }

            return levelsMapWithElevations;
        }

        private double GetOffsetFromLevel(Element element)
        {
            var offsetFromLevel = 0.0;
            
            ParameterSet parameterSet = element.Parameters;
            foreach (Parameter param in parameterSet)
            {
                if (param.Definition == null)
                    continue;

                const string offsets = "Смещение;Смещение от уровня;Смещение снизу;Высота нижнего бруса";
                List<string> offsetList = new List<string>(offsets.Split(';'));

                if (!offsetList.Contains(param.Definition.Name))
                    continue;

                string valueString = param.AsValueString();

                if (valueString.Length == 0)
                    valueString = param.AsString();
                
                offsetFromLevel = Convert.ToDouble(valueString) * MultiplierMmToFt;
                break;
            }

            return offsetFromLevel;
        }
    }
}