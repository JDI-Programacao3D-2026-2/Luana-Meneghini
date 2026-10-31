using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class ShootPool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int poolSize = 20;
    public int currentAmmo;
    private int activeProjectiles = 0;
    private ObjectPool<GameObject> pool;
    public bool isEnemy = false;
    private bool canShoot = false;
    private float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public LayerMask layerMask;
    public Transform player;
    public GameManager gameManager;
    public Skills playerSkills;

    void Awake()
    {
        currentAmmo = poolSize;

        // Cria a piscina de objetos (Object Pool)
        pool = new ObjectPool<GameObject>(
            () => Instantiate(projectilePrefab), // Instancia quando a pool precisa crescer
            projectile => projectile.SetActive(true), // Ativa o projétil ao reutilizar
            projectile => projectile.SetActive(false), // Desativa o projétil ao devolver
            projectile => Destroy(projectile),
            false,
            poolSize,
            poolSize
        );
    }
    void Update()
    {
        // Impede tiros e recargas antes do início da partida
        if (gameManager == null || !gameManager.GameStarted || gameManager.GameEnded)
            return;

        // Lógica de disparo autônomo para inimigos
        if (isEnemy && canShoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // Recarga automática para o inimigo quando fica sem munição
        if (isEnemy && currentAmmo <= 0 && !IsInvoking(nameof(Reload)))
        {
            Invoke(nameof(Reload), 2f);
        }

        // Disparo do jogador (Botão esquerdo do mouse)
        if (!isEnemy &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }

        // Recarga do jogador (Tecla R)
        if (!isEnemy &&
            Keyboard.current != null &&
            Keyboard.current[Key.R].isPressed &&
            !IsInvoking(nameof(Reload)))
        {
            Invoke(nameof(Reload), 2f);
        }
    }
    void FixedUpdate()
    {
        if (gameManager == null ||
            !gameManager.GameStarted ||
            gameManager.GameEnded)
        {
            canShoot = false;
            return;
        }

        canShoot = false;

        // Só os inimigos precisam verificar a visão do jogador.
        if (!isEnemy)
            return;

        // A camuflagem impede os inimigos de atirar.
        if (playerSkills != null &&
            playerSkills.IsCamouflaged)
        {
            return;
        }

        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // O jogador precisa estar dentro do alcance do tiro.
        if (distance > 7f)
            return;

        // Verifica se o primeiro objeto atingido é o jogador.
        if (Physics.Raycast(
            transform.position,
            direction.normalized,
            out RaycastHit hit,
            7f,
            layerMask))
        {
            if (hit.transform == player ||
                hit.transform.IsChildOf(player))
            {
                canShoot = true;

                Debug.DrawLine(
                    transform.position,
                    hit.point,
                    Color.green
                );
            }
        }
    }
    void Shoot()
    {
        if (currentAmmo <= 0 || activeProjectiles >= poolSize)
            return;

        // Retira um projétil inativo da pool
        GameObject projectile = pool.Get();

        currentAmmo--;
        activeProjectiles++;

        projectile.transform.SetPositionAndRotation(
            firePoint.position,
            firePoint.rotation
        );

        projectile.GetComponent<Projectile>()
            .StartProjectile(firePoint.forward, this);
    }
    // Retorna o projétil para a piscina de objetos após colidir ou expirar
    public void ReturnProjectile(GameObject projectile)
    {
        if (activeProjectiles > 0)
            activeProjectiles--;

        pool.Release(projectile);
    }

    private void Reload()
    {
        currentAmmo = poolSize;
    }
    public void RefillAmmo(int amount)
    {
        if (isEnemy)
            return;

        currentAmmo = Mathf.Min(currentAmmo + amount, poolSize);

        Debug.Log("Munição atual: " + currentAmmo);
    }
}