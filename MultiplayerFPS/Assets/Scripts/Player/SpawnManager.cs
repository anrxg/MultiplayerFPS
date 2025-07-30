using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    SpawnPoint[] spawnPoints;
    void Awake()
    {
        instance = this;
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        Debug.Log("SpawnPoints Found: " + spawnPoints.Length);
    }

    public Transform GetSpawnPoints()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }

}
