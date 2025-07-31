using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private List<SpawnGroup> _spawnGroups = new List<SpawnGroup>
    {
        new SpawnGroup { ItemName = "Trash", MaxCount = 1 },
        new SpawnGroup { ItemName = "Dust", MaxCount = 1 }
    };

    private Dictionary<string, List<GameObject>> _activeItems = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, float> _spawnTimers = new Dictionary<string, float>();

    private void Start()
    {
        foreach (var group in _spawnGroups)
        {
            _activeItems[group.ItemName] = new List<GameObject>();
            _spawnTimers[group.ItemName] = 0f;
            SpawnItem(group);
        }
    }

    private void Update()
    {
        foreach (var group in _spawnGroups)
        {
            _spawnTimers[group.ItemName] += Time.deltaTime;

            if (_spawnTimers[group.ItemName] >= group.SpawnInterval)
            {
                TrySpawnItems(group);
                _spawnTimers[group.ItemName] = 0f;
            }
        }
    }

    private void TrySpawnItems(SpawnGroup group)
    {
        // Удаляем уничтоженные предметы
        _activeItems[group.ItemName].RemoveAll(item => item == null);

        // Вычисляем сколько нужно заспавнить
        int itemsToSpawn = group.MaxCount - _activeItems[group.ItemName].Count;

        for (int i = 0; i < itemsToSpawn; i++)
        {
            if (_spawnTimers[group.ItemName] >= group.SpawnInterval)
            {
                SpawnItem(group);
                _spawnTimers[group.ItemName] = 0f;
            }
        }
    }

    private void SpawnItem(SpawnGroup group)
    {
        GameObject newItem = Instantiate(
            group.ItemPrefab,
            group.SpawnPoint.position,
            group.SpawnPoint.rotation
        );

        var pickup = newItem.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            pickup.SetSpawnerReference(this, group.ItemName);
        }

        _activeItems[group.ItemName].Add(newItem);
    }

    public void NotifyItemDestroyed(string itemType, GameObject item)
    {
        if (_activeItems.ContainsKey(itemType))
        {
            _activeItems[itemType].Remove(item);
        }
    }
}
