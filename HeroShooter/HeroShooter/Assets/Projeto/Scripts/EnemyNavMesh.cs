using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyNavMesh : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public float detectionRange = 10f;
    public float fleeDistance = 4f;
    public LayerMask layerMask;
    public float losePlayerDistance = 15f;
    public GameManager gameManager;
    public Skills playerSkills;

    public enum EnemyState
    {
        WayPatrol,
        RandomPatrol,
        Pursuit,
        Flee
    }

    public EnemyState currentState = EnemyState.WayPatrol;

    void Start()
    {
        // Mantém os inimigos parados durante a contagem
        if (agent != null)
            agent.isStopped = true;
    }
    void Update()
    {
        if (gameManager == null || !gameManager.GameStarted || gameManager.GameEnded)
        {
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }

            return;
        }

        if (agent != null && agent.isStopped)
            agent.isStopped = false;

        FiniteStateMachine();
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

            case EnemyState.Flee:
                Flee();
                break;

            default:
                break;
        }
    }
    void Pursuit()
    {
        if (player == null)
            return;

        // Se o jogador ativar a camuflagem,o inimigo interrompe a perseguição.
        if (playerSkills != null && playerSkills.IsCamouflaged)
        {
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.ResetPath();
            }

            ChangeState(EnemyState.WayPatrol);
            return;
        }

        agent.stoppingDistance = 6f;
        agent.SetDestination(player.position);
        transform.LookAt(player);

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer > losePlayerDistance)
        {
            ChangeState(EnemyState.WayPatrol);
        }
        else if (distanceToPlayer <= fleeDistance)
        {
            ChangeState(EnemyState.Flee);
        }
        else if (distanceToPlayer > detectionRange)
        {
            ChangeState(EnemyState.WayPatrol);
        }
    }
    void WayPatrol()
    {
        agent.stoppingDistance = 0f;

        if (waypoints != null &&
            waypoints.Length != 0 &&
            !agent.pathPending &&
            agent.remainingDistance < 0.5f)
        {
            int randomIndex = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[randomIndex].position);
        }

        SearchPlayer();
    }
    void SearchPlayer()
    {
            if (player == null)
                return;

            // Se o jogador estiver camuflado, o inimigo não o detecta.
            if (playerSkills != null && playerSkills.IsCamouflaged)
                return;

            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            // O jogador precisa estar dentro do alcance.
            if (distance > detectionRange)
                return;

            // Verifica se existe uma linha de visão até o jogador.
            if (Physics.Raycast(
                transform.position,
                direction.normalized,
                out RaycastHit hit,
                detectionRange,
                layerMask))
            {
                if (hit.transform == player ||
                    hit.transform.IsChildOf(player))
                {
                    ChangeState(EnemyState.Pursuit);
                    CallBackup();
                }
            }
        }
    void CallBackup()
    {
        // Se a comunicação estiver bloqueada,este inimigo não pode chamar reforços.
        if (communicationsBlocked)
            return;

        EnemyNavMesh[] todosInimigos =
            FindObjectsByType<EnemyNavMesh>(FindObjectsSortMode.None);

        foreach (EnemyNavMesh aliado in todosInimigos)
        {
            // Não precisa chamar a si mesmo.
            if (aliado == this)
                continue;

            float distancia = Vector3.Distance(
                transform.position,
                aliado.transform.position
            );

            if (distancia < 10f)
            {
                aliado.ChangeState(EnemyState.Pursuit);
            }
        }
    }
    void Flee()
    {
        if (player == null ||
            waypoints == null ||
            waypoints.Length == 0)
            return;

        agent.stoppingDistance = 0f;

        Transform furthestWaypoint = waypoints[0];

        float greatestDistance = Vector3.Distance(
            player.position,
            furthestWaypoint.position
        );

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
    private bool communicationsBlocked = false;

    public void BlockCommunications(float duration)
    {
        StopCoroutine(nameof(CommunicationBlockTimer));
        StartCoroutine(CommunicationBlockTimer(duration));
    }

    private IEnumerator CommunicationBlockTimer(float duration)
    {
        communicationsBlocked = true;

        yield return new WaitForSeconds(duration);

        communicationsBlocked = false;
    }
}