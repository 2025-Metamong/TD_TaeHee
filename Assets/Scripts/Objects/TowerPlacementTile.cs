using MyGame.Managers;
using UnityEngine;

// 타워를 얹을 수 있는 타일들에 들어가는 스크립트. 타일 위에 마우스가 올라가 있는지 확인하고 색상을 변경함.
[RequireComponent(typeof(Collider))]    // 충돌 처리가 되어있어야 마우스가 올라오는 것을 확인 가능.
public class TowerPlacementTile : MonoBehaviour
{
    [Header("Tile Info")]   // 타일 정보 인스펙터에 표기용.
    [SerializeField, Tooltip("설치 가능/불가 표기")] private bool canBuild = true;
    [SerializeField, Tooltip("마우스 On 시 색상")] private Color hoverColor = Color.red;
    [SerializeField, Tooltip("타일 위치")] private Vector3 tilePosition;

    private Renderer rend;  // 타일의 랜더러 컴포넌트 저장용.
    private Color originalColor;
    private MaterialPropertyBlock propBlock;
    // URP Lit 머티리얼의 Base Color 프로퍼티 이름
    private static readonly int _BaseColorID = Shader.PropertyToID("_BaseColor");
    private static readonly Vector3 offset = new Vector3(0, 4.5f, 0);   // 타일의 높이는 4.5

    void Awake()
    {
        // MaterialPropertyBlock 을 이용해서 Renderer에 접근하면 훨씬 빠르다고 함.
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        // 원래 타일 배경색 저장해두기.
        originalColor = rend.sharedMaterial.GetColor(_BaseColorID);
        // 타일 위치 저장해두기.
        this.tilePosition = this.transform.position;
    }

    // 마우스가 타일에 올라오면 색상 변경.
    void OnMouseEnter()
    {
        // if (!TowerManager.Instance.GetMode())
        // {
        //     Debug.Log("설치 모드가 아닙니다.");
        //     return;
        // }
        Debug.Log("타일에 마우스 들어옴");
        rend.GetPropertyBlock(propBlock);   // MaterialPropertyBlock에서 값 복사해오기
        propBlock.SetColor(_BaseColorID, hoverColor);   // 타일 색상 변경
        rend.SetPropertyBlock(propBlock);   // MaterialPropertyBlock에 변경 값 저장하기
    }

    // 마우스가 타일에서 나갈 때 원래 색상으로 복귀
    void OnMouseExit()
    {
        // if (TowerManager.Instance.GetMode() != true)
        // {
        //     Debug.Log("설치 모드가 아닙니다.");
        //     return;
        // }        
        Debug.Log("타일에서 마우스 나감");
        rend.GetPropertyBlock(propBlock);   // MaterialPropertyBlock에서 값 복사해오기
        propBlock.SetColor(_BaseColorID, originalColor);    // 타일 색상 되돌리기
        rend.SetPropertyBlock(propBlock);   // MaterialPropertyBlock에 변경 값 저장하기
    }

    // 해당 타일을 클릭했을 때 타워 생성 호출.
    void OnMouseDown()
    {
        // if (TowerManager.Instance.GetMode() != true)
        // {
        //     Debug.Log("설치 모드가 아닙니다.");
        //     return;
        // }
        if (!canBuild)
        {
            Debug.Log("여기에 타워를 지을 수 없습니다.");
            return;
        }
        // 현재 타일 위치 타워 매니저에 전달해서 타워 설치 요청.
        Vector3 installPosition = this.tilePosition + offset;
        TowerManager.Instance.InstallTower(installPosition);

        // 타워 설치 완료 시 이 타일에는 다른 타워를 놓을 수 없다.
        this.canBuild = false;
        TowerManager.Instance.SetMode(false);
    }
}

