using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace CleanCode.VariableBindingTimes
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
            _connectingMode = !_connectingMode;
            if (!_connectingMode)
                return;

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

                    // (1)
                    // in-place binding
                    // why: variable describes tolerance value, which is always constant
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