using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UniHelper
{
    public static class VectorExtensions
    {
        public static Vector3 Add(this Vector3 value, float x = 0, float y = 0, float z = 0) =>
            new(value.x + x, value.y + y, value.z + z);
        public static Vector3 With(this Vector3 value, float? x = null, float? y = null, float? z = null) =>
            new(x ?? value.x, y ?? value.y, z ?? value.z);

        public static Vector3Int Add(this Vector3Int value, int x = 0, int y = 0, int z = 0) =>
            new(value.x + x, value.y + y, value.z + z);
        public static Vector3Int With(this Vector3Int value, int? x = null, int? y = null, int? z = null) =>
            new(x ?? value.x, y ?? value.y, z ?? value.z);

        public static Vector2 Add(this Vector2 value, float x = 0, float y = 0) =>
            new(value.x + x, value.y + y);
        public static Vector2 With(this Vector2 value, float? x = null, float? y = null) =>
            new(x ?? value.x, y ?? value.y);

        public static Vector2Int Add(this Vector2Int value, int x = 0, int y = 0) =>
            new(value.x + x, value.y + y);
        public static Vector2Int With(this Vector2Int value, int? x = null, int? y = null) =>
            new(x ?? value.x, y ?? value.y);
    }

    public static class GameObjectExtensions
    {
        public static T GetOrAdd<T>(this GameObject value) where T : Component =>
            value.GetComponent<T>() ?? value.AddComponent<T>();

        public static T OrNull<T>(this T value) where T : UnityEngine.Object => value ? value : null;

        public static void DestroyChildren(this GameObject value) =>
            value.PerformActionOnChildren(e => UnityEngine.Object.Destroy(e));

        public static void EnableChildren(this GameObject value) =>
            value.PerformActionOnChildren(e => e.SetActive(true));

        public static void DisableChildren(this GameObject value) =>
            value.PerformActionOnChildren(e => e.SetActive(false));

        public static void PerformActionOnChildren(this GameObject value, System.Action<GameObject> action) =>
            Enumerable.Range(0, value.transform.childCount)
            .Select(i => value.transform.GetChild(i).gameObject)
            .ToList()
            .ForEach(action);
    }

    public static class TransformExtensions
    {
        public static void Reset(this Transform value)
        {
            value.position = Vector3.zero;
            value.localRotation = Quaternion.identity;
            value.localScale = Vector3.one;
        }

        public static IEnumerable<Transform> Children(this Transform value)
        {
            foreach(Transform child in value) yield return child;
        }

        public static void DestroyChildren(this Transform value) =>
            value.PerformActionOnChildren(e => UnityEngine.Object.Destroy(e.gameObject));

        public static void EnableChildren(this Transform value) =>
            value.PerformActionOnChildren(e => e.gameObject.SetActive(true)); 

        public static void DisableChildren(this Transform value) =>
            value.PerformActionOnChildren(e => e.gameObject.SetActive(false));

        public static void PerformActionOnChildren(this Transform value, System.Action<Transform> action) =>
            Enumerable.Range(0, value.childCount)
            .Select(i => value.GetChild(i))
            .ToList()
            .ForEach(action);
    }

    public static class FloatExtensions
    {
        public static float Map(this float value, float fromMin, float fromMax, float toMin, float toMax) =>
            (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    public static class StringExtensions
    {
        public static string ToLiteral(this string value) =>
            Regex.Escape(value);
    }

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> value, Action<T, int> action)
        {
            int i = 0;
            foreach (var item in value)
                action(item, i++);
        }

        public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (var item in value)
                action(item);
        }
    }

    public static class ListExtensions
    {
        private static readonly System.Random random = new System.Random();
        private static readonly object accessLock = new object();

        public static T Random<T>(this IReadOnlyList<T> self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self), "Uninitialized list accessed");
            if (self.Count == 0)
                throw new InvalidOperationException("List is empty");

            lock (accessLock)
            {
                int index = random.Next(self.Count);
                return self[index];
            }
        }
    }

}