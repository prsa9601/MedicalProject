using System.Globalization;

namespace MedicalProject.Infrastructure.Extensions
{
    public static class PersianNumberExtensions
    {
        public static string ToPersianNumbers(this int number)
        {
            return number.ToString("N0", new CultureInfo("fa-IR"));
        }

        public static string ToPersianNumbers(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var persianDigits = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            var englishDigits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < englishDigits.Length; i++)
            {
                text = text.Replace(englishDigits[i], persianDigits[i]);
            }

            return text;
        }
    }
}