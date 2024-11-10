using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NianController : MonoBehaviour, IDamagable, IKnockbackable
{
    private enum State
    {
        Walking,
        Knockback,
        Dead
    }

    private State currentState;

    [SerializeField] private float groundCheckDistance, wallCheckDistance, movementSpeed, maxHealth, knockbackDuration;
    [SerializeField] private Transform groundCheck, wallCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector2 knockbackSpeed;
    [SerializeField] private GameObject hitParticle, deathChunkParticle, deathBloodParticle;

    private float currentHealth, knockbackStartTime;
    private int facingDirection, damageDirection;
    private Vector2 movement;
    private bool groundDetected, wallDetected;
    private bool knockback;


    private bool playerFacingRight;
    private bool playerFacingLeft;

    private GameObject alive;
    private Rigidbody2D aliveRb;
    private Animator aliveAnim;

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent <Animator>();

        currentHealth = maxHealth;
        facingDirection = 1;
    }
    private void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                UpdateWalkingState();
                break;

            case State.Knockback: 
                UpdateKnockbackState(); 
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    #region Walking State
    private void EnterWalkingState()
    {

    }
    private void UpdateWalkingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);

        if (!groundDetected || wallDetected)
        {
            Flip();

        }
        else
        {
            movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }
    private void ExitWalkingState()
    {

    }
    #endregion

    #region Knockback State
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("Knockback", true);
    }
    private void UpdateKnockbackState()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Walking);
        }
    }
    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("Knockback", false);
    }
    #endregion

    #region Dead State
    private void EnterDeadState()
    {
        // Spawns particles 
        Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
        AudioManager.Instance.PlaySFX(SFXKey.PlayerDeath);
        Destroy(gameObject);
    }
    private void UpdateDeadState() { 

    }
    private void ExitDeadState() { 

    }
    #endregion

    #region Other Functions
    public void TakeDamage(DamageData damageData)
    {
        currentHealth -= damageData.amount;

        // If the damage is from player, update the facing direction
        if (damageData.isFromPlayer)
        {
            playerFacingRight = PlayerDirectionManager.Instance.IsFacingRight;
            playerFacingLeft = !playerFacingRight;
        }

        // Hit particle
        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));


            
        if (currentHealth > 0) {
            SwitchState(State.Knockback);
        } else if (currentHealth <= 0)
        {
            SwitchState(State.Dead);
        }
    }

    public void ApplyKnockBack(Vector2 direction, float force)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        aliveRb.velocity = direction * force;
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            aliveRb.velocity = new Vector2(0.0f, aliveRb.velocity.y);
        }
    }

    public bool CanBeDamagedBy(GameObject source)
    {
        return true; // or add any additional checks here
    }

    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f); 
    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalkingState();
                break;

            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Walking:
                EnterWalkingState();
                break;

            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
        currentState = state;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
