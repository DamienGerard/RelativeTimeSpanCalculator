using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace RelativeTimeSpanCalculator
{
    class Program
    {
        const int NumberOfMonthsPerYear = 12;
        const int NumberOfHoursPerDay = 24;
        const int NumberOfMinutesPerHour = 60;
        const int NumberOfSecondsPerMinute = 60;
        const int NumberOfMilliSecondsPerSecond = 1000;
        const int February = 2;
        readonly static ImmutableList<int> MonthsWith31Days = ImmutableList.Create<int>(new int[] { 1, 3, 5, 6, 8, 10, 12 });
        readonly static ImmutableList<int> MonthsWith30Days = ImmutableList.Create<int>(new int[] { 4, 7, 9, 11 });
        static void Main(string[] args)
        {
            //some quick test cases
            RelativeDateTimeDiff(new DateTime(2000, 10, 20, 0, 0, 0), new DateTime(2000, 10, 22, 2, 2, 2)).Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(Console.WriteLine);
            RelativeDateTimeDiff(new DateTime(2000, 09, 15, 0, 0, 0), new DateTime(2010, 03, 15, 23, 59, 59)).Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(Console.WriteLine);
        }

        static Dictionary<string, int> RelativeDateTimeDiff(DateTime startDate, DateTime endDate) {
            int years=0, months = 0, days = 0, hours = 0, minutes = 0, seconds = 0, milliSeconds;

            if (startDate>endDate) {
                DateTime temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            if (endDate.Year != startDate.Year){
                years = endDate.Year - startDate.Year - 1;
                if (endDate.Month >= startDate.Month) years++;
            }

            if (endDate.Month != startDate.Month) {
                months = (endDate.Month + NumberOfMonthsPerYear - startDate.Month - 1) % NumberOfMonthsPerYear;
                if (endDate.Day >= startDate.Day) months++;
            }

            if (endDate.Day != startDate.Day){
                days = (endDate.Day + TotalNumberOfDaysInCurrentMonth(startDate) - startDate.Day - 1) % TotalNumberOfDaysInCurrentMonth(startDate);
                if (endDate.Hour >= startDate.Hour) days++;
            }

            if (endDate.Hour != startDate.Hour) {
                hours = (endDate.Hour + NumberOfHoursPerDay - startDate.Hour - 1) % NumberOfHoursPerDay;
                if (endDate.Minute >= startDate.Minute) hours++;
            }

            if (endDate.Minute != startDate.Minute) {
                minutes = (endDate.Minute + NumberOfMinutesPerHour - startDate.Minute - 1) % NumberOfMinutesPerHour;
                if (endDate.Second >= startDate.Second) minutes++;
            }

            if (endDate.Second != startDate.Second) {
                seconds = (endDate.Second + NumberOfSecondsPerMinute - startDate.Second - 1) % NumberOfSecondsPerMinute;
                if (endDate.Millisecond >= startDate.Millisecond) seconds++;
            }
            
            if (endDate.Millisecond == startDate.Millisecond) milliSeconds = 0;
            else if (endDate.Millisecond > startDate.Millisecond) milliSeconds = endDate.Millisecond - startDate.Millisecond;
            else milliSeconds = (endDate.Millisecond + NumberOfMilliSecondsPerSecond - startDate.Millisecond) % NumberOfMilliSecondsPerSecond;

            return new Dictionary<string, int> { { "Years", years}, { "Months", months }, { "Days", days }, { "Hours", hours }, { "Minutes", minutes }, { "Seconds", seconds }, { "Milliseconds", milliSeconds } };
        }

        static int TotalNumberOfDaysInCurrentMonth(DateTime currentDate) {
            if (currentDate.Month == February)
                return 28 + ((currentDate.Year % 4 == 0) ? 1 : 0);
            else if (MonthsWith31Days.Contains(currentDate.Month))
                return 31;
            else if (MonthsWith30Days.Contains(currentDate.Month))
                return 30;
            else
                return 0;
        }
    }
}
