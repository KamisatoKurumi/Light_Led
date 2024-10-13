using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IDragHandler
{
    [SerializeField]private ItemTrack trackThisIsOn;


    void Start()
    {
        transform.position = trackThisIsOn.trackStartPos.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = trackThisIsOn.GetPosOnTrack(eventData.delta.x);
    }
    
}
