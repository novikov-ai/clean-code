using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace CleanCode.VariableBindingTimes.Engineering
{
    public class LinesConnector
    {
        private int _selectedAngle = 45;

        private bool _isRightAngle = false;

        private readonly Document _doc;
        private readonly UIDocument _uiDoc;

        private bool _connectingMode = false;

        private void Connect()
        {
            var window = new LinesConnectorWindow();
            
            // ...
            // business logic removed
            // ...
            
            _connectingMode = !_connectingMode;
            if (!_connectingMode)
                return;
            
            
            // (1)
            // in-place binding
            // why: variable not needed at this case,
            // auto property (ConnectionDirectionX) is set only once and doesn't change during the execution
            window.ConnectionDirectionX = 1;
            window.ShowUserDialog();
            
            // ...
            // business logic removed
            // ...

            while (_connectingMode)
            {
                var leadingElement = _doc.GetElement(_uiDoc.Selection.PickObject(ObjectType.Element).ElementId);

                XYZ leadingEndPoint = null;
                XYZ secondEndPoint = null;

                // ...
                // business logic removed
                // ...

                using (Transaction tx = new Transaction(_doc, "Соединение элементов под углом"))
                {
                    tx.Start();

                    // ...
                    // business logic removed
                    // ...

                    var leadingElemWidthHeightDiameter = GetParametersWidthHeightDiameter(leadingElement);

                    double width = leadingElemWidthHeightDiameter.Item1?.AsDouble() ?? 0;
                    double height = leadingElemWidthHeightDiameter.Item2?.AsDouble() ?? 0;
                    
                    const double offsetTolerance = 0.01;

                    bool isLeadOffsetFromSecond = Math.Abs(leadingEndPoint.Z - secondEndPoint.Z) > offsetTolerance;

                    if (_isRightAngle && isLeadOffsetFromSecond)
                        (width, height) = (height, width);

                    // ...
                    // business logic removed
                    // ...

                    tx.Commit();
                }
            }
        }

        private (Parameter, Parameter, Parameter) GetParametersWidthHeightDiameter(Element mepElement)
        {
            // ...
            // business logic removed
            // ...

            return (null, null, null);
        }
    }
}