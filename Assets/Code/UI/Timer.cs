using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] private Image crackWindow;

    public static bool timerActive;

    // Update is called once per frame

    void Start()
    {
        StartCoroutine(EnableCrack());
    }
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

    IEnumerator EnableCrack()
    {
        yield return new     WaitForSeconds(30);//wait for 115 seconds (1:55minutes as your video)
        crackWindow.enabled = true;
    }
}
 