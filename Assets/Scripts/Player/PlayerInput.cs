using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class PlayerInput : MonoBehaviour
{
    private static PlayerInput _instance;
    public static PlayerInput Instance{get => _instance;}

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
    }
    private bool isControllingPlayer{get => currentPlayer != null;}
    [SerializeField]private PlayerController currentPlayer;

    public PlayerController CurrentPlayer
    {
        get => currentPlayer;
    }
    
    void Start()
    {
        currentPlayer = null;
    }

    void Update()
    {
        if(isControllingPlayer)
        {
            currentPlayer.GatherInput();
            if(Input.GetMouseButtonDown(1))
            {
                currentPlayer = null;
                CameraController.Instance.SwitchToFull();
            }
        }
        else
        {
            
        }
    }

    public bool GetCurrentPlayer(out PlayerController controller)
    {
        if(currentPlayer == null) 
        {
            controller = null;
            return false;
        }
        else controller = currentPlayer;
        return true;
    }

    public void ChoosePlayer(PlayerController playerController)
    {
        currentPlayer = playerController;
    }
}
