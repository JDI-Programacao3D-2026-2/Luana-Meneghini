using UnityEngine;
using UnityEngine.InputSystem;

public class MouseOrbit : MonoBehaviour
{
    public Transform player;
    public float distance = 5f;
    public float MouseSensitivity = 0.2f;
    private float rotX;
    private float rotY;
    public float minY = -30f;
    public float maxY = 60f;

void Start()
    {
        // Esconde o ponteiro do mouse e prende-o ao centro da tela
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void LateUpdate()
    {
        if (player != null) 
        {
            // Lê o deslocamento do mouse no novo Input System
            Vector2 delta = Mouse.current.delta.ReadValue() * MouseSensitivity;
        rotX += delta.x;

            // Limpa e restringe a rotação vertical entre o mínimo e o máximo para não inverter a visão
            rotY = Mathf.Clamp(rotY - delta.y,minY,maxY);  
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);

            // Posiciona a câmera mantendo a distância definida atrás do jogador
            transform.position = player.position - (rotation * Vector3.forward * distance);
        transform.rotation = rotation;
        }
    }
}



























//Mathf.Clamp(0,0,0);




// rotY = Mathf.Clamp(rotY - delta.y, -30f, 60f);