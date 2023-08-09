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
            if (collision.gameObject.tag == "Rail" && !player.GetPlayerIsGrounded())
            {
                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 playerRotation = player.m_skateboard.transform.forward;

                Rail currentRail = collision.gameObject.GetComponentInParent<Rail>();
                Vector3 railAngle = currentRail.GetRailInclination();
                Vector3[] points = currentRail.GetPoints();
                player.transform.position = new Vector3(contactPoint.x, player.transform.position.y, contactPoint.z);
                player.m_skateboard.transform.right = railAngle;
                player.MoveAlongRail(currentRail.GetTargetPoint(playerRotation));
            }
        }
    }
}
