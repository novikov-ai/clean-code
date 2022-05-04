# About

Refactored 15 bad variable usage according best practices:

- declare variables explicitly

- initialize every variable in place where you declare it or before you use it firstly

- assign invalid values after variable usage

- variables at cycles initialize directly before cycle itself

- check the important variables for valid values (if values are invalid - throw warning)


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

### [AocDay1 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableValues/AocDay1.cs)

~~~
// (1)
// int sum;
...
// (1)
// improved: moved variable initializing to declaration place
int sum = Convert.ToInt32(item) + Convert.ToInt32(elem);
...
~~~

### [AocDay5 class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableValues/AocDay5.cs)

~~~
// (2)
// int mySeatId = -1;
...
// (2)
// improved: moved variable initializing directly before cycle
int mySeatId = -1;
foreach (int item in AllNums)
{
    if (!allSeatIds.Contains(item)) // my seat is missing in the list
        mySeatId = item;
}
...
~~~

### [TableVisibility class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableValues/TableVisibility.cs)

~~~
// (3)
// prev: var tableSectionData = viewSchedule.GetTableData().GetSectionData(SectionType.Body);
// improved: declared variables explicitly
TableSectionData tableSectionData = viewSchedule.GetTableData().GetSectionData(SectionType.Body);

...

// (4)
// prev: var viewScheduleDef = viewSchedule.Definition;
// improved: declared variables explicitly
ScheduleDefinition viewScheduleDef = viewSchedule.Definition;

// (5)
// int fieldsAmount = viewScheduleDef.GetFieldCount();

using (Transaction tx = new Transaction(doc, "Скрыть столбцы со значением ячеек равными нулю"))
{
    ...
    
    // (5)
    // improved: moved variable initializing directly before cycle
    int fieldsAmount = viewScheduleDef.GetFieldCount();
    for (int i = 0; i < fieldsAmount; i++)
    {
        // (6)
        // prev: var field = viewScheduleDef.GetField(i);
        // improved: declared variables explicitly
        ScheduleField field = viewScheduleDef.GetField(i);

        ...
    }
    ...
}
~~~

### [ViewCreator class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableValues/AutoViews/ViewCreator.cs)

~~~
// (7)
// var sheetsCreated = false;
try
{
    using (var txGroup = new TransactionGroup(_document, "Автоформирование листов"))
    {
        txGroup.Start();

        // (7)
        // improved: moved variable initializing to declaration place
        var sheetsCreated = false;
        switch (_viewType)
        {
            case AutoViewsType.Slabs:
            {
                sheetsCreated = CreateSheetsSlabs();
                break;
            }
            
            ...
        }
        
    ...
    
}

...

private bool CreateSheetsColumn()
{
    ...
    
    using (Transaction tx = new Transaction(_document, "Создание чертежного вида"))
    {
        tx.Start();
    
        // (8)
        // prev: var createdViewDrafting = GetViewDrafting("place_holder");
        // improved: declared variables explicitly
        ViewDrafting createdViewDrafting = GetViewDrafting("place_holder");
    
        if (tx.Commit() == TransactionStatus.Committed)
            createdSheet.Drafting = createdViewDrafting;
        else
            throw new Exception("Чертежный вид создан не был!");
    }

    ... 

}

...

private void AutoColumnFamilySelect_OnClick()
{
    ...
    
    // (9)
    // Family family;
    try
    {
        using (Transaction tx = new Transaction(_document, "Загрузка семейства Автоколонны"))
        {
            tx.Start();
    
            // (9)
            // improved: moved variable initializing to declaration place
            if (_document.LoadFamily("place_holder", out Family family))
            {
                tx.Commit();
    
                // ...
                // business logic removed
                // ...
            }
            else
            {
                throw new Exception("Не удалось загрузить семейство.");
            }
        }
    }
   ...
}

private AutoColumnBlockedParams GetBlockedParams()
{
    // (10)
    // prev: var element = _selectedElements.FirstOrDefault();
    // improved: declared variables explicitly
    Element element = _selectedElements.FirstOrDefault();
    if (element is null)
        return null;

    double length = 0, width = 0;
    int concreteClass = 0;

    // ...
    // business logic removed
    // ...

    return new AutoColumnBlockedParams(width, length, concreteClass);
}

...

foreach (View view in createdViews)
{
    // (11)
    // prev: var param = view.get_Parameter(Constants.GuidProjectDisc);
    // improved: declared variables explicitly
    Parameter param = view.get_Parameter(new Guid());
    if (param is null || param.IsReadOnly)
        continue;

    param.Set(_projectDiscValue);
}

private List<ViewSchedule> PlaceSchedulesOnSheet(SheetLayout sheetLayout)
{
    using (var txSchedules = new Transaction(_document, "Перенос спецификаций на лист"))
    {
        txSchedules.Start();

        // (12)
        // prev: var createdSchedules = CreateSchedules(new PlacementManager(_document, sheetLayout.Sheet,
        //          "place_holder", _markValueOfSelectedElems, sheetLayout.Height)).ToList();
        // improved: declared variables explicitly
        List<ViewSchedule> createdSchedules = CreateSchedules(new PlacementManager(_document, sheetLayout.Sheet,
            "place_holder", _markValueOfSelectedElems, sheetLayout.Height)).ToList();

        if (!createdSchedules.Any())
        {
            throw new Exception("Отметьте хотя бы 1 спецификацию!");
}

        txSchedules.Commit();
        return createdSchedules;
    }
}

private void SetViewTemplates(string templateName)
{
    // (13)
    // prev: var templates = GetViewsTemplate(templateName);
    // improved: declared variables explicitly
    ObservableCollection<ViewTemplate> templates = GetViewsTemplate(templateName);
    var selectedTemplate = templates.FirstOrDefault(template => template.Name == templateName);

    // ...
    // business logic removed
    // ...
}

...

private Line GetViewSectionVerticalLine(ViewSection viewSection, BoundingBoxXYZ boundingBoxXyz,
    double sideOffset = 0)
{
    var rightDirectionX = viewSection.RightDirection.X;

    // (14)
    // prev:
    // if (rightDirectionX == 0)
    //     var offset = boundingBoxXyz.Max.Y - boundingBoxXyz.Min.Y - sideOffset;
    // else
    //     var offset = boundingBoxXyz.Max.X - boundingBoxXyz.Min.X - sideOffset;
    // improved: checked the important variables for valid values and throw exception if invalid

    var offset = rightDirectionX switch
    {
        -1 => boundingBoxXyz.Max.X - boundingBoxXyz.Min.X - sideOffset,
        0 => boundingBoxXyz.Max.Y - boundingBoxXyz.Min.Y - sideOffset,
        _ => throw new Exception("Not handled ViewSection case")
    };

    // (15)
    // prev:
    // if (rightDirectionX == 0)
    // {
    //     return Line.CreateBound(
    //         new XYZ(viewSection.Origin.X, boundingBoxXyz.Min.Y + offset, boundingBoxXyz.Min.Z),
    //         new XYZ(viewSection.Origin.X, boundingBoxXyz.Min.Y + offset, boundingBoxXyz.Max.Z));
    // }
    //
    // return Line.CreateBound(
    //     new XYZ(boundingBoxXyz.Min.X + offset, viewSection.Origin.Y, boundingBoxXyz.Min.Z),
    //     new XYZ(boundingBoxXyz.Min.X + offset, viewSection.Origin.Y, boundingBoxXyz.Max.Z));
    // improved: checked the important variables for valid values and throw exception if invalid
    
    return rightDirectionX switch
    {
        -1 => Line.CreateBound(
            new XYZ(boundingBoxXyz.Min.X + offset, viewSection.Origin.Y, boundingBoxXyz.Min.Z),
            new XYZ(boundingBoxXyz.Min.X + offset, viewSection.Origin.Y, boundingBoxXyz.Max.Z)),
        0 => Line.CreateBound(
            new XYZ(viewSection.Origin.X, boundingBoxXyz.Min.Y + offset, boundingBoxXyz.Min.Z),
            new XYZ(viewSection.Origin.X, boundingBoxXyz.Min.Y + offset, boundingBoxXyz.Max.Z)),
        _ => throw new Exception("Not handled ViewSection case")
    };
}
~~~