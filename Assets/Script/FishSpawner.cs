using UnityEngine;

[System.Serializable]
public class FishSpawnData
{
    public GameObject prefab;
    public int weight; // probabilidad
}

public class FishSpawner : MonoBehaviour
{
    public FishSpawnData[] fishPrefabs;

    [Header("Spawn Area")]
    public float minX = -4f;
    public float maxX = 4f;
    public float spawnY = -2f;
    public float minZ = -2f;
    public float maxZ = 2f;

    [Header("Force")]
    public float minUpForce = 8f;
    public float maxUpForce = 12f;
    public float sideForce = 2f;

    [Header("Timing")]
    public float spawnRate = 1f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnFish), 1f, spawnRate);
    }

    void SpawnFish()
    {
        Vector3 spawnPos = new Vector3(
        Random.Range(minX, maxX),
        spawnY,
        Random.Range(minZ, maxZ)
        );

        GameObject fish = Instantiate(GetRandomFish(), spawnPos, Quaternion.identity);

        Rigidbody rb = fish.GetComponent<Rigidbody>();

        Vector3 force = new Vector3(
        0,
        Random.Range(minUpForce, maxUpForce),
        Random.Range(-sideForce, sideForce) // opcional
        );

        rb.linearVelocity = force;
        rb.angularVelocity = new Vector3(
            Random.Range(-2f, 2f),
            0,
            Random.Range(-2f, 2f)
        );
    }

    GameObject GetRandomFish()
    {
        int totalWeight = 0;

        foreach (var fish in fishPrefabs)
            totalWeight += fish.weight;

        int random = Random.Range(0, totalWeight);

        foreach (var fish in fishPrefabs)
        {
            if (random < fish.weight)
                return fish.prefab;

            random -= fish.weight;
        }

        return fishPrefabs[0].prefab;
    }
}