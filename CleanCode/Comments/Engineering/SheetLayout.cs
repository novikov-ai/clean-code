using Autodesk.Revit.DB;

namespace CleanCode.Comments.Engineering
{
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
}