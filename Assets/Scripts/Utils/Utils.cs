using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector2[] ProjectParabola(Vector3[] parabola, Vector3 planeNormal)
    {
        // Get the normal vector and distance of the plane from the origin
        float a = planeNormal.x;
        float b = planeNormal.y;
        float c = planeNormal.z;
        float d = -Vector3.Dot(planeNormal, parabola[0]);

        // Find the intersection points of the parabola with the plane
        Vector2[] projection = new Vector2[parabola.Length];
        for (int i = 0; i < parabola.Length; i++)
        {
            float x = parabola[i].x;
            float y = parabola[i].y;
            float z = parabola[i].z;

            float t = (-a * x - b * y - c * z - d) / (a * x + b * y + c * z);
            float x_proj = x + t * a * x;
            float y_proj = y + t * b * y;

            projection[i] = new Vector2(x_proj, y_proj);
        }

        return projection;
    }
}
