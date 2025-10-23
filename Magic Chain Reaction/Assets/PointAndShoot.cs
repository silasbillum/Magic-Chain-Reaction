using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    public GameObject Wand;
    private Vector3 target;

    public GameObject Fireball;
    public float projectileSpeed = 30.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Wand.transform.position = new Vector2(target.x, target.y);

        Vector3 difference = target - Wand.transform.position;
        
       

        if (Input.GetMouseButtonDown(0))
        {
            float distance = difference.magnitude;
            Vector2 direction = difference / distance;
            direction.Normalize();
            FireProjectile(direction);
        }
        
    }

    void FireProjectile (Vector2 direction)
    {
        GameObject f = Instantiate(Fireball) as GameObject;
        f.transform.position = Wand.transform.position;
        f.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;
        
    }
}
