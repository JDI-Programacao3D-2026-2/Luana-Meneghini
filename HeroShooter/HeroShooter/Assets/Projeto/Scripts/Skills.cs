using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    [Header("Referências")]
    public GameManager gameManager;
    public PlayerHealth playerHealth;
    public Rigidbody playerRb;
    public PlayerMove playerMove;

    [Header("Cooldowns")]
    public Image[] skill_Image;
    public float cooldown1 = 10f;
    public float cooldown2 = 10f;

    [Header("Granada")]
    public GameObject grenadePrefab;
    public Transform grenadeThrowPoint;
    private bool hasGrenade = false;

    [Header("Defensor")]
    public float shieldDuration = 5f;
    public float impactRadius = 25f;
    public float impactForce = 12f;

    [Header("Infiltrador")]
    public float camouflageDuration = 5f;
    public float dashForce = 15f;

    [Header("Reconhecimento")]
    public float adrenalineDuration = 5f;
    public float adrenalineSpeedMultiplier = 1.8f;
    public float communicationBlockDuration = 5f;
    public float communicationRadius = 10f;

    public bool IsCamouflaged { get; private set; }
    private int selectedAgent;
    private bool skill1Ready = true;
    private bool skill2Ready = true;

    void Start()
    {
        selectedAgent = GameSession.SelectedAgent;
        Debug.Log("Agente selecionado: " + selectedAgent);

        if (skill_Image != null)
        {
            foreach (Image image in skill_Image)
            {
                if (image != null)
                    image.fillAmount = 1f;
            }
        }
    }
    void Update()
    {
        if (gameManager == null ||
            !gameManager.GameStarted ||
            gameManager.GameEnded)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("Tecla 1 detectada!");

            if (skill1Ready)
            {
                Debug.Log("Habilidade 1 ativada!");
                StartCoroutine(Cooldown(1));
                ActivateSkill1();
            }
            else
            {
                Debug.Log("Habilidade 1 em cooldown!");
            }
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("Tecla 2 detectada!");

            if (skill2Ready)
            {
                Debug.Log("Habilidade 2 ativada!");
                StartCoroutine(Cooldown(2));
                ActivateSkill2();
            }
            else
            {
                Debug.Log("Habilidade 2 em cooldown!");
            }
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ThrowGrenade();
        }
    }
    void ActivateSkill1()
    {
        Debug.Log("ActivateSkill1 executado. Agente: " + selectedAgent);

        switch (selectedAgent)
        {
            case 0:
                StartCoroutine(Shield());
                break;

            case 1:
                StartCoroutine(Camouflage());
                break;

            case 2:
                StartCoroutine(Adrenaline());
                break;

            default:
                Debug.LogError("Agente inválido: " + selectedAgent);
                break;
        }
    }
    void ActivateSkill2()
    {
        Debug.Log("ActivateSkill2 executado. Agente: " + selectedAgent);

        switch (selectedAgent)
        {
            case 0:
                Impact();
                break;

            case 1:
                Dash();
                break;

            case 2:
                BlockCommunication();
                break;

            default:
                Debug.LogError("Agente inválido: " + selectedAgent);
                break;
        }
    }
    IEnumerator Cooldown(int skillNumber)
    {
        if (skillNumber == 1)
            skill1Ready = false;
        else
            skill2Ready = false;

        float duration = skillNumber == 1
            ? cooldown1
            : cooldown2;

        Image image = skill_Image != null &&
                      skill_Image.Length >= skillNumber
            ? skill_Image[skillNumber - 1]
            : null;

        if (image != null)
            image.fillAmount = 0f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (image != null)
                image.fillAmount = elapsed / duration;

            yield return null;
        }

        if (image != null)
            image.fillAmount = 1f;

        if (skillNumber == 1)
            skill1Ready = true;
        else
            skill2Ready = true;
    }
    IEnumerator Shield()
    {
        if (playerHealth == null)
            yield break;

        playerHealth.SetShield(true);
        yield return new WaitForSeconds(shieldDuration);
        playerHealth.SetShield(false);
    }
    void Impact()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            impactRadius
        );

        HashSet<EnemyNavMesh> enemies =
            new HashSet<EnemyNavMesh>();

        foreach (Collider hit in hits)
        {
            EnemyNavMesh enemy =
                hit.GetComponentInParent<EnemyNavMesh>();

            if (enemy == null)
                enemy = hit.GetComponentInChildren<EnemyNavMesh>();

            if (enemy == null || !enemies.Add(enemy))
                continue;

            Vector3 direction =
                enemy.transform.position - transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f)
                direction = transform.forward;

            enemy.PushAway(direction, 8f);
        }

        Debug.Log(
            "Impacto ativado! Inimigos afastados: " + enemies.Count
        );
    }
    IEnumerator Camouflage()
    {
        IsCamouflaged = true;
        yield return new WaitForSeconds(camouflageDuration);
        IsCamouflaged = false;
    }
    void Dash()
    {
        if (playerMove == null)
            return;

        playerMove.StartDash(15f, 0.3f);
    }
    IEnumerator Adrenaline()
    {
        if (playerMove == null)
            yield break;

        float originalSpeed = playerMove.speed;

        playerMove.speed =
            originalSpeed * adrenalineSpeedMultiplier;

        Debug.Log("Adrenalina ativada!");

        yield return new WaitForSeconds(adrenalineDuration);

        if (playerMove != null)
        {
            playerMove.speed = originalSpeed;
        }

        Debug.Log("Adrenalina encerrada!");
    }
    void BlockCommunication()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position, communicationRadius);

        HashSet<EnemyNavMesh> enemies =
            new HashSet<EnemyNavMesh>();

        foreach (Collider hit in hits)
        {
            EnemyNavMesh enemy =
                hit.GetComponentInParent<EnemyNavMesh>();

            if (enemy != null)
                enemies.Add(enemy);
        }

        foreach (EnemyNavMesh enemy in enemies)
        {
            enemy.BlockCommunications(
                communicationBlockDuration);
        }

        Debug.Log("Bloqueio de comunicação ativado!");
    }
    public void EquipGrenade()
    {
        hasGrenade = true;

        Debug.Log("Granada equipada!");
    }
    void ThrowGrenade()
    {
        if (!hasGrenade || grenadePrefab == null ||
            grenadeThrowPoint == null)
            return;

        GameObject grenadeObject = Instantiate(
            grenadePrefab,
            grenadeThrowPoint.position,
            grenadeThrowPoint.rotation
        );

        Grenade grenade = grenadeObject.GetComponent<Grenade>();

        if (grenade != null)
        {
            grenade.Throw(grenadeThrowPoint.forward);
            hasGrenade = false;
        }
    }
}