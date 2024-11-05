using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXKey
{
    None,
    // Player
    PlayerHurt,
    PlayerDeath,
    PlayerJump,
    PlayerFootstep,
    PlayerDeathBG,

    // Enemy
    EnemyDeath,

    // Level
    CheckpointActivate,

    // UI
    UIClick,
    UIHover,
    // Add more
}

public enum MusicKey
{
    None,
    MainTheme,
    BossTheme,
    MenuTheme,
    // Add more
}

public enum AmbientKey
{
    None,
    // Open Area (Plains, Forest, Village, etc)
    ForestAmbience,

    // Close Area (Buildings, Dungeons, etc)
    CaveAmbience,

    // Add more
}
