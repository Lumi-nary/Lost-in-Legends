using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorData : ItemData
{
    [Header("Armor Properties")]
    public Sprite playerSprite;
    public float defense;
    public float moveSpeedModifier = 1f;

    [Header("SFX")]
    public SFXKey movementSound;

    [Header("Resistances")]
    public ElementType[] resistances;

    [Header("Set Effects")]
    public float healthBonus;
    public float manaBonus;
    public float regenRate;
    public bool enableWallJump;

    public override Item CreateInstance(GameObject gameObject)
    {
        Armor armor = gameObject.AddComponent<Armor>();
        armor.Initialize(this);
        return armor;
    }
}
