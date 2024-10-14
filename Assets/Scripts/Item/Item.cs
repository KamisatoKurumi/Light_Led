using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]private ItemTrack trackThisIsOn;


    void Start()
    {
        transform.position = trackThisIsOn.trackStartPos.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.delta.x);
        //transform.position = trackThisIsOn.GetPosOnTrack(eventData.delta.x);
    }
    void OnMouseDown()
    {
        Debug.Log(111);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
    }
}
