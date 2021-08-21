using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<IEnumerable<T>> Subsets<T>(this IEnumerable<T> source)
            => source != null
                 ? SubsetsInternal(source)
                 : throw new ArgumentException(nameof(source));

        public static IEnumerable<IEnumerable<T>> SubsetsOfSize<T>(this IEnumerable<T> source, int size)
            => source != null
                ? source
                    .ToList()
                    .SubsetsOfSizeInternal(size)
                : throw new ArgumentNullException(nameof(source));

        private static IReadOnlyCollection<IEnumerable<T>> SubsetsOfSizeInternal<T>(this IReadOnlyCollection<T> source, int size)
        {
            if (size == source.Count())
            {
                return new[] { source };
            }

            if (size <= 0 || source.Count() < size)
            {
                return Enumerable.Empty<IEnumerable<T>>().ToList();
            }

            if (size == 1)
            {
                return source.Select(item => new[] { item }).ToList();
            }

            var referenceElementAsSet = new[] { source.First() };
            var remainder = source.Skip(1).ToList();

            var setsWithoutReferenceElement = SubsetsOfSizeInternal(remainder, size - 1);
            var setsWithoutReferenceElementOfSameSize = SubsetsOfSizeInternal(remainder, size);

            var setsWithReferenceElement = setsWithoutReferenceElement
                .Select(set => set.Concat(referenceElementAsSet).ToList());

            return setsWithoutReferenceElementOfSameSize.Concat(setsWithReferenceElement).ToList();
        }

        
        private static IEnumerable<IEnumerable<T>> SubsetsInternal<T>(IEnumerable<T> source)
        {
            if (!source.Any())
            {
                return new[] { Enumerable.Empty<T>() };
            }

            var referenceElementAsSet = new[] { source.First() };
            var remainder = source.Skip(1).ToList();

            var setsWithoutReferenceElement = SubsetsInternal(remainder);
            var setsWithReferenceElement = setsWithoutReferenceElement.Select(set => set.Concat(referenceElementAsSet));

            return setsWithoutReferenceElement.Concat(setsWithReferenceElement);
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<T> source, IEnumerable<T> other)
            => source.SelectMany(item => other.Select(otherItem => new[] { item, otherItem }));

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<T> source, IEnumerable<IEnumerable<T>> other)
            => source.SelectMany(item => other.Select(otherSequence => new[] { item }.Concat(otherSequence)));

        public static IEnumerable<IEnumerable<T>> CartesianPower<T>(this IEnumerable<T> source, int power)
        {
            if (power < 0)
            {
                throw new ArgumentException("The power of the Cartesian product cannot be negative.");
            }

            if (power == 0)
            {
                return Enumerable.Empty<IEnumerable<T>>();
            }

            var initialValue = source
                .Select(item => new[] { item }.AsEnumerable());

            return Enumerable
                .Range(1, power - 1)
                .Aggregate(initialValue, (acc, _) => source.CartesianProduct(acc));
        }
    }
}
