using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}

