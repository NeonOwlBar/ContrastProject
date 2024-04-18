using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class SaveData : MonoBehaviour
{
    public static List<MoveData> movementData = new();

    public GameObject dataPanel;
    public TextMeshProUGUI jsonDataTextBox;

    // Start is called before the first frame update
    void Start()
    {
        dataPanel.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    // include contrast

    public static void CreateSaveData(float timeHere, float timeTotal, Vector2Int fromCoordinates, Vector2Int toCoordinates, int numberOfMoves)
    {
        MoveData moveData = new MoveData(timeHere, timeTotal, fromCoordinates, toCoordinates, numberOfMoves);
        movementData.Add(moveData);
        string jsonData = JsonUtility.ToJson(moveData);
        Debug.Log("json data created: " + jsonData);
    }

    public void PrintSaveData()
    {
        dataPanel.SetActive(true);
        string jsonData = JsonUtility.ToJson(movementData);
        jsonDataTextBox.text = jsonData;
        Debug.Log("movementData = " + jsonData);
    }
}
