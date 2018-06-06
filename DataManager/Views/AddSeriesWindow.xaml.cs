using DataManager.ViewModels;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataManager.Views
{
    /// <summary>
    /// Interaction logic for AddSeriesWindow.xaml
    /// </summary>
    public partial class AddSeriesWindow : Window
    {
        AddSeriesViewModel _viewModel;
        public AddSeriesWindow()
        {
            InitializeComponent();
            _viewModel = new AddSeriesViewModel();
            this.DataContext = _viewModel;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Input.Cursors.Wait;
                var series = await Task.Factory.StartNew(() => _viewModel.CreateSeries());
                this.SeriesCreated.Invoke(this, new SeriesEventArgs()
                {
                    Series = series
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), ex.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        public event EventHandler SeriesCreated;

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorButton.Background = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                _viewModel.Color = colorButton.Background;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            PathControl pathControl = new PathControl();
            pathControl.Removed += PathControl_Removed;
            pathControl.Added += PathControl_Added;
            this.paths.Children.Add(pathControl);
        }

        private void PathControl_Removed(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this.paths.Children.Remove(sender as PathControl);
            }
        }

        private void PathControl_Added(object sender, EventArgs e)
        {
            _viewModel.Paths.Add((sender as PathControl).Path);
        }
    }

    public class SeriesEventArgs : EventArgs
    {
        public List<List<LineSeries>> Series;
    }
}
