using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    public GameObject Wand;
    public GameObject Fireball;

    public float projectileSpeed = 5.0f;

    public float forwardOffset = 1.0f;
    public float sideOffset = 0.2f;
    public float angleOffset = 45f;

    public int lifeTime = 7;

    private Vector3 target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float camDistance = Mathf.Abs(Camera.main.transform.position.z - Wand.transform.position.z);
        target = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDistance)
        );

        target.z = Wand.transform.position.z;
        Wand.transform.position = target;

        if (Input.GetMouseButtonDown(0))
        {
          
            FireProjectile();
        }
        
    }

    

    void FireProjectile ()
    {
        Vector2 direction = -Wand.transform.right;

        
        direction = Quaternion.Euler(0, 0, -angleOffset) * direction;
        direction.Normalize();

       
        Vector3 spawnPos = Wand.transform.position
                         + (Vector3)(direction * forwardOffset)
                         + (Wand.transform.right * sideOffset);

       
        GameObject f = Instantiate(Fireball, spawnPos, Quaternion.identity);

       
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        f.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

       
        Rigidbody2D rb = f.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * projectileSpeed;

    }

    
}
