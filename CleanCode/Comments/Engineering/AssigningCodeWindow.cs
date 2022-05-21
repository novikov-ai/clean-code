using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CleanCode.Comments.Engineering
{
    public class AssigningCodeWindow
    {
        // ...
        // business logic removed
        // ...

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
                TreeNode rootFilter = new TreeNode("Элементы на активном виде", null, null, new List<Element>());
                rootFilter.IsExpanded = true;
                Tree treeFilter = new Tree(rootFilter);

                int count = 0;
                foreach (string cat in allCategories)
                {
                    TreeNode catNode = new TreeNode(cat, count, null, new List<Element>());
                    treeFilter.AddChild(catNode, rootFilter);
                    count++;

                    var allElemsTypeOfCategory = new SortedDictionary<string, List<Element>>();
                    allElemsFilter.TryGetValue(cat, out allElemsTypeOfCategory);

                    foreach (KeyValuePair<string, List<Element>> kvp in allElemsTypeOfCategory)
                    {
                        string name = kvp.Key;

                        string[] nameMaterial = name.Split('|');

                        TreeNode typeElem = new TreeNode(name, null, count, kvp.Value);
                        if (nameMaterial.Length > 1)
                        {
                            typeElem.Name = nameMaterial[0];
                            typeElem.Tooltip = nameMaterial[1];
                        }
                        else
                        {
                            var element = kvp.Value.FirstOrDefault();
                            if (element is FamilyInstance familyInstance)
                            {
                                typeElem.Tooltip = familyInstance.Symbol.Family.Name;
                            }
                        }

                        treeFilter.AddChild(typeElem, catNode);
                    }

                    foreach (var node in catNode.GetChildren())
                    {
                        catNode.AllElements.AddRange(node.AllElements);
                    }
                }

                foreach (var node in rootFilter.GetChildren())
                {
                    rootFilter.AllElements.AddRange(node.AllElements);
                }

                var allElems = new ObservableCollection<TreeNode>();
                allElems.Add(rootFilter);
            }
            
            // ...
            // business logic removed
            // ...

        }

        // ...
        // business logic removed
        // ...
    }
}