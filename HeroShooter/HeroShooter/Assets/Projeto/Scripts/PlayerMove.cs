using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private float speed = 5f;
    public Rigidbody rb;
    public GameManager gameManager;
    private Vector3 direction;

    void Update()
    {
        // Impede o movimento antes do início da partida
        if (gameManager == null || !gameManager.GameStarted || gameManager.GameEnded)
        {
            direction = Vector3.zero;
            return;
        }

        transform.rotation = Quaternion.Euler(
            0,
            Camera.main.transform.eulerAngles.y,
            0
        );

        direction = Vector3.zero;

        if (Keyboard.current[Key.W].isPressed)
            direction += transform.forward;

        if (Keyboard.current[Key.S].isPressed)
            direction -= transform.forward;

        if (Keyboard.current[Key.D].isPressed)
            direction += transform.right;

        if (Keyboard.current[Key.A].isPressed)
            direction -= transform.right;

        direction = Vector3.ClampMagnitude(direction, 1f);
    }
    void FixedUpdate()
    {
        if (gameManager == null || !gameManager.GameStarted || gameManager.GameEnded)
        {
            rb.linearVelocity = new Vector3(
                0,
                rb.linearVelocity.y,
                0
            );

            return;
        }

        rb.linearVelocity = new Vector3(
            direction.x * speed,
            rb.linearVelocity.y,
            direction.z * speed
        );
    }
}