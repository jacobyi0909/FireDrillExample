using System;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public enum State
    {
        Cough,
        Getup,
        Idle,
        Patrol,
        Chase,
    }

    public State state = State.Cough;
    NavMeshAgent agent;
    Animator anim;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Cough: UpdateCough(); break;
            case State.Getup: UpdateGetup(); break;
            case State.Idle: UpdateIdle(); break;
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase: UpdateChase(); break;
        }
    }

    public void Getup()
    {
        anim.SetTrigger("Getup");
    }

    private void UpdateCough()
    {
        
    }

    private void UpdateGetup()
    {
        
    }

    GameObject player;
    private void UpdateIdle()
    {
        // 주인공을 찾아서 기억하고싶다.
        player = GameObject.Find("Player");
        // 만약 주인공이 있다면
        if (player)
        {
            // Patrol상태로 전이하고싶다.
            SetState(State.Patrol);
        }
    }
    Vector3 patrolPoint;
    void UpdatePatrolPoint(float radius = 5f)
    {
        patrolPoint = transform.position + UnityEngine.Random.insideUnitSphere * radius;
        patrolPoint.y = 0;
    }
    public float chaseDistance = 3f;
    public float giveupDistance = 10f;
    public float speed = 5f;
    private void UpdatePatrol()
    {
        // 목적지를 향해서 이동하고싶다. 
        Vector3 dir = patrolPoint - transform.position;
        //transform.position += dir.normalized * speed * Time.deltaTime;
        agent.SetDestination(patrolPoint);
        // 목적지에 도착했다면 목적지를 재설정 하고싶다.
        if (dir.magnitude <= agent.stoppingDistance)
        {
            UpdatePatrolPoint();
        }
        // 만약 주인공과의 거리가 3M이내라면 
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= chaseDistance)
        {
            // Chase상태로 전이하고싶다.
            SetState(State.Chase);
        }
    }

    private void UpdateChase()
    {
        // 주인공을 향해 이동하고싶다.
        Vector3 dir = player.transform.position - transform.position;
        //transform.position += dir.normalized * speed * Time.deltaTime;
        agent.SetDestination(player.transform.position);
        // 만약 주인공과의 거리가 10M보다 크다면
        if (dir.magnitude > giveupDistance)
        {
            // 추적을 포기하고 Patrol상태로 전이하고싶다.
            SetState(State.Patrol);
        }
    }

    public void SetState(State next)
    {
        state = next;

        if (next == State.Patrol)
        {
            UpdatePatrolPoint();
        }
        
        if (next == State.Chase || next == State.Patrol)
        {
            anim.SetTrigger("Walking");
        }
    }
}
