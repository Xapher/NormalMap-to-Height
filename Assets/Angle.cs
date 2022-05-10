using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle : MonoBehaviour
{
    float angle;
    public Angle(Vector3 colors)
    {
        angle = Vector3.Angle(Vector3.up, colors);
    }

    public float getAngle()
    {
        return angle;
    }
}
