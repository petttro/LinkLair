using System.Collections.Generic;

namespace LinkLair.Common.Extensions;

public static class EnumerableExtensions
{
    public static string ToJoinedString<T>(this IEnumerable<T> source)
    {
        return string.Join(",", source);
    }
}
