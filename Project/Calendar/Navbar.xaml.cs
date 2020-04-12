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
        private const string DayNumberResourceKeyPrefix = "dayResource";
        private const string DayNumberResourceBlankValue = "";
        private const int IterationIndexOffset = 1;
        private const int GridRowIndexOffset = 1;
        private const int DaysInWeek = 7;
        private const int FirstDayNumberInMonth = 1;
        private const int GridColumnIndexOffset = 1;
        private const int NumberOfCellsInGrid = 42;
        private const int NumberOfMonthsToAdvance = 1;
        private const int NumberOfMonthToGoBack = -1;

        public Navbar()
        {
            InitializeComponent();
            SetDisplayedDateResourceValue(DateTime.Now);
            AssignValueToMonthAndYearResource(GetDisplayedDateResourceValue());
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateToDisplay = GetDisplayedDateResourceValue().AddMonths(NumberOfMonthToGoBack);
            SetDisplayedDateAndAssingAllResources(dateToDisplay);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateToDisplay = GetDisplayedDateResourceValue().AddMonths(NumberOfMonthsToAdvance);
            SetDisplayedDateAndAssingAllResources(dateToDisplay);
        }

        private void SetDisplayedDateAndAssingAllResources(DateTime dateToDisplay)
        {
            SetDisplayedDateResourceValue(dateToDisplay);
            AssignValueToMonthAndYearResource(GetDisplayedDateResourceValue());
            AssingValuesToDayNumberResources(GetDisplayedDateResourceValue());
        }

        private void AssignValueToMonthAndYearResource(DateTime date)
        {
            App.Current.Resources["monthAndYear"] = date.ToString(NavBarMonthFormat);
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

        private Point GetGridCoordinatesByIterationIndex(int iterationIndex)
        {
            int gridColumn = (iterationIndex) % DaysInWeek;
            int gridRow = (iterationIndex / DaysInWeek) + GridRowIndexOffset;
            return new Point(gridColumn, gridRow); ;
        }

        private DateTime GetDisplayedDateResourceValue()
        {
            return (DateTime)App.Current.Resources["displayedDate"];
        }

        private void SetDisplayedDateResourceValue(DateTime dateToDisplay)
        {
            App.Current.Resources["displayedDate"] = dateToDisplay;
        }

    }
}
