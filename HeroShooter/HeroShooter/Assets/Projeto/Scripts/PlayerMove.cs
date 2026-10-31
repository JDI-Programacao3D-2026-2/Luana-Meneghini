using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody rb;
    public GameManager gameManager;
    private Vector3 direction;

    void Update()
    {
        // Impede o movimento antes do início da partida
        if (gameManager == null ||
            !gameManager.GameStarted ||
            gameManager.GameEnded)
        {
            direction = Vector3.zero;
            return;
        }

        // Alinha a orientação do jogador com a direção Y da Câmera
        transform.rotation = Quaternion.Euler(
            0,
            Camera.main.transform.eulerAngles.y,
            0
        );

        direction = Vector3.zero;

        // Processa as teclas de movimentação de acordo com o vetor de visão do personagem
        if (Keyboard.current[Key.W].isPressed)
            direction += transform.forward;

        if (Keyboard.current[Key.S].isPressed)
            direction -= transform.forward;

        if (Keyboard.current[Key.D].isPressed)
            direction += transform.right;

        if (Keyboard.current[Key.A].isPressed)
            direction -= transform.right;

        // Normaliza a magnitude para evitar que andar na diagonal seja mais rápido
        direction = Vector3.ClampMagnitude(direction, 1f);
    }
    void FixedUpdate()
    {
        if (gameManager == null ||
            !gameManager.GameStarted ||
            gameManager.GameEnded)
        {
            rb.linearVelocity = new Vector3(
                0,
                rb.linearVelocity.y,
                0
            );

            return;
        }

        // Gerencia a movimentação com física (Rigidbody) durante o efeito de Dash
        if (isDashing)
        {
            if (Time.time < dashEndTime)
            {
                rb.linearVelocity = new Vector3(
                    dashDirection.x * 15f,
                    rb.linearVelocity.y,
                    dashDirection.z * 15f
                );

                return;
            }

            isDashing = false;
        }

        // Aplicação normal de velocidade no Rigidbody
        rb.linearVelocity = new Vector3(
            direction.x * speed,
            rb.linearVelocity.y,
            direction.z * speed
        );
    }
    private bool isDashing;
    private Vector3 dashDirection;
    private float dashEndTime;

    public void StartDash(float dashSpeed, float dashDuration)
    {
        dashDirection = transform.forward;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        isDashing = true;
        dashEndTime = Time.time + dashDuration;

        rb.linearVelocity = new Vector3(
            dashDirection.x * dashSpeed,
            rb.linearVelocity.y,
            dashDirection.z * dashSpeed
        );
    }
}