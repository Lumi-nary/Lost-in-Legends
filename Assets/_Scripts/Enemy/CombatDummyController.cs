using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyController : MonoBehaviour, IDamagable, IKnockbackable
{
    [SerializeField]
    private float maxHealth, knockbackDuration, deathTorque;
    private Vector2 customKnockbackDirection = Vector2.right;
    private float currentHealth, knockbackStart;
    private bool playerFacingRight;
    private bool playerFacingLeft;
    private bool knockback;
    private PlayerMovement _playerMovement;
    private GameObject aliveGO, brokenTopGO, brokenBotGO;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBot;
    private Animator aliveAnim;

    private void Update()
    {
        CheckKnockback();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("Broken Top").gameObject;
        brokenBotGO = transform.Find("Broken Bottom").gameObject;

        aliveAnim = aliveGO.GetComponent<Animator>();

        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBotGO.SetActive(false);
    }
    public void TakeDamage(DamageData damageData)
    {
        currentHealth -= damageData.amount;
        playerFacingRight = _playerMovement.IsFacingRight;
        if (playerFacingRight = !_playerMovement.IsFacingRight)
        {
            playerFacingLeft = true;
        }
        else
        {
            playerFacingLeft = false;
        }
        aliveAnim.SetBool("playerOnLeft", playerFacingLeft);
        aliveAnim.SetTrigger("damage");
        AudioManager.Instance.PlaySFX(SFXKey.DummyCombat, aliveGO.transform.position);

        if (damageData.knockbackForce > 0)
        {
            ApplyKnockBack(damageData.knockbackDirection, damageData.knockbackForce);
        }

        if (currentHealth <= 0.0f)
        {
            Die(damageData.knockbackDirection, damageData.knockbackForce);
        }
    }

    public void ApplyKnockBack(Vector2 direction, float force)
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = direction * force;
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    public bool CanBeDamagedBy(GameObject source)
    {
        return true; // or add any additional checks here
    }

    private void Die(Vector2 knockbackDirection, float knockbackForce)
    {
        aliveGO.SetActive(false);
        brokenBotGO.SetActive(true);
        brokenTopGO.SetActive(true);
        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBotGO.transform.position = aliveGO.transform.position;

        // Apply knockback on death
        rbBrokenTop.velocity = knockbackDirection * knockbackForce * 5;
        //rbBrokenBot.velocity = knockbackDirection * knockbackForce;

        rbBrokenTop.AddTorque(deathTorque * Mathf.Sign(knockbackDirection.x), ForceMode2D.Impulse);
    }
}
