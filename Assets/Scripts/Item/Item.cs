using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]private ItemTrack trackThisIsOn;
    [SerializeField]private float dragSpeed = 1;
    [SerializeField]private bool isDraging;
    float deltaX;
    float deltaPos;
    float startDeltaPos;
    void Start()
    {
        transform.position = trackThisIsOn.trackStartPos.position;
    }

    void Update()
    {
        if(isDraging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            deltaPos = mousePos.x - deltaX;
            transform.position = trackThisIsOn.GetPosOnTrack(startDeltaPos + deltaPos * dragSpeed);
        }
    }
    
    void OnMouseDown()
    {
        Debug.Log(111);
        isDraging = true;
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        startDeltaPos = trackThisIsOn.GetStartDelta(transform.position);
    }

    void OnMouseUp()
    {
        Debug.Log(222);
        isDraging = false;
    }
}
