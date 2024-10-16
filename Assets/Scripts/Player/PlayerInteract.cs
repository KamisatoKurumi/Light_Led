using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerController controller;
    private ILightController lightHandler;
    private IInteract interactableItemInRange;
    
    public readonly LightType lightType = LightType.Yellow;
    bool isLightOn;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        lightHandler = GetComponentInChildren<ILightController>();
        isLightOn = lightHandler.GetIsLightOn();
    }
    PlayerController currentPlayer;
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

    public LightType GetLightType()
    {
        return controller.lightType;
    }
    public void LockPlayer()
    {
        controller.DeActivate();
        controller.SetUsingGravity(false);
    }
    public void UnlockPlayer()
    {
        controller.Activate();
        controller.SetUsingGravity(true);
    }
}
