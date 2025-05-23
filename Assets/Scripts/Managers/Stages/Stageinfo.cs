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

    private void OnValidate()
    {
        if (map == null) return;

        var MapController = map.GetComponent<MapController>();

        // map 안에서 "MonsterSpawner" 이름의 자식을 찾아서 spawnPoint로 할당
        spawnPoint = MapController.spawnPoint.gameObject;

        // MapController.pathHolder을 찾아서 pathHolder로 할당
        pathHolder = MapController.pathHolder;

        // map 안에서 양쪽 끝 두 블럭의 범위로 카메라 이동 범위 제한
        if (MapController.pos1 != null && MapController.pos2 != null)
        {
            panXLimits.x = Math.Min(MapController.pos1.x, MapController.pos2.x);
            panXLimits.y = Math.Max(MapController.pos1.x, MapController.pos2.x);
            panZLimits.x = Math.Min(MapController.pos1.z, MapController.pos2.z);
            panZLimits.y = Math.Max(MapController.pos1.z, MapController.pos2.z);
        }
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