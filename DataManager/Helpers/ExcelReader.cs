using DataManager.Models;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataManager.Helpers
{
    public static class ExcelReader
    {
        public static List<ColumnData> GetData(string fileName)
        {
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;extended properties=\"excel 8.0;hdr=no;IMEX=1\";data source={0}", fileName);

            var adapter = new OleDbDataAdapter("SELECT * FROM [Лист1$]", connectionString);
            var dataSet = new DataSet();

            adapter.Fill(dataSet, "Values");

            var data = dataSet.Tables["Values"].AsEnumerable();
            return TransformData(data.ToList());
        }

        private static List<ColumnData> TransformData(List<DataRow> inputData)
        {
            List<ColumnData> outputData = new List<ColumnData>();
            for (int column = 0; column < inputData[0].ItemArray.Length; column++)
            {
                ColumnData columnData = new ColumnData();
                columnData.Column = column;
                for (int row = 0; row < inputData.Count; row++)
                {
                    if (!(inputData[row].ItemArray[column] is DBNull))
                    {
                        columnData.Values.Add(Convert.ToDouble(inputData[row].ItemArray[column]));
                    }
                }
                outputData.Add(columnData);
            }
            return outputData;
        }
    }
}
