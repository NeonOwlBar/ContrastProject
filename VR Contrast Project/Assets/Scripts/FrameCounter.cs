using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        fpsText.text = Mathf.RoundToInt(1f / Time.deltaTime) + " fps";
    }
}
