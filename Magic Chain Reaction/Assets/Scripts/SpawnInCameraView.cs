using UnityEngine;

public class SpawnInCameraView : MonoBehaviour, IUpgradeListener
{
    [Header("Prefab & Timing")]
    public GameObject Circle;
    public float baseInterval = 1f;  // base spawn interval
    public float spawnZ = 0f;
    public float lifetime = 5f;

    [Header("Random Depth")]
    public bool useRandomDepthFromCamera = false;
    public float minDepth = 1f;
    public float maxDepth = 5f;

    [Header("Spawn Settings")]
    public int baseSpawnCount = 1; // always spawn at least 1 circle
    private int spawnCount;         // modified by upgrades

    private Camera cam;
    private float timer;
    public float currentInterval;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogWarning("No Camera.main found. Spawner will not spawn until a camera exists.");

        spawnCount = baseSpawnCount;
        currentInterval = baseInterval;

        // Apply upgrades at start if UpgradeManager exists
        UpgradeManager.Instance?.ApplyUpgradesToScene();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentInterval)
        {
            timer = 0f;
            SpawnCircles();
        }
    }

    private void SpawnCircles()
    {
        if (Circle == null || cam == null) return;

        for (int i = 0; i < spawnCount; i++)
        {
            float vx = Random.value;
            float vy = Random.value;
            float zDistance = useRandomDepthFromCamera
                ? Random.Range(Mathf.Min(minDepth, maxDepth), Mathf.Max(minDepth, maxDepth))
                : Mathf.Abs(spawnZ - cam.transform.position.z);

            Vector3 viewportPoint = new Vector3(vx, vy, zDistance);
            Vector3 worldPos = cam.ViewportToWorldPoint(viewportPoint);
            worldPos.z = spawnZ;

            GameObject go = Instantiate(Circle, worldPos, Quaternion.identity);
            if (lifetime > 0f)
                Destroy(go, lifetime);
        }
    }

    // Called automatically by UpgradeManager.ApplyUpgradesToScene()
    public void OnUpgradesApplied(UpgradeManager upgrades)
    {
        currentInterval = upgrades.circleInterval;
        spawnCount = upgrades.circleSpawnCount;
    }

}
