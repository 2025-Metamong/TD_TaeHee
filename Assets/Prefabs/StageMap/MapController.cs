using UnityEngine;

public class MapController : MonoBehaviour
{
    [Tooltip("몬스터가 스폰될 위치")]
    public Transform spawnPoint;
    [Tooltip("웨이 포인트가 담긴 부모 오브젝트")]
    public Transform pathHolder;
}