using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour, IInteract
{
    public LightType lightType;
    // Start is called before the first frame update
    private bool isPlayerIn = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool isOn;
    public void Interact(PlayerInteract from)
    {
        if(from.GetLightType() == lightType)
        {
            if(!isPlayerIn)
            {
                from.transform.position = transform.position;
                from.LockPlayer();
            }
            else
            {
                from.UnlockPlayer();
            }
            isPlayerIn = !isPlayerIn;
        }
    }
}
