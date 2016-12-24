using System.Collections;
using UnityEngine;

public class QuadTreeVert
{
    public bool enabled;
    public float x, y, z;
    public QuadTreeVert(float X, float Y, float Z)
    {
        x = X;
        y = Y;
        z = Z;
        enabled = false;
    }

    public QuadTreeVert(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
        enabled = false;
    }
    public Vector3 ToVector()
    {
        return new Vector3(x, y, z);
    }
}