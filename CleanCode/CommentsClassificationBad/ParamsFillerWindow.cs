using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using System.Globalization;

namespace CleanCode.CommentsClassificationBad
{
    public class ParamsFillerWindow
    {
        private Document _doc;
        private List<Parameter> _elementParams;
        private SortedDictionary<string, SortedDictionary<string, List<Element>>> _allElementsTree;
        private ICollection<Element> _allMaterialsTree;

        private TextBox _selectedTextBox;
        private List<Element> _seletedElements;

        private Guid _selectedParam;

        public ParamsFillerWindow(SortedDictionary<string, SortedDictionary<string, List<Element>>> elems,
            ICollection<Element> materials, Document doc)
        {
            // ...
            // business logic removed
            // ...

            _doc = doc;
            _allElementsTree = elems;
            _allMaterialsTree = materials;

            // 13 (4. noise)
            var allCategories = elems.Keys; // все категории (string)
            // renamed variable allCat collection to allCategories and removed bad comment

            if (elems.Count > 0)
            {
                // ...
                // business logic removed
                // ...
            }

            // ...
            // business logic removed
            // ...
            
           
            try
            {
                // 14 (3. unreliable comments)
                // Заложить выбор базы данных под классификатор
                // <bad comment was removed>
                
                var codes = GetListOfCodes(
                    "https://placeholder.com/api/Classifier.getList.json");

                CultureInfo rusLocal = new CultureInfo("ru-RU");
                
                // ...
                // business logic removed
                // ...
            }
            catch (Exception e)
            {
               throw new Exception("Проверьте подключение к интернету", e);
            }
            
            // ...
            // business logic removed
            // ...
        }

        private IEnumerable<string> GetListOfCodes(string url)
        {
            // ...
            // business logic removed
            // ...
            
            return null;
        }

        // ...
        // business logic removed
        // ...

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
}