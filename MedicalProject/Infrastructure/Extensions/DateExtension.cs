namespace MedicalProject.Infrastructure.Extensions
{
    public static class DateExtension
    {
        public static string GetRelativeTimeAdvanced(DateTime creationDate)
        {
            var now = DateTime.Now;
            var timeSpan = now - creationDate;

            if (timeSpan.TotalDays >= 365)
            {
                var years = (int)(timeSpan.TotalDays / 365);
                return years == 1 ? "۱ سال پیش" : $"{years} سال پیش";
            }
            else if (timeSpan.TotalDays >= 30)
            {
                var months = (int)(timeSpan.TotalDays / 30);
                return months == 1 ? "۱ ماه پیش" : $"{months} ماه پیش";
            }
            else if (timeSpan.TotalDays >= 7)
            {
                var weeks = (int)(timeSpan.TotalDays / 7);
                return weeks == 1 ? "۱ هفته پیش" : $"{weeks} هفته پیش";
            }
            else if (timeSpan.TotalDays >= 1)
            {
                var days = (int)timeSpan.TotalDays;
                return days == 1 ? "۱ روز پیش" : $"{days} روز پیش";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                var hours = (int)timeSpan.TotalHours;
                return hours == 1 ? "۱ ساعت پیش" : $"{hours} ساعت پیش";
            }
            else if (timeSpan.TotalMinutes >= 1)
            {
                var minutes = (int)timeSpan.TotalMinutes;
                return minutes == 1 ? "۱ دقیقه پیش" : $"{minutes} دقیقه پیش";
            }
            else if (timeSpan.TotalSeconds >= 30)
            {
                var seconds = (int)timeSpan.TotalSeconds;
                return $"{seconds} ثانیه پیش";
            }
            else
            {
                return "همین الان";
            }
        }
    }
}
