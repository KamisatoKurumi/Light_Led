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


}
