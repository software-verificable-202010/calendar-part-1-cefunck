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
    /// <summary>App.Current
    /// Lógica de interacción para Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        private const string NavBarMonthFormat = "MMMM yyyy";
        private const int GridRowIndexOffset = 1;
        private const int DaysInWeek = 7;
        public Navbar()
        {
            
            InitializeComponent();
            App.Current.Resources["displayedDate"] = DateTime.Now;
            App.Current.Resources["monthAndYear"] = ((DateTime)App.Current.Resources["displayedDate"]).ToString(NavBarMonthFormat);
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["displayedDate"] = ((DateTime)App.Current.Resources["displayedDate"]).AddMonths(-1);
            App.Current.Resources["monthAndYear"] = ((DateTime)App.Current.Resources["displayedDate"]).ToString(NavBarMonthFormat);
            AssignValueToResources((DateTime)App.Current.Resources["displayedDate"]);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["displayedDate"] = ((DateTime)App.Current.Resources["displayedDate"]).AddMonths(1);
            App.Current.Resources["monthAndYear"] = ((DateTime)App.Current.Resources["displayedDate"]).ToString(NavBarMonthFormat);            
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
                Point gridCoordinates = GetGridCoordinatesByIterationIndex(i);
                bool a = gridCoordinates.Y == 1 && gridCoordinates.X + ColumnOffsetInGrid > firstDayGridColumnNumber;
                bool b = gridCoordinates.Y > 1 && ((i + iterationOffSetInLoopFor - firstDayGridColumnNumber) <= numberOfDaysOfDisplayedMonth);
                if (a || b)
                {
                    resourceValue = (i + iterationOffSetInLoopFor - firstDayGridColumnNumber).ToString();
                }
                App.Current.Resources[resourceKey] = resourceValue;
            }
        }

        private Point GetGridCoordinatesByIterationIndex(int iterationIndex)
        {
            int gridColumn = (iterationIndex) % DaysInWeek;
            int gridRow = (iterationIndex / DaysInWeek) + GridRowIndexOffset;
            return new Point(gridColumn, gridRow); ;
        }
    }
}
