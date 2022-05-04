using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CleanCode.VariableValues.AutoViews
{
    public class ViewCreator
    {
        private AutoViewsType _viewType;

        private UIDocument _uiDocument;
        private Document _document;

        private bool? HeaderAutoColumnOption = null;

        private List<Element> _selectedElements = new List<Element>();

        private BoundingBoxXYZ _boundingBoxOfSelected;
        private BoundingBoxXYZ _boundingBoxOfSelectedSolid;

        private ElementId _levelIdOfSelected;

        private string _markValueOfSelectedElems;
        private string _placeholderPrefix = "Префикс";
        private string _projectDiscValue;

        // optional reinforcement
        private bool _isViewPlansSymbolDownloaded = false;
        private bool _isViewSectionsSymbolDownloaded = false;
        private FamilySymbol _selectedSymbolRs08 = null;
        private FamilySymbol _selectedSymbolRs01 = null;
        private FamilySymbol _selectedSymbolRs03 = null;
        private FamilySymbol _selectedSymbolDot = null;
        private FamilySymbol _selectedSymbolLine = null;

        // auto column values
        private FamilySymbol _displayedAutoColumnSymbol = null;
        private FamilySymbol _newAutoColumnSymbol = null;
        private bool _isAutoColumnSymbolDownloaded = false;

        private void ButtonCreateViews_OnClick()
        {
            if (!_selectedElements.Any())
            {
                TaskDialog.Show("Warning", "Выберите элементы!");
                return;
            }

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
                        case AutoViewsType.Columns:
                        {
                            if (_displayedAutoColumnSymbol is null)
                            {
                                TaskDialog.Show("Warning", "Выберите или создайте новый типоразмер автоколонны!");
                                return;
                            }

                            sheetsCreated = CreateSheetsColumn();
                            break;
                        }
                        case AutoViewsType.Walls:
                        {
                            sheetsCreated = CreateSheetsWalls();
                            break;
                        }
                        default:
                        {
                            return;
                        }
                    }

                    if (!sheetsCreated)
                    {
                        TaskDialog.Show("Warning", @"Лист создан не был!

Проверьте настройки выбранных видов.");

                        txGroup.RollBack();
                        return;
                    }

                    txGroup.Assimilate();
                }
            }
            catch (Exception exception)
            {
                TaskDialog.Show("Error", exception.Message);
            }
        }

        private bool CreateSheetsSlabs()
        {
            // ...
            // business logic removed
            // ...

            return true;
        }

        private bool CreateSheetsColumn()
        {
            // ...
            // business logic removed
            // ...

            var createdSheet = new SheetLayout("place_holder", "place_holder", _document);

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

            // ...
            // business logic removed
            // ...

            return true;
        }

        private void AutoColumnFamilySelect_OnClick()
        {
            // ...
            // business logic removed
            // ...

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
            catch (Exception exception)
            {
                TaskDialog.Show("Warning", $@"Произошла ошибка.
        
        Подробнее: {exception.Message}");
            }
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

        private bool CreateSheetsWalls()
        {
            // ...
            // business logic removed
            // ...

            return true;
        }


        private void AssignProjDiscToViews(IEnumerable<View> createdViews)
        {
            if (string.IsNullOrEmpty(_projectDiscValue))
                return;

            using (var txDiscSet = new Transaction(_document, "Назначение раздела проекта"))
            {
                txDiscSet.Start();

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

                txDiscSet.Commit();
            }
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

        private ObservableCollection<ViewTemplate> GetViewsTemplate(string templateNameStarts)
        {
            // ...
            // business logic removed
            // ...

            return new ObservableCollection<ViewTemplate>();
        }

        private ViewDrafting GetViewDrafting(string name)
        {
            // ...
            // business logic removed
            // ...

            return null;
        }

        private IEnumerable<ViewSchedule> CreateSchedules(PlacementManager placementManager)
        {
            // ...
            // business logic removed
            // ...

            return new List<ViewSchedule>();
        }

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
    }
}