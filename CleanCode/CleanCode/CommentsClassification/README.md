# About

Correct comments are classified:

- informative comments, which could explain context (but always try to explain context through function names)

- representation of intentions

- clarification (useful when you can't rename function)

- warn of consequences (eg. long tests - a few hours)

- magnification of used something (methods, etc.)

- TODO comments

~~~
// example format

// X
// <comment type>
code usage
~~~

### [BasicCommand abstract class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CommentsClassification/BasicCommand.cs)

~~~
// 1
// informative comment
/// <summary>
/// Use this method to set up modeless window.
/// </summary>
/// <param name="window"></param>
protected void SetUpModeless(Window window)
{
    _isWindowModal = false;

    if (_window is null)
    {
        _window = window;
        
        // ...
        // business logic removed
        // ...
    }
    else
    {
        // ...
        // business logic removed
        // ...

        _window.WindowState = WindowState.Normal;
    }
}

// 2
// informative comment
/// <summary>
/// Use this method to turn off statistics module.
/// </summary>
protected void TurnOffStatistics()
{
    _collectStats = false;
}
~~~

### [Constants class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CommentsClassification/Engineering/Constants.cs)

~~~
// 3
// clarification
// unit conversion coeff - 1 feet = 304.8 mm
public const double FtToMm = 304.8;

// 4
// clarification
// instance parameter - dimension along Y axis
public const string InstanceParamDim1= "Dim1";

// 5
// clarification
// instance parameter - dimension along X axis
public const string InstanceParamDim2 = "Dim2";
~~~

### [SetArmatureBySlab class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CommentsClassification/Engineering/SetArmatureBySlab.cs)

~~~
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

...

using (Transaction tx = new Transaction(doc, $"Reinforcing along floor id {selectedSlab.Id}"))
{
    ...

    foreach (Curve curve in externalCurveArray)
    {
        ...

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
~~~

### [CropViews class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CommentsClassification/Engineering/CropViews.cs)

~~~
protected void RunFunc(ExternalCommandData commandData)
{
    ...
    
    foreach (View view in views)
    {
        try
        {
            view.CropBoxActive = true;
    
            var viewCropManager = view.GetCropRegionShapeManager();
            viewCropManager.SetCropShape(curveLoop);
    
            view.CropBoxVisible = false;
        }
    
        // 8
        // TODO comments
        // add other shapes support (not only rectangle) to the next release
        catch (Autodesk.Revit.Exceptions.InvalidOperationException e)
        {
            throw new WarningException(
                "You can crop only rectangle shape.");
        }
    }
    
    ...
}

...

// 9
// informative comments
// check if two views have the same orientation
private bool IsRightDirectionMatch(XYZ rightDirection, XYZ benchmark)
{
    if (Math.Abs(rightDirection.X * benchmark.X) == 1)
        return true;
    if (Math.Abs(rightDirection.Y * benchmark.Y) == 1)
        return true;
    if (Math.Abs(rightDirection.Z * benchmark.Z) == 1)
        return true;

    return false;
}
~~~

### [Monitor class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CommentsClassification/Monitor/MonitorWindow.cs)

~~~
private async void BtnExport_OnClick(object sender, RoutedEventArgs e)
{
    try
    {
        // 10
        // TODO comments
        // todo: add other raw formats and design separate class for SaveFileDialog
        const string exportedFormat = "csv";
        var dialog = new SaveFileDialog()
        {
            Filter = $"{exportedFormat} files (*.{exportedFormat})||All files (*.*)|*.*",
            FileName = $"statistics_{DateTime.Now.ToString("yy-MM-dd")}.{exportedFormat}"
        };

        ...
    }
    catch (Exception exception)
    {
        MessageBox.Show(exception.Message);
    }
}

...

private void BtnGetLast180_OnClick(object sender, RoutedEventArgs e)
{
    // 11
    // warn of consequences
    // use carefully, if UpdateData(...) takes more than 100 days it could take a while
    // and freeze UI 
    UpdateData(180);
}

...

private void SetDisplayedStatistics(IEnumerable<(Statistic, Plugin, Panel)> statistics)
{
    ...

    foreach (var groupByPlugin in grouped)
    {
        foreach (var (st, pl, pn) in groupByPlugin)
        {
            // 12
            // magnification of used something
            // symbol "!" used for handling errors
            // if you want edit that, take care of changing Statistics creating logic
            var displayedEntity = new DisplayedEntity
            {
                Name = pl.Name,
                Version = pl.Version,
                Panel = pn.Name,
                Launches = 1,
                Errors = st.Result.StartsWith("!") ? 1 : 0
            };
            ...
        }
    }
}
~~~
