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
    /// Lógica de interacción para Body.xaml
    /// </summary>
    public partial class Body : UserControl
    {
        private string dayElementNamePrefix = "dayElement";
        private string dayResourceKeyPrefix = "dayResource";
        private const int NumberOfWeekDays = 7;
        private const int iterationOffSetInLoopFor = 1;
        private const int RowOffsetInGrid = 1;
        private const int ColumnOffsetInGrid = 1;
        private const int numberOfFirstDayInMonth = 1;
        private const int NumberOfCellsInGrid = 42;
        private DateTime displayedDate = (DateTime)App.Current.Resources["displayedDate"];

        public Body()
        {
            InitializeComponent();
            GenerateResourcesForDayElements();
            AssignValueToResources();
            FillCellsWithDayElements();
            HighLightWeekend();
        }

        private void FillCellsWithDayElements()
        {
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dynamicResourceName = dayResourceKeyPrefix + i.ToString();
                string dayElementName = dayElementNamePrefix + i.ToString();
                TextBlock dayElement = new TextBlock();
                dayElement.SetResourceReference(TextBlock.TextProperty, dynamicResourceName);
                dayElement.Name = dayElementName;
                Point gridCoordinates = GridCoordinates(i);
                dayElement.SetValue(Grid.ColumnProperty, (int)gridCoordinates.X);
                dayElement.SetValue(Grid.RowProperty, (int)gridCoordinates.Y);
                BodyGrid.Children.Add(dayElement);
            }
        }

        private void GenerateResourcesForDayElements()
        {
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dynamicResourceName = dayResourceKeyPrefix + i.ToString();
                string resourceKey = dynamicResourceName;
                string resourceValue = "";
                App.Current.Resources.Add(resourceKey, resourceValue);
            }
        }

        private void AssignValueToResources()
        {
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
                bool b = gridCoordinates.Y > 1 && gridCoordinates.Y < 5;
                bool c = gridCoordinates.Y == 5 && gridCoordinates.X + ColumnOffsetInGrid < lastDayGridColumnNumber;
                if (a || b || c)
                {
                    resourceValue = (i + iterationOffSetInLoopFor - firstDayGridColumnNumber ).ToString();
                }               
                App.Current.Resources[resourceKey] = resourceValue;
            }
        }

        private Point GridCoordinates(int iterationIndex)
        {
            int gridColumn = (iterationIndex) % NumberOfWeekDays;
            int gridRow = (iterationIndex / 7) + RowOffsetInGrid;
            Point gridCoordinates = new Point(gridColumn, gridRow);
            return gridCoordinates;
        }

        private void HighLightWeekend()
        {
            foreach (TextBlock dayElement in BodyGrid.Children)
            {
                int gridColumn = (int)dayElement.GetValue(Grid.ColumnProperty);
                if (gridColumn == 5 || gridColumn == 6)
                {
                    dayElement.Foreground = Brushes.Red;
                }
            }
        }
    }
}
