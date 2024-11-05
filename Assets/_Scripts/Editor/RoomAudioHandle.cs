//using UnityEngine;
//using UnityEditor;
//using System.Linq;

//[CustomEditor(typeof(RoomAudioConfig))]
//public class RoomAudioConfigHandle : Editor
//{
//    private AudioConfig cachedAudioConfig;
//    private string[] musicTrackNames;
//    private string[] ambientSoundNames;
//    private SerializedProperty roomSetupsProp;
//    private GUIStyle headerStyle;
//    private GUIStyle roomIdStyle;

//    private void LoadAudioConfig()
//    {
//        if (cachedAudioConfig == null)
//        {
//            string[] guids = AssetDatabase.FindAssets("t:AudioConfig");
//            if (guids.Length > 0)
//            {
//                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
//                cachedAudioConfig = AssetDatabase.LoadAssetAtPath<AudioConfig>(path);

//                if (cachedAudioConfig != null)
//                {
//                    musicTrackNames = cachedAudioConfig.musicTracks
//                        .Select(t => t.name)
//                        .ToArray();

//                    ambientSoundNames = cachedAudioConfig.ambientSounds
//                        .Select(a => a.name)
//                        .ToArray();
//                }
//            }
//        }
//    }

//    private void OnEnable()
//    {
//        roomSetupsProp = serializedObject.FindProperty("roomSetups");
//    }

//    public override void OnInspectorGUI()
//    {
//        LoadAudioConfig();
//        serializedObject.Update();

//        if (headerStyle == null)
//        {
//            headerStyle = new GUIStyle(EditorStyles.boldLabel)
//            {
//                fontSize = 12,
//                fontStyle = FontStyle.Bold,
//                alignment = TextAnchor.MiddleLeft
//            };
//        }

//        if (roomIdStyle == null)
//        {
//            roomIdStyle = new GUIStyle(EditorStyles.textField)
//            {
//                alignment = TextAnchor.MiddleLeft
//            };
//        }

//        EditorGUILayout.LabelField("Room Setups", EditorStyles.boldLabel);
//        EditorGUILayout.Space(5);

//        // Draw room setups
//        for (int i = 0; i < roomSetupsProp.arraySize; i++)
//        {
//            SerializedProperty roomSetup = roomSetupsProp.GetArrayElementAtIndex(i);
//            SerializedProperty roomId = roomSetup.FindPropertyRelative("roomId");
//            SerializedProperty roomName = roomSetup.FindPropertyRelative("roomName");

//            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

//            // Room header with ID and name
//            EditorGUILayout.BeginHorizontal();
//            roomSetup.isExpanded = EditorGUILayout.Foldout(roomSetup.isExpanded, "", true);

//            // Room Name as header
//            EditorGUI.BeginChangeCheck();
//            string newName = EditorGUILayout.TextField(roomName.stringValue, headerStyle);
//            if (EditorGUI.EndChangeCheck())
//            {
//                roomName.stringValue = newName;
//            }
//            EditorGUILayout.EndHorizontal();

//            if (roomSetup.isExpanded)
//            {
//                EditorGUI.indentLevel++;

//                // Room ID field (left-aligned)
//                EditorGUILayout.BeginHorizontal();
//                EditorGUILayout.LabelField("Room ID", GUILayout.Width(65));
//                EditorGUI.BeginChangeCheck();
//                string newId = EditorGUILayout.TextField(roomId.stringValue, roomIdStyle);
//                if (EditorGUI.EndChangeCheck())
//                {
//                    roomId.stringValue = newId;
//                }
//                EditorGUILayout.EndHorizontal();

//                // Music Track dropdown
//                SerializedProperty musicTrackName = roomSetup.FindPropertyRelative("musicTrackName");
//                EditorGUILayout.BeginHorizontal();
//                EditorGUILayout.LabelField("Music Track", GUILayout.Width(90));
//                if (musicTrackNames != null && musicTrackNames.Length > 0)
//                {
//                    int currentIndex = System.Array.IndexOf(musicTrackNames, musicTrackName.stringValue);
//                    EditorGUI.BeginChangeCheck();
//                    int newIndex = EditorGUILayout.Popup(currentIndex < 0 ? 0 : currentIndex, musicTrackNames);
//                    if (EditorGUI.EndChangeCheck())
//                    {
//                        musicTrackName.stringValue = musicTrackNames[newIndex];
//                    }
//                }
//                else
//                {
//                    EditorGUILayout.LabelField("No music tracks found");
//                }
//                EditorGUILayout.EndHorizontal();

//                // Crossfade and Exclusive options
//                EditorGUILayout.PropertyField(roomSetup.FindPropertyRelative("crossfadeMusic"));
//                EditorGUILayout.PropertyField(roomSetup.FindPropertyRelative("exclusiveAmbient"));

//                // Ambient Sounds
//                SerializedProperty ambientList = roomSetup.FindPropertyRelative("ambientSoundNames");
//                EditorGUILayout.Space(5);

//                EditorGUILayout.BeginHorizontal();
//                ambientList.isExpanded = EditorGUILayout.Foldout(ambientList.isExpanded, "Ambient Sounds", true);

//                GUILayout.FlexibleSpace();
//                using (new EditorGUI.DisabledScope(!ambientList.isExpanded))
//                {
//                    if (GUILayout.Button("+", GUILayout.Width(25)))
//                    {
//                        ambientList.arraySize++;
//                    }
//                    if (GUILayout.Button("-", GUILayout.Width(25)) && ambientList.arraySize > 0)
//                    {
//                        ambientList.arraySize--;
//                    }
//                }
//                EditorGUILayout.EndHorizontal();

//                if (ambientList.isExpanded)
//                {
//                    EditorGUI.indentLevel++;
//                    for (int j = 0; j < ambientList.arraySize; j++)
//                    {
//                        SerializedProperty element = ambientList.GetArrayElementAtIndex(j);
//                        if (ambientSoundNames != null && ambientSoundNames.Length > 0)
//                        {
//                            int currentIndex = System.Array.IndexOf(ambientSoundNames, element.stringValue);
//                            EditorGUI.BeginChangeCheck();
//                            int newIndex = EditorGUILayout.Popup($"Sound {j}", currentIndex < 0 ? 0 : currentIndex, ambientSoundNames);
//                            if (EditorGUI.EndChangeCheck())
//                            {
//                                element.stringValue = ambientSoundNames[newIndex];
//                            }
//                        }
//                    }
//                    EditorGUI.indentLevel--;
//                }

//                EditorGUI.indentLevel--;
//            }

//            EditorGUILayout.EndVertical();
//            EditorGUILayout.Space(5);
//        }

//        // Room Setup controls at the bottom
//        EditorGUILayout.Space(5);
//        EditorGUILayout.BeginHorizontal();
//        GUILayout.FlexibleSpace();
//        if (GUILayout.Button("Add Room", GUILayout.Width(100)))
//        {
//            roomSetupsProp.arraySize++;
//            // Initialize the new room's name
//            var newRoom = roomSetupsProp.GetArrayElementAtIndex(roomSetupsProp.arraySize - 1);
//            var newRoomName = newRoom.FindPropertyRelative("roomName");
//            newRoomName.stringValue = $"Room {roomSetupsProp.arraySize - 1}";
//        }
//        if (GUILayout.Button("Remove Room", GUILayout.Width(100)) && roomSetupsProp.arraySize > 0)
//        {
//            roomSetupsProp.arraySize--;
//        }
//        EditorGUILayout.EndHorizontal();

//        if (cachedAudioConfig == null)
//        {
//            EditorGUILayout.HelpBox("No AudioConfig asset found in the project. Please create one to enable dropdowns.", MessageType.Warning);
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}