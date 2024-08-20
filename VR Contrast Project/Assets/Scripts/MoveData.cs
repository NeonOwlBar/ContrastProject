using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveData// : MonoBehaviour
{
    // which move number is this? initial spawn = move 0
    public int moveNumber;
    // time since last changed location
    public float timeSpentHere;
    // time since first spawned in research level
    public float timeSpentTotal;
    // xIndex and yIndex in research level
    public Vector2Int coordsFrom = new Vector2Int(0, 0);
    public Vector2Int coordsTo = new Vector2Int(0, 0);

    public MoveData(int moveNum, float timeHere, float timeTotal, Vector2Int fromCoordinates, Vector2Int toCoordinates)
    {
        moveNumber = moveNum;
        timeSpentHere = timeHere;
        timeSpentTotal = timeTotal;
        coordsFrom = fromCoordinates;
        coordsTo = toCoordinates;
    }
}
