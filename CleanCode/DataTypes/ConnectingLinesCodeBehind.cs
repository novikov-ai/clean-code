using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace CleanCode.DataTypes
{
    public class ConnectingLinesCodeBehind
    {
        private int _selectedAngle = 45;

        private bool _isRightAngle = false;

        private readonly Document _doc;
        private readonly UIDocument _uiDoc;

        private bool _connectingMode = false;

        private void ConnectLines()
        {
            _connectingMode = !_connectingMode;
            if (!_connectingMode)
                return;

            while (_connectingMode)
            {
                var leadingElement = _doc.GetElement(
                    _uiDoc.Selection.PickObject(ObjectType.Element).ElementId);

                var secondElement = _doc.GetElement(
                    _uiDoc.Selection.PickObject(ObjectType.Element).ElementId);

                if (leadingElement is null || secondElement is null)
                    continue;

                var leadingLineType = _doc.GetElement(leadingElement.GetTypeId());

                var leadingLineCurve = (leadingElement.Location as LocationCurve)?.Curve;
                var secondLineCurve = (secondElement.Location as LocationCurve)?.Curve;

                if (leadingLineCurve is null || secondLineCurve is null)
                    continue;

                var leadingEndPoints = new List<XYZ>
                    {leadingLineCurve.GetEndPoint(0), leadingLineCurve.GetEndPoint(1)};
                var secondEndPoints = new List<XYZ>
                    {secondLineCurve.GetEndPoint(0), secondLineCurve.GetEndPoint(1)};

                XYZ leadingEndPoint = null;
                XYZ secondEndPoint = null;

                double minDiff = 0;
                foreach (XYZ point in leadingEndPoints)
                {
                    foreach (XYZ pointSecond in secondEndPoints)
                    {
                        var diff = Math.Abs(point.DistanceTo(pointSecond));
                        if (diff < minDiff || minDiff == 0)
                        {
                            leadingEndPoint = point;
                            secondEndPoint = pointSecond;

                            minDiff = diff;
                        }
                    }
                }

                XYZ secondEndPointFarAway =
                    secondEndPoints[secondEndPoint == secondEndPoints[0] ? 1 : 0];

                if (leadingEndPoint is null || secondEndPoint is null)
                    continue;

                var leadingLine = leadingLineCurve as Line;
                var secondLine = secondLineCurve as Line;

                var intersectionPoint = ConnectLinesLogic.GetIntersectionPoint(
                    leadingLine, secondLine, _selectedAngle,
                    leadingEndPoint, secondEndPoint, secondEndPointFarAway);

                if (intersectionPoint is null)
                    continue;

                using (Transaction tx = new Transaction(_doc, "Соединение элементов под углом"))
                {
                    tx.Start();

                    LocationCurve locationCurveSecondElement = secondElement.Location as LocationCurve;
                    try
                    {
                        locationCurveSecondElement.Curve = Line.CreateBound(secondEndPointFarAway, intersectionPoint);
                    }
                    catch (Exception)
                    {
                        locationCurveSecondElement.Curve = Line.CreateBound(intersectionPoint, secondEndPointFarAway);
                        throw;
                    }

                    var leadMepCurve = leadingElement as MEPCurve;
                    var secondMepCurve = secondElement as MEPCurve;
                    
                    var leadConnectorSet = leadMepCurve.ConnectorManager.Connectors;
                    var secondConnectorSet = secondMepCurve.ConnectorManager.Connectors;

                    var levelId = leadMepCurve.ReferenceLevel.Id;
                    if (levelId is null)
                        continue;

                    var connectors = ConnectLinesLogic
                        .GetLeadAndSecondConnectors(leadConnectorSet, secondConnectorSet);

                    MEPCurve mepCurve = null;
                    ConnectorSet connectorSet = null;

                    switch (leadingElement.GetType().Name)
                    {
                        case nameof(Duct):
                        {
                            mepCurve = Duct.Create(_doc, leadingLineType.Id,
                                levelId, connectors.Item1, connectors.Item2);
                            break;
                        }
                        case nameof(Pipe):
                        {
                            mepCurve = Pipe.Create(_doc, leadingLineType.Id,
                                levelId, connectors.Item1, connectors.Item2);
                            break;
                        }
                        case nameof(CableTray):
                        {
                            mepCurve = CableTray.Create(_doc, leadingLineType.Id,
                                connectors.Item1.Origin, connectors.Item2.Origin,
                                levelId);
                            break;
                        }
                        case nameof(Conduit):
                        {
                            mepCurve = Conduit.Create(_doc, leadingLineType.Id,
                                connectors.Item1.Origin, connectors.Item2.Origin,
                                levelId);
                            break;
                        }
                        default:
                        {
                            return;
                        }
                    }

                    var leadingElemWidthHeightDiameter = GetParametersWidthHeightDiameter(leadingElement);

                    double width = leadingElemWidthHeightDiameter.Item1?.AsDouble() ?? 0;
                    double height = leadingElemWidthHeightDiameter.Item2?.AsDouble() ?? 0;
                    double diameter = leadingElemWidthHeightDiameter.Item3?.AsDouble() ?? 0;

                    if (mepCurve != null)
                        connectorSet = mepCurve.ConnectorManager.Connectors;
                    else
                        return;

                    // (10)
                    // prev: bool isLeadOffsetFromSecond = Math.Abs(leadingEndPoint.Z - secondEndPoint.Z) > 0.01;
                    // improved: created a double constant variable instead of magical number
                    const double offsetTolerance = 0.01;
                    
                    bool isLeadOffsetFromSecond = Math.Abs(leadingEndPoint.Z - secondEndPoint.Z) > offsetTolerance;
                    if (_isRightAngle && isLeadOffsetFromSecond)
                        (width, height) = (height, width);

                    var mepCurveWidthHeightDiameter = GetParametersWidthHeightDiameter(mepCurve);

                    mepCurveWidthHeightDiameter.Item1?.Set(width);
                    mepCurveWidthHeightDiameter.Item2?.Set(height);
                    mepCurveWidthHeightDiameter.Item3?.Set(diameter);

                    try
                    {
                        var fittingFirst = ConnectLinesLogic
                            .GetLeadAndSecondConnectors(leadConnectorSet, connectorSet);
                        _doc.Create.NewElbowFitting(fittingFirst.Item1, fittingFirst.Item2);

                        var fittingSecond = ConnectLinesLogic
                            .GetLeadAndSecondConnectors(secondConnectorSet, connectorSet);
                        _doc.Create.NewElbowFitting(fittingSecond.Item1, fittingSecond.Item2);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            _doc.Create.NewElbowFitting(connectors.Item1, connectors.Item2);
                            _doc.Delete(mepCurve.Id);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    tx.Commit();
                }
            }
        }

        private (Parameter, Parameter, Parameter) GetParametersWidthHeightDiameter(Element mepElement)
        {
            switch (mepElement.GetType().Name)
            {
                case nameof(Duct):
                {
                    var width = mepElement.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM);
                    var height = mepElement.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM);
                    return (width, height, null);
                }
                case nameof(Pipe):
                {
                    var diameter = mepElement.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM);
                    return (null, null, diameter);
                }
                case nameof(CableTray):
                {
                    var width = mepElement.get_Parameter(BuiltInParameter.RBS_CABLETRAY_WIDTH_PARAM);
                    var height = mepElement.get_Parameter(BuiltInParameter.RBS_CABLETRAY_HEIGHT_PARAM);
                    return (width, height, null);
                }
                case nameof(Conduit):
                {
                    var diameter = mepElement.get_Parameter(BuiltInParameter.RBS_CONDUIT_DIAMETER_PARAM);
                    return (null, null, diameter);
                }
                default:
                {
                    return (null, null, null);
                }
            }
        }
    }
}