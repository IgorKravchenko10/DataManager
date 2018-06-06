using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataManager.Views
{
    /// <summary>
    /// Interaction logic for PathControl.xaml
    /// </summary>
    public partial class PathControl : UserControl, INotifyPropertyChanged
    {

        public string Path { get; set; }

        public PathControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                this.Path = openFileDialog.FileName;
                this.OnAdded(this, new EventArgs());
                this.OnPropertyChanged("Path");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnAdded(object sender, EventArgs e)
        {
            this.Added?.Invoke(this, e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Removed?.Invoke(this, e);
        }

        public event EventHandler Removed;
        public event EventHandler Added;
    }    
}
