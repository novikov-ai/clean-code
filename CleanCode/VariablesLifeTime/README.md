# About

Refactored 15 bad variable usage according best practices:

- minimize variables scope and extend it if you really need it

- localize variable usage 

- group related commands into separate methods

~~~
// example format

// (X)
// code usage
...
// (X)
// improved: <something>
code usage

OR

// (X)
// prev: <old code>
// improved: <something>
code usage
~~~

### [RightOffset class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariablesLifeTime/RightOffset.cs)

~~~
// (1)
// improved: changed scope of local variable usage
private Document _document;

// (2)
// improved: changed scope of local variable usage
const double MultiplierMmToFt = 0.00328084;
const double OffsetValueMin = 3500 * MultiplierMmToFt;

public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
{
   ...
    
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
        
        ...

        // (3)
        // prev:
        // ICollection<Element> allElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
        //     .WhereElementIsNotElementType().Excluding(allLevels).ToElements();

        ...

        // (4)
        // prev:
        // List<ElementId> wrongOffsetElems = new List<ElementId>();

        ...

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
            
            ...
        }
        ...
    }
    ...
}  
~~~

### [ImportDelete class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariablesLifeTime/ImportDelete.cs)

~~~
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
    
    ...
}
~~~

### [OpeningsArea class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariablesLifeTime/OpeningsArea.cs)

~~~
(double width, double height) = (0, 0);

if (item.Parameters != null)
{
    // (8)
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
    // (9)
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
~~~

### [StairsVolume class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariablesLifeTime/StairsVolume.cs)

~~~
// (10)
// prev:
// var allStairsLandings = new FilteredElementCollector(doc)
//      .OfCategory(BuiltInCategory.OST_StairsLandings)
//      .WhereElementIsNotElementType().ToElements();

...

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

        ...
    }
}

// (10)
// improved: grouped variable declaration and it's usage
var allStairsLandings = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_StairsLandings)
    .WhereElementIsNotElementType().ToElements();
    
if (allStairsLandings.Count > 0)
{
    ...
}
~~~

### [AutoViewsWindow class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariablesLifeTime/Views/AutoViewsWindow.cs)

~~~
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

private AutoColumnBlockedParams GetBlockedParams()
{
    ...

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
        ...
    }

    return new AutoColumnBlockedParams(Math.Round(width), Math.Round(length), concreteClass);
}
~~~