using UnityEngine;

public class RailDebugger : MonoBehaviour
{
    BoxCollider bc;
    Vector3 max, min, center;
    Vector3 minPoint, maxPoint;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();      
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

        Debug.DrawRay(center, towardsRight, Color.green);    
        Debug.DrawRay(transform.position, transform.up * 3, Color.cyan);
    }

    private void OnDrawGizmos()
    {
       Gizmos.DrawSphere(maxPoint, 0.5f);
        Gizmos.DrawSphere(minPoint, 0.5f);
    }
}
