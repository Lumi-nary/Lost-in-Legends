using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorInteract))]
class LabelHandle : Editor
{
    private static GUIStyle labelStyle;
    private void OnEnable()
    {
        labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.MiddleCenter;
    }
    private void OnSceneGUI()
    {
        DoorInteract door = (DoorInteract)target;

        Handles.BeginGUI();
        Handles.Label(door.transform.position + new Vector3(0f, 4f, 0f), door.CurrentDoorPosition.ToString(), labelStyle);
        Handles.EndGUI();
    }
}
