using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniHelper.Editor
{
    public static class GizmoUtils
    {
        public static void DrawPath(IEnumerable<Vector3> path, bool looped = false) =>
            Gizmos.DrawLineStrip(path.ToArray<Vector3>(), looped);

        public static void DrawPath(IEnumerable<Vector2> path, bool looped = false, float zOffset = 0) =>
            DrawPath(path.Select(p => new Vector3(p.x, p.y, zOffset)).ToArray(), looped);

        public static void DrawChildrenPath(this Transform value, bool looped = false) =>
            DrawPath(value.Children().Select(p => p.position).ToArray(), looped);

        public static void DrawChildrenPath(this GameObject value, bool looped = false) =>
            DrawChildrenPath(value.transform, looped);
    }
}