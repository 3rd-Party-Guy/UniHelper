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

        public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}