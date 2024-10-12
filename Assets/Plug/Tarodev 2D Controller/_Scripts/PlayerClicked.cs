using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerClicked : MonoBehaviour
{
    private PlayerController thisController;
    void Start()
    {
        thisController = GetComponent<PlayerController>();
    }
    void OnMouseDown()
    {
        Debug.Log("Player Be Clicked");
        PlayerInput.Instance.ChoosePlayer(thisController);
    }
}
