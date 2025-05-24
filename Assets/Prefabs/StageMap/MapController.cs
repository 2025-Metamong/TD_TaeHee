using UnityEngine;

public class MapController : MonoBehaviour
{
    [Tooltip("몬스터가 스폰될 위치")]
    public Transform spawnPoint;

    [Tooltip("웨이 포인트가 담긴 부모 오브젝트")]
    public Transform pathHolder;

    [Tooltip("카메라 이동 한계 블럭 1")]
    public GameObject block1;
    [ReadOnly]
    public Vector3 pos1;

    [Tooltip("카메라 이동 한계 블럭 2")]
    public GameObject block2;
    [ReadOnly]
    public Vector3 pos2;

    private void OnValidate()
    {
        if (block1 != null) pos1 = block1.transform.position;
        if (block2 != null) pos2 = block2.transform.position;
    }
}