using UnityEngine;

public class SpawnInCameraView : MonoBehaviour
{
    [Tooltip("Prefab to spawn (your circle)")]
    public GameObject Circle;

    [Tooltip("Seconds between spawns before upgrades")]
    public float interval = 3f; // editable in Inspector, also used at runtime

    [Tooltip("World Z plane to place spawned objects (usually 0 for 2D)")]
    public float spawnZ = 0f;

    [Tooltip("Auto-destroy spawned objects after this many seconds. 0 = never")]
    public float lifetime = 5f;

    [Tooltip("If true, pick a random depth (distance) from the camera between minDepth and maxDepth")]
    public bool useRandomDepthFromCamera = false;
    public float minDepth = 1f;
    public float maxDepth = 5f;

    private Camera cam;
    private float timer;

    private float currentInterval; // runtime-calculated interval

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogWarning("No Camera.main found. Spawner will not spawn until a camera exists.");

        ApplyUpgradeEffects(); // make sure it’s applied at start
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentInterval)
        {
            timer = 0f;
            SpawnRandomInCamera();
        }
    }

    public void ApplyUpgradeEffects()
    {
        // start from the base interval (1 second in your case)
        currentInterval = interval;

        if (UpgradeManager.Instance != null)
        {
            // each "More Circles" upgrade reduces spawn interval by 0.2s
            float bonus = UpgradeManager.Instance.moreCirclesLevel * 0.2f;
            currentInterval = Mathf.Max(0.1f, currentInterval - bonus); // never go below 0.1s
        }

        Debug.Log($"[SpawnInCameraView] Spawn interval now = {currentInterval}s");
    }


    void SpawnRandomInCamera()
    {
        if (Circle == null)
        {
            Debug.LogWarning("SpawnInCameraView: circlePrefab is not assigned.");
            return;
        }

        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null) return;
        }

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
