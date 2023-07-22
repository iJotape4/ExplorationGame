using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class DeliveryCheck : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeliveryPoint")
        {
            EventManager.Dispatch(DeliveryEvents.Delivered);
        }
    }
}
