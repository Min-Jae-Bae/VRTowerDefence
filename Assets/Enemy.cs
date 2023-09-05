using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//�¾ �� ������� �������� �˷��ְ�ʹ�.
public class Enemy : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //����� �������� �̵��ϰ� �ϰ�ʹ�.
        agent.SetDestination(target.position);
    }
}