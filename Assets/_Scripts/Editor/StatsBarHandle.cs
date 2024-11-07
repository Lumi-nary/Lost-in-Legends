using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;


[CustomEditor(typeof(StatsBar))]
public class StatsBarHandle : Editor
{
    private bool showColorControls = false;
    public override void OnInspectorGUI()
    {
        StatsBar statsBar = (StatsBar)target;

        // Draw default inspector
        DrawDefaultInspector();

        // Get the config
        var configProperty = serializedObject.FindProperty("config");
        var config = configProperty.objectReferenceValue as StatsBarConfig;

        if (config != null && !Application.isPlaying)
        {
            EditorGUILayout.Space();

            // Add fill amount slider
            var fillProperty = serializedObject.FindProperty("editorFillAmount");
            EditorGUI.BeginChangeCheck();
            float newFillAmount = EditorGUILayout.Slider("Fill Amount", fillProperty.floatValue, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                fillProperty.floatValue = newFillAmount;
                serializedObject.ApplyModifiedProperties();
                statsBar.SetEditorFillAmount(newFillAmount);
            }

            EditorGUILayout.Space();
            showColorControls = EditorGUILayout.Foldout(showColorControls, "Color Controls", true);

            if (showColorControls)
            {
                EditorGUI.indentLevel++;

                // Only show color controls if useColorChange is enabled
                if (config.useColorChange)
                {
                    EditorGUI.BeginChangeCheck();

                    Color newMaxColor = EditorGUILayout.ColorField("Max Color", config.maxColor);
                    Color newMinColor = EditorGUILayout.ColorField("Min Color", config.minColor);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(config, "Change Stats Bar Colors");

                        config.maxColor = newMaxColor;
                        config.minColor = newMinColor;

                        // Update the fill image color based on current fill amount
                        var fillImage = statsBar.GetComponentInChildren<UnityEngine.UI.Image>();
                        if (fillImage != null)
                        {
                            statsBar.SetEditorFillAmount(fillImage.fillAmount);
                        }

                        EditorUtility.SetDirty(config);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Enable 'Use Color Change' in the config to modify colors.", MessageType.Info);
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}