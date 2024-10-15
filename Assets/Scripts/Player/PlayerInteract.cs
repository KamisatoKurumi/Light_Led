using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerController controller;
    private PlayerLightHandler lightHandler;
    private IInteract interactableItemInRange;
    PlayerController currentPlayer;
    bool isLightOn;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        lightHandler = GetComponentInChildren<PlayerLightHandler>();
        isLightOn = lightHandler.GetIsLightOn();
    }

    void Update()
    {
        
        if(PlayerInput.Instance.GetCurrentPlayer(out currentPlayer) && currentPlayer == controller && currentPlayer.Input.Interact)
        {
            if(interactableItemInRange != null)
            {
                interactableItemInRange.Interact(this);
            }
            else
            {
                isLightOn = !isLightOn;
                if(isLightOn) lightHandler.TurnOnLight();
                else lightHandler.TurnOffLight();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Interactable"))
        {
            interactableItemInRange = other.GetComponent<IInteract>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Interactable"))
        {
            interactableItemInRange = null;
        }
    }
}
