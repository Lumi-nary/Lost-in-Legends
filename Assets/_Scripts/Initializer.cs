using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Initializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void Execute()
    {
        //Debug.Log("Loaded by the Persist objects from the initializer script");
        //Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Persistence Objects")));
    }
}
