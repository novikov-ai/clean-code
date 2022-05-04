using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autodesk.Revit.Attributes;

namespace CleanCode.VariableValues
{
    [Transaction(TransactionMode.Manual)]
    public class TableVisibility
    {
        protected void Hide(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            if (doc.ActiveView is ViewSchedule == false)
            {
                throw new WarningException("Откройте спецификацию!");
            }

            var viewSchedule = (ViewSchedule) doc.ActiveView;

            // (3)
            // prev: var tableSectionData = viewSchedule.GetTableData().GetSectionData(SectionType.Body);
            // improved: declared variables explicitly
            TableSectionData tableSectionData = viewSchedule.GetTableData()
                .GetSectionData(SectionType.Body);

            int firstRowNumber = tableSectionData.FirstRowNumber,
                lastRowNumber = tableSectionData.LastRowNumber;

            // (4)
            // prev: var viewScheduleDef = viewSchedule.Definition;
            // improved: declared variables explicitly
            ScheduleDefinition viewScheduleDef = viewSchedule.Definition;
            
            // (5)
            // int fieldsAmount = viewScheduleDef.GetFieldCount();

            using (Transaction tx = new Transaction(doc, "Скрыть столбцы со значением ячеек равными нулю"))
            {
                tx.Start();

                bool hideColumns = false;
                int skippedHiddenFields = 0;

                // (5)
                // improved: moved variable initializing directly before cycle
                int fieldsAmount = viewScheduleDef.GetFieldCount();
                for (int i = 0; i < fieldsAmount; i++)
                {
                    // (6)
                    // prev: var field = viewScheduleDef.GetField(i);
                    // improved: declared variables explicitly
                    ScheduleField field = viewScheduleDef.GetField(i);

                    if (field.IsHidden)
                    {
                        skippedHiddenFields++;
                        continue;
                    }

                    List<string> rowValues = null;

                    for (int j = firstRowNumber; j <= lastRowNumber; j++)
                    {
                        string cellFieldValue =
                            viewSchedule.GetCellText(SectionType.Body, j, i - skippedHiddenFields);

                        if (cellFieldValue == field.ColumnHeading)
                        {
                            rowValues = new List<string>();
                            continue;
                        }

                        rowValues?.Add(cellFieldValue);
                    }

                    if (rowValues != null && rowValues.Count > 0 &&
                        rowValues.All(cell => cell == "0"))
                    {
                        field.IsHidden = true;
                        hideColumns = true;
                    }
                }

                if (hideColumns)
                    tx.Commit();
            }
        }
    }
}