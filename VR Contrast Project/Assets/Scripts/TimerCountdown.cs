using TMPro;
using UnityEngine;

public class TimerCountdown : MonoBehaviour
{
    // remaining time
    [SerializeField] private float currentTime;
    [SerializeField] private float maxTime;
    [SerializeField] private TextMeshProUGUI timerUI;
    // whether timer is counting down or not
    public bool isCounting = false;
    public LevelController levelController;

    void OnEnable()
    {
        // set current time remaining to max time
        currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        // if timer is running, work out time to be displayed
        if (isCounting)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                // end
                levelController.LevelOneToEnd();
            }
            DisplayTime(currentTime);
        }
        else
        {
            // not in a level where timer is needed, so display infinity
            DisplayTime(Mathf.Infinity);
        }
    }

    // takes the time in seconds and converts it into minutes and seconds before displaying it
    void DisplayTime(float timeInSeconds)
    {
        // only used when timer is not needed in a level
        if (timeInSeconds == Mathf.Infinity)
        {
            timerUI.text = "infi\nnity";
        }
        else
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

    public void ResetTimer(bool isRunning)
    {
        // reset current time remaining to max time
        currentTime = maxTime;
        // determines whether timer is running or not
        isCounting = isRunning;
    }
}
