using PaladinsFaith.Math;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class IntersectTest : MonoBehaviour
{
    [OnValueChanged(nameof(GeneratePrismFromCollider))]
    public Collider collider1 = null;
    [OnValueChanged(nameof(GeneratePrismFromCollider))]
    public Collider collider2 = null;

    public Prism prism1 = new Prism();
    public Prism prism2 = new Prism();

    [ShowInInspector]
    private bool intersection = false;
    [ShowInInspector, ShowIf(nameof(intersection))]
    private Vector3 intersectionPoint = Vector3.zero;

    private void Update()
    {
        GeneratePrismFromCollider();
    }

    [Button]
    public void GeneratePrismFromCollider()
    {
        prism1 = new Prism(collider1);
        prism2 = new Prism(collider2);
    }

    [Button]
    public void StressTest(int iterations)
    {
        float startTime = Time.realtimeSinceStartup;
        for (int i = 0; i < iterations; i++)
        {
            //GeneratePrismFromCollider();
            intersection = IntersectionCalculator.IntersectColliders(collider1, collider2, out intersectionPoint);
        }

        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log($"Elapsed time: {elapsedTime}");
    }

    private void OnDrawGizmos()
    {
        float sphereSize = 0.1f;
        GeneratePrismFromCollider();
        DrawPrismGizmos(prism1, Color.red);
        DrawPrismGizmos(prism2, Color.green);

        intersection = IntersectionCalculator.IntersectColliders(collider1, collider2, out intersectionPoint);
        if (intersection)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(intersectionPoint, sphereSize);
        }
    }

    private void DrawPrismGizmos(Prism prism, Color color)
    {
        float sphereSize = 0.1f;
        float directionDistance = 0.5f;

        Gizmos.color = color;
        Vector3[] corners = prism.corners;
        foreach (var corner in corners)
        {
            Gizmos.DrawSphere(corner, sphereSize);
        }

        LineSegment[] edges = prism.edges;
        foreach (var edge in edges)
        {
            Gizmos.DrawLine(edge.start, edge.end);
        }

        PrismFace[] faces = prism.faces;
        foreach (var face in faces)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(face.center, face.up * directionDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(face.center, face.right * directionDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(face.center, face.normal * directionDistance);

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(face.center, sphereSize);
        }
    }
}
