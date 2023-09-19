using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//태어날 때 요원에게 목적지를 알려주고싶다.
//FSM: 검색, 이동, 공격, 피격, 죽음
public class Enemy : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public State state;
    public Transform firePosition;
    public AnimationCurve animCurve;

    public enum State
    {
        Search,
        Move,
        Attack,
        Damage,
        Die,
        FlyingDie
    }

    private void Start()
    {
        droneOriginSize = drone.localScale;
        droneOriginLocalPosition = drone.localPosition;
        agent = GetComponent<NavMeshAgent>();

        state = State.Search;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Search: Search(); break;
            case State.Move: Move(); break;
            case State.Attack: Attack(); break;
            case State.Damage: Damage(); break;
            case State.Die: Die(); break;
            case State.FlyingDie: FlyingDie(); break;
        }
    }

    public GameObject destoryFactory;

    private void FlyingDie()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 2)
        {
            Destroy(gameObject);
            Instantiate(destoryFactory, drone.position, Quaternion.identity);
        }
    }

    private void Search()
    {
        //Scene에 배치된 타워들을 모두 찾아서
        Tower[] towers = GameObject.FindObjectsOfType<Tower>();

        //임시거리, 선택 배열 번호
        float tempDistance = float.MaxValue;
        int tempIndex = -1;

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i].HP <= 0) continue;

            towerTarget = towers[i];
            //타워와 나와의 거리를 제고
            float temp = Vector3.Distance(towers[i].transform.position, transform.position);
            //그 거리가 최단거리보다 작다면
            if (temp < tempDistance)
            {
                //최단거리를 갱신하고 선택배열번호를 갱신하고싶다.
                tempDistance = temp;
                tempIndex = i;
            }
        }
        //나와 가장 가까운 타워를 기억하고싶다.
        if (tempIndex != -1)
        {
            //기억했다면
            target = towers[tempIndex].transform;
            //나의 상태를 Move로 바꾸고싶다.
            state = State.Move;
        }
    }

    private void Move()
    {
        //요원이 목적지로 이동하게 하고싶다.
        agent.SetDestination(target.position);

        //만약 도착했다면 -> 목적지와의 거리가 StoppingDistance 이하라면
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist <= agent.stoppingDistance)
        {
            //공격상태로 전이하고싶다.
            state = State.Attack;
        }
        //공격상태로 전이하고싶다.
    }

    private float currentTime;
    [SerializeField] private float fireTime = 2;
    public GameObject bulletFactory;
    private Tower towerTarget;

    private void Attack()
    {
        //만약 목표로하는 타워의 체력이 0이되면 검색상태로 전이하고 싶다.
        if (towerTarget.HP <= 0)
        {
            state = State.Search;
            return;
        }
        //시간이 흐르다가
        currentTime += Time.deltaTime;
        //현재시간이 fireTime을 초과하면
        if (currentTime > fireTime)
        {
            currentTime = 0;
            //현재시간을 0으로 초기화 하고
            //총알공장에서 총알을 만들고
            var enemyBullet = Instantiate(bulletFactory);
            //총알의 앞방향을 목적지를 향하는 방향(타겟 방향 - 나의 방향을 빼면 = 타겟 방향이다)으로 회전하고싶다.
            enemyBullet.transform.position = firePosition.position;
            Vector3 dir = target.position - firePosition.position;
            enemyBullet.transform.forward = dir + Vector3.up * 10f;
        }
    }

    //데미지가 발생한 시간을 저장하고
    // 그 시간으로부터 damageTime만큼 시간이 흘렀다면
    private float damageMoments;

    public float damageTime = 1;

    private void Damage()
    {
        //시간이 흐르다가
        currentTime += Time.deltaTime;
        //시간이 흐르다가 정신차리는 시간이되면
        if (Time.time - damageMoments > damageTime)
        {
            //이동을 재개하고싶다.
            agent.isStopped = false;
            //상태를 Move로 전이하고싶다.
            state = State.Move;
        }
    }

    private HPBase hpBase;
    public Transform drone;
    private Vector3 droneOriginSize;
    private Vector3 droneOriginLocalPosition;

    private void Die()
    {
        // 1초 동안
        currentTime += Time.deltaTime;
        if (currentTime <= 1)
        {
            float t = animCurve.Evaluate(currentTime);
            Vector3 targetScale = droneOriginSize * 2;
            drone.localScale = Vector3.Lerp(droneOriginSize, targetScale, t);
            drone.localPosition = droneOriginLocalPosition + UnityEngine.Random.insideUnitSphere * 0.1f * currentTime;
        }
        else
        {
            // 1초 후에 펑 터지고 싶다.
            Destroy(gameObject, 1);
            Instantiate(destoryFactory, drone.position, Quaternion.identity);
        }
    }

    public void DiePlz(int damage, Vector3 origin)
    {
        //이동을 정지하고싶다.
        agent.isStopped = true;
        if (hpBase == null) hpBase = GetComponent<HPBase>();

        hpBase.HP -= damage;

        if (hpBase.HP <= 0)
        {
            //충돌체를 비활성화 하고싶다.
            GetComponent<Collider>().enabled = false;
            //죽음 상태로 전이하고
            state = State.Die;
            Destroy(gameObject, 1);

            if (damage == 1)
            {
                state = State.Die;
            }
            else
            {
                state = State.FlyingDie;
                agent.enabled = false;
                var rb = GetComponent<Rigidbody>();
                if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
                Vector3 force = transform.position - origin;
                force = Vector3.ClampMagnitude(force, 10);
                rb.AddForceAtPosition(force, origin, ForceMode.Impulse);
            }
        }
        else
        {
            state = State.Damage;
            damageMoments = Time.time;
        }
    }
}