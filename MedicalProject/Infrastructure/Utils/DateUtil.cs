using System.Globalization;
using System.Text;
using System;
using System.Globalization;
namespace MedicalProject.Infrastructure.Utils;

public static class DateUtil
{
    //public static DateTime ToGregorianDateTime(this string? persianDate)
    //{
    //    try
    //    {
    //        var parts = persianDate.Split('/');
    //        int year = int.Parse(parts[0]);
    //        int month = int.Parse(parts[1]);
    //        int day = int.Parse(parts[2]);

    //        var persianCalendar = new PersianCalendar();
    //        return new DateTime(year, month, day, persianCalendar);
    //    }
    //    catch
    //    {
    //        return DateTime.MinValue;
    //    }
    //}


    public static DateTime ConvertToGregorianDateTime(this string persianDateTime)
    {
        try
        {
            PersianCalendar pc = new PersianCalendar();

            // Convert Persian digits to standard Arabic digits
            persianDateTime = ConvertPersianDigitsToEnglish(persianDateTime);

            // Extract year, month, day, hour, and minute
            string[] parts = persianDateTime.Split(' ');
            string[] dateParts = parts[0].Split('/');
            string[] timeParts = parts.Length > 1 ? parts[1].Split(':') : new string[] { "0", "0" };

            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);

            // Convert to Gregorian
            DateTime gregorianDate = new DateTime(year, month, day, hour, minute, 0, pc);
            return gregorianDate;
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    private static string ConvertPersianDigitsToEnglish(string input)
    {
        string[] persianDigits = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        for (int i = 0; i < persianDigits.Length; i++)
        {
            input = input.Replace(persianDigits[i], i.ToString());
        }
        return input;
    }
    public static DateTime ToMiladi(this string persianDate)
    {
        try
        {
            // عضو اول سال ، عضو دوم ماه ، عضو سوم روز
            //std[0]=سال | std[1]= ماه | std[2]=روز
            PersianCalendar persianCalendar = new PersianCalendar();

            // جدا کردن تاریخ و زمان از رشته ورودی
            string[] dateTimeParts = persianDate.Split(' ');
            string[] dateParts = dateTimeParts[0].Split('/');
            string[] timeParts = dateTimeParts.Length > 1 ? dateTimeParts[1].Split(':') : new string[] { "0", "0" };

            // استخراج سال، ماه، روز، ساعت و دقیقه
            int year = int.Parse(dateParts[2]);
            int month = int.Parse(dateParts[0]);
            int day = int.Parse(dateParts[1]);
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);

            // بررسی مقدار ماه و روز
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month", "Month must be between one and twelve.");
            }

            if (day < 1 || day > 31)
            {
                throw new ArgumentOutOfRangeException("day", "Day must be between one and thirty-one.");
            }

            // تبدیل تاریخ شمسی به تاریخ میلادی
            DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, hour, minute, 0, 0);

            return gregorianDate;
        }
        catch
        {
            return DateTime.MinValue;
        }
    }
    
        public static DateTime ConvertToPersianDateTime(this DateTime gregorianDate)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            // استخراج سال، ماه و روز به شمسی
            int year = persianCalendar.GetYear(gregorianDate);
            int month = persianCalendar.GetMonth(gregorianDate);
            int day = persianCalendar.GetDayOfMonth(gregorianDate);

            // استخراج ساعت، دقیقه و ثانیه
            int hour = gregorianDate.Hour;
            int minute = gregorianDate.Minute;
            int second = gregorianDate.Second;

            // ایجاد تاریخ شمسی
           // DateTime persianDate = new DateTime(year, month, day, hour, minute, second, persianCalendar);
            //DateTime persianDate = new DateTime(year, month, day, hour, minute, second);

            // ایجاد تاریخ شمسی با استفاده از تقویم شمسی
            DateTime persianDate = persianCalendar.ToDateTime(year, month, day, hour, minute, second, gregorianDate.Millisecond);
            return persianDate;

         }
        public static string ConvertToPersianDateString(this DateTime gregorianDate)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            int year = persianCalendar.GetYear(gregorianDate);
            int month = persianCalendar.GetMonth(gregorianDate);
            int day = persianCalendar.GetDayOfMonth(gregorianDate);

            int hour = gregorianDate.Hour;
            int minute = gregorianDate.Minute;
            int second = gregorianDate.Second;

            return $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";
        }
     
    public static string ToPersianTime(this TimeSpan ts)
    {
        return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0');
    }
    public static string ToPersianTime(this TimeSpan? ts)
    {
        return ts.HasValue ? ts.Value.ToPersianTime() : "";
    }
    public static string ToPersianDate(this DateTime dateTime)
    {
        PersianCalendar pc = new PersianCalendar();
        try
        {
            return string.Format("{0}/{1}/{2}", pc.GetYear(dateTime).ToString().PadLeft(4, '0'),
                pc.GetMonth(dateTime).ToString().PadLeft(2, '0'),
                pc.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0'));
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Sample Format = ds dd ms Y
    /// </summary>
    /// <param name="dateTime">تاریخ میلادی</param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ToPersianDate(this DateTime dateTime, string format)
    {
        //@DateTime.Now.ToPersianDate("ds dd ms Y")
        PersianCalendar pc = new PersianCalendar();
        try
        {
            string date = format.Replace("Y", pc.GetYear(dateTime).ToString().PadLeft(4, '0'))
                .Replace("mm", pc.GetMonth(dateTime).ToString())
                .Replace("MM", pc.GetMonth(dateTime).ToString().PadLeft(2, '0'))
                .Replace("dd", pc.GetDayOfMonth(dateTime).ToString())
                .Replace("DD", pc.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0'))
                .Replace("ds", GetDayOfWeekString((int)pc.GetDayOfWeek(dateTime)).ToString())
                .Replace("ms", GetMonthString(pc.GetMonth(dateTime)).ToString())
                ;
            return date;
        }
        catch
        {
            return "";
        }
    }

    private static string GetDayOfWeekString(int day)
    {
        string[] days = new string[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
        if (day <= days.Length)
        {
            return days[day];
        }
        return "";
    }
    private static string GetMonthString(int month)
    {
        string[] months = new string[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        if (month <= months.Length)
        {
            return months[month - 1];
        }
        return "";
    }

    public static string ToPersianDateTime(this DateTime dateTime)
    {
        try
        {
            return string.Format("{0}:{1} {2}", dateTime.Hour.ToString().PadLeft(2, '0'),
                dateTime.Minute.ToString().PadLeft(2, '0'), dateTime.ToPersianDate());
        }
        catch
        {
            return "";
        }
    }
    public static string ToPersianDate(this DateTime? dateTime)
    {
        if (dateTime != null)
            return dateTime.Value.ToPersianDate();

        return string.Empty;
    }
    public static string ToPersianDateTime(this DateTime? dateTime)
    {
        if (dateTime != null)
            return dateTime.Value.ToPersianDateTime();

        return string.Empty;
    }

    public static DateTime ToGregorianDateTime(this string persianDate)
    {
        if (string.IsNullOrEmpty(persianDate))
            return DateTime.MinValue;
        try
        {
            var pc = new PersianCalendar();

            var arrPersianDateTime = persianDate.Split(' ');
            var arrPersianDate = arrPersianDateTime[0].Split('/');
            var arrPersianTime = new string[] { "0", "0", "0" };

            if (arrPersianDateTime.Length == 2)
            {
                arrPersianTime = arrPersianDateTime[1].Split(':');
            }

            var year = int.Parse(arrPersianDate[0]);
            var month = short.Parse(arrPersianDate[1]);
            var day = short.Parse(arrPersianDate[2]);

            var hour = short.Parse(arrPersianTime[0]);
            var minute = short.Parse(arrPersianTime[1]);
            var second = arrPersianTime.Length == 3 ? short.Parse(arrPersianTime[2]) : 0;

            return pc.ToDateTime(year, month, day, hour, minute, second, 0);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

}