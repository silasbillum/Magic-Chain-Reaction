using System.Threading;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float speed = 2f;
    public float changeDirectionTime = 1.5f;

    private Vector2 direction;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sign(angle * Mathf.Deg2Rad)).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fireball"))
        {
            Destroy(gameObject);
        }
    }


}
