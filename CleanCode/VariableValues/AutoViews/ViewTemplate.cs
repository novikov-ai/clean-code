using Autodesk.Revit.DB;

namespace CleanCode.VariableValues.AutoViews
{
    internal class ViewTemplate
    {
        public readonly View View;
        public string Name => View.Name;
        
        public ViewTemplate(View view)
        {
            View = view;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
