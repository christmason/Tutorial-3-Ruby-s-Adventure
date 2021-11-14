using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    public ParticleSystem HealthBurst;
    Rigidbody2D rigidbody2d;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
            	controller.ChangeHealth(1);
            	Destroy(gameObject);
            	controller.PlaySound(collectedClip);
                
            }
        }

    }


}