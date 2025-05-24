using System.Collections.Generic;
using UnityEngine;
using MyGame.Managers;
using System;
using System.Collections;

namespace MyGame.Objects
{
    // 타워 클릭 처리를 위해서는 Collider 가 필요
    [RequireComponent(typeof(Collider))]
    public class Tower : MonoBehaviour
    {
        /*
            1. 사정거리 내 몬스터 타겟 지정해서 Bullet에 전달
            2. 클릭 시 타워 사정거리 표시
            3. 타워 업그레이드 수행
        */
        [Header("Tower Stats")]   // 타워 정보 inspector에 표기하기 위한 용도.
        [SerializeField, Tooltip("타워 ID")] private int ID = -1;   // 디폴트는 -1로 설정.
        [SerializeField, Tooltip("설치 비용")] private int cost = 10;
        [SerializeField, Tooltip("판매 비용")] private int sellPrice = 10;
        [SerializeField, Tooltip("설치 위치")] private Transform position;
        [SerializeField, Tooltip("타워 이미지")] public Sprite portrait;
        [SerializeField, Tooltip("사거리")] private float range = 10f;
        [SerializeField, Tooltip("공격 빈도")] private float attackPeriod = 1f;
        [SerializeField, Tooltip("피해량")] private float damage = 15f; // 피해량 추가.
        [SerializeField, Tooltip("레벨")] private int upgradeLevel = 0;
        [SerializeField, Tooltip("업그레이드 비용")] private int upgradeCost = 5;
        //[SerializeField] private singleBullet bullet;  
        [SerializeField, Tooltip("탄환 Prefab")] private GameObject bullet;
        [SerializeField, Tooltip("사거리 표기 시간")] private float displayTime = 1f;
        [SerializeField, Tooltip("사거리 표기용 Material")] private Material rangeMat;
        [SerializeField, Tooltip("타워 디버프 종류")] private List<debuffBase> debuffAssets = new List<debuffBase>();
        public GameObject towerSelectUIPrefab;    // 타워 조작 UI Prefab
        private GameObject towerSelectUI;
        private UpgradeSellLogic modifyLogic;

        private List<debuffBase> debuffList;   // 실제 디버프 전달용 리스트
        private float attack = 0f;  // 공격 결정용 flag. 주기가 되면 1, 아니면 0
        private Transform target;   // 타워가 공격해야 할 몬스터의 transform 컴포넌트. 
        private GameObject rangeCylinder;  // 타워 사거리 표출용 컴포넌트.
        private Coroutine rangeCoroutine;
        void Awake()
        {
            // 사거리 표시 용 실린더 만들기.
            rangeCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rangeCylinder.name = "range indicate cylinder";
            rangeCylinder.transform.SetParent(this.transform);  // 부모는 타워.
            rangeCylinder.transform.localPosition = new Vector3(0f, 0f, 0f); // 타워 바닥에 위치.

            // 실린더에 메테리얼 적용.
            Renderer rend = rangeCylinder.GetComponent<Renderer>();
            rend.sharedMaterial = rangeMat;

            // 충돌 판정이 필요 없으므로 제거해야 함.
            var collider = rangeCylinder.GetComponent<CapsuleCollider>();
            Destroy(collider);

            // 처음에는 보이지 않음.
            rangeCylinder.SetActive(false);

        }

        private void SetCylinderSize()
        {
            // 실린더 크기 설정. 
            // 실린더는 Prefab의 스케일 말고 월드 스케일에 영향을 받는 것이 좋으므로
            // 현재 Prefab의 스케일로 나눠주는 것이 좋다.
            float diameter = 2f * range / this.transform.localScale.x; // Prefab이 x, y, z 모두 같은 스케일로 증가한다고 가정.
            float height = 0.1f / this.transform.localScale.y;    // 높이 수정.
            rangeCylinder.transform.localScale = new Vector3(diameter, height, diameter);
        }
        void Start()
        {
            // position 변수 초기화
            this.position = gameObject.transform;
            // 디버프 종류 ScriptableObject들 인스턴스화
            this.debuffList = new List<debuffBase>(debuffAssets);

        }

        // Update is called once per frame
        void Update()
        {
            /*
            Case 1. 타겟을 찾은 경우 : bullet의 SetDirections를 부르고 종료
            Case 2. 타겟을 못 찾은 경우 : 아무것도 안하고 종료
                0. 탄환을 발사해야할 경우(공격 주기)에만 타겟을 찾을 것이다. ( 연산 수 줄이기 )
                1. 타워에서 가장 가까운 사거리 내 몬스터를 찾는다.
                2. 탄환 Prefab 인스턴스화 시킨 후 bullet.SetDirection(this); 를 호출한다.
                3. 다음 공격 주기까지 대기
            */

            this.attack += this.attackPeriod * Time.deltaTime;   // attack 플래그에 주기 누적시키기.
            if (this.attack >= 1f)
            {  // 공격 주기에 도달하면
                this.target = FindTarget(); // 타겟 찾기

                if (this.target != null)
                {
                    this.attack = 0f;   // 다음번 공격 주기 계산을 위해 플래그 0으로 초기화.
                    
                    // Case 1. 타겟을 찾은 경우
                    // GameObject bullt_object = Instantiate(bullet, transform.position, Quaternion.identity);
                    GameObject bullet_object = Instantiate(bullet, transform.position, Quaternion.identity);

                    var bullet_script = bullet_object?.GetComponent<MonoBehaviour>();   // prefab의 스크립트 찾기
                    // SetDirection 으로 bullet 날아갈 방향 적용
                    var directionMethod = bullet_script?.GetType().GetMethod("SetDirection", new Type[] { typeof(Transform), typeof(Transform) });
                    directionMethod?.Invoke(bullet_script, new object[] { this.transform, this.target });
                    // SetDebuff 로 bullet에 디버프 리스트 전달.
                    var debuffMethod = bullet_script?.GetType().GetMethod("SetDebuff", new Type[] { typeof(List<debuffBase>) });
                    debuffMethod?.Invoke(bullet_script, new object[] { this.debuffList });
                    // SetDamage로 bullet에 공격력 전달.
                    var damageMethod = bullet_script?.GetType().GetMethod("SetDamage", new Type[] { typeof(float) });
                    damageMethod?.Invoke(bullet_script, new object[] { this.damage });
                    // SetRange로 bullet에 사거리 전달.
                    var rangeMethod = bullet_script?.GetType().GetMethod("SetRange", new Type[] { typeof(float) });
                    rangeMethod?.Invoke(bullet_script, new object[] { this.range });
                    // 몬스터를 보도록 타워를 회전 시킬지 고민중.
                }
                else
                {
                    // Case 2. 타겟을 못 찾은 경우.
                    return;
                }
            }
        }

        public void TurnOffSelectUI()
        {
            Destroy(this.towerSelectUI);
        }

        void OnMouseDown()
        {
            Debug.Log("마우스 클릭!");
            ShowRange();        // 사거리 표기.
            // 판매, 업그레이드 버튼 표기.

            if (this.towerSelectUI != null) // Upgrade, Sell 버튼이 여러개 나오는 것 방지.
            {
                Destroy(this.towerSelectUI);
            }
            
            // Prefab 인스턴스 화.
            this.towerSelectUI = Instantiate(towerSelectUIPrefab);
            Vector3 uiPosition = this.transform.position;
            // uiPosition.y += 4;
            uiPosition.y += 5 * this.transform.localScale.y;    // 타워 Prefab 스케일에 따라 생성 높이 차등 적용
            towerSelectUI.transform.position = uiPosition;
            this.modifyLogic = towerSelectUI.GetComponent<UpgradeSellLogic>();

            // 인스턴스 활성화.
            towerSelectUI.SetActive(true);
            modifyLogic.SetTowerScript(this.GetComponent<Tower>());
            modifyLogic.ShowUI();
        }

        private void ShowRange()
        {
            SetCylinderSize();  // 그리기 전 실린더 크기 업데이트.
            if (rangeCoroutine != null)
                StopCoroutine(rangeCoroutine);
            rangeCoroutine = StartCoroutine(ShowRangeForSeconds());  // 정해진 시간 만큼 사거리 표기            
        }
        private IEnumerator ShowRangeForSeconds()
        {
            rangeCylinder.SetActive(true);
            yield return new WaitForSeconds(this.displayTime);
            rangeCylinder.SetActive(false);
            rangeCoroutine = null;
        }

        public Transform GetPosition()
        {     // 타워 오브젝트의 Transform 컴포넌트 리턴.
            return this.position;
        }
        public float GetRange()
        {    // 타워 사거리 리턴
            return this.range;
        }

        public void AddDebuff(debuffBase dbuff)
        {
            // 현재 디버프 종류에 전달 받은 디버프 추가하기.
            this.debuffAssets.Add(dbuff);
            // 실제 디버프 전달에 사용하는 리스트에도 추가하기.
            this.debuffList.Add(Instantiate(dbuff));
        }

        // 타워 업그레이드 로직.
        public bool UpgradeTower()
        {
            // 타워 레벨 업그레이드 용 함수.
            // Case 1 : 돈이 충분해서 업그레이드 성공 == stat 업데이트 하고 true 리턴
            // Case 2 : 업그레이드 실패 == false 리턴
            bool upgradeAble = StageManager.Instance.UseCoin(this.upgradeCost);
            //bool upgradeAble = true;    // 테스트용. 고쳐야 함.
            // bool upgradeAble = false;    // 테스트용. 고쳐야 함.

            if (upgradeAble)
            {
                Debug.Log("업그레이드 성공");
                this.upgradeLevel += 1; // 레벨 증가
                this.attackPeriod += this.attackPeriod * this.upgradeLevel * 0.1f;  // 공격 속도 10% 증가
                this.range += this.range * this.upgradeLevel * 0.1f;    // 사거리 10% 증가
                this.damage += this.damage * this.upgradeLevel * 0.1f;    // 사거리 10% 증가
                this.upgradeCost += Mathf.RoundToInt(this.upgradeCost * 0.25f);   // 업그레이드 비용 증가.
                this.sellPrice += Mathf.RoundToInt(this.upgradeCost * 0.1f);   // 판매 비용 증가.
                ShowRange();        // 사거리 표기.
                // 업그레이드 성공 사운드 재생
                TowerSoundController.Instance.PlayUpgradeSound();
                return true;
            }

            // 업그레이드 실패 사운드 재생
            TowerSoundController.Instance.PlayFailedSound();
            return false;
        }

        // 타워가 타겟 찾는 로직.
        private Transform FindTarget()
        {

            /*사거리 안에서 타워와 가장 가까운 몬스터의 Transform 컴포넌트 리턴.
            Case 1. 몬스터가 없는 경우 : null 리턴
            Case 2. 사정거리 내 몬스터 없는 경우 : null 리턴.
            Case 3. 가장 가까운 몬스터의 Transform 컴포넌트 리턴

                max : 최대 거리 기준.
                monsterList : 받아올 몬스터 리스트.
                minDistance : 가장 가까운 몬스터와 거리.
                minIndex : 가장 가까운 몬스터의 인덱스.
            */
            float max = 10000000f;
            float minDistance = max;
            int minIndex = 0;
            Transform target = null;

            foreach (var pair in MonsterManager.Instance.GetMonsterList())
            {
                float distance = Vector3.Distance(this.position.position, pair.Value.transform.position);
                if (distance > this.range)  // 몬스터가 사거리 밖이면면
                    continue;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minIndex = pair.Key;
                    target = pair.Value.transform;
                }
            }
            return target; // Case 3. 가장 가까운 몬스터의 Transform 컴포넌트 리턴. 못 찾으면 null 리턴.
        }

        public int GetCost()
        {   // 타워 설치 비용 리턴.
            return this.cost;
        }

        public int GetUpgradeCost()
        {    // 타워 업그레이드 비용 리턴.
            return this.upgradeCost;
        }

        public int GetLevel()
        {      // 타워의 레벨 리턴.
            return this.upgradeLevel;
        }
        /// <summary>
        /// int GetID()
        /// returns ID of the Tower
        /// </summary>
        public int GetID()
        {
            return this.ID;
        }
        /// <summary>
        /// void SetID(int)
        /// set Tower's ID to the passed Value.
        /// </summary>
        public void SetID(int id)
        {
            this.ID = id;
        }

        public void SetDamage(float dam)
        {
            this.damage = dam;
        }

        public float GetDamage()
        {
            return this.damage;
        }

        public float GetAttackPeriod()
        {
            return this.attackPeriod;
        }

        public void SetAttackPeriod(float newAttackPeriod)
        {
            this.attackPeriod = newAttackPeriod;
        }

        public int GetSellPrice()
        {
            return this.sellPrice;
        }

        // 타워 파괴 될 때 가지고 있는 오브젝트들 같이 파괴.
        public void OnDestroy()
        {
            Destroy(this.towerSelectUI);
            Destroy(this.rangeCylinder);
        }
    }

}