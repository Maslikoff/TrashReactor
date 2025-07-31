using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneForObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var item = collision.gameObject.GetComponent<ItemPickup>();

        if (item != null)
            Destroy(item.gameObject);
    }
}
