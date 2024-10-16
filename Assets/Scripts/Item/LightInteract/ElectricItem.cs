using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricItem : Item, IInteractWithLight
{
    public float maxHealth;
    public float initHealth;
    [SerializeField]private float currentHealth;
    public float cureSpeed = 1f;
    // Start is called before the first frame update
    private bool isInteracting = false;
    void Start()
    {
        currentHealth = initHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInteracting)
        {
            currentHealth += cureSpeed*Time.deltaTime;
            if(currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        
    }
    public void Interact(LightType lightType)
    {
        if(lightType == LightType.Red)
        {
            isInteracting = true;
        }
    }

    public void EndInteract(LightType lightType)
    {
        if(lightType == LightType.Red)
        {
            isInteracting = false;
        }
    }
}
