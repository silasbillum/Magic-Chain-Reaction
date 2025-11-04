using UnityEngine;

public class SpawnInCameraView : MonoBehaviour, IUpgradeListener
{
    [Header("Prefabs & Timing")]
    public GameObject normalCirclePrefab;
    public GameObject tankCirclePrefab;
    public GameObject blackholeCirclePrefab;
    public GameObject spawnerCirclePrefab;

    public float baseInterval = 1f;
    public float spawnZ = 0f;
    public float lifetime = 5f;

    private bool wasInHighComboModeLastFrame = false;

    [Header("Random Depth")]
    public bool useRandomDepthFromCamera = false;
    public float minDepth = 1f;
    public float maxDepth = 5f;

    [Header("Spawn Settings")]
    public int baseSpawnCount = 1;
    private int spawnCount;
    private float timer;
    private Camera cam;
    public float currentInterval;


    // Spawn chances (total doesn’t have to be 100, just relative)
    [Header("Spawn Chances (weights)")]
    [Range(0, 100)] public float normalChance = 65f;
    [Range(0, 100)] public float blackholeChance = 1f;
    [Range(0, 100)] public float tankChance = 15f;
    [Range(0, 100)] public float spawnerChance = 15f;

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
        ComboSystem comboSystem = FindFirstObjectByType<ComboSystem>();
        bool highCombo = comboSystem != null && comboSystem.comboScore >= 200;

        if (highCombo && !wasInHighComboModeLastFrame)
        {
            Debug.Log("🌀 High Combo Mode! Blackholes entering the field!");
            // You can trigger a sound or particle effect here if you want
        }

        timer += Time.deltaTime;
        if (timer >= currentInterval)
        {
            timer = 0f;
            SpawnCircles();
        }
    }

    private void SpawnCircles()
    {
        if (cam == null) return;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefabToSpawn = ChooseCircleType();
            if (prefabToSpawn == null) continue;

            float vx = Random.value;
            float vy = Random.value;
            float zDistance = useRandomDepthFromCamera
                ? Random.Range(Mathf.Min(minDepth, maxDepth), Mathf.Max(minDepth, maxDepth))
                : Mathf.Abs(spawnZ - cam.transform.position.z);

            Vector3 viewportPoint = new Vector3(vx, vy, zDistance);
            Vector3 worldPos = cam.ViewportToWorldPoint(viewportPoint);
            worldPos.z = spawnZ;

            GameObject go = Instantiate(prefabToSpawn, worldPos, Quaternion.identity);

            // Assign base stats depending on type
            Target target = go.GetComponent<Target>();
            if (target != null)
            {
                ConfigureTargetByType(go.name, target);

                // ✅ Apply all upgrades AFTER base stats
                if (UpgradeManager.Instance != null)
                    target.OnUpgradesApplied(UpgradeManager.Instance);
            }

            if (lifetime > 0f)
                Destroy(go, lifetime);
        }
    }


    // Weighted random choice
    private GameObject ChooseCircleType()
    {
        float currentCombo = 0f;
        ComboSystem comboSystem = FindFirstObjectByType<ComboSystem>();
        if (comboSystem != null)
            currentCombo = comboSystem.comboScore;

        bool highComboMode = currentCombo >= 200;

        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        // --- Normal circles always available before combo 200 ---
        if (!highComboMode)
        {
            cumulative += normalChance;
            if (roll < cumulative) return normalCirclePrefab;
        }

        // --- Spawner and Tank are always possible ---
        cumulative += spawnerChance;
        if (roll < cumulative) return spawnerCirclePrefab;

        cumulative += tankChance;
        if (roll < cumulative) return tankCirclePrefab;

        // --- Blackhole only appears after combo 200 ---
        if (highComboMode)
        {
            cumulative += blackholeChance;
            if (roll < cumulative) return blackholeCirclePrefab;
        }

        // fallback
        return normalCirclePrefab;
    }


    // Configure stats based on prefab type
    private void ConfigureTargetByType(string name, Target t)
    {
        // Reset defaults
        t.projectileCount = 2;
        t.speed = 2f;
        t.fireBallSpeed = 5f;
        t.lifetime = 5f;
        

        if (name.Contains("Tank"))
        {
            t.targetType = TargetType.Tank;
            t.health = 5;
            t.projectileCount = 2; // tanks still multiply
            t.speed = 1.2f;
        }
        else if (name.Contains("Blackhole"))
        {
            t.targetType = TargetType.Blackhole;
            t.health = 20;
            t.projectileCount = 0; // blackhole doesn't multiply
            t.speed = 0.8f;
        }
        else if (name.Contains("Spawner"))
        {
            t.targetType = TargetType.Spawner;
            t.health = 1;
            t.projectileCount = 15; // spawns 15 on death
            t.speed = 1.5f;
        }
        else
        {
            t.targetType = TargetType.Normal;
            t.health = 1;
            t.projectileCount = 1;
            t.speed = 2.5f;
            t.lifetime = 1f;
        }
    }


    // Called automatically by UpgradeManager.ApplyUpgradesToScene()
    public void OnUpgradesApplied(UpgradeManager upgrades)
    {
        currentInterval = upgrades.circleInterval;
        spawnCount = upgrades.circleSpawnCount;
    }
}
