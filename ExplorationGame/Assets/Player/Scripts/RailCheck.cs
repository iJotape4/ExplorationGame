using System.Collections;
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
                BoxCollider bc = collision.transform.GetComponent<BoxCollider>();
                Vector3 contactPoint = collision.contacts[0].point;
                float min = Vector3.Distance(contactPoint, bc.bounds.min);
                float max = Vector3.Distance(contactPoint, bc.bounds.max);

                Vector3 farthestPoint = min > max ?
                    new Vector3(bc.bounds.min.x, transform.position.y, bc.bounds.min.z) :
                    new Vector3(bc.bounds.max.x, transform.position.y, bc.bounds.max.z);
                Vector3 directionToFarthestPoint = (farthestPoint - transform.position).normalized;
                farthestPoint = farthestPoint + directionToFarthestPoint * distanceOffset;
                Debug.Log(farthestPoint);
                //farthestPoint = new Vector3(farthestPoint.x * 3f, farthestPoint.y, farthestPoint.z);

                Vector3 cross = Vector3.Cross(bc.bounds.min.normalized, bc.bounds.min);
                Debug.DrawRay(contactPoint, bc.bounds.center, Color.cyan, 10f);

                player.transform.position = new Vector3( contactPoint.x, bc.bounds.max.y, contactPoint.z);
                player.MoveAlongRail(farthestPoint, contactPoint, bc.bounds.center);
            }
        }

       /* private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(farthestPoint, 3f);
            Gizmos.DrawSphere( new Vector3( farthestPoint.x *1.5f, farthestPoint.y,farthestPoint.z), 3f);

        }*/

        /*  private void OnCollisionExit(Collision collision)
          {
              if (collision.gameObject.tag == "Rail")
              {
                  StartCoroutine(DisableColliderForAWhile());
              }
          }

          public IEnumerator DisableColliderForAWhile()
          {
              bc.enabled = false;
              yield return new WaitForSeconds(0.5f);
              bc.enabled = true;
          }*/
    }
}
