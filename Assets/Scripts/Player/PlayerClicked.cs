using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClicked : MonoBehaviour
{
    private PlayerController thisController;
    void Start()
    {
        thisController = GetComponent<PlayerController>();
    }
    void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlayerInput.Instance.ChoosePlayer(thisController);
            CameraController.Instance.SwitchToLocal(transform);
        }
    }
}
