using UnityEngine;

public class Rail : MonoBehaviour
{
    Vector3 railAngle = Vector3.zero;
    [SerializeField] Transform pointA, pointB, center2;
    Vector3[] points = new Vector3[2];
    Vector3[] directionToPoints = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
       points = new Vector3[2]{ pointA.position , pointB.position};
       railAngle = points[1] - points[0];
       directionToPoints = new Vector3[2] { center2.position - points[0], center2.position - points[1] };
    }

    public Vector3 GetRailInclination() => railAngle;
    public Vector3[] GetPoints() => points;

    public Vector3 GetTargetPoint(Vector3 skateDirection)
    {
        float angleToA = Vector3.Angle(skateDirection, directionToPoints[0]);
        float angleToB = Vector3.Angle(skateDirection, directionToPoints[1]);
        Vector3 targePoint = angleToA > angleToB ? points[0] : points[1];
        return targePoint;
    }
}
