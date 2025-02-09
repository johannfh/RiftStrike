using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Godot;

namespace Riftstrike {
    public enum CollisionLayer {
        Units = 1 << 0,
        Enemies = 1 << 1,

        Selection = 1 << 24,
    }

    public static class IEnumerableExtensions {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source) {
                action(item);
            }
        }
    }

    public static class Vector2Extensions {
        public static Vector2 FromValue(float value)
            => new(value, value);

        public static Vector2 AverageVector(this IEnumerable<Vector2> vectors) {
            var count = vectors.Count();
            if (count == 0) return Vector2.Zero;

            var sum = vectors.Aggregate(Vector2.Zero, (acc, v) => acc + v);
            return sum / count;
        }
    }
}