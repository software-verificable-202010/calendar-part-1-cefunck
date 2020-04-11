using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Calendar
{
    /// <summary>
    /// Lógica de interacción para Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public Navbar()
        {
            InitializeComponent();
            App.Current.Resources["displayedDate"] = DateTime.Now;
            this.Resources["monthAndYear"] = ((DateTime)App.Current.Resources["displayedDate"]).ToString("MMMM yyyy");
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["displayedDate"] = ((DateTime)App.Current.Resources["displayedDate"]).AddMonths(-1);
            this.Resources["monthAndYear"] = ((DateTime)App.Current.Resources["displayedDate"]).ToString("MMMM yyyy");
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["displayedDate"] = ((DateTime)App.Current.Resources["displayedDate"]).AddMonths(1);
            this.Resources["monthAndYear"] = ((DateTime)App.Current.Resources["displayedDate"]).ToString("MMMM yyyy");
        }
    }
}
