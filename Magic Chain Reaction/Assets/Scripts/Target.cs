using UnityEngine;

public enum TargetType { Normal, Tank, Blackhole, Spawner }

public class Target : MonoBehaviour
{
    [Header("Stats")]
    public TargetType targetType = TargetType.Normal;
    public int health = 1;
    public float speed = 2f;
    public float changeDirectionTime = 1.5f;
    public int projectileCount = 2;
    public float fireBallSpeed = 5;
    public float lifetime = 5f;

    [Header("Effects")]
    public GameObject Explosion;
    public GameObject Fireball;
    public GameObject Lightning;

    private Vector2 direction;
    private float timer;
    private bool isDestroyed = false;

    private ComboSystem comboSystem;

    void Start()
    {
        comboSystem = FindFirstObjectByType<ComboSystem>();
        PickNewDirection();

        if (UpgradeManager.Instance != null)
            OnUpgradesApplied(UpgradeManager.Instance);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            PickNewDirection();
            timer = 0;
        }
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        if (pos.x < cam.transform.position.x - camWidth / 2 || pos.x > cam.transform.position.x + camWidth / 2)
            direction.x *= -1;

        if (pos.y < cam.transform.position.y - camHeight / 2 || pos.y > cam.transform.position.y + camHeight / 2)
            direction.y *= -1;

        transform.position = pos;
    }

    void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Fireball"))
        {
            // fireball always disappears when it hits anything
            Destroy(other.gameObject);

            TakeDamage(1);


            // only destroy target when health reaches 0
            if (health <= 0)
            {
                isDestroyed = true;

                if (Explosion != null)
                    Instantiate(Explosion, transform.position, transform.rotation);

               
                if (comboSystem != null)
                    comboSystem.AddCombo();

                if (targetType == TargetType.Spawner)
                {
                    SpawnLightning(); // ⚡ special behavior
                }


                if (targetType != TargetType.Blackhole && projectileCount > 0)
                    Multiply();

                Destroy(gameObject);
            }
            else
            {
                
                transform.localScale *= 0.95f;
            }
        }
    }

    void Multiply()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject f = Instantiate(Fireball, transform.position, Quaternion.identity);

            float angle = Random.Range(0f, 360f);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            Rigidbody2D rb = f.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir * fireBallSpeed;
        }
    }

    void SpawnLightning()
    {
        if (Lightning == null) return;

        GameObject lightning = Instantiate(Lightning, transform.position, Quaternion.identity);

        // Rotate randomly
        lightning.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        // Auto destroy after 1 second
        Destroy(lightning, 1f);
    }

    public void TakeDamage(int amount)
    {
        if (isDestroyed) return;

        health -= amount;
        if (health <= 0)
        {
            isDestroyed = true;

            if (Explosion != null)
                Instantiate(Explosion, transform.position, transform.rotation);

            if (comboSystem != null)
                comboSystem.AddCombo();

            if (targetType == TargetType.Spawner)
                SpawnLightning();
            else if (targetType != TargetType.Blackhole && projectileCount > 0)
                Multiply();

            Destroy(gameObject);
        }
        else
        {
            transform.localScale *= 0.95f;
        }
    }


    public void OnUpgradesApplied(UpgradeManager upgrades)
    {
        projectileCount = Mathf.Max(projectileCount, upgrades.targetProjectileCount);

    }

}
