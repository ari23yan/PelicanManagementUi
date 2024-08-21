using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace UsersManagementUi.Utilities
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
  

        public static string GetDisplayName(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                                            .GetField(enumValue.ToString())
                                            .GetCustomAttribute<DisplayAttribute>();
            return displayAttribute == null ? enumValue.ToString() : displayAttribute.GetName();
        }
    }
}
