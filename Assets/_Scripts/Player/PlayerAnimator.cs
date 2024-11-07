using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _movement;
    private Rigidbody2D _rb;
    private bool _isDead;

    [Header("Animation Parameters")]
    //[SerializeField] private float _runningThreshold = 0.01f;
    [Header("Hurt Animation")]
    [SerializeField] private float hurtAnimationDuration = 0.5f; // Duration of hurt animation


    // Animator Parameter Names
    private readonly string IS_GROUNDED = "isGrounded";
    private readonly string IS_WALL_SLIDING = "isWallSliding";
    private readonly string X_VELOCITY = "xVelocity";
    private readonly string Y_VELOCITY = "yVelocity";
    private readonly string IS_HURT = "isHurt";
    private readonly string DIE = "Die";
    private readonly string IS_DEAD = "isDead";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        // Check death state first
        bool isDead = _animator.GetBool(IS_DEAD);

        // Only update other animations if not in death state
        if (!isDead)
        {
            // Update grounded state
            _animator.SetBool(IS_GROUNDED, _movement.LastOnGroundTime > 0);

            // Update velocity parameters
            _animator.SetFloat(X_VELOCITY, Mathf.Abs(_rb.velocity.x));
            _animator.SetFloat(Y_VELOCITY, _rb.velocity.y);

            // Update wall sliding state
            _animator.SetBool(IS_WALL_SLIDING, _movement.IsSliding);
        }
    }
    public void PlayHurtAnimation()
    {
        if (!_animator.GetBool(IS_DEAD))
        {
            StartCoroutine(HurtAnimationRoutine());
        }
    }

    private IEnumerator HurtAnimationRoutine()
    {
        _animator.SetBool(IS_HURT, true);
        yield return new WaitForSeconds(hurtAnimationDuration);
        if (!_animator.GetBool(IS_DEAD)) // Check if not dead before resetting hurt
        {
            _animator.SetBool(IS_HURT, false);
        }
    }

    public void PlayDeathAnimation()
    {
        // Force stop all other animations immediately
        _animator.StopPlayback();

        // Set the death state
        _animator.SetBool(IS_DEAD, true);

        // Reset all other boolean parameters
        _animator.SetBool(IS_WALL_SLIDING, false);
        _animator.SetBool(IS_HURT, false);
        _animator.SetBool(IS_GROUNDED, false);

        // Reset velocity parameters
        _animator.SetFloat(X_VELOCITY, 0);
        _animator.SetFloat(Y_VELOCITY, 0);

        // Trigger death animation
        _animator.SetTrigger(DIE);
    }

    // Call this if you need to reset the death state (e.g., when respawning)
    public void ResetDeathState()
    {
        _animator.SetBool(IS_DEAD, false);
        _isDead = false;
    }
}
