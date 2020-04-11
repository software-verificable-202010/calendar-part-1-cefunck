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
        private Brush highlightColor = Brushes.Red;
        private DateTime displayedDate = (DateTime)App.Current.Resources["displayedDate"];
        private const string DayElementNamePrefix = "dayElement";
        private const string DayNumberResourceKeyPrefix = "dayResource";
        private const string DayNumberResourceBlankValue = "";
        private const int DaysInWeek = 7;
        private const int IterationIndexOffset = 1;
        private const int GridRowIndexOffset = 1;
        private const int GridColumnIndexOffset = 1;
        private const int FirstDayNumberInMonth = 1;
        private const int NumberOfCellsInGrid = 42;
        private const int SaturdayGridColumnIndex = 5;
        private const int SundayGridColumnIndex = 6;

        public Body()
        {
            InitializeComponent();
            GenerateDayNumberResources();
            AssingValuesToDayNumberResources();
            CreateAndInsertDayElementsToGrid();
            HighLightWeekends();
        }
       
        private void GenerateDayNumberResources()
        {
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dayNumberResourceKey = DayNumberResourceKeyPrefix + i.ToString();
                string dayNumberResourceValue = DayNumberResourceBlankValue;
                App.Current.Resources.Add(dayNumberResourceKey, dayNumberResourceValue);
            }
        }

        private void AssingValuesToDayNumberResources()
        {
            DateTime firstDayOfDisplayedMonth = new DateTime(displayedDate.Year, displayedDate.Month, FirstDayNumberInMonth);
            int numberOfDaysOfDisplayedMonth = DateTime.DaysInMonth(displayedDate.Year, displayedDate.Month);
            int firstDayGridColumnIndex = (int)(firstDayOfDisplayedMonth.DayOfWeek) - GridColumnIndexOffset;

            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dayNumberResourceKey = DayNumberResourceKeyPrefix + i.ToString();
                string dayNumberResourceValue = DayNumberResourceBlankValue;
                int candidateDayNumber = i - firstDayGridColumnIndex + IterationIndexOffset;
                Point dayElementGridCoordinates = GetGridCoordinatesByIterationIndex(i);
                bool isFirstRow = dayElementGridCoordinates.Y == 1;
                bool isDisplayableColumnOfFirstRow = dayElementGridCoordinates.X >= firstDayGridColumnIndex;
                bool isDisplayableDayElementOfFirstRow = isFirstRow && isDisplayableColumnOfFirstRow;
                bool isNotFirstRow = dayElementGridCoordinates.Y > 1;
                bool isCandidateDayNumberInDisplayedMonth = candidateDayNumber <= numberOfDaysOfDisplayedMonth;
                bool isDisplayableDayElementOfRemainsRows = isNotFirstRow && isCandidateDayNumberInDisplayedMonth;
                if (isDisplayableDayElementOfFirstRow || isDisplayableDayElementOfRemainsRows)
                {
                    dayNumberResourceValue = candidateDayNumber.ToString();
                }
                App.Current.Resources[dayNumberResourceKey] = dayNumberResourceValue;
            }
        }

        private void CreateAndInsertDayElementsToGrid()
        {
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                TextBlock dayElement = new TextBlock();                
                dayElement.Name = DayElementNamePrefix + i.ToString();
                string dayNumberResourceKey = DayNumberResourceKeyPrefix + i.ToString();
                dayElement.SetResourceReference(TextBlock.TextProperty, dayNumberResourceKey);
                Point dayElementGridCoordinates = GetGridCoordinatesByIterationIndex(i);
                dayElement.SetValue(Grid.ColumnProperty, (int)dayElementGridCoordinates.X);
                dayElement.SetValue(Grid.RowProperty, (int)dayElementGridCoordinates.Y);
                BodyGrid.Children.Add(dayElement);
            }
        }

        private void HighLightWeekends()
        {
            foreach (TextBlock dayElement in BodyGrid.Children)
            {
                int dayElementGridColumnIndex = (int)dayElement.GetValue(Grid.ColumnProperty);
                if (dayElementGridColumnIndex == SaturdayGridColumnIndex || dayElementGridColumnIndex == SundayGridColumnIndex)
                {
                    dayElement.Foreground = highlightColor;
                }
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
