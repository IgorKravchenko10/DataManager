using DataManager.Helpers;
using DataManager.Models;
using LiveCharts.Geared;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DataManager.ViewModels
{
    public class AddSeriesViewModel : ViewModel
    {
        public List<string> Paths { get; set; }

        private string _title = "";
        public string SeriesTitle
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    this.OnPropertyChanged("SeriesTitle");
                }
            }
        }

        private Brush _color;
        public Brush Color
        {
            get
            {
                return _color;
            }
            set
            {
                if (value != _color)
                {
                    _color = value;
                    this.OnPropertyChanged("Color");
                }
            }
        }

        public AddSeriesViewModel() : base()
        {
            this.Paths = new List<string>();
        }

        public List<List<LineSeries>> CreateSeries()
        {
            List<List<LineSeries>> fullList = new List<List<LineSeries>>();
            List<Task<List<ColumnData>>> tasks = new List<Task<List<ColumnData>>>();

            foreach (string pathItem in this.Paths)
            {
                if (!String.IsNullOrEmpty(pathItem) && !String.IsNullOrEmpty(this.SeriesTitle) && this.Color != null)
                {
                    tasks.Add(new Task<List<ColumnData>>(() =>
                    {
                        System.Diagnostics.Debug.WriteLine("Started");
                        List<ColumnData> columns;
                        columns = ExcelReader.GetData(pathItem);
                        System.Diagnostics.Debug.WriteLine("End");
                        return columns;
                    }));
                }
                else
                {
                    throw new Exception("Please fill all inputs");
                }
            }
            foreach (var task in tasks)
            {
                task.Start();
            }
            var resultTask = Task.WhenAll(tasks);

            List<ColumnData> columnData = new List<ColumnData>();
            for (int i = 0; i < resultTask.Result.Length; i++)
            {
                columnData.AddRange(resultTask.Result[i]);
            }
            var series = this.GetSeries(columnData);
            fullList.Add(series);
            this.Paths.Clear();
            return fullList;
        }

        private List<LineSeries> GetSeries(List<ColumnData> columns)
        {

            List<LineSeries> lineSeriesCollection = new List<LineSeries>();
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (ColumnData column in columns)
                {
                    LineSeries lineSeries = new LineSeries()
                    {
                        Title = column.Column.ToString(),
                        //Values = values.AsGearedValues().WithQuality(Quality.Low),
                        Values = column.Values.AsChartValues(),
                        Fill = Brushes.Transparent,
                        Stroke = this.Color,
                        StrokeThickness = .5,
                    };
                    lineSeriesCollection.Add(lineSeries);
                }
            });

            return lineSeriesCollection;
        }
    }
}
