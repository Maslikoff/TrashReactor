using UnityEngine;

[System.Serializable]
public class SpawnGroup
{
    public string ItemName;
    public GameObject ItemPrefab;
    public int MaxCount = 5;
    public float SpawnInterval = 3f;
    public Transform SpawnPoint;
}
