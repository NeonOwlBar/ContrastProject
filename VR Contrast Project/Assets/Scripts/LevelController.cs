using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject tutorialUI;
    public GameObject checkProceedUI;
    public GameObject levelOneUI;
    // current coordinate and favourite button
    public GameObject debugUI;

    public ChangeSkybox skybox;

    public SaveData saveData;

    private void Awake()
    {
        // only shows debugging UI if in the editor
#if UNITY_EDITOR
        debugUI.SetActive(true);
#else
        debugUI.SetActive(false);
#endif
    }

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
        saveData.PrintSaveData();
    }
}
