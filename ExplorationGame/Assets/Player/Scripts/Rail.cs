using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    Vector3 railAngle = Vector3.zero;
    [SerializeField] Transform pointA, pointB, center2;
    Vector3[] points = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
       points = new Vector3[2]{ pointA.position , pointB.position};
       railAngle = points[1] - points[0];
    }

    public Vector3 GetRailInclination() => railAngle;
    public Vector3[] GetPoints() => points;

    public Vector3 GetTargetPoint(Vector3 contactPoint)
    {
        float distanceToA = Vector3.Distance(contactPoint, points[0]);
        float distanceToB = Vector3.Distance(contactPoint, points[1]);

        Vector3 farthestpoint = distanceToA > distanceToB ? points[0] : points[1];
        return farthestpoint;
    }
}
