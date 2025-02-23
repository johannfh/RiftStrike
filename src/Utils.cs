using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Riftstrike
{
    public enum CollisionLayer
    {
        Units = 1 << 0,
        Enemies = 1 << 1,

        UnitPushCollisions = 1 << 16,
        EnemyPushCollisions = 1 << 17,

        Experience = 1 << 8,

        Selectable = 1 << 24,
        Selection = 1 << 25,
    }

    public enum NavigationLayer
    {
        Main = 1 << 0,
    }

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Selects a random element from the given enumerable collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The enumerable collection to select a random element from.</param>
        /// <returns>A randomly selected element from the collection.</returns>
        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            var rng = new RandomNumberGenerator();
            rng.Randomize();
            var idx = rng.RandiRange(0, source.Count() - 1);
            return source.ElementAt(idx);
        }

        /// <summary>
        /// Selects a random element from the given enumerable collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The enumerable collection to select a random element from.</param>
        /// <param name="rng">A randomized random number generator used to generate the index.</param>
        /// <returns>A randomly selected element from the collection.</returns>
        public static T RandomElement<T>(this IEnumerable<T> source, RandomNumberGenerator rng)
        {
            if (!source.Any()) return default;
            var idx = rng.RandiRange(0, source.Count() - 1);
            return source.ElementAt(idx);
        }
    }

    public static class Vector2Extensions
    {
        public static Vector2 FromValue(float value)
            => new(value, value);

        public static Vector2 AverageVector(this IEnumerable<Vector2> vectors)
        {
            var count = vectors.Count();
            if (count == 0) return Vector2.Zero;

            var sum = vectors.Aggregate(Vector2.Zero, (acc, v) => acc + v);
            return sum / count;
        }
    }

    /// <summary>
    /// Contains extension methods for Godot's <seealso cref="Resource"/> <see langword="type"/>.
    /// </summary>
    public static class ResourceExtensions
    {
        /// <summary>
        /// A utility wrapper for <see cref="Resource.Duplicate(bool)"/> that casts to <typeparamref name="T"/>.
        /// </summary>
        /// 
        /// <typeparam name="T">
        /// The <see langword="type"/> that the <paramref name="resource"/> will be casted to.
        /// </typeparam>
        /// 
        /// <seealso cref="Resource.Duplicate(bool)"/>
        public static T Duplicate<T>(this T resource, bool subresources = false) where T : Resource
        {
            return (T)resource.Duplicate();
        }
    }

}