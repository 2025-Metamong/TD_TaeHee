using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MyGame.Managers;
using Unity.VisualScripting;
using UnityEngine;

// 타워를 얹을 수 있는 타일들에 들어가는 스크립트. 타일 위에 마우스가 올라가 있는지 확인하고 색상을 변경함.
[RequireComponent(typeof(Collider))]    // 충돌 처리가 되어있어야 마우스가 올라오는 것을 확인 가능.
public class TowerPlacementTile : MonoBehaviour
{
    [Header("Tile Info")]   // 타일 정보 인스펙터에 표기용.
    [SerializeField, Tooltip("설치 모드인지 아닌지 표기")] private bool mode = true; // 타워 매니저에서 관리하는 게 나을까?
    [SerializeField, Tooltip("설치 가능/불가 표기")] private bool canBuild = true;
    [SerializeField, Tooltip("마우스 On 시 색상")] private Color hoverColor = Color.red;
    [SerializeField, Tooltip("타일 위치")] private Vector3 tilePosition;

    private Renderer rend;  // 타일의 랜더러 컴포넌트 저장용.
    private Color originalColor;
    private MaterialPropertyBlock propBlock;
    private static readonly int _BaseColorID = Shader.PropertyToID("_BaseColor");
    // URP Lit 머티리얼의 Base Color 프로퍼티 이름

    void Awake()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        originalColor = rend.sharedMaterial.GetColor(_BaseColorID);
        this.tilePosition = this.transform.position;
    }

    // 마우스가 타일에 올라오면 색상 변경.
    void OnMouseEnter()
    {
        Debug.Log("타일에 마우스 들어옴");
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor(_BaseColorID, hoverColor);
        rend.SetPropertyBlock(propBlock);
    }

    // 마우스가 타일에서 나갈 때 원래 색상으로 복귀
    void OnMouseExit()
    {
        Debug.Log("타일에서 마우스 나감");
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor(_BaseColorID, originalColor);
        rend.SetPropertyBlock(propBlock);
    }

    // 해당 타일을 클릭했을 때 타워 생성 호출.
    void OnMouseDown()
    {
        if (!canBuild)
        {
            Debug.Log("여기에 타워를 지을 수 없습니다.");
            return;
        }
        // 현재 타일 위치 타워 매니저에 전달해서 타워 설치 요청.
        Vector3 installPosition = new Vector3(tilePosition.x, tilePosition.y+4.5f, tilePosition.z);
        TowerManager.Instance.InstallTower(installPosition);
    }
}

