using System.Collections;
using UnityEngine;

namespace PlayerScripts
{
    public class RailCheck : MonoBehaviour
    {
        PlayerMovement player;
        SphereCollider bc;

        private void Start()
        {
            player = transform.parent.GetComponent<PlayerMovement>();
            bc = GetComponent<SphereCollider>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Rail")
            {
                BoxCollider bc = collision.transform.GetComponent<BoxCollider>();
                Vector3 contactPoint = collision.contacts[0].point;
                float min = Vector3.Distance(contactPoint, bc.bounds.min);
                float max = Vector3.Distance(contactPoint, bc.bounds.max);

                Vector3 farthestPoint = min > max ? bc.bounds.min : bc.bounds.max; 
               
                player.MoveAlongRail(farthestPoint, contactPoint);
            }
        }

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
