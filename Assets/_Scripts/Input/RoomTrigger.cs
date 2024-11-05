using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using System.Linq;
public class RoomTrigger : BasePC2D
{
    [System.Serializable]
    public class RoomSettings
    {
        public string RoomID;
        public AmbientKey AmbientSound;
        public MusicKey MusicTrack;
    }

    public List<RoomSettings> RoomConfigurations; // Configurations for each room
    private ProCamera2DRooms _proCamera2DRooms;

    private string _currentRoomID = string.Empty;
    private AmbientKey _currentAmbient;
    private MusicKey _currentMusic;
    private bool _isTransitioning = false;

    public bool IsTransitioning { get => _isTransitioning; set => _isTransitioning = value; }

    protected override void Awake()
    {
        // Fetch the ProCamera2DRooms component
        _proCamera2DRooms = ProCamera2D.GetComponent<ProCamera2DRooms>();

        if (_proCamera2DRooms == null)
        {
            Debug.LogError("ProCamera2DRooms component not found!");
            return;
        }

        // Subscribe to both start and finish of transitions
        _proCamera2DRooms.OnStartedTransition.AddListener(OnStartTransition);
        _proCamera2DRooms.OnFinishedTransition.AddListener(OnRoomChanged);
    }

    private void OnStartTransition(int newRoomIndex, int previousRoomIndex)
    {
        IsTransitioning = true;
    }

    private void OnRoomChanged(int newRoomIndex, int previousRoomIndex)
    {
        IsTransitioning = false;
        var room = _proCamera2DRooms.CurrentRoom;
        if (room != null && room.ID != _currentRoomID)
        {
            _currentRoomID = room.ID;
            ApplyRoomSettings(room.ID);
        }
    }

    private void ApplyRoomSettings(string roomID)
    {
        var roomSettings = RoomConfigurations.Find(r => r.RoomID == roomID);
        if (roomSettings == null) return;

        // Check if the current room settings match the new room settings
        if (roomSettings.AmbientSound.Equals(_currentAmbient) && roomSettings.MusicTrack.Equals(_currentMusic))
        {
            return;
        }

        // Handle ambient sound changes
        if (!roomSettings.AmbientSound.Equals(_currentAmbient))
        {
            if (_currentAmbient != AmbientKey.None)
            {
                AudioManager.Instance.StopAmbient(_currentAmbient);
            }
            AudioManager.Instance.PlayAmbient(roomSettings.AmbientSound);
            _currentAmbient = roomSettings.AmbientSound;
        }

        // Handle music track changes
        if (!roomSettings.MusicTrack.Equals(_currentMusic))
        {
            if (_currentMusic != MusicKey.None)
            {
                AudioManager.Instance.StopMusic(true);
            }
            AudioManager.Instance.PlayMusic(roomSettings.MusicTrack);
            _currentMusic = roomSettings.MusicTrack;
        }
    }

    // Optional: Handle scene load to reset room index and audio
    protected override void OnDisable()
    {
        base.OnDisable();
        if (_proCamera2DRooms != null)
        {
            _proCamera2DRooms.OnStartedTransition.RemoveListener(OnStartTransition);
            _proCamera2DRooms.OnFinishedTransition.RemoveListener(OnRoomChanged);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _currentRoomID = string.Empty;
        _isTransitioning = false;
    }
}
