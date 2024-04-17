using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject tutorialUI;
    public GameObject checkProceedUI;
    public GameObject levelOneUI;

    public ChangeSkybox skybox;

    public void TutorialToProceed()
    {
        tutorialUI.SetActive(false);
        checkProceedUI.SetActive(true);
    }

    public void ReturnToTutorial()
    {
        checkProceedUI.SetActive(false);
        tutorialUI.SetActive(true);
    }

    public void ProceedToLevelOne()
    {
        checkProceedUI.SetActive(false);
        levelOneUI.SetActive(true);
        skybox.SetLevelOne();
    }

    public void LevelOneToEnd()
    {
        // save all data to JSON
        Debug.Log("LevelOneToEnd() called");
    }
}
