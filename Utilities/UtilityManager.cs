using System.Globalization;

namespace PelicanManagementUi.Utilities
{
    public static class UtilityManager
    {
        public static string PersianDateConverter(DateTime date)
        {

            PersianCalendar persianCalendar = new PersianCalendar();
            int year = persianCalendar.GetYear(date);
            int month = persianCalendar.GetMonth(date);
            int day = persianCalendar.GetDayOfMonth(date);
            return $"{year}/{month.ToString("D2")}/{day.ToString("D2")}";

        }
    }
}
