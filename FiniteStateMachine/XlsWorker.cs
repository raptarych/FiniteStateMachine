using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemBox.Spreadsheet;

namespace FiniteStateMachine
{
    class XlsWorker
    {
        public Dictionary<string,Dictionary<string,string>> Read(string path)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var excelFile = ExcelFile.Load(path);
            var workSheet = excelFile.Worksheets.FirstOrDefault();
            if (workSheet == null) return null;

            var table = new Dictionary<string, Dictionary<string, string>>();

            foreach (var row in workSheet.Rows)
            {
                var rowState = "";
                foreach (var cell in row.AllocatedCells)
                {
                    if (row.Index == 0)
                    {
                        if (!(cell.Value is string) || (string) cell.Value == "State") continue;
                        table[cell.StringValue] = new Dictionary<string, string>();
                        continue;
                    }

                    var columnNameValue = cell.Column.Cells.FirstOrDefault()?.Value;
                    if (columnNameValue is string)
                    {
                        var columnName = columnNameValue as string;
                        if (columnName == "State")
                        {
                            rowState = cell.StringValue;
                            continue;
                        }

                        table[columnName][rowState] = cell.StringValue;
                    }
                }
            }

            return table;
        }
    }
}
