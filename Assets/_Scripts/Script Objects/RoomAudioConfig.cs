using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomAudioConfig", menuName = "Audio/Room Audio Configuration")]
public class RoomAudioConfig : ScriptableObject
{
    [System.Serializable]
    public class RoomAudio
    {
        public string roomName;
        [Tooltip("Should match the Room ID in ProCamera2DRooms")]
        public string roomId;
        [Tooltip("Name of the music track from AudioConfig")]
        public string musicTrackName;
        [Tooltip("Names of ambient sounds from AudioConfig")]
        public List<string> ambientSoundNames = new List<string>();
        [Tooltip("If true, music will crossfade when entering/leaving this room")]
        public bool crossfadeMusic = true;
        [Tooltip("If false, ambient sounds from previous room will continue playing")]
        public bool exclusiveAmbient = true;
    }

    public List<RoomAudio> roomSetups = new List<RoomAudio>();

    public RoomAudio GetRoomSetup(string roomId)
    {
        return roomSetups.FirstOrDefault(setup => setup.roomId == roomId);
    }
}