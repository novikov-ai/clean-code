# About

Bad comments are classified:

1. not obvious comments - the code and comment have to link each other

2. mumbling - take your time to write a best of possible comment

3. unreliable comments - comments have to be true

4. noise - don't state the obvious writing comment (it has to be a new information)

5. position markers - use it wisely, don't use it very often

6. don't comment after closing bracket - it's better to shorten your functions

7. redundant enormous comments

8. too many information (don't include irrelevant details)

9. non-local information - comment has to describe code nearby

10. necessary comments (if framework guide-style demands to comment every variable - it's stupid and useless)

11. commented code (!) - it's terrible, don't do it ever

12. don't use comments where you could use a function or variable (it's better to create a compact function with 1 operation than header with detailed comment)

~~~
// example format

// X (Y. <comment type>
// bad comment
// new comment
~~~

### [Params class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CommentsClassificationBad/ParamsFiller.cs)

~~~
public void AddSharedParams(Document doc)
{
    ...

    // 1 (2. mumbling)
    // устаревшие параметры
    // bad comment renamed to: "Параметры, которые в новых моделях не используются."
    
    Definition defNavis = null, defSectionNum = null, defOpenings = null, defRtype = null;

    // 2 (2. mumbling)
    // параметры для элементов
    // <bad comment was removed>
    
    Definition
        defDiscipline = null,
        defPhase = null,
        defBuildingType = null,
        defBuildingNum = null,
        defBuildingName = null,
        defVolume = null,
        defSkksVolume = null;

    // 3 (2. mumbling)
    // параметры для проекта
    // <bad comment was removed>
    
    Definition
        defCode = null,
        defSection = null,
        defLevel = null,
        defLevelType = null,
        defGroup = null;

    // ...
    // business logic removed
    // ...
    
    
    // 4 (4. noise)
    // получение всех категорий проекта
    // <bad comment was removed>
    
    Categories allCats = doc.Settings.Categories;
    
    // 5 (11. commented code)
    //string cats = "Сведения о проекте;Виды;Облака точек";
    //string cats = "Антураж;";
    // <bad comment was removed>
    
    string includeCats =
        "Ребра плит;Подвески из базы данных производителя MEP;Система коммутации;Электрические приборы;Трубопроводные системы;Электрооборудование;Трубы;Электрические цепи;Окна;Элементы герметизации из базы данных производителя MEP;Колонны;Фермы;Обобщенные модели;Несущая арматура;Материалы;Соединительные детали коробов;Крыши;Спринклеры;Проемы для шахты;Арки моста;Оборудование;Перекрытия;Гибкие трубы;Ограждение;Топография;Устройства связи;Каркас несущий;Антураж;Соединительные детали трубопроводов;Соединительные детали кабельных лотков;Воздуховоды;Соединители несущей арматуры;Импосты витража;Части;Наборы оборудования;Арматура трубопроводов;Армирование по траектории несущей конструкции;Устройства вызова и оповещения;Группы модели;Воздухораспределители;Трубопровод по осевой;Охранная сигнализация;Несущие колонны;Формы;Парковка;Настилы моста;Короба;Специальное оборудование;Стены;Опоры моста;Фундамент несущей конструкции;Мебель;Материалы внутренней изоляции воздуховодов;Дорожки;Потолки;Кабельные лотки;Осветительные приборы;Форма арматурного стержня;Воздуховоды по осевой;Фермы моста;Озеленение;Генплан;Арматурная сетка несущей конструкции;Соединительные детали воздуховодов;Системы воздуховодов;Балочные системы;Панели витража;Трубы из базы данных производителя MEP;Комплекты мебели;Шкафы;Витражные системы;Участки кабельного лотка;Армирование по площади несущей конструкции;Пожарная сигнализация;Гибкие воздуховоды;Элементы воздуховодов из базы данных производителя MEP;Провода;Материалы изоляции труб;Телефонные устройства;Арматура воздуховодов;Лестницы;Пандус;Материалы изоляции воздуховодов;Сантехнические приборы;Двери;Выключатели";

    // 6 (11. commented code)
    //List<string> excludeCats = new List<string>(cats.Split(';'));
    // <bad comment was removed>
    
    CategorySet categorySet = new CategorySet();

    foreach (Category cat in allCats)
    {
        if (includeCats.Contains(cat.Name))
            categorySet.Insert(cat);
    }

    categorySet.Insert(Category.GetCategory(doc, BuiltInCategory.OST_StairsRuns));
    categorySet.Insert(Category.GetCategory(doc, BuiltInCategory.OST_StairsLandings));
    categorySet.Insert(Category.GetCategory(doc, BuiltInCategory.OST_EdgeSlab));

    // 7 (4. noise)
    // связка на экземпляр элемента
    // <bad comment was removed>
    
    InstanceBinding instBindingElems = new InstanceBinding(categorySet);

    // 8 (4. noise)
    // категории для сведений о проекте
    // <bad comment was removed>
    
    CategorySet projectCatSet = new CategorySet();

    Category projectCat = null;
    try
    {
        projectCat = doc.Settings.Categories.get_Item("Сведения о проекте");
    }
    catch (Exception e)
    {
        projectCat = doc.Settings.Categories.get_Item("Project Information");
    }

    projectCatSet.Insert(projectCat);
    InstanceBinding instBindingProject = new InstanceBinding(projectCatSet);

    // 9 (3. unreliable comments)
    // площадь проема
    // bad comment renamed to: "Добавления параметра Площадь Проема к категориям Окна."
    CategorySet winCatSet = new CategorySet();

    Category winCat = null;

    try
    {
        winCat = doc.Settings.Categories.get_Item("Окна");
    }
    catch (Exception e)
    {
        winCat = doc.Settings.Categories.get_Item("Windows");
    }

    winCatSet.Insert(winCat);
    InstanceBinding instBindingWindows = new InstanceBinding(winCatSet);

    BindingMap docBindingMap = doc.ParameterBindings;

    // 10 (4. noise)
    // выбор категорий для параметра Объем
    // <bad comment was removed>
    CategorySet categorySetVolume = new CategorySet();
    categorySetVolume.Insert(Category.GetCategory(doc, BuiltInCategory.OST_StairsRuns));
    categorySetVolume.Insert(Category.GetCategory(doc, BuiltInCategory.OST_StairsLandings));

    // 11 (3. unreliable comments)
    // объем для контекстных моделей
    // bad comment renamed to: "Добавление параметра Объем для моделей контекста."
    // and moved closer to code usage
    var contextModelsBuiltInCategories = new List<BuiltInCategory>
    {
        BuiltInCategory.OST_StructuralFoundation,
        BuiltInCategory.OST_Columns,
        BuiltInCategory.OST_StructuralColumns,
        BuiltInCategory.OST_Ramps,
        BuiltInCategory.OST_Floors,
        BuiltInCategory.OST_Walls,
        BuiltInCategory.OST_Parts
    };

    var contextModelsCategories = contextModelsBuiltInCategories
        .Select(bc => Category.GetCategory(doc, bc))
        .ToList();

    var categorySetVolumeToContext = new CategorySet();
    
    contextModelsCategories
        .ForEach(c => categorySetVolumeToContext.Insert(c));

    // 12 (4. noise)
    // связка на экземпляр элемента
    // <bad comment was removed>
    InstanceBinding instBindingStairs = new InstanceBinding(categorySetVolume);
    InstanceBinding instBindingContexts = new InstanceBinding(categorySetVolumeToContext);

    using (Transaction tx = new Transaction(doc, "Добавление параметров из ФОП"))
    {
        tx.Start();
        
        // ...
        // business logic removed
        // ...
        
        tx.Commit();
    }
}
~~~

### [ParamsFillerWindow class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/CommentsClassificationBad/ParamsFillerWindow.cs)

~~~
public class ParamsFillerWindow
{
    ...

    public ParamsFillerWindow(SortedDictionary<string, SortedDictionary<string, List<Element>>> elems,
        ICollection<Element> materials, Document doc)
    {
        ...

        // 13 (4. noise)
        var allCategories = elems.Keys; // все категории (string)
        // renamed variable allCat collection to allCategories and removed bad comment

        ...
        
        try
        {
            // 14 (3. unreliable comments)
            // Заложить выбор базы данных под классификатор
            // <bad comment was removed>
            
            var codes = GetListOfCodes(
                "https://placeholder.com/api/Classifier.getList.json");

            CultureInfo rusLocal = new CultureInfo("ru-RU");
            
            ...
        }
        
        ...
    }

   ...

    // 15 (11. commented code)
    // private void TbMaterials_OnSelectionChanged(object sender, RoutedEventArgs e)
    // {
    //     if (TbElements is null || TbMaterials is null)
    //         return;
    //     
    //     TbElements.Foreground = Brushes.DarkGray;
    //     TbElements.FontSize = 14;
    //
    //     TbMaterials.Foreground = Brushes.Black;
    //     TbMaterials.FontSize = 18;
    // }
    // <bad comment was removed>
}
~~~