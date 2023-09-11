using System;
using UnityEngine;
using UnityEngine.AI;

//�¾ �� ������� �������� �˷��ְ�ʹ�.
//FSM: �˻�, �̵�, ����, �ǰ�, ����
public class Enemy : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public State state;
    public Transform firePosition;

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

    private void FlyingDie()
    {
        throw new NotImplementedException();
    }

    private void Search()
    {
        //Scene�� ��ġ�� Ÿ������ ��� ã�Ƽ�
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        //�ӽðŸ�, ���� �迭 ��ȣ
        float tempDistance = float.MaxValue;
        int tempIndex = -1;

        for (int i = 0; i < towers.Length; i++)
        {
            //Ÿ���� ������ �Ÿ��� ����
            float temp = Vector3.Distance(towers[i].transform.position, transform.position);
            //�� �Ÿ��� �ִܰŸ����� �۴ٸ�
            if (temp < tempDistance)
            {
                //�ִܰŸ��� �����ϰ� ���ù迭��ȣ�� �����ϰ�ʹ�.
                tempDistance = temp;
                tempIndex = i;
            }
        }
        //���� ���� ����� Ÿ���� ����ϰ�ʹ�.
        if (tempIndex != -1)
        {
            //����ߴٸ�
            target = towers[tempIndex].transform;
            //���� ���¸� Move�� �ٲٰ�ʹ�.
            state = State.Move;
        }
    }

    private void Move()
    {
        //����� �������� �̵��ϰ� �ϰ�ʹ�.
        agent.SetDestination(target.position);

        //���� �����ߴٸ� -> ���������� �Ÿ��� StoppingDistance ���϶��
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist <= agent.stoppingDistance)
        {
            //���ݻ��·� �����ϰ�ʹ�.
            state = State.Attack;
        }
        //���ݻ��·� �����ϰ�ʹ�.
    }

    private float currentTime;
    [SerializeField] private float fireTime = 2;
    public GameObject bulletFactory;

    private void Attack()
    {
        //�ð��� �帣�ٰ�
        currentTime += Time.deltaTime;
        //����ð��� fireTime�� �ʰ��ϸ�
        if (currentTime > fireTime)
        {
            currentTime = 0;
            //����ð��� 0���� �ʱ�ȭ �ϰ�
            //�Ѿ˰��忡�� �Ѿ��� �����
            var enemyBullet = Instantiate(bulletFactory);
            //�Ѿ��� �չ����� �������� ���ϴ� ����(Ÿ�� ���� - ���� ������ ���� = Ÿ�� �����̴�)���� ȸ���ϰ�ʹ�.
            enemyBullet.transform.position = firePosition.position;
            enemyBullet.transform.forward = target.position - firePosition.position + Vector3.up * 10f;
        }
    }

    //�������� �߻��� �ð��� �����ϰ�
    // �� �ð����κ��� damageTime��ŭ �ð��� �귶�ٸ�
    private float damageMoments;

    public float damageTime = 1;

    private void Damage()
    {
        //�ð��� �帣�ٰ�
        currentTime += Time.deltaTime;
        //�ð��� �帣�ٰ� ���������� �ð��̵Ǹ�
        if (Time.time - damageMoments > damageTime)
        {
            //�̵��� �簳�ϰ�ʹ�.
            agent.isStopped = false;
            //���¸� Move�� �����ϰ�ʹ�.
            state = State.Move;
        }
    }

    private HPBase hpBase;
    public Transform drone;
    private Vector3 droneOriginSize;
    private Vector3 droneOriginLocalPosition;

    private void Die()
    {
        // 1�� ����
        currentTime += Time.deltaTime;
        if (currentTime <= 1)
        {
            Vector3 targetScale = droneOriginSize * 2;
            drone.localScale = Vector3.Lerp(droneOriginSize, targetScale, currentTime);
            drone.localPosition = droneOriginLocalPosition + UnityEngine.Random.insideUnitSphere * 0.1f * currentTime;
        }
        else
        {
            // 1�� �Ŀ� �� ������ �ʹ�.
            Destroy(gameObject, 1);
        }
    }

    public void DiePlz(int damage)
    {
        //�̵��� �����ϰ�ʹ�.
        agent.isStopped = true;
        if (hpBase == null) hpBase = GetComponent<HPBase>();

        hpBase.HP -= damage;

        if (hpBase.HP <= 0)
        {
            //���� ���·� �����ϰ�
            state = State.Die;
            //�����϶�
            //1�� �Ŀ� �ı��ǰ�ʹ�.
            Destroy(gameObject, 1);
            if (damage == 1)
            {
                state = State.Die;
            }
            else
            {
                state = State.FlyingDie;
            }
        }
        else
        {
            state = State.Damage;
            damageMoments = Time.time;
        }
    }
}