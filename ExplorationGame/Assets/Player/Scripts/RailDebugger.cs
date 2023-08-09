using UnityEngine;

[ExecuteAlways]
public class RailDebugger : MonoBehaviour
{
    MeshCollider bc;
    Vector3 max, min, center;
    Vector3 minPoint, maxPoint;
    Vector3 railAngle = Vector3.zero;

    [SerializeField] Transform pointA, pointB, center2;

    private void Start()
    {
        bc = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        max = bc.bounds.max;
        min = bc.bounds.min;
        center = bc.bounds.center;

        float distance = Vector3.Distance(center, min);

        Vector3 towardsRight = transform.TransformDirection(Vector3.right) * distance;
        minPoint = center + towardsRight;
        maxPoint = center - towardsRight;

        Vector3 perpendicular = Vector3.Cross(towardsRight, transform.up);
        railAngle = pointB.position - pointA.position;
        Debug.DrawRay(pointB.position, railAngle*10, Color.black);    

        //Debug.DrawRay(center, towardsRight, Color.green);    
        Debug.DrawRay(center, transform.up * 3, Color.cyan);
        Debug.DrawRay(center, transform.right * 3, Color.yellow);
        Debug.DrawRay(center, transform.forward * 3, Color.blue);
        //Debug.DrawRay(transform.position, perpendicular * 3, Color.red);
    }

    public Vector3 GetRailInclination() => railAngle;

    private void OnDrawGizmos()
    {
       /*Gizmos.DrawSphere(maxPoint, 0.5f);
        Gizmos.DrawSphere(minPoint, 0.5f);*/
    }
}
