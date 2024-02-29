using PaladinsFaith.Math;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectTest : MonoBehaviour
{
    [BoxGroup("Cube")]
    public Vector3 origin = Vector3.zero;
    [BoxGroup("Cube")]
    public Vector3 size = Vector3.one;
    [BoxGroup("Cube")]
    public Vector3 forward = Vector3.forward;
    [BoxGroup("Cube")]
    public Vector3 up = Vector3.up;
    public Vector3 point = Vector3.zero;

    [Button]
    public bool CubeContainsPoint()
    {
        Cube cube = new Cube(origin, size, forward, up);
        return IntersectionCalculator.DoesContainPoint(cube, point);
    }
}
