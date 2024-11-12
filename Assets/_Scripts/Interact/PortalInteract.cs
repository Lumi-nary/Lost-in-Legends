using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteract : TriggerInteractionBase
{
    [SerializeField] private SceneField _sceneToLoad;

    public override void Interact()
    {
        SceneSwapManager.SwapScene(_sceneToLoad);
    }
}
