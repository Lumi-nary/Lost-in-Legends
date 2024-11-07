using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : DamageDealer
{
    [Header("Player Combat")]
    [SerializeField] private bool combatEnabled;
    [SerializeField] private float inputTimer, attack1Radius;
    [SerializeField] private Transform attack1HitboxPos;
    [SerializeField] private LayerMask whatIsDamageable;

    private bool gotInput, isAttacking;
    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (userInput.WasAttackPressed)
        {
           if (combatEnabled)
            {
                // Attempt Combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            // Perform attack
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                anim.SetBool("attack1", true);
                anim.SetBool("isAttacking", isAttacking);
                AudioManager.Instance.PlaySFX(SFXKey.SwordWhoosh, attack1HitboxPos.position);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            // Wait for new input
            gotInput = false;
        }
    }

    // Do not remove this
    // the reference is from Attack Animation
    private void CheckAttackHitbox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitboxPos.position, attack1Radius, whatIsDamageable);
        foreach (Collider2D collider in detectedObjects)
        {

            //var damageable = collider.transform.parent.GetComponent<IDamagable>();
            //if (damageable != null)
            //{
            //    damageable.TakeDamage(new DamageData(amount: 10, gameObject, knockbackForce: 50));
            //}
            GameObject target = collider.transform.parent.gameObject;
            DealDamage(target);
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitboxPos.position, attack1Radius);
    }
}
