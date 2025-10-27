using System.ComponentModel;
using System.Reflection;

namespace MedicalProject.Infrastructure.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// دریافت نام نمایشی برای مقادیر enum
        /// </summary>
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// دریافت نام‌های نمایشی برای مقادیر ترکیبی enum
        /// </summary>
        public static string GetDisplayNames(this Enum value)
        {
            if (value.Equals(default(Enum)))
                return "هیچکدام";

            var values = Enum.GetValues(value.GetType()).Cast<Enum>();
            var selectedValues = values.Where(value.HasFlag).ToList();

            return string.Join("، ", selectedValues.Select(GetDisplayName));
        }
    }
}
