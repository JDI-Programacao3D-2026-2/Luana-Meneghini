using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMelee : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public GameManager gameManager;
    public Skills playerSkills;
    public PlayerHealth playerHealth;

    [Header("Detecção")]
    public float detectionRange = 10f;
    public float losePlayerDistance = 15f;
    public LayerMask layerMask;

    [Header("Ataque corpo a corpo")]
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Impacto")]
    private bool beingPushed = false;
    private Coroutine pushCoroutine;

    public enum EnemyState
    {
        WayPatrol,
        RandomPatrol,
        Pursuit,
        Attack
    }

    public EnemyState currentState = EnemyState.WayPatrol;
    private bool communicationsBlocked = false;

    void Start()
    {
        if (agent != null)
            agent.isStopped = true;

        if (player != null && playerHealth == null)
            playerHealth = player.GetComponentInParent<PlayerHealth>();
    }
    void Update()
    {
        if (gameManager == null ||
            !gameManager.GameStarted ||
            gameManager.GameEnded)
        {
            if (agent != null &&
                agent.enabled &&
                agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }

            return;
        }

        if (beingPushed)
            return;

        if (agent != null &&
            agent.enabled &&
            agent.isOnNavMesh &&
            agent.isStopped)
        {
            agent.isStopped = false;
        }

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
            case EnemyState.WayPatrol:
                WayPatrol();
                break;

            case EnemyState.RandomPatrol:
                WayPatrol();
                break;

            case EnemyState.Pursuit:
                Pursuit();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }
    void Pursuit()
    {
        if (player == null || agent == null)
            return;

        if (playerSkills != null && playerSkills.IsCamouflaged)
        {
            agent.ResetPath();
            ChangeState(EnemyState.WayPatrol);
            return;
        }

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer > losePlayerDistance)
        {
            ChangeState(EnemyState.WayPatrol);
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        agent.stoppingDistance = attackRange * 0.8f;
        agent.SetDestination(player.position);

        LookAtPlayer();

        if (distanceToPlayer > detectionRange)
            ChangeState(EnemyState.WayPatrol);
    }
    void Attack()
    {
        if (player == null || agent == null)
            return;

        if (playerSkills != null && playerSkills.IsCamouflaged)
        {
            agent.ResetPath();
            ChangeState(EnemyState.WayPatrol);
            return;
        }

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer > losePlayerDistance)
        {
            ChangeState(EnemyState.WayPatrol);
            return;
        }

        if (distanceToPlayer > attackRange)
        {
            ChangeState(EnemyState.Pursuit);
            return;
        }

        agent.stoppingDistance = attackRange * 0.8f;
        agent.ResetPath();

        LookAtPlayer();

        if (Time.time >= nextAttackTime)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;

                Debug.Log(name + " atacou o jogador!");
            }
            else
            {
                Debug.LogWarning(
                    name + ": PlayerHealth não foi encontrado no jogador."
                );
            }
        }
    }
    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    void WayPatrol()
    {
        if (agent == null)
            return;

        agent.stoppingDistance = 0f;

        if (waypoints != null &&
            waypoints.Length > 0 &&
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

        if (playerSkills != null && playerSkills.IsCamouflaged)
            return;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > detectionRange)
            return;

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
        if (communicationsBlocked)
            return;

        EnemyNavMesh[] rangedEnemies =
            FindObjectsByType<EnemyNavMesh>(
                FindObjectsSortMode.None
            );

        foreach (EnemyNavMesh ally in rangedEnemies)
        {
            if (ally == null || ally.gameObject == gameObject)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                ally.transform.position
            );

            if (distance < 10f)
                ally.ChangeState(EnemyNavMesh.EnemyState.Pursuit);
        }

        EnemyMelee[] meleeEnemies =
            FindObjectsByType<EnemyMelee>(
                FindObjectsSortMode.None
            );

        foreach (EnemyMelee ally in meleeEnemies)
        {
            if (ally == null || ally == this)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                ally.transform.position
            );

            if (distance < 10f)
                ally.ChangeState(EnemyState.Pursuit);
        }
    }
    public void BlockCommunications(float duration)
    {
        StopCoroutine(nameof(CommunicationBlockTimer));
        StartCoroutine(CommunicationBlockTimer(duration));
    }
    IEnumerator CommunicationBlockTimer(float duration)
    {
        communicationsBlocked = true;

        yield return new WaitForSeconds(duration);

        communicationsBlocked = false;
    }
    public void PushAway(Vector3 direction, float distance)
    {
        if (agent == null ||
            !agent.enabled ||
            !agent.isOnNavMesh)
            return;

        if (pushCoroutine != null)
            StopCoroutine(pushCoroutine);

        pushCoroutine = StartCoroutine(
            PushAwayCoroutine(direction, distance)
        );
    }
    IEnumerator PushAwayCoroutine(Vector3 direction, float distance)
    {
        beingPushed = true;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            direction = -transform.forward;

        direction.Normalize();

        agent.isStopped = false;
        agent.stoppingDistance = 0f;
        agent.ResetPath();

        Vector3 desiredPosition =
            transform.position + direction * distance;

        if (NavMesh.SamplePosition(
            desiredPosition,
            out NavMeshHit hit,
            5f,
            agent.areaMask))
        {
            NavMeshPath path = new NavMeshPath();

            if (agent.CalculatePath(hit.position, path) &&
                path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(hit.position);

                float timeout = 2f;

                while (timeout > 0f &&
                       agent != null &&
                       agent.enabled &&
                       agent.isOnNavMesh &&
                       (agent.pathPending ||
                        agent.remainingDistance >
                        agent.stoppingDistance + 0.2f))
                {
                    timeout -= Time.deltaTime;
                    yield return null;
                }
            }
        }

        beingPushed = false;
        pushCoroutine = null;
    }
}