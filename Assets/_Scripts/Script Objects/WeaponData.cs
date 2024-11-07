using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponData : ItemData
{
    [Header("Weapon Properties")]
    public WeaponType weaponType;
    public ElementType elementType;
    public float damage;
    public float attackRate;
    public float manaCost;

    [Header("SFX Settings")]
    public SFXKey useSound;
    public SFXKey impactSound;
    public SFXKey chargeSound;
    public bool hasLoopingSound;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed;

    public override Item CreateInstance(GameObject gameObject)
    {
        Weapon weapon = gameObject.AddComponent<Weapon>();
        weapon.Initialize(this);
        return weapon;
    }
}
