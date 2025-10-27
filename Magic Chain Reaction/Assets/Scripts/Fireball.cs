using UnityEngine;


public class Fireball : MonoBehaviour
{
    public float destroytimer = 4;
    public float projectileSpeed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroytimer);
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Circle"))
        {
            
          
            Destroy(gameObject);
      

        }
    }
}
