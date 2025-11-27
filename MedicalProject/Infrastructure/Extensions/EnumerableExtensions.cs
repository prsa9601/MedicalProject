namespace MedicalProject.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static decimal SafeSum<T>(this IEnumerable<T> source, Func<T, string> selector)
        {
            return source
                .Select(item => selector(item))
                .Where(priceStr => decimal.TryParse(priceStr, out _))
                .Sum(priceStr => decimal.Parse(priceStr));
        }
        public static decimal SafeSum<T>(this IEnumerable<T> source, Func<T, decimal> selector)
        {
            return source
                .Select(item => selector(item))
                .Sum(priceStr => priceStr);
        }
        public static decimal SafeSum<T>(this IEnumerable<T> source, Func<T, long> selector)
        {
            return source
                .Select(item => selector(item))
                .Sum(priceStr => priceStr);
        }
    }
}
