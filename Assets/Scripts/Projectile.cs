using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public ParticleSystem HitBurst;
 

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    
    

    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }

        Instantiate(HitBurst, rigidbody2d.position + Vector2.up, Quaternion.identity);

        HardEnemyController s = other.collider.GetComponent<HardEnemyController>();
        if (s != null)
        {
            s.Fix();
        }
    
        Instantiate(HitBurst, rigidbody2d.position + Vector2.up, Quaternion.identity);

        Destroy(gameObject);
    }

}
