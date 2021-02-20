using System;
using System.Collections.Generic;

namespace TaskDateCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> years = new List<int>();
            for (int i = 1900; i < 2100; i++)
            {
                years.Add(i);
            }

            List<DateTime> holidays = GetHolidaysData(years);

            TaskManager taskManager = new TaskManager(holidays);

            DateTime testStartDate1 = new DateTime(2018, 1, 3, 6, 0, 0);
            int testMinutes1 = 55;
            DateTime endDate1 = taskManager.GetEndDate(testStartDate1, testMinutes1);
            Console.WriteLine($"Test 1: Start Date - {FormatDate(testStartDate1)}, Minutes - {testMinutes1}");
            Console.WriteLine($"End Date: {FormatDate(endDate1)}");
            Console.WriteLine("-------------------------");

            DateTime testStartDate2 = new DateTime(2018, 7, 3, 21, 0, 0);
            int testMinutes2 = 720;
            DateTime endDate2 = taskManager.GetEndDate(testStartDate2, testMinutes2);
            Console.WriteLine($"Test 2: Start Date - {FormatDate(testStartDate2)}, Minutes - {testMinutes2}");
            Console.WriteLine($"End Date: {FormatDate(endDate2)}");
            Console.WriteLine("-------------------------");

            DateTime testStartDate3 = new DateTime(2018, 8, 21, 5, 0, 0);
            int testMinutes3 = 4800;
            DateTime endDate3 = taskManager.GetEndDate(testStartDate3, testMinutes3);
            Console.WriteLine($"Test 3: Start Date - {FormatDate(testStartDate3)}, Minutes - {testMinutes3}");
            Console.WriteLine($"End Date: {FormatDate(endDate3)}");
            Console.WriteLine("-------------------------");

            DateTime testStartDate4 = new DateTime(2018, 8, 21, 11, 30, 0);
            int testMinutes4 = 30;
            DateTime endDate4 = taskManager.GetEndDate(testStartDate4, testMinutes4);
            Console.WriteLine($"Test 4: Start Date - {FormatDate(testStartDate4)}, Minutes - {testMinutes4}");
            Console.WriteLine($"End Date: {FormatDate(endDate4)}");
            Console.WriteLine("-------------------------");

            DateTime testStartDate5 = new DateTime(2018, 12, 31, 16, 30, 0);
            int testMinutes5 = 31;
            DateTime endDate5 = taskManager.GetEndDate(testStartDate5, testMinutes5);
            Console.WriteLine($"Test 4: Start Date - {FormatDate(testStartDate5)}, Minutes - {testMinutes5}");
            Console.WriteLine($"End Date: {FormatDate(endDate5)}");
            Console.WriteLine("-------------------------");

            Console.ReadKey();
        }

        public static List<DateTime> GetHolidaysData(List<int> years)
        {
            List<DateTime> holidays = new List<DateTime>();

            foreach (int year in years)
            {
                // New Year
                holidays.Add(new DateTime(year, 1, 1));

                // Memorial Day (Last Monday in May)
                DateTime memorial = new DateTime(year, 5, 31);
                while (memorial.DayOfWeek != DayOfWeek.Monday)
                    memorial = memorial.AddDays(-1);
                holidays.Add(memorial);

                // 4th Of July
                holidays.Add(new DateTime(year, 7, 4));

                // Labor Day (First Monday in September)
                DateTime labor = new DateTime(year, 10, 1);
                while (labor.DayOfWeek != DayOfWeek.Monday)
                    labor = labor.AddDays(1);
                holidays.Add(labor);

                // Thanksgiving (4th Thursday in November)
                DateTime thanksgiving = new DateTime(year, 11, 1);
                int count = thanksgiving.DayOfWeek == DayOfWeek.Thursday ? 1 : 0;
                while (count != 4)
                {
                    thanksgiving = thanksgiving.AddDays(1);
                    if (thanksgiving.DayOfWeek == DayOfWeek.Thursday)
                        count++;
                }
                holidays.Add(thanksgiving);

                // Chisrtmas
                holidays.Add(new DateTime(year, 12, 25));
            }

            return holidays;
        }

        public static string FormatDate(DateTime date)
        {
            return $"{date.ToLongDateString()} {date.ToShortTimeString()}";
        }
    }
}
