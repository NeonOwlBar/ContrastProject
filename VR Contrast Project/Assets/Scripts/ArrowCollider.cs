using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollider : MonoBehaviour
{
    public void ChangePosition(Vector3 newPos)
    {
        transform.position = newPos;
    }
}
