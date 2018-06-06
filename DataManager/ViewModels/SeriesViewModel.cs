using DataManager.Helpers;
using DataManager.Views;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.ViewModels
{
    public class SeriesViewModel : ViewModel
    {
        public SeriesCollection SeriesCollection { get; private set; }

        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private ZoomingOptions _zoomingMode;
        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                this.OnPropertyChanged();
            }
        }
        
        public SeriesViewModel()
        {
            this.SeriesCollection = new SeriesCollection();
            this.YFormatter = value => value.ToString("f");
            this.ZoomingMode = ZoomingOptions.X;
        }

        public void ClearSeries()
        {
            this.SeriesCollection.Clear();
        }

        public void ToggleZoomingMode()
        {
            switch (ZoomingMode)
            {
                case ZoomingOptions.None:
                    ZoomingMode = ZoomingOptions.X;
                    break;
                case ZoomingOptions.X:
                    ZoomingMode = ZoomingOptions.Y;
                    break;
                case ZoomingOptions.Y:
                    ZoomingMode = ZoomingOptions.Xy;
                    break;
                case ZoomingOptions.Xy:
                    ZoomingMode = ZoomingOptions.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddSeries()
        {
            AddSeriesWindow addSeriesWindow = new AddSeriesWindow();
            addSeriesWindow.SeriesCreated += AddSeriesWindow_SeriesCreated;
            addSeriesWindow.ShowDialog();
        }

        private void AddSeriesWindow_SeriesCreated(object sender, EventArgs e)
        {
            this.IsBusy = true;
            foreach (var item in (e as SeriesEventArgs).Series)
            {
                this.SeriesCollection.AddRange(item);
            }
            this.IsBusy = false;
        }
    }
}
