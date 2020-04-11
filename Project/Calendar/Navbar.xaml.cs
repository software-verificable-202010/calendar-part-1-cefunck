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
            AssignValueToResources((DateTime)App.Current.Resources["displayedDate"]);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["displayedDate"] = ((DateTime)App.Current.Resources["displayedDate"]).AddMonths(1);
            this.Resources["monthAndYear"] = ((DateTime)App.Current.Resources["displayedDate"]).ToString("MMMM yyyy");
            //App.Current.Resources["dayResource0"] = "20";
            AssignValueToResources((DateTime)App.Current.Resources["displayedDate"]);
        }

        private void AssignValueToResources(DateTime displayedDate)
        {
            const int NumberOfCellsInGrid = 42;
            const int ColumnOffsetInGrid = 1;
            const int iterationOffSetInLoopFor = 1;
            const string dayResourceKeyPrefix = "dayResource";

            DateTime firstDateOfMonth = new DateTime(displayedDate.Year, displayedDate.Month, 1);
            int numberOfDaysOfDisplayedMonth = DateTime.DaysInMonth(displayedDate.Year, displayedDate.Month);
            int firstDayGridColumnNumber = (int)(firstDateOfMonth.DayOfWeek) - 1;
            int lastDayGridColumnNumber = (int)firstDateOfMonth.AddDays(numberOfDaysOfDisplayedMonth).DayOfWeek;

            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dynamicResourceName = dayResourceKeyPrefix + i.ToString();
                string resourceKey = dynamicResourceName;
                string resourceValue = "";
                Point gridCoordinates = GridCoordinates(i);
                bool a = gridCoordinates.Y == 1 && gridCoordinates.X + ColumnOffsetInGrid > firstDayGridColumnNumber;
                bool b = gridCoordinates.Y > 1 && ((i + iterationOffSetInLoopFor - firstDayGridColumnNumber) <= numberOfDaysOfDisplayedMonth);
                if (a || b)
                {
                    resourceValue = (i + iterationOffSetInLoopFor - firstDayGridColumnNumber).ToString();
                }
                App.Current.Resources[resourceKey] = resourceValue;
            }
        }

        private Point GridCoordinates(int iterationIndex)
        {
            const int NumberOfWeekDays = 7;
            const int RowOffsetInGrid = 1;
            int gridColumn = (iterationIndex) % NumberOfWeekDays;
            int gridRow = (iterationIndex / 7) + RowOffsetInGrid;
            Point gridCoordinates = new Point(gridColumn, gridRow);
            return gridCoordinates;
        }
    }
}
