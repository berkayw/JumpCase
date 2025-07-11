using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;

    [Header("X Position")]
    [SerializeField] private float minXGap = 5f;
    [SerializeField] private float maxXGap = 8f;

    [Header("Y Position")]
    [SerializeField] private float maxYVariation = 2f;

    [Header("Platform Length")]
    [SerializeField] private float minScaleX = 2f;
    [SerializeField] private float maxScaleX = 4f;

    [SerializeField] private Transform player;
    [SerializeField] private float spawnAheadDistance = 30f;

    private Vector3 lastSpawnPos;

    private void Start()
    {
        lastSpawnPos = transform.position;
    }

    private void Update()
    {
        if (player.position.x + spawnAheadDistance > lastSpawnPos.x)
        {
            SpawnNextPlatform();
        }
    }

    private void SpawnNextPlatform()
    {
        float xGap = Random.Range(minXGap, maxXGap);
        float yOffset = Random.Range(-maxYVariation, maxYVariation);

        Vector3 newPos = lastSpawnPos + new Vector3(xGap, yOffset, 0f);
        GameObject newPlatform = Instantiate(platformPrefab, newPos, Quaternion.identity);
        
        float newScaleX = Random.Range(minScaleX, maxScaleX);
        Vector3 newScale = newPlatform.transform.localScale;
        newScale.x = newScaleX;
        newPlatform.transform.localScale = newScale;

        lastSpawnPos = newPos;
    }
}