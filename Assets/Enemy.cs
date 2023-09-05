using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//태어날 때 요원에게 목적지를 알려주고싶다.
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
        //요원이 목적지로 이동하게 하고싶다.
        agent.SetDestination(target.position);
    }
}