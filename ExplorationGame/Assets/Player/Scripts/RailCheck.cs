using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class RailCheck : MonoBehaviour
    {
        PlayerMovement player;

        private void Start()
        {
            player = transform.parent.GetComponent<PlayerMovement>();
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
               // MoveTowards(collision.transform.up);
               // StartCoroutine(MoveAlongRail(collision.transform.up));
            }
        }

        public void MoveTowards(Vector3 railDirection)
        {
           // player.parent = rail;
       
           // BoxCollider bc = rail.GetComponent<BoxCollider>();
            //Vector3 closestPoint = bc.ClosestPointOnBounds(transform.position);
       
        }

       /* IEnumerator MoveAlongRail(Vector3 railDirection)
        {
            while ()
            Vector3 movement = railDirection * railMoveSpeed;
            transform.Translate(movement);
            yield return null;

            /*while(transform.position != closestPoint)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, closestPoint, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }*/

            //player.parent = null;
        //}
    }
}
