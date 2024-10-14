using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]private bool hasTrack;
    [SerializeField]private ItemTrack trackThisIsOn;
    [SerializeField]private float dragSpeed = 1;
    [SerializeField]private bool isDraging;
    float deltaX;
    float deltaPosX;
    float startDeltaPosX;

    Vector3 delta;
    Vector3 deltaPos;
    Vector3 startDeltaPos;
    void Start()
    {
        if(hasTrack)
        {
            transform.position = trackThisIsOn.trackStartPos.position;
        }
    }

    void Update()
    {
        if(isDraging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(hasTrack)
            {
                deltaPosX = mousePos.x - deltaX;
                transform.position = trackThisIsOn.GetPosOnTrack(startDeltaPosX + deltaPosX * dragSpeed * 0.2f);
            }
            else
            {
                deltaPos = mousePos - delta;
                transform.position = startDeltaPos + deltaPos * dragSpeed;
            }
        }
    }
    
    void OnMouseDown()
    {
        Debug.Log(111);
        isDraging = true;
        if(hasTrack)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            startDeltaPosX = trackThisIsOn.GetStartDelta(transform.position);
        }
        else
        {
            startDeltaPos = transform.position;
            delta = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void OnMouseUp()
    {
        Debug.Log(222);
        isDraging = false;
    }
}
