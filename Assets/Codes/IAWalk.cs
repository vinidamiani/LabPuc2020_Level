using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAWalk : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public Animator anim;
    public Vector3 patrolposition;
    public float stoppedTime;
    public enum IaState
    {
        Stopped,
        Berserk,
        Attack,
        Damage,
        Dying,
        Patrol,
    }

    public IaState currentState;
    // Start is called before the first frame update
    void Start()
    {
        patrolposition = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case IaState.Stopped:
                Stopped();
                break;
            case IaState.Berserk:
                Berserk();
                break;
            case IaState.Attack:
                Attack();
                break;
            case IaState.Damage:
                Damage();
                break;
            case IaState.Dying:
                Dying();
                break;
            case IaState.Patrol:
                Patrol();
                break;
        }

        anim.SetFloat("Velocity", agent.velocity.magnitude);
    }

    void Patrol()
    {
        agent.isStopped = false;
        agent.SetDestination(patrolposition);
        anim.SetBool("Attack", false);
        //tempo parado
        if (agent.velocity.magnitude < 0.1f)
        {
            stoppedTime += Time.deltaTime;
        }
        //se for mais q 3 segundos
        if(stoppedTime>3)
        {
            stoppedTime = 0;
            patrolposition = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 10)
        {
            currentState = IaState.Berserk;
        }

    }

    void Stopped()
    {
        agent.isStopped = true;
        anim.SetBool("Attack", false);
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            currentState = IaState.Patrol;
        }
    }
    void Berserk()
    {
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
        anim.SetBool("Attack", false);
        if (Vector3.Distance(transform.position, target.transform.position) < 3)
        {
            currentState = IaState.Attack;
        }
    }

    void Attack()
    {
        agent.isStopped = true;
        anim.SetBool("Attack", true);
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            currentState = IaState.Berserk;
        }
    }
    void Damage()
    {
        agent.isStopped = true;
        anim.SetBool("Attack", false);
        anim.SetTrigger("Hit");
        currentState = IaState.Stopped;
    }
    void Dying()
    {
        agent.isStopped = true;
        anim.SetBool("Attack", false);
        anim.SetBool("Die",true);
        Destroy(gameObject, 3);
    }
}
