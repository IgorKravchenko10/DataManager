using DataManager.Models;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataManager.Helpers
{
    public class ExcelReader : IDisposable
    {
        Excel.Application _excelApp;
        Excel.Workbook _excelWorkbook;
        Excel._Worksheet _excelWorksheet;
        Excel.Range _excelRange;

        public ExcelReader(string fileName, int sheet = 1)
        {
            _excelApp = new Excel.Application();
            _excelWorkbook = _excelApp.Workbooks.Open(fileName);
            _excelWorksheet = _excelWorkbook.Sheets[sheet];
            _excelRange = _excelWorksheet.UsedRange;
        }        

        public List<ColumnData> GetData()
        {
            List<ColumnData> series = new List<ColumnData>();
            int rowCount = _excelRange.Rows.Count;
            int columnCount = _excelRange.Columns.Count;
            
            for (int i = 1; i <= columnCount; i++)
            {
                series.Add(this.GetColumn(rowCount, i));
            }
            return series;
        }

        private ColumnData GetColumn(int rowCount, int column)
        {
            ColumnData columnData = new ColumnData();
            for (int j = 1; j <= rowCount; j++)
            {
                if (_excelRange.Cells[j, column] != null && _excelRange.Cells[j, column].Value2 != null)
                {
                    columnData.Values.Add(_excelRange.Cells[j, column].Value2);
                    columnData.Column = column;
                }
            }
            return columnData;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(_excelRange);
            Marshal.ReleaseComObject(_excelWorksheet);

            _excelWorkbook.Close();
            Marshal.ReleaseComObject(_excelWorkbook);

            _excelApp.Quit();
            Marshal.ReleaseComObject(_excelApp);
        }
    }
}
