using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomAudioManager : MonoBehaviour
{
    [SerializeField] private RoomAudioConfig roomAudioConfig;
    [SerializeField] private ProCamera2DRooms proCamera2DRooms;

    private AudioManager audioManager;
    private HashSet<string> currentAmbientSounds = new HashSet<string>();
    private string currentMusicTrack;
    private string currentRoomId = "";

    private void Start()
    {
        audioManager = AudioManager.Instance;

        // Subscribe to the ProCamera2DRooms events
        if (proCamera2DRooms != null)
        {
            // OnStartedTransition is called when entering a new room
            proCamera2DRooms.OnStartedTransition.AddListener(OnRoomTransitionStarted);
            // OnExitedAllRooms is called when the player leaves any room
            proCamera2DRooms.OnExitedAllRooms.AddListener(OnExitedAllRooms);
        }
        else
        {
            Debug.LogError("ProCamera2DRooms reference not set in RoomAudioManager!");
        }
    }

    private void OnDestroy()
    {
        // Clean up event subscriptions
        if (proCamera2DRooms != null)
        {
            proCamera2DRooms.OnStartedTransition.RemoveListener(OnRoomTransitionStarted);
            proCamera2DRooms.OnExitedAllRooms.RemoveListener(OnExitedAllRooms);
        }

        // Stop all ambient sounds when the manager is destroyed
        foreach (var ambient in currentAmbientSounds.ToList())
        {
            audioManager.StopAmbient(ambient, fadeOut: true);
        }
        currentAmbientSounds.Clear();

        // Stop music if any is playing
        if (!string.IsNullOrEmpty(currentMusicTrack))
        {
            audioManager.StopMusic(fadeOut: true);
            currentMusicTrack = null;
        }
    }

    // Called when transitioning to a new room
    private void OnRoomTransitionStarted(int toRoomIndex, int fromRoomIndex)
    {
        // Convert room index to room ID
        string toRoomId = "";
        string fromRoomId = "";

        if (toRoomIndex >= 0 && toRoomIndex < proCamera2DRooms.Rooms.Count)
        {
            toRoomId = proCamera2DRooms.Rooms[toRoomIndex].ID;
        }
        if (fromRoomIndex >= 0 && fromRoomIndex < proCamera2DRooms.Rooms.Count)
        {
            fromRoomId = proCamera2DRooms.Rooms[fromRoomIndex].ID;
        }

        HandleRoomTransition(fromRoomId, toRoomId);
    }

    // Called when exiting all rooms
    private void OnExitedAllRooms()
    {
        foreach (var ambient in currentAmbientSounds.ToList())
        {
            audioManager.StopAmbient(ambient, fadeOut: true);
            currentAmbientSounds.Remove(ambient);
        }

        if (!string.IsNullOrEmpty(currentMusicTrack))
        {
            audioManager.StopMusic(fadeOut: true);
            currentMusicTrack = null;
        }

        currentRoomId = "";
    }

    private void HandleRoomTransition(string fromRoomId, string toRoomId)
    {
        if (currentRoomId == toRoomId) return;
        currentRoomId = toRoomId;

        var roomSetup = roomAudioConfig.GetRoomSetup(toRoomId);

        if (roomSetup != null)
        {
            // Handle music transition
            if (roomSetup.musicTrackName != currentMusicTrack)
            {
                if (roomSetup.crossfadeMusic)
                {
                    audioManager.PlayMusic(roomSetup.musicTrackName, fadeIn: true);
                }
                else
                {
                    audioManager.StopMusic(fadeOut: false);
                    audioManager.PlayMusic(roomSetup.musicTrackName, fadeIn: false);
                }
                currentMusicTrack = roomSetup.musicTrackName;
            }

            // Handle ambient sounds
            if (roomSetup.exclusiveAmbient)
            {
                foreach (var ambient in currentAmbientSounds.ToList())
                {
                    if (!roomSetup.ambientSoundNames.Contains(ambient))
                    {
                        audioManager.StopAmbient(ambient, fadeOut: true);
                        currentAmbientSounds.Remove(ambient);
                    }
                }
            }

            foreach (var ambient in roomSetup.ambientSoundNames)
            {
                if (!currentAmbientSounds.Contains(ambient))
                {
                    audioManager.PlayAmbient(ambient, fadeIn: true);
                    currentAmbientSounds.Add(ambient);
                }
            }
        }
    }

    // Optional Method to manually trigger room audio
    // Might be useful in future
    public void TriggerRoomAudio(string roomId)
    {
        HandleRoomTransition(currentRoomId, roomId);
    }
}
