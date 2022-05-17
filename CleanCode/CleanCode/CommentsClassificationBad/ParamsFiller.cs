using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace CleanCode.CommentsClassificationBad
{
    [Transaction(TransactionMode.Manual)]
    public class ParamsFiller : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var selectedFilePath = SetUpSharedParamsFilePath(doc);
            if (string.IsNullOrEmpty(selectedFilePath))
                return Result.Failed;

            try
            {
                AddSharedParams(doc);
            }
            catch (Exception)
            {
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private string GetPathFromShowDialog()
        {
            var openedFile = new FileOpenDialog("txt file (*.txt)|*.txt|All files (*.*)|*.*");

            return openedFile.Show() == ItemSelectionDialogResult.Confirmed
                ? ModelPathUtils.ConvertModelPathToUserVisiblePath(openedFile.GetSelectedModelPath())
                : null;
        }

        public string SetUpSharedParamsFilePath(Document doc)
        {
            // ...
            // business logic removed
            // ...
            
            return doc.Application.SharedParametersFilename;
        }

        public void AddSharedParams(Document doc)
        {
            DefinitionFile sharedFile = doc.Application.OpenSharedParameterFile();

            if (sharedFile is null)
            {
                return;
            }

            DefinitionGroups defGroups = sharedFile.Groups;

            Definitions defsSKKS = null, defsRevit = null, defsNavis = null;

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
    }
}