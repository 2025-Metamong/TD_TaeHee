using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageInfo", menuName = "Game/Stage Info", order = 1)]
public class StageInfo : ScriptableObject
{
    [Header("Basic Settings")]
    [Tooltip("Initial amount of coins the player starts with.")]
    public int startCoins = 100;

    [Tooltip("Player's starting HP.")]
    public int playerHP = 10;

    [Header("Monster Spawn List")]
    [Tooltip("List of monsters to spawn: spawn time (seconds) and monster data index.")]
    public List<StageMonsterEntry> monsterSpawnList = new List<StageMonsterEntry>();

    [Header("Map & Camera")]
    [Tooltip("Reference to the root GameObject of the map for this stage.")]
    public GameObject map;

    [Tooltip("Allowed camera movement bounds in world units (x: min/max, y: min/max).")]
    public Rect cameraBounds = new Rect(-100f, -100f, 100f, 100f);
}

[Serializable]
public struct StageMonsterEntry
{
    [Tooltip("Time after stage start (in seconds) when this monster will spawn.")]
    public float spawnTime;

    [Tooltip("Index (0-based) into your monster data table or array.")]
    public int monsterDataIndex;

    public StageMonsterEntry(float time, int index)
    {
        spawnTime = time;
        monsterDataIndex = index;
    }
}