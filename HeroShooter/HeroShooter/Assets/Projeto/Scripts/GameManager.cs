using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gameStartText;
    public float startTime = 5f;
    public bool StopCountdown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StopCountdown = false;
        startTime = 5;
    }

    // Update is called once per frame
    void Update()
    {
        StartCountDown();
    }

    void StartCountDown()
    {
        if (startTime > 0 && !StopCountdown)
        {
            startTime -= Time.deltaTime;
            gameStartText.text = "Game Starts in: " + startTime.ToString("0");
        }
        if (startTime <= 0 && !StopCountdown)
        {
            gameStartText.text = "Game Started!";
            Invoke("ClearCountDown", 1f);
        }
    }
    void ClearCountDown()
    {
        gameStartText.text = "";
        StopCountdown = true;
        startTime = 5;
    }
}
