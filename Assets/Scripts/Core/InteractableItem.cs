using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableItem : MonoBehaviour
{
    public abstract string ItemName { get; }
    public abstract void OnPickedUp(Transform holdPoint);
    public abstract void OnThrown(Vector3 direction, float force);
}
