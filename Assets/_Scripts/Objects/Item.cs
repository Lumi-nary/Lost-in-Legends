using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected ItemData data;

    public int ID => data.id;
    public string ItemName => data.itemName;
    public Sprite ItemSprite => data.itemSprite;
    public ItemClass ItemClass => data.itemClass;
    public string Description => data.description;

    public abstract void Initialize(ItemData itemData);
    public abstract void OnEquip(PlayerController player);
    public abstract void OnUnequip(PlayerController player);
}
