using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ItemTrack : MonoBehaviour
{
    public Transform trackStartPos;
    public Transform trackEndPos;
    public Vector3 GetPosOnTrack(float deltaPos)
    {
        Vector3 newPos = Vector3.Lerp(trackStartPos.position, trackEndPos.position, deltaPos);
        return newPos;
    }

    public float GetStartDelta(Vector3 pos)
    {
        float currentDelta = (pos.x - trackStartPos.position.x) / (trackEndPos.position.x - trackStartPos.position.x);
        return currentDelta;
    }
}
