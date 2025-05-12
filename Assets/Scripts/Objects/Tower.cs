using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MyGame.Managers;
using System; // Assuming you have a MonsterManager script to handle monster logic

namespace MyGame.Objects
{
    public class Tower : MonoBehaviour
    {
        // Represents a tower that also serves as a bullet generator.
        [Header("Tower Stats")]   // 타워 정보 inspector에 표기하기 위한 용도.
        [SerializeField, Tooltip("설치 비용")] private int cost = 10;
        [SerializeField, Tooltip("설치 위치")] private Transform position;
        [SerializeField, Tooltip("사거리")] private float range = 10f;
        [SerializeField, Tooltip("공격 빈도")] private float attackPeriod = 1f; 
        [SerializeField, Tooltip("레벨")] private int upgradeLevel = 0;
        [SerializeField, Tooltip("업그레이드 비용")] private int upgradeCost = 5;
        //[SerializeField] private singleBullet bullet;  
        [SerializeField, Tooltip("탄환 Prefab")] private GameObject bullet;
        [SerializeField, Tooltip("타워 디버프 종류")] private List<debuffBase> debuffAssets = new List<debuffBase>();

        private List<debuffBase> debuffList;   // 실제 디버프 전달용 리스트
        private float attack = 0f;  // 공격 결정용 flag. 주기가 되면 1, 아니면 0
        private Transform target;   // 타워가 공격해야 할 몬스터의 transform 컴포넌트. 

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
                this.attack = 0f;   // 다음번 공격 주기 계산을 위해 플래그 0으로 초기화.
                this.target = FindTarget(); // 타겟 찾기

                if (this.target != null)
                {
                    // Case 1. 타겟을 찾은 경우
                    // GameObject bullt_object = Instantiate(bullet, transform.position, Quaternion.identity);
                    GameObject bullet_object = Instantiate(bullet, transform.position, Quaternion.identity);
                    // 여러가지 타입의 bullet에 적용 가능하도록 수정할 것.
                    // bullt_object.GetComponent<singleBullet>().SetDirection(this.transform, target); // bullet 이 알아서 발사 될 것.
                    var bullet_script = bullet_object?.GetComponent<MonoBehaviour>();   // prefab의 스크립트 찾기
                    // SetDirection 적용
                    var directionMethod = bullet_script?.GetType().GetMethod("SetDirection", new Type[] { typeof(Transform), typeof(Transform)});
                    directionMethod?.Invoke(bullet_script, new object[] {this.transform, this.target});
                    // // SetDebuff 로 bullet에 디버프 리스트 전달.
                    var debuffMethod = bullet_script?.GetType().GetMethod("SetDebuff", new Type[] { typeof(List<debuffBase>)});
                    debuffMethod?.Invoke(bullet_script, new object[] {this.debuffList});
                    // 몬스터를 보도록 타워를 회전 시킬지 고민중.
                }
                else
                {
                    // Case 2. 타겟을 못 찾은 경우.
                    return;
                }
            }
        }

        public Transform GetPosition()
        {     // 타워 오브젝트의 Transform 컴포넌트 리턴.
            return this.position;
        }
        public float GetRange()
        {    // 타워 사거리 리턴턴
            return this.range;
        }

        //public bool UpgradeTower(){
        //    // 타워 레벨 업그레이드 용 함수.
        //    // Case 1 : 돈이 충분해서 업그레이드 성공 == stat 업데이트 하고 true 리턴
        //    // Case 2 : 업그레이드 실패 == false 리턴
        //    if (ResourceManager.Instance.useCoins(this.upgradeCost) == true){
        //        this.upgradeLevel += 1; // 레벨 증가
        //        this.attackPeriod += this.attackPeriod * 0.1f;  // 공격 속도 10% 증가
        //        this.range += this.range * 0.1f;    // 사거리 10% 증가
        //        // this.bullet.UpgradeBullet();     // 탄환 업그레이드???
        //    }

        //    return false;
        //}

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
            List<GameObject> monsterList = MonsterManager.Instance.GetMonsterList();
            float minDistance = max;
            int minIndex = 0;

            if (monsterList == null)
            {    // Case 1. 몬스터가 없는 경우
                //Debug.Log("몬스터가 없습니다.");
                return null;
            }

            // 몬스터 배열 크기 만큼 검사. ( 거리 단위로 정렬이 안 되어 있을테니. )
            for (int i = 0; i < monsterList.Count; i++)
            {
                // 거리 구하기. Vector3 내장 함수 Distance 사용. 
                // monsterList[i].GetComponent<Monster>().GetPosition() 을 사용하면 될 것 같은데 귀찮아서..
                // float distance = Vector3.Distance(this.transform.position, monsterList[i].GetPosition());
                float distance = Vector3.Distance(this.transform.position, monsterList[i].transform.position);
                // 사정거리 안에 몬스터가 있는 경우에만 가장 가까운 거리보다 더 가까운지 계산. 더 가까우면 update
                if (distance <= this.range && distance < minDistance)
                {
                    minDistance = distance;
                    minIndex = i;
                }
            }

            if (minDistance == max)
            {     // Case 2. 사정거리 내 몬스터 없으면 null 리턴.
                //Debug.Log("사정거리 내 몬스터가 없습니다.");
                return null;
            }
            //Debug.Log("사정거리 내 몬스터가 있습니다.");
            return monsterList[minIndex].transform; // Case 3. 가장 가까운 몬스터의 Transform 컴포넌트 리턴.
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
    }

}