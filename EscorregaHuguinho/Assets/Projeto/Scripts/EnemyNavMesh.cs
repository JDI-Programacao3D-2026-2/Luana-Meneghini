using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public float detectionRange = 10f;
    public float fleeDistance = 4f;
    public LayerMask layerMask;

    public float losePlayerDistance = 15f;
    public enum EnemyState{
        WayPatrol, RandomPatrol, Pursuit, Flee
    }
    public EnemyState currentState = EnemyState.WayPatrol;

    void Update(){
    FiniteStateMachine();
    }
    public void ChangeState(EnemyState newState){
        currentState = newState;
    }
 void FiniteStateMachine(){
        switch (currentState){
            case EnemyState.Pursuit:
                Pursuit();
                break;

            case EnemyState.WayPatrol:
                WayPatrol();
                break;

            case EnemyState.Flee:
                Flee();
                break;

            default:
                break;
        }
    }

 void Pursuit(){
        if(player != null){
            agent.stoppingDistance = 6f;
            agent.SetDestination(player.position);
            transform.LookAt(player);      
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > losePlayerDistance)
        {
            ChangeState(EnemyState.WayPatrol);
        }
        // se o jogador chegar muito perto, o inimigo foge
        if (distanceToPlayer <= fleeDistance)
        {
            ChangeState(EnemyState.Flee);
        }
        // se o jogador ficar longe demais, volta a patrulhar
        else if (distanceToPlayer > detectionRange)
        {
            ChangeState(EnemyState.WayPatrol);
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
        SearchPlayer();
    }
    void SearchPlayer()
    {
        if(Physics.Raycast(transform.position, transform.forward, 
        out RaycastHit hit, 20f, layerMask)){
            ChangeState(EnemyState.Pursuit);
            CallBackup();
        }
    }
    void CallBackup()
    {
        EnemyNavMesh[] todosInimigos = FindObjectsByType<EnemyNavMesh>();
        foreach (EnemyNavMesh aliado in todosInimigos)
        {
            float distancia = Vector3.Distance(transform.position, aliado.transform.position);
            if (distancia < 10f)
            {
                aliado.ChangeState(EnemyState.Pursuit);
            }
        }
    }
    void Flee()
    {
        if (player == null || waypoints.Length == 0) return;

        agent.stoppingDistance = 0f;

        Transform furthestWaypoint = waypoints[0];

        float greatestDistance = Vector3.Distance(
            player.position,
            furthestWaypoint.position
        );

        // procura o waypoint mais distante do jogador
        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(
                player.position,
                waypoint.position
            );

            if (distance > greatestDistance)
            {
                greatestDistance = distance;
                furthestWaypoint = waypoint;
            }
        }

        // foge para o waypoint mais distante
        agent.SetDestination(furthestWaypoint.position);

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer > detectionRange)
        {
            ChangeState(EnemyState.WayPatrol);
        }
    }
}
