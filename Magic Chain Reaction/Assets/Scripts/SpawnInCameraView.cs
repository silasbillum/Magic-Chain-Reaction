using UnityEngine;

public class SpawnInCameraView : MonoBehaviour
{
    [Tooltip("Prefab to spawn (your circle)")]
    public GameObject Circle;

    [Tooltip("Seconds between spawns")]
    public float interval = 3f;

    [Tooltip("World Z plane to place spawned objects (usually 0 for 2D)")]
    public float spawnZ = 0f;

    [Tooltip("Auto-destroy spawned objects after this many seconds. 0 = never")]
    public float lifetime = 5f;

    [Tooltip("If true, pick a random depth (distance) from the camera between minDepth and maxDepth")]
    public bool useRandomDepthFromCamera = false;
    public float minDepth = 1f;
    public float maxDepth = 5f;

    Camera cam;
    float timer;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogWarning("No Camera.main found. Spawner will not spawn until a camera exists.");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            SpawnRandomInCamera();
        }
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

        // Random viewport coords (inside camera view)
        float vx = Random.value; // 0..1
        float vy = Random.value; // 0..1

        // Determine z distance from camera for ViewportToWorldPoint
        float zDistance;
        if (useRandomDepthFromCamera)
        {
            // ensure min/max are valid
            if (minDepth > maxDepth) { var t = minDepth; minDepth = maxDepth; maxDepth = t; }
            zDistance = Random.Range(minDepth, maxDepth);
        }
        else
        {
            // distance from camera to desired world z plane
            zDistance = Mathf.Abs(spawnZ - cam.transform.position.z);
        }

        Vector3 viewportPoint = new Vector3(vx, vy, zDistance);
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPoint);

        // Force exact z plane (useful for 2D)
        worldPos.z = spawnZ;

        GameObject go = Instantiate(Circle, worldPos, Quaternion.identity);
        if (lifetime > 0f)
            Destroy(go, lifetime);
    }
}