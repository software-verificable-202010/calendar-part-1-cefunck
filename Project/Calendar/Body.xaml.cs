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
            AssingValuesToDayNumberResources(GetDisplayedDateResourceValue());
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

        private void AssingValuesToDayNumberResources(DateTime displayedDate)
        {
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dayNumberResourceKey = DayNumberResourceKeyPrefix + i.ToString();
                string dayNumberResourceValue = DayNumberResourceBlankValue;
                int candidateDayNumber = i - GetfirstDayGridColumnIndex() + IterationIndexOffset;
                Point dayElementGridCoordinates = GetGridCoordinatesByIterationIndex(i);
                if (IsDayNumberInDisplayedMonth(candidateDayNumber, dayElementGridCoordinates))
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

        private bool IsDayNumberInDisplayedMonth(int candidateDayNumber, Point dayElementGridCoordinates)
        {
            DateTime displayedDate = GetDisplayedDateResourceValue();
            bool isFirstDayRow = dayElementGridCoordinates.Y == 1;
            bool isNotFirstDayRow = dayElementGridCoordinates.Y > 1;
            bool isFirstDayColumnOrLater = dayElementGridCoordinates.X >= GetfirstDayGridColumnIndex();
            bool isDisplayableDayElementOfFirstRow = isFirstDayRow && isFirstDayColumnOrLater;
            bool isCandidateDayNumberInDisplayedMonth = candidateDayNumber <= GetNumberOfDaysOfMonth(displayedDate);
            bool isDisplayableDayElementOfRemainsRows = isNotFirstDayRow && isCandidateDayNumberInDisplayedMonth;
            return (isDisplayableDayElementOfFirstRow || isDisplayableDayElementOfRemainsRows);
        }

        private int GetNumberOfDaysOfMonth(DateTime date)
        {
            DateTime displayedDate = date;
            int numberOfDaysOfDisplayedMonth = DateTime.DaysInMonth(displayedDate.Year, displayedDate.Month);
            return numberOfDaysOfDisplayedMonth;
        }

        private int GetfirstDayGridColumnIndex()
        {
            DateTime displayedDate = GetDisplayedDateResourceValue();
            DateTime firstDayOfDisplayedMonth = new DateTime(displayedDate.Year, displayedDate.Month, FirstDayNumberInMonth);
            int firstDayGridColumnIndex = (int)(firstDayOfDisplayedMonth.DayOfWeek) - GridColumnIndexOffset;
            return firstDayGridColumnIndex;
        }

        private DateTime GetDisplayedDateResourceValue()
        {
            return (DateTime)App.Current.Resources["displayedDate"];
        }

        private Point GetGridCoordinatesByIterationIndex(int iterationIndex)
        {
            int gridColumn = (iterationIndex) % DaysInWeek;
            int gridRow = (iterationIndex / DaysInWeek) + GridRowIndexOffset;
            return new Point(gridColumn, gridRow); ;
        }
    }
}
