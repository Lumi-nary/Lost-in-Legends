using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    public UIElementType ElementType { get; protected set; }
    public abstract void Initialize();
}
