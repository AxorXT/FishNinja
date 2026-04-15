using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs;

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
        int index = Random.Range(0, fishPrefabs.Length);

        Vector3 spawnPos = new Vector3(
        Random.Range(minX, maxX),
        spawnY,
        Random.Range(minZ, maxZ)
        );

        GameObject fish = Instantiate(fishPrefabs[index], spawnPos, Quaternion.identity);

        Rigidbody rb = fish.GetComponent<Rigidbody>();

        Vector3 force = new Vector3(
        0,
        Random.Range(minUpForce, maxUpForce),
        Random.Range(-sideForce, sideForce) // opcional
        );

        rb.linearVelocity = force;
    }
}