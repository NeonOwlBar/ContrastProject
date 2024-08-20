using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    public static List<MoveData> movementData = new();
    private bool isDataSaved = false;
    public static string allJsonData;

    public GameObject levelOneUI;
    public GameObject dataSavedUI;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        dataSavedUI.SetActive(false);

        isDataSaved = false;
    }

    // include contrast

    public static void CreateSaveData(float timeHere, float timeTotal, Vector2Int fromCoordinates, Vector2Int toCoordinates, int numberOfMoves)
    {
        MoveData moveData = new MoveData(numberOfMoves, timeHere, timeTotal, fromCoordinates, toCoordinates);
        movementData.Add(moveData);
        string jsonData = JsonUtility.ToJson(moveData);
        allJsonData += "\n" + jsonData;
        //Debug.Log("json data created: " + jsonData);
    }

    public void PrintSaveData()
    {
        // only save data if not yet saved
        if (!isDataSaved)
        {
            string path = Application.persistentDataPath + "/AugustTesting.txt";
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine("\n\nNEW PARTICIPANT!!!! ****************************************\n");
            //foreach (MoveData item in movementData)
            //{
            //    writer.WriteLine("Move number " + item.moveNumber + " at timestamp: " + item.timeSpentTotal +
            //        ";      Moved from (" + item.oldCoordinates.x + ", " + item.oldCoordinates.y + ") to (" + item.nextCoordinates.x + ", " + item.nextCoordinates.y +
            //        ");      Time spent at the location: " + item.timeSpentHere + "\n");
            //}
            writer.WriteLine(allJsonData);

            writer.Close();

            if (File.Exists(path))
            {
                dataSavedUI.SetActive(true);
                levelOneUI.SetActive(false);
                // assign data as saved so no duplicates
                isDataSaved = true;
            }
        }
    }
}
