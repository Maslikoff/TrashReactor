using UnityEngine;
using TMPro;

public class ItemInteractionSystem : MonoBehaviour
{
    [SerializeField] private float _interactionRadius = 2f;
    [SerializeField] private LayerMask _itemLayer;
    [SerializeField] private Transform _itemHoldPoint;
    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private TextMeshPro _pickupPromptPrefab;
    [SerializeField] private float _promtHeight = 2f; 
    

    private InteractableItem _currentCandidate;
    private TextMeshPro _pickupPrompt;
    private int _maxCapacity = 1;

    private void Awake()
    {
        InitializePrompt();
    }

    public void UpgradeCapacity(int additionalSlots)
    {
        _maxCapacity += additionalSlots;
    }

    private void InitializePrompt()
    {
        _pickupPrompt = Instantiate(_pickupPromptPrefab);
        _pickupPrompt.gameObject.SetActive(false);
    }

    private void Update()
    {
        FindClosestInteractable();
        HandleInteractionInput();

        if (_pickupPrompt.gameObject.activeSelf && _currentCandidate != null)
        {
            UpdatePromptPosition();
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
            TryPickupItem();

        if (Input.GetKeyDown(KeyCode.Q)) 
            TryThrowItem();
    }

    private void TryPickupItem()
    {
        if (_currentCandidate == null) return;

        _currentCandidate.OnPickedUp(_itemHoldPoint);
        _currentCandidate = null;
        _pickupPrompt.gameObject.SetActive(false);
    }

    private void TryThrowItem()
    {
        if (_itemHoldPoint.childCount == 0) return;

        var item = _itemHoldPoint.GetChild(0).GetComponent<InteractableItem>();
        item.OnThrown(transform.forward, _throwForce);
    }

    private void FindClosestInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _interactionRadius, _itemLayer);
        InteractableItem closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in hitColliders)
        {
            var item = collider.GetComponent<InteractableItem>();
            if (item != null && _itemHoldPoint.childCount < _maxCapacity)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }

        UpdateCurrentCandidate(closestItem);
    }

    private void UpdateCurrentCandidate(InteractableItem newCandidate)
    {
        if (_currentCandidate == newCandidate) return;

        _currentCandidate = newCandidate;
        _pickupPrompt.gameObject.SetActive(_currentCandidate != null);

        if (_currentCandidate != null)
        {
            _pickupPrompt.text = $"Нажмите 'E' чтобы поднять";
            UpdatePromptPosition();
        }
    }

    private void UpdatePromptPosition()
    {
        Vector3 promptPos = _currentCandidate.transform.position + Vector3.up * _promtHeight;
        _pickupPrompt.transform.position = promptPos;
        _pickupPrompt.transform.rotation = Quaternion.LookRotation(
            _pickupPrompt.transform.position - Camera.main.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }
}