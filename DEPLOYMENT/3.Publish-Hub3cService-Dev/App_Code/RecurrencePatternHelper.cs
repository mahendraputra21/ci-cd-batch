using Hub3c.Data.Core.Models;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

//namespace Hub3c.Service
//{
    public class RecurrencePatternHelper
    {
        public static DayOfTheWeek GetDayName(string day)
        {
            DayOfTheWeek dayofWeek = DayOfTheWeek.Day;
            switch (day)
            {
                case "MO":
                    dayofWeek = DayOfTheWeek.Monday;
                    break;
                case "TU":
                    dayofWeek = DayOfTheWeek.Tuesday;
                    break;
                case "WE":
                    dayofWeek = DayOfTheWeek.Wednesday;
                    break;
                case "TH":
                    dayofWeek = DayOfTheWeek.Thursday;
                    break;
                case "FR":
                    dayofWeek = DayOfTheWeek.Friday;
                    break;
                case "SA":
                    dayofWeek = DayOfTheWeek.Saturday;
                    break;
                case "SU":
                    dayofWeek = DayOfTheWeek.Sunday;
                    break;
            }
            return dayofWeek;
        }
        public static DayOfTheWeekIndex GetWeekIndex(int week)
        {
            DayOfTheWeekIndex weekIndex = DayOfTheWeekIndex.First;

            switch (week)
            {
                case 1:
                    weekIndex = DayOfTheWeekIndex.First;
                    break;
                case 2:
                    weekIndex = DayOfTheWeekIndex.Second;
                    break;
                case 3:
                    weekIndex = DayOfTheWeekIndex.Third;
                    break;
                case 4:
                    weekIndex = DayOfTheWeekIndex.Fourth;
                    break;
                case -1:
                    weekIndex = DayOfTheWeekIndex.Last;
                    break;
            }

            return weekIndex;
        }
        public static int GetIntegerInPattern(string value)
        {
            var numAlpha = new Regex("(?<Numeric>[0-9]*)(?<Alpha>[a-zA-Z]*)");
            var match = numAlpha.Match(value);
            int numeric;
            int.TryParse(match.Groups["Numeric"].Value, out numeric);
            return numeric;
        }

        public static string GetStringInPattern(string value)
        {
            var numAlpha = new Regex("(?<Numeric>[0-9]*)(?<Alpha>[a-zA-Z]*)");
            var match = numAlpha.Match(value);
            return match.Groups["Alpha"].Value;
        }

        public static Recurrence GetByDailyRecurrence(Schedule schedule)
        {
            Recurrence recurrence = null;
            string rule = schedule.RecurrenceRule;
            string[] pattern = rule.Split(';');
            if (rule.Contains("INTERVAL") && rule.Contains("UNTIL"))
            {
                recurrence = new Recurrence.DailyPattern(schedule.StartTime.Value, int.Parse(pattern[1].Split('=')[1]));
                recurrence.EndDate = DateTime.ParseExact(pattern[2].Split('=')[1], "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }
            else if (rule.Contains("INTERVAL") && rule.Contains("COUNT"))
            {
                recurrence = new Recurrence.DailyPattern(schedule.StartTime.Value, int.Parse(pattern[1].Split('=')[1]));
                recurrence.NumberOfOccurrences = int.Parse(pattern[1].Split('=')[1]);
            }
            else if (rule.Contains("INTERVAL"))
            {
                recurrence = new Recurrence.DailyPattern(schedule.StartTime.Value, int.Parse(pattern[1].Split('=')[1]));
                recurrence.NeverEnds();
            }
            else if (rule.Contains("UNTIL"))
            {
                recurrence = new Recurrence.DailyPattern();
                recurrence.StartDate = schedule.StartTime.Value;
                recurrence.EndDate = DateTime.ParseExact(pattern[1].Split('=')[1], "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }
            else if (rule.Contains("COUNT"))
            {
                recurrence = new Recurrence.DailyPattern();
                recurrence.StartDate = schedule.StartTime.Value;
                recurrence.NumberOfOccurrences = int.Parse(pattern[1].Split('=')[1]);
            }
            else if (rule.Contains("FREQ"))
            {
                recurrence = new Recurrence.DailyPattern();
                recurrence.StartDate = schedule.StartTime.Value;
                recurrence.NeverEnds();
            }
            return recurrence;
        }

        public static Recurrence GetByWeeklyRecurrence(Schedule schedule)
        {

            Recurrence recurrence = null;
            string rule = schedule.RecurrenceRule;
            string[] pattern = rule.Split(';');

            if (rule.Contains("COUNT") && rule.Contains("BYDAY"))
            {
                string[] days = pattern[2].Split('=')[1].Split(',');
                DayOfTheWeek[] listDayOfWeek = new DayOfTheWeek[days.Count()];

                for (int i = 0; i < days.Count(); i++)
                {
                    listDayOfWeek[i] = GetDayName(days[i]);
                }
                recurrence = new Recurrence.WeeklyPattern(schedule.StartTime.Value, 1, listDayOfWeek);
                recurrence.NumberOfOccurrences = int.Parse(pattern[1].Split('=')[1]);
            }
            if (rule.Contains("INTERVAL") && rule.Contains("UNTIL") && rule.Contains("BYDAY"))
            {
                string[] days = pattern[3].Split('=')[1].Split(',');
                DayOfTheWeek[] listDayOfWeek = new DayOfTheWeek[days.Count()];

                for (int i = 0; i < days.Count(); i++)
                {
                    listDayOfWeek[i] = GetDayName(days[i]);
                }
                recurrence = new Recurrence.WeeklyPattern(schedule.StartTime.Value, int.Parse(pattern[1].Split('=')[1]), listDayOfWeek);
                recurrence.EndDate = DateTime.ParseExact(pattern[2].Split('=')[1], "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }

            if (rule.Contains("INTERVAL") && rule.Contains("BYDAY") && pattern.Count() == 3)
            {
                string[] days = pattern[2].Split('=')[1].Split(',');
                DayOfTheWeek[] listDayOfWeek = new DayOfTheWeek[days.Count()];

                for (int i = 0; i < days.Count(); i++)
                {
                    listDayOfWeek[i] = GetDayName(days[i]);
                }
                recurrence = new Recurrence.WeeklyPattern(schedule.StartTime.Value, int.Parse(pattern[1].Split('=')[1]), listDayOfWeek);
                recurrence.NeverEnds();
            }
            if (rule.Contains("UNTIL") && rule.Contains("BYDAY"))
            {
                string[] days = pattern[2].Split('=')[1].Split(',');
                DayOfTheWeek[] listDayOfWeek = new DayOfTheWeek[days.Count()];

                for (int i = 0; i < days.Count(); i++)
                {
                    listDayOfWeek[i] = GetDayName(days[i]);
                }
                recurrence = new Recurrence.WeeklyPattern(schedule.StartTime.Value, 1, listDayOfWeek);
                recurrence.EndDate = DateTime.ParseExact(pattern[1].Split('=')[1], "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }

            if (rule.Contains("BYDAY") && rule.Count() == 2)
            {
                string[] days = pattern[2].Split('=')[1].Split(',');
                DayOfTheWeek[] listDayOfWeek = new DayOfTheWeek[days.Count()];

                for (int i = 0; i < days.Count(); i++)
                {
                    listDayOfWeek[i] = GetDayName(days[i]);
                }
                recurrence = new Recurrence.WeeklyPattern(schedule.StartTime.Value, 1, listDayOfWeek);
                recurrence.NeverEnds();
            }

            return recurrence;
        }
        public static Recurrence GetByMonthlyRecurrence(Schedule schedule)
        {
            Recurrence recurrence = null;
            string rule = schedule.RecurrenceRule;
            string[] pattern = rule.Split(';');

            if (rule.Contains("COUNT") && rule.Contains("BYMONTHDAY") && pattern.Count() == 3)
            {
                recurrence = new Recurrence.MonthlyPattern(schedule.StartTime.Value, 1, int.Parse(pattern[2].Split('=')[1]));
                recurrence.NumberOfOccurrences = int.Parse(pattern[1].Split('=')[1]);
            }
            if (rule.Contains("COUNT") && pattern.Count() == 3 && rule.Contains("BYDAY"))
            {
                int weekIndex = GetIntegerInPattern(pattern[2].Split('=')[1]);
                string day = GetStringInPattern(pattern[2].Split('=')[1]);

                recurrence = new Recurrence.RelativeMonthlyPattern(schedule.StartTime.Value, 1, GetDayName(day), GetWeekIndex(weekIndex));
                recurrence.NumberOfOccurrences = int.Parse(pattern[1].Split('=')[1]);

            }
            if (rule.Contains("INTERVAL") && pattern.Count() == 3 && rule.Contains("BYDAY"))
            {
                int weekIndex = GetIntegerInPattern(pattern[2].Split('=')[1]);
                string day = GetStringInPattern(pattern[2].Split('=')[1]);

                recurrence = new Recurrence.RelativeMonthlyPattern(schedule.StartTime.Value, int.Parse(pattern[1].Split('=')[1]), GetDayName(day), GetWeekIndex(weekIndex));
                recurrence.NeverEnds();

            }
            if (rule.Contains("INTERVAL") && pattern.Count() == 3 && rule.Contains("BYMONTHDAY"))
            {
                int weekIndex = GetIntegerInPattern(pattern[2].Split('=')[1]);
                string day = GetStringInPattern(pattern[2].Split('=')[1]);

                recurrence = new Recurrence.MonthlyPattern(schedule.StartTime.Value, int.Parse(pattern[1].Split('=')[1]), int.Parse(pattern[2].Split('=')[1]));
                recurrence.NeverEnds();

            }
            if (rule.Contains("UNTIL") && rule.Contains("BYMONTHDAY") && pattern.Count() == 3)
            {
                recurrence = new Recurrence.MonthlyPattern(schedule.StartTime.Value, 1, int.Parse(pattern[2].Split('=')[1]));
                recurrence.EndDate = DateTime.ParseExact(pattern[1].Split('=')[1], "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }
            if (rule.Contains("UNTIL") && rule.Contains("BYDAY") && pattern.Count() == 3)
            {
                int weekIndex = GetIntegerInPattern(pattern[2].Split('=')[1]);
                string day = GetStringInPattern(pattern[2].Split('=')[1]);

                recurrence = new Recurrence.RelativeMonthlyPattern(schedule.StartTime.Value, 1, GetDayName(day), GetWeekIndex(weekIndex));
                recurrence.EndDate = DateTime.ParseExact(pattern[1].Split('=')[1], "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }
            if (rule.Contains("BYDAY") && rule.Count() == 2)
            {
                int weekIndex = GetIntegerInPattern(pattern[2].Split('=')[1]);
                string day = GetStringInPattern(pattern[2].Split('=')[1]);

                recurrence = new Recurrence.RelativeMonthlyPattern(schedule.StartTime.Value, 1, GetDayName(day), GetWeekIndex(weekIndex));
                recurrence.NeverEnds();
            }
            if (rule.Contains("BYMONTHDAY") && rule.Count() == 2)
            {
                recurrence = new Recurrence.MonthlyPattern(schedule.StartTime.Value, 1, int.Parse(pattern[1].Split('=')[1]));
                recurrence.NeverEnds();
            }
            return recurrence;
        }

        //private Aspose.Email.Mail.Calendaring.RecurrencePattern GetByYearlyRecurrence(Data.Schedule schedule)
        //{
        //    return new Aspose.Email.Mail.Calendaring.DailyRecurrencePattern(0);
        //}
    }
//}