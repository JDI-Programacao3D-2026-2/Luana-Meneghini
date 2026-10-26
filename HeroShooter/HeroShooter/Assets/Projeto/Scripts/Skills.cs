using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    public float cd_1 = 5f;
    public float cd_2 = 8f;
    public Image[] skill_Image;
    public GameObject grenadePrefab;
    public Transform grenadeThrowPoint;
    private bool hasGrenade = false;
    private float cooldown1 = 0f;
    private float cooldown2 = 0f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // Atualiza os cooldowns das habilidades 1 e 2
        cooldown1 = Mathf.Max(0f, cooldown1 - Time.deltaTime);
        cooldown2 = Mathf.Max(0f, cooldown2 - Time.deltaTime);

        if (skill_Image.Length > 0)
            skill_Image[0].fillAmount =
                1f - cooldown1 / cd_1;

        if (skill_Image.Length > 1)
            skill_Image[1].fillAmount =
                1f - cooldown2 / cd_2;

        // Habilidade 1
        if (Keyboard.current[Key.Digit1].wasPressedThisFrame
            && cooldown1 <= 0f)
        {
            ActivateSkill1();
            cooldown1 = cd_1;
        }

        // Habilidade 2
        if (Keyboard.current[Key.Digit2].wasPressedThisFrame
            && cooldown2 <= 0f)
        {
            ActivateSkill2();
            cooldown2 = cd_2;
        }

        // Granada: E para lançar
        if (Keyboard.current[Key.E].wasPressedThisFrame
            && hasGrenade)
        {
            ThrowGrenade();
        }

        // Terceira habilidade: indica se há granada equipada
        if (skill_Image.Length > 2)
        {
            skill_Image[2].fillAmount = hasGrenade ? 1f : 0f;
        }
    }
    public void EquipGrenade()
    {
        hasGrenade = true;

        Debug.Log("Granada pronta para lançar!");
    }
    void ThrowGrenade()
    {
        if (grenadePrefab == null || grenadeThrowPoint == null)
        {
            Debug.LogWarning(
                "Configure o prefab e o ponto de lançamento da granada."
            );
            return;
        }

        GameObject grenade = Instantiate(
            grenadePrefab,
            grenadeThrowPoint.position,
            grenadeThrowPoint.rotation
        );

        Grenade grenadeScript = grenade.GetComponent<Grenade>();

        if (grenadeScript != null)
        {
            grenadeScript.Throw(grenadeThrowPoint.forward);
            hasGrenade = false;
        }
        else
        {
            Debug.LogError("O prefab não possui o script Grenade!");
            Destroy(grenade);
        }
    }
    void ActivateSkill1()
    {
        Debug.Log("Habilidade 1 ativada!");
        // Aqui entra a habilidade específica do personagem.
    }

    void ActivateSkill2()
    {
        Debug.Log("Habilidade 2 ativada!");
        // Aqui entra a habilidade específica do personagem.
    }
}