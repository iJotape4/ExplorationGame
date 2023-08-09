using UnityEngine;

namespace PlayerScripts
{
    public class RailCheck : MonoBehaviour
    {
        PlayerMovement player;
        Collider bc;
        Rigidbody rb;
        [SerializeField, Range(0.5f,5f)] float distanceOffset = 2f;

        private void Start()
        {
            player = GetComponentInParent<PlayerMovement>();
            bc = GetComponent<Collider>();
            rb = player.GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            /*
            if (collision.gameObject.tag == "Rail" && !player.GetPlayerIsGrounded())
            {
                BoxCollider bc = collision.transform.GetComponent<BoxCollider>();
                Vector3 contactPoint = collision.contacts[0].point;

                Vector3 center = bc.bounds.center;
                float distance = Vector3.Distance(center, bc.bounds.min);

                Vector3 towardsRight = collision.transform.TransformDirection(Vector3.right) * distance;
                Vector3 minPoint = center - towardsRight;
                Vector3 maxPoint = center + towardsRight;

                float min = Vector3.Distance(contactPoint, minPoint);
                float max = Vector3.Distance(contactPoint, maxPoint);

                Vector3 farthestPoint = min > max ? minPoint : maxPoint;

                Vector3 directionToFarthestPoint = (farthestPoint - transform.position).normalized;
                farthestPoint = farthestPoint + directionToFarthestPoint * distanceOffset;

                Vector3 lane = collision.gameObject.transform.up;

                player.transform.position = new Vector3( contactPoint.x, player.transform.position.y, contactPoint.z);
                Vector3 perpendicular = Vector3.Cross(towardsRight, collision.transform.up);
                //Debug.DrawRay(transform.position, perpendicular*10, Color.yellow);
                player.MoveAlongRail(farthestPoint, collision.gameObject.transform.forward, lane);
            }*/

            if (collision.gameObject.tag == "Rail" && !player.GetPlayerIsGrounded())
            {
                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 playerRotation = player.m_skateboard.transform.forward;

                Rail currentRail = collision.gameObject.GetComponentInParent<Rail>();
                Vector3 railAngle = currentRail.GetRailInclination();
                Vector3[] points = currentRail.GetPoints();
                Vector3 rotationAxis = Vector3.Cross(playerRotation, railAngle);
                float rotationAngle = Vector3.Angle(playerRotation, currentRail.transform.up.normalized);

                player.transform.position = new Vector3(contactPoint.x, player.transform.position.y, contactPoint.z);

               player.m_skateboard.transform.right = railAngle;
               player.MoveAlongRail(currentRail.GetTargetPoint(contactPoint), contactPoint, points[0]);
            }
        }
    }
}
