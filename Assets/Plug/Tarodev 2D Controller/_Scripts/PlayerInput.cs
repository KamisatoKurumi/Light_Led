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
    public class InputPackage
    {
        public float X,Y;
        public bool JumpDown;
        public bool JumpUp;
    }
    private bool isControllingPlayer{get => currentPlayer != null;}
    [SerializeField]private PlayerController currentPlayer;
    
    void Start()
    {
        currentPlayer = null;
    }

    void Update()
    {
        if(isControllingPlayer)
        {
            Debug.Log(111);
            currentPlayer.GatherInput();
        }
        else
        {
            Debug.Log(222);
        }
    }

    public void ChoosePlayer(PlayerController playerController)
    {
        currentPlayer = playerController;
    }
}
