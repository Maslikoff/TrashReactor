using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : InteractableItem
{
    [SerializeField] private ItemType _name;

    private ItemSpawner _spawner;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _wasThrown;

    public override string ItemName => _name.ToString();
    public override string GetItemType() => _name.ToString();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public override void OnPickedUp(Transform holdPoint)
    {
        _wasThrown = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _rigidbody.isKinematic = true;
        _collider.enabled = false;
    }

    public override void OnThrown(Vector3 direction, float force)
    {
        _wasThrown = true;

        transform.SetParent(null);

        _rigidbody.isKinematic = false;
        _collider.enabled = true;

        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }

    public void SetSpawnerReference(ItemSpawner spawner, string type)
    {
        _spawner = spawner;

        if (System.Enum.TryParse(type, out ItemType parsedType))
            _name = parsedType;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_wasThrown != false) return;

        if (collision.gameObject.CompareTag("Reactor"))
            PickupInReactor(collision);
    }

    private void PickupInReactor(Collision collision)
    {
        ReactorSystem reactor = collision.gameObject.GetComponent<ReactorSystem>();

        if (reactor != null)
            reactor.OnItemThrown(_name.ToString());
    }

    private void OnDestroy()
    {
        if (_spawner != null)
        {
            _spawner.NotifyItemDestroyed(_name.ToString(), gameObject);
        }
    }
}
