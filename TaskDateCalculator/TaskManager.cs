using System;
using System.Collections.Generic;

namespace TaskDateCalculator
{
    public class TaskManager
    {
        private readonly Dictionary<int, HashSet<int>> Holidays = new Dictionary<int, HashSet<int>>();

        public TaskManager(List<DateTime> holidays)
        {
            Holidays = PopulateHolidays(holidays);
        }

        /// <summary>
        /// Returns new DateTime that adds specifiend number of minutes to the given date and time
        /// </summary>
        /// <param name="startDate">DateTime start date</param>
        /// <param name="minutes">Number of minutes to add</param>
        /// <returns>A DateTime object whose value is sum of start date and time and given number of minutes counting only valid working hours</returns>
        public DateTime GetEndDate(DateTime startDate, int minutes)
        {
            // If Start date is outside working hours - move start date to 8.00 AM of next business day
            startDate = GetNextValidDateTime(startDate, Holidays);

            // Split minutes to days + hours + minutes
            int daysToAdd = 0, hoursToAdd = 0, minutesToAdd = minutes;
            if (minutesToAdd >= 60)
            {
                hoursToAdd = minutesToAdd / 60;
                minutesToAdd = minutesToAdd % 60;
            }
            if (hoursToAdd >= 8)
            {
                daysToAdd = hoursToAdd / 8;
                hoursToAdd = hoursToAdd % 8;
            }

            // Add days skipping weekends and holidays
            while (daysToAdd != 0)
            {
                startDate = startDate.AddDays(1);
                if (!IsHolidayOrWeekend(startDate, Holidays))
                    daysToAdd--;
            }

            // Add hours skipping lunch and wrap to next business day if time goes later than 5.00 PM
            while (hoursToAdd != 0)
            {
                startDate = startDate.AddHours(1);
                hoursToAdd--;
                if (startDate.Hour == 12)
                {
                    startDate.AddHours(1);
                }
                else if (startDate.Hour > 17)
                {
                    minutesToAdd += startDate.Minute;
                    startDate = GetNextValidDateTime(startDate, Holidays);
                    if (minutesToAdd >= 60)
                    {
                        minutesToAdd -= 60;
                        hoursToAdd++;
                    }
                }
            }

            // Add minutes skipping lunch and wrap to next business day if time goes later than 5.00 PM
            while (minutesToAdd != 0)
            {
                startDate = startDate.AddMinutes(minutesToAdd);
                minutesToAdd = 0;
                if (startDate.Hour == 12)
                {
                    startDate.AddHours(1);
                }
                else if (startDate.Hour > 17)
                {
                    minutesToAdd += startDate.Minute;
                    startDate = GetNextValidDateTime(startDate, Holidays);
                }
            }

            return startDate;
        }

        /// <summary>
        /// Returns new DateTime that is valid business hours or moves date and time to 8 AM of next closest business day
        /// </summary>
        /// <param name="start">Start date and time</param>
        /// <param name="Holidays">List of Holidays by year</param>
        /// <returns>A DateTime object that is within valid business hours</returns>
        private DateTime GetNextValidDateTime(DateTime start, Dictionary<int, HashSet<int>> Holidays)
        {
            if (start.Hour < 8)
            {
                start = new DateTime(start.Year, start.Month, start.Day, 8, 0, 0);
            }
            else if (start.Hour > 17)
            {
                start = start.AddDays(1);
                start = new DateTime(start.Year, start.Month, start.Day, 8, 0, 0);
            }

            start = SkipHolidaysAndWeekends(start, Holidays);

            return start;
        }

        /// <summary>
        /// Returns new DateTime that is next valid business day from given date that is not Holiday or Weekend
        /// </summary>
        /// <param name="date">DateTime to check</param>
        /// <param name="Holidays">List of Holidays by year</param>
        /// <returns>A DateTime object that is next closest business day for given date</returns>
        private DateTime SkipHolidaysAndWeekends(DateTime date, Dictionary<int, HashSet<int>> Holidays)
        {
            while (IsHolidayOrWeekend(date, Holidays))
            {
                date = date.AddDays(1);
            }

            return date;
        }

        /// <summary>
        /// Returns boolean that checks if date is holiday or weekend
        /// </summary>
        /// <param name="date">DateTime to check</param>
        /// <param name="Holidays">List of Holidays by year</param>
        /// <returns>A boolean that tells if given date is holiday or weekend</returns>
        private bool IsHolidayOrWeekend(DateTime date, Dictionary<int, HashSet<int>> Holidays)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || Holidays[date.Year].Contains(date.DayOfYear);
        }

        /// <summary>
        /// Function to populate Dictionary that contains holdiays in type of int representing day of the year. Holidays stored by each year
        /// </summary>
        /// <param name="holidaysList">List of valid holidays</param>
        /// <returns>A Dictionary collection that stores number of day in the year for each holidays that is stored by year</returns>
        private Dictionary<int, HashSet<int>> PopulateHolidays(List<DateTime> holidaysList)
        {
            Dictionary<int, HashSet<int>> returnHolidays = new Dictionary<int, HashSet<int>>();

            holidaysList.Sort();
            foreach (DateTime holiday in holidaysList)
            {
                int year = holiday.Year;
                if (returnHolidays.ContainsKey(year))
                {
                    if (!returnHolidays[year].Contains(holiday.DayOfYear))
                        returnHolidays[year].Add(holiday.DayOfYear);
                }
                else
                {
                    returnHolidays.Add(year, new HashSet<int>() { holiday.DayOfYear });
                }
            }

            return returnHolidays;
        }

    }
}
