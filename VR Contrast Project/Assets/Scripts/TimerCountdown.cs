using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerCountdown : MonoBehaviour
{
    [SerializeField] private float currentTime;
    [SerializeField] private float maxTime;
    [SerializeField] private TextMeshProUGUI timerUI;

    void OnEnable()
    {
        currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            // end
        }
        DisplayTime(currentTime);
    }

    // takes the time in seconds and converts it into minutes and seconds before displaying it
    void DisplayTime(float timeInSeconds)
    {
        // display zero if value is below zero
        if (timeInSeconds < 0) timeInSeconds = 0;
        // determine how many minutes remain
        float minutes = Mathf.FloorToInt(timeInSeconds / 60);
        // determine how many seconds remain in the current minute
        float seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // format is: firstArgument (as a) two-digitNumber : secondArgument (as a) two-digitNumber
        // i.e. within each {}, the value to the left of the colon denotes the index of the parameters to be used.
        // and the value to the right of the colon denotes how many digits should be displayed
        timerUI.text = string.Format("{0:00}\n{1:00}", minutes, seconds);
    }
}
