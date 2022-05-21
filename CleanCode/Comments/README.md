# About

Comments treatment using best practice:

- comments are always failure (your code has to express your intentions)

- comments don't compensate bad code style (don't comment if you think your code is complicated, it's better to refactor)

~~~
// example format

---examples of relevant comments---
// 3.1 (X)
code usage

---examples of redundant comments---
// 3.2 (X)
// prev:
refactored code
~~~

### [BasicCommand abstract class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/ExternalCommands/BasicCommand.cs)

~~~
public abstract class BasicCommand : IExternalCommand, ICommandInfo
{
    // 3.1 (1)
    /// <summary>
    /// Displayed name of RibbonItem for user.
    /// </summary>
    public virtual string Name => "Безымянный";

    /// <summary>
    /// Displayed tooltip of RibbonItem for user.
    /// </summary>
    public virtual string Description => "Описание отсутствует";

    /// <summary>
    /// Image of the RibbonItem at custom RibbonPanel.
    /// </summary>
    public virtual string Picture => "unknown_32x32";

    /// <summary>
    /// Version of the RibbonItem. Displayed like long description.
    /// </summary>
    public virtual string Version => "1.0";

    // ...
    // business logic removed
    // ...

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            // ...
            // business logic removed
            // ...

            RunFunc(commandData);

            return Result.Succeeded;
        }
        catch (Exception e)
        {
            // ...
            // business logic removed
            // ...

            return Result.Failed;
        }
    }
}
~~~

### [AutoColumnBlockedParams class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/Engineering/AutoColumnBlockedParams.cs)

~~~
// 3.1 (2)
/// <summary>
/// Blocked parameters are read only for user when NewTypeWindow is invoked
/// </summary>
public class AutoColumnBlockedParams
{
    public const string ColumnParamWidthName = "Ширина колонны";
    public const string ColumnParamLengthName = "Длинна колонны";
    public const string ColumnParamConcreteClassName = "Класс бетона";

    public readonly double Width;
    public readonly double Length;
    public readonly double ConcreteClass;
    
    ...
}
~~~

### [ButtonDataUtils class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/ExternalCommands/ButtonDataUtils.cs)

~~~
// 3.1 (3)
/// <summary>
/// Easy PushButton creation
/// </summary>
/// <param name="assembly">Current assembly</param>
/// <param name="buttonData">ExternalCommand</param>
/// <returns></returns>
public static PushButtonData CreatePbData(string assembly, BasicCommand buttonData)
{
    try
    {
        var type = buttonData.GetType();
        var assemblyName = type.Assembly.GetName().Name;

        // ...
        // business logic removed
        // ...

        return new PushButtonData(buttonData.Name, buttonData.Name, assembly, type.FullName)
        {
            // ...
            // business logic removed
            // ...
            
            ToolTip = buttonData.Description,
            LongDescription = buttonData.Version
        };
    }
    catch (Exception e)
    {
        TaskDialog.Show("Ошибка",
            $@"Во время создания кнопки {buttonData.Name} {buttonData.Version} произошла ошибка.

{e.Message} | {e.StackTrace}");

        throw;
    }
}
~~~

### [ConnectLinesLogic class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/Engineering/ConnectLinesLogic.cs)

~~~
// 3.1 (4)
// random valid (big enough) bound value for getting lines intersections
const int boundValue = 1500;
const int boundValueNegative = -1500;

rotatedCurveFirst.MakeBound(boundValueNegative, boundValue);
rotatedCurveSecond.MakeBound(boundValueNegative, boundValue);
~~~

### [SheetLayout class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/Engineering/SheetLayout.cs)

~~~
// 3.1 (5)
/// <summary>
/// The class contains all the views placed in SheetInstance
/// </summary>
public class SheetLayout
{
    private readonly Document _document;
    public ViewDrafting Drafting { get; set; }
    
    public readonly ViewSheet Sheet;
    
    // ...
    // business logic removed
    // ...

    public readonly double Height;
    

    public SheetLayout(string sheetName, string sheetSizeTypeName, Document document)
    {
        _document = document;

        using (var txSheet = new Transaction(_document, "Создание листа"))
        {
            txSheet.Start();

            // ...
            // business logic removed
            // ...

            txSheet.Commit();
        }
    }

    // ...
    // business logic removed
    // ...
}
~~~

### [ElementCompoundStructure class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/Engineering/ElementCompoundStructure.cs)

~~~
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
~~~

### [OpeningsArea class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/Engineering/OpeningsArea.cs)

~~~
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

...

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
~~~

### [AssigningCodeWindow class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/Comments/Engineering/AssigningCodeWindow.cs)

~~~
// 3.2 (3)
// prev:
// public string Code; // назначаемый код
public string AssigningCodeValue;

public List<ElementId> CheckedElementIds;

SortedDictionary<string, SortedDictionary<string, List<Element>>> allElemsFilter;

public AssigningCodeWindow(Document doc, UIDocument uiDoc, IEnumerable<Element> selectedElems)
{
    // ...
    // business logic removed
    // ...

    allElemsFilter = new SortedDictionary<string, SortedDictionary<string, List<Element>>>();

    foreach (Element elem in selectedElems)
    {
        if (elem.Category == null)
            continue;

        if (!allElemsFilter.ContainsKey(elem.Category.Name))
    allElemsFilter.Add(elem.Category.Name, new SortedDictionary<string, List<Element>>());

        var allElemsType = new SortedDictionary<string, List<Element>>();
        allElemsFilter.TryGetValue(elem.Category.Name, out allElemsType);

        string name = elem.Name;

        if (elem.Category.Id == Category.GetCategory(doc, BuiltInCategory.OST_Parts).Id)
        {
            string nameType = "", nameMaterial = "";

            // ...
            // business logic removed
            // ...
        }

        if (!allElemsType.ContainsKey(name))
            allElemsType.Add(name, new List<Element>());

        List<Element> elemsOfType = new List<Element>();
        allElemsType.TryGetValue(name, out elemsOfType);

        if (elemsOfType == null)
            continue;

        elemsOfType.Add(elem);
    }
    
    // ...
    // business logic removed
    // ...

    // 3.2 (4)
    // prev:
    // ParamGuidSelected = new Guid("0e020998-2c76-4159-8ad5-66349dd3c19f"); // Navisworks
    var paramNavisworksGuid = new Guid("0e020998-2c76-4159-8ad5-66349dd3c19f");
    
    // 3.2 (5)
    // prev:
    // var allCat = allElemsFilter.Keys; // все категории (string)
    var allCategories = allElemsFilter.Keys;
    
    if (allElemsFilter.Count > 0)
    {
        ...
    }
~~~
