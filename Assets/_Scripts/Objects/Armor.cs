using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    private ArmorData armorData;

    public override void Initialize(ItemData itemData)
    {
        if (itemData is ArmorData armorSpecificData)
        {
            data = itemData;
            armorData = armorSpecificData;
        }
        else
        {
            Debug.LogError("Tried to initialize Armor with non-armor data!");
        }
    }

    public override void OnEquip(PlayerController player)
    {
        if (armorData.playerSprite != null)
        {
            // Update player sprite
        }

        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ModifyHealth(armorData.healthBonus);
        }

        if (armorData.enableWallJump)
        {
            player.EnableWallJump();
        }
    }

    public override void OnUnequip(PlayerController player)
    {

        var playerHealth = player.GetComponent<PlayerHealth>();
        var playerMana = player.GetComponent<PlayerMana>();


        player.ModifySpeed(1f / armorData.moveSpeedModifier);
        playerHealth.ModifyHealth(-armorData.healthBonus);
        playerMana.ModifyMana(-armorData.manaBonus);

        if (armorData.enableWallJump)
        {
            player.DisableWallJump();
        }
    }

    public bool HasResistance(ElementType element)
    {
        return Array.Exists(armorData.resistances, e => e == element);
    }
}
