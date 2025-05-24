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
    [Tooltip("Each element is one 'wave' of monsters.")]
    public List<MonsterWave> monsterSpawnList = new List<MonsterWave>();

    [Header("Map")]
    [Tooltip("Reference to the root GameObject of the map for this stage.")]
    public GameObject map;
    [Tooltip("Transparent GameObject where monster spanws")]
    public GameObject spawnPoint;
    [Tooltip("Way Points that monster moves")]
    public Transform pathHolder;

    [Header("Camera Bounds")]
    [Tooltip("Allowed camera movement bounds in world units (x: min/max, y: min/max).")]
    public Vector2 panXLimits = new Vector2(-50f, 50f);
    public Vector2 panZLimits = new Vector2(-50f, 50f);

    [Header("Tower Placement Settings")]
    [Tooltip("Positions where towers can be placed in this stage.")]
    public List<Vector3> towerSpawnPoints = new List<Vector3>();

    [Tooltip("List of tower prefabs allowed in this stage.")]
    public List<GameObject> availableTowers = new List<GameObject>();

    private void OnValidate()
    {
        if (map == null) return;

        // map 안에서 "MonsterSpawner" 이름의 자식을 찾아서 spawnPoint로 할당
        var sp = map.transform.Find("MonsterSpawner");
        if (sp != null) spawnPoint = sp.gameObject;

        // map 안에서 "Way" 이름의 자식을 찾아서 pathHolder로 할당
        var ph = map.transform.Find("Way");
        if (ph != null) pathHolder = ph;
    }
}

[System.Serializable]
public class MonsterWave
{
    [Tooltip("List of monsters to spawn: spawn time (seconds) and monster data index.")]
    public List<StageMonsterEntry> entries = new List<StageMonsterEntry>();
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