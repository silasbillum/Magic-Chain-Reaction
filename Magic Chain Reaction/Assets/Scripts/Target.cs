using System.Threading;
using UnityEngine;


public class Target : MonoBehaviour
{
    public float speed = 2f;
    public float changeDirectionTime = 1.5f;
    public int Count = 2;
    public float fireBallSpeed = 5;

    private Vector2 direction;
    private float timer;

    public GameObject Explosion;

    public GameObject Fireball;

    private ComboSystem comboSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        comboSystem = FindFirstObjectByType<ComboSystem>();
        PickNewDirection();
       
    }

    // Update is called once per frame
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
        if (other.CompareTag("Fireball"))
        {
            Instantiate(Explosion, transform.position, transform.rotation);

            if(comboSystem != null)
            {
                comboSystem.AddCombo();
            }
            
            Destroy(gameObject);
            Multiply();
               
        }
    }

    void Multiply()
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject f = Instantiate(Fireball, transform.position, Quaternion.identity);

            float angle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            Rigidbody2D rb = f.GetComponent<Rigidbody2D>();
            if(rb != null )
            {
                rb.linearVelocity = direction * fireBallSpeed;
            }
        }
    }


}
