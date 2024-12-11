namespace MyUtilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int Index, T Value)> Enumerate<T>(this IEnumerable<T> source)
        {
            int index = 0;
            foreach (var item in source)
            {
                yield return (index++, item);
            }
        }
    }
}