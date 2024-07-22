using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveData// : MonoBehaviour
{
    // time since last changed location
    public float timeSpentHere;
    // time since first spawned in research level
    public float timeSpentTotal;
    // xIndex and yIndex in research level
    public Vector2Int oldCoordinates = new Vector2Int(0, 0);
    public Vector2Int nextCoordinates = new Vector2Int(0, 0);
    // which move number is this? initial spawn = move 0
    public int moveNumber;

    public MoveData(float timeHere, float timeTotal, Vector2Int fromCoordinates, Vector2Int toCoordinates, int moveNum)
    {
        timeSpentHere = timeHere;
        timeSpentTotal = timeTotal;
        oldCoordinates = fromCoordinates;
        nextCoordinates = toCoordinates;
        moveNumber = moveNum;
    }
}
