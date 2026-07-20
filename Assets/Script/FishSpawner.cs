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

    [Header("Dynamic Difficulty")]
    public float baseSpawnRate = 1f;
    public float minSpawnRate = 0.3f;
    public float maxSpawnRate = 1.5f;

    public float waveSpeed = 0.5f; // qué tan rápido oscila
    public float difficultyScale = 0.0005f; // qué tanto influye el score
    private bool canSpawn = false;
    float timer;

    void Update()
    {
        if (!canSpawn)
            return;

        float score = ScoreManager.Instance.score;

        //efecto montaña rusa
        float wave = Mathf.Sin(Time.time * waveSpeed);

        //dificultad base por score
        float difficulty = 1f + (score * difficultyScale);

        //spawn dinámico
        float currentSpawnRate = Mathf.Clamp(
            baseSpawnRate / difficulty + wave * 0.5f,
            minSpawnRate,
            maxSpawnRate
        );

        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnFish();
            timer = 0f;
        }
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

    public void StartSpawning()
    {
        timer = 0f;
        canSpawn = true;
    }

    public void StopSpawning()
    {
        canSpawn = false;
    }
}