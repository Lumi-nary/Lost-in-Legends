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
    private readonly string IS_JUMPING = "isJumping";
    private readonly string X_VELOCITY = "xVelocity";
    private readonly string Y_VELOCITY = "yVelocity";
    private readonly string IS_HURT = "isHurt";
    private readonly string DIE = "Die";
    private readonly string isDead = "isDead";
    //private readonly string IS_WALL_SLIDING = "isWallSliding";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isDead)
        {
            UpdateAnimationState();
        }
    }

    private void UpdateAnimationState()
    {
        // Update jumping state
        _animator.SetBool(IS_JUMPING, _movement.IsJumping);

        // Cancel jump animation and transition to idle when player touches the ground
        if (_movement.LastOnGroundTime > 0 && _animator.GetBool(IS_JUMPING))
        {
            _animator.SetBool(IS_JUMPING, false);
        }

        // Update velocity parameters
        _animator.SetFloat(X_VELOCITY, Mathf.Abs(_rb.velocity.x));
        _animator.SetFloat(Y_VELOCITY, _rb.velocity.y);


        // Update wall sliding state
        //_animator.SetBool(IS_WALL_SLIDING, _movement.IsSliding);
    }
    public void PlayHurtAnimation()
    {
        if (!_isDead)
        {
            StartCoroutine(HurtAnimationRoutine());
        }
    }

    private IEnumerator HurtAnimationRoutine()
    {
        _animator.SetBool(IS_HURT, true);
        yield return new WaitForSeconds(hurtAnimationDuration);
        _animator.SetBool(IS_HURT, false);
    }

    public void PlayDeathAnimation()
    {
        _isDead = true;
        _animator.SetBool(isDead, _isDead);
        _animator.SetTrigger(DIE);
    }

    // Call this if you need to reset the death state (e.g., when respawning)
    public void ResetDeathState()
    {
        _isDead = false;
        _animator.SetBool(isDead, _isDead);
    }
}
