using System;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CleanCode.Comments.Engineering
{
    [Transaction(TransactionMode.Manual)]
    public class ElementCompoundStructure
    {
        // ...
        // business logic removed
        // ...

        private const string ParamSharedDescription = "place_holder";
        private const string LastLayer = "Железобетонная плита основания -см.КЖ";

        private const string ParamWallType = "Группа модели";
        private const string ParamWallTypeEng = "Model";
        private const string ParamWallTypeValue = "Отделочная стена";

        // ...
        // business logic removed
        // ...

        private Document _doc;

        protected void RunFunc(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;

            _doc = uiDoc.Document;
            
            var filteredElems = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Walls)
                    .UnionWith(new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Floors))
                    .UnionWith(new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Roofs))
                    .UnionWith(new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_Ceilings))
                    .WhereElementIsElementType()
                    .Cast<HostObjAttributes>();

                using (Transaction tx = new Transaction(_doc, "Состав конструкций"))
                {
                    tx.Start();

                    foreach (HostObjAttributes host in filteredElems)
                    {
                        bool isLastLayerSlab = false;

                        switch (host.GetType().Name)
                        {
                            case nameof(WallType):
                            {
                                var wallParam = host.LookupParameter(ParamWallType);
                                wallParam ??= host.LookupParameter(ParamWallTypeEng);

                                if (wallParam is null || wallParam.AsString() != ParamWallTypeValue)
                                    continue;

                                break;
                            }
                            case nameof(FloorType):
                            case nameof(RoofType):
                            {
                                isLastLayerSlab = true;
                                break;
                            }
                            case nameof(CeilingType):
                            {
                                break;
                            }
                            default:
                            {
                                continue;
                            }
                        }

                        var param = host.get_Parameter(new Guid(ParamSharedDescription));
                        if (param is null)
                        {
                            throw new Exception(@"
Общий параметр для заполнения состава конструкций не найден у одного или более элементов.

Проверьте наличие параметра у категорий: стена, перекрытие, крыша, потолок.");
                        }

                        try
                        {
                            param.Set(GetLayersDescription(host, isLastLayerSlab));
                        }
                        catch (Autodesk.Revit.Exceptions.InvalidOperationException e)
                        {
                            throw new Exception("Проверьте наличие общего параметра типа (!) в составных конструкциях.");
                        }
                    }

                    if (tx.Commit() == TransactionStatus.Committed)
                    {
                        // ...
                        // business logic removed
                        // ...
                    }   

                }
        }

        // 3.1 (6)
        /// <summary>
        /// Retrieving layers if they exist
        /// </summary>
        /// <param name="host">Compound element</param>
        /// <param name="lastLayer">Add structure slab to the end</param>
        /// <returns></returns>
        private string GetLayersDescription(HostObjAttributes host, bool lastLayer)
        {
            var structure = host.GetCompoundStructure();
            if (structure is null)
                return null;

            var layers = structure.GetLayers();

            var variableLayer = structure.VariableLayerIndex;

            var sb = new StringBuilder();

            for (int i = 1; i <= layers.Count; i++)
            {
                string variableWidth = null;
                
                // 3.1 (7)
                // variableLayer == -1 means: there is only one layer
                if (variableLayer != -1 && variableLayer == i - 1)
                    variableWidth = GetVariableWidth(host);

                var layerDescription = GetMaterialDescription(layers[i - 1], variableWidth);

                char endPoint = ';';
                if (layers.Count == i && !lastLayer)
                    endPoint = '.';

                sb.Append($@"{i} {layerDescription}{endPoint}
");
            }

            if (lastLayer)
                sb.Append($"{layers.Count + 1}. {LastLayer}.");

            return sb.ToString();
        }

        private string GetMaterialDescription(CompoundStructureLayer layer, string variableWidth)
        {
            // ...
            // business logic removed
            // ...

            return null;
        }

        private string GetVariableWidth(HostObjAttributes host)
        {
            // ...
            // business logic removed
            // ...

            return null;
        }

        private (double, double) GetDiffMinMaxFromVertexArray(SlabShapeVertexArray array)
        {
            double minDiff = 0;
            double maxDiff = 0;

            // ...
            // business logic removed
            // ...

            return (minDiff, maxDiff);
        }
    }
}