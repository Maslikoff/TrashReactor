using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ReactorCollisionHandler : MonoBehaviour
{
    [SerializeField] private ReactorSystem _reactorSystem;
    [SerializeField] private ParticleSystem _impactParticles;
    [SerializeField] private AudioClip _impactSound;

    private void OnCollisionEnter(Collision collision)
    {
        var item = collision.gameObject.GetComponent<ItemPickup>();

        if (item != null)
        {
            if (_impactParticles != null)
            {
                _impactParticles.transform.position = collision.contacts[0].point;
                _impactParticles.Play();
            }

            if (_impactSound != null)
                AudioSource.PlayClipAtPoint(_impactSound, transform.position);

            _reactorSystem.OnItemThrown(item.GetItemType());

            Destroy(item.gameObject);   
        }
    }
}
