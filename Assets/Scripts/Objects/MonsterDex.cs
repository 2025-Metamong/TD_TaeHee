using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDex", menuName = "Roguelike/MonsterDex")]
public class MonsterDex : ScriptableObject
{
    [SerializeField]
    private List<MonsterEntry> monsterEntries = new List<MonsterEntry>();

    // Get monster by id
    public MonsterEntry GetEntryByID(int id)
    {
        if (id >= 0 && id < monsterEntries.Count)
        {
            return monsterEntries[id];
        }
        return null;
    }

    // monster dict return
    public List<MonsterEntry> GetAllEntries()
    {
        return monsterEntries;
    }

}
