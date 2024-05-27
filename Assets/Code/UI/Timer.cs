using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public static bool timerActive;

    // Update is called once per frame
    void Update()
    {
        if (!timerActive) return;
        
        if (remainingTime > 0) {
            remainingTime -= Time.deltaTime;
        } else if (remainingTime < 0) {
            remainingTime = 0;
            ResetGame.ReloadScene();
            timerText.color = Color.red;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("O2 left: {0:00}:{1:00}", minutes, seconds);
    }
}
 