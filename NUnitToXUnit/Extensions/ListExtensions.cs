using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitToXUnit.Extensions
{
    public static class ListExtensions
    {
        public static T Second<T>(this IEnumerable<T> source)
        {
            return source.Skip(1).First();
        }

        public static T Third<T>(this IEnumerable<T> source)
        {
            return source.Skip(2).First();
        }
    }
}
