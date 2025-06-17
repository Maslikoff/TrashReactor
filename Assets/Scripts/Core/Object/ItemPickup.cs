using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : InteractableItem
{
    [SerializeField] private ItemType _name;

    private Rigidbody _rigidbody;
    private Collider _collider;

    public override string ItemName => _name.ToString();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public override void OnPickedUp(Transform holdPoint)
    {
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _rigidbody.isKinematic = true;
        _collider.enabled = false;
    }

    public override void OnThrown(Vector3 direction, float force)
    {
        transform.SetParent(null);

        _rigidbody.isKinematic = false;
        _collider.enabled = true;

        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }
}
