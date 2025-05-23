using UnityEngine;

[System.Serializable]
public class MonsterEntry
{
    public int id;                   
    public GameObject prefab;
    public string monsterName;
    public Sprite icon;
    public string description;
    public AudioClip audioClip;
}
