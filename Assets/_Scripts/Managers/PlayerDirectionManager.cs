using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionManager : MonoBehaviour
{
    public static PlayerDirectionManager Instance { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;

    public bool IsFacingRight => playerMovement.IsFacingRight;
    public Vector2 FacingDirection => IsFacingRight ? Vector2.right : Vector2.left;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
