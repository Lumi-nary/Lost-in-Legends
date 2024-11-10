using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : DamageDealer
{
    [Header("Contact Damage")]
    [SerializeField] private bool useCollision = true;
    [SerializeField] private bool useTrigger = true;
    [SerializeField] private bool useRaycast = true;
    [SerializeField] private Vector2 detectionSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 detectionOffset = Vector2.zero;
    [SerializeField] private LayerMask targetLayers;

    private void Update()
    {
        if (useRaycast)
        {
            // Calculate the center position with offset
            Vector2 center = (Vector2)transform.position + detectionOffset;

            // Check for overlapping colliders using a box
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, detectionSize, 0f, targetLayers);

            foreach (Collider2D hitCollider in hitColliders)
            {
                DealDamage(hitCollider.gameObject);
            }
        }

    }

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
    // Visualize the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + detectionOffset;
        Gizmos.DrawWireCube(center, detectionSize);
    }
}
