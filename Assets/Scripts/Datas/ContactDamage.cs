using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : DamageDealer
{
    [SerializeField] private bool useCollision = true;
    [SerializeField] private bool useTrigger = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useCollision)
            DealDamage(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTrigger)
            DealDamage(collision.gameObject);
    }
}
