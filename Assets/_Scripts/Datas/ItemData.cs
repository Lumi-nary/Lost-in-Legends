using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("Basic Properties")]
    public int id;
    public string itemName;
    public Sprite itemSprite;
    public ItemClass itemClass;
    [TextArea(3, 5)]
    public string description;

    // Factory method to create the actual item behavior component
    public abstract Item CreateInstance(GameObject gameObject);
}
