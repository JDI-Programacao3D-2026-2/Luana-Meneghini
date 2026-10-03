using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Transform[] waypoints;
<<<<<<< HEAD

    public LayerMask layerMask;
    public enum EnemyState
    {
        WayPatrol, RandomPatrol, Pursuit
    }
    public EnemyState currentState = EnemyState.Pursuit;
=======
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
>>>>>>> 31190f289988be061d9d3f5c9b5c702becf35f0a

    void Update()
    {
<<<<<<< HEAD
        FiniteStateMachine();
=======
        // Pursuit();
        WayPatrol();
        // RandomPatrol();
    }

    void Pursuit()
    {
        if (player != null) 
        {
            agent.stoppingDistance = 6f;
            agent.SetDestination(player.position);
            transform.LookAt(player);
        }
    }

    void WayPatrol()
    {
        agent.stoppingDistance = 0f;
        if (waypoints.Length != 0 && !agent.pathPending && agent.remaningDistance < 0.5f)
        {
            int randomIndex = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[randomIndex].position);
        }
>>>>>>> 31190f289988be061d9d3f5c9b5c702becf35f0a
    }
    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }
    void FiniteStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.Pursuit:
                Pursuit();
                break;

            case EnemyState.WayPatrol:
                WayPatrol();
                break;

            default:
                break;
        }
        if (Physics.Raycast(transform.position, transform.forward,
        out RaycastHit hit, 30f, layerMask))
        {
            ChangeState(EnemyState.Pursuit);
        }
        else
        {
            ChangeState(EnemyState.WayPatrol);
        } 
    }

    void Pursuit()
    {
        if (player != null)
        {
            agent.stoppingDistance = 3f;
            agent.SetDestination(player.position);
            transform.LookAt(player);
        }
    }
    void WayPatrol()
    {
        agent.stoppingDistance = 0f;
        if (waypoints.Length != 0 && !agent.pathPending &&
        agent.remainingDistance < 0.5f)
        {
            int randomIndex = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[randomIndex].position);
        }
    }

    /*
     void RandomPatrol()
     {
         agent.stoppingDistance = 0f;

         // Se chegou ao ponto OU se tentou andar mas ficou preso/parado contra a borda
         if (!agent.pathPending && (agent.remainingDistance < 0.5f || agent.velocity.sqrMagnitude < 0.1f))
         {
             if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * 15f, out NavMeshHit hit, 15f, NavMesh.AllAreas))
             {
                 agent.SetDestination(hit.position);
             }
         }
     } */
}

