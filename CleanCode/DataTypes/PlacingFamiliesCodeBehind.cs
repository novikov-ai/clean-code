using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;

namespace CleanCode.DataTypes
{
    public class PlacingFamiliesCodeBehind
    {
        private bool _sketchMode = false;
        private Room _selectedRoom = null;
        private PickedBox _selectedBox = null;
        private FamilySymbol _selectedFamilyType = null;

        private int _rows = 0;
        private int _columns = 0;

        private int _rotationAngle = 0;
        private double _heightFromFloor = 0;

        private readonly Document _doc;
        private readonly UIDocument _uiDoc;

        private RevitLinkInstance _rvtLink;

        private (double, double) _xMinMax;
        private (double, double) _yMinMax;
        private (double, double) _zMinMax;

        private ViewPlan _activeView;

        private void ButtonBase_OnClick()
        {
            if (_selectedFamilyType is null || _rows == -1 || _columns == -1 || _rotationAngle == -1 ||
                _selectedRoom is null && !_sketchMode)
                return;

            var familySymbol = new FilteredElementCollector(_doc)
                .OfClass(typeof(FamilySymbol))
                .WhereElementIsElementType()
                .Cast<FamilySymbol>()
                .FirstOrDefault(s => s.Id == _selectedFamilyType.Id);

            using (Transaction tx = new Transaction(_doc, "Расстановка оборудования"))
            {
                tx.Start();

                var level = _doc.GetElement(_activeView.GenLevel.Id) as Level;

                for (int i = 1; i <= _columns; i++)
                {
                    var divX = (_xMinMax.Item2 - _xMinMax.Item1) / _columns * i;
                    var placementX = _xMinMax.Item1 + divX - divX / (i * 2);

                    for (int j = 1; j <= _rows; j++)
                    {
                        var divY = (_yMinMax.Item2 - _yMinMax.Item1) / _rows * j;
                        var placementY = _yMinMax.Item1 + divY - divY / (j * 2);

                        var placementPoint = new XYZ(placementX, placementY, _zMinMax.Item1);

                        if (!_sketchMode && !_selectedRoom.IsPointInRoom(placementPoint))
                            continue;

                        if (!familySymbol.IsActive)
                            familySymbol.Activate();

                        var instance = _doc.Create.NewFamilyInstance(
                            placementPoint, familySymbol, level, level, StructuralType.NonStructural);

                        if (!_sketchMode)
                            ElementTransformUtils.MoveElement(_doc, instance.Id, _rvtLink.GetTransform().Origin);

                        if (_rotationAngle != 0 || _rotationAngle != 360)
                        {
                            try
                            {
                                if (!_sketchMode)
                                    placementPoint = (instance.Location as LocationPoint).Point;

                                var angle = Math.PI / 180 * _rotationAngle;

                                ElementTransformUtils.RotateElement(_doc, instance.Id,
                                    Line.CreateBound(placementPoint,
                                        new XYZ(placementPoint.X, placementPoint.Y, placementPoint.Z + 10)),
                                    angle);
                            }
                            catch (Exception exception)
                            {
                            }
                        }

                        if (_heightFromFloor != 0)
                        {
                            try
                            {
                                foreach (Parameter parameter in instance.Parameters)
                                {
                                    // (11)
                                    // prev: 
                                    // if (parameter.Definition != null && !parameter.IsReadOnly &&
                                    //     parameter.Definition.Name == "Offset from Host" ||
                                    //     parameter.Definition.Name == "Смещение от главной модели")
                                    // improved: created a boolean variable
                                    bool isParameterModifiable = parameter.Definition != null && !parameter.IsReadOnly;
                                    bool isParameterNameOffset = parameter.Definition.Name == "Offset from Host" || 
                                                                 parameter.Definition.Name == "Смещение от главной модели";
                                    
                                    if (isParameterModifiable && isParameterNameOffset)
                                    {
                                        // (12)
                                        // prev: parameter.Set(_heightFromFloor / 304.8);
                                        // improved: created a double constant variable instead of magical number
                                        const double multiplierFtToMm = 1 / 304.8;
                                        
                                        parameter.Set(_heightFromFloor * multiplierFtToMm);
                                        break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                throw;
                            }
                        }
                    }
                }

                tx.Commit();
            }
        }
    }
}