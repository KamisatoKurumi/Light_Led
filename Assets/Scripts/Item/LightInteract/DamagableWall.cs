using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamagableWall : Item, IInteractWithLight
{
    public void Interact(LightType lightType)
    {
        if(lightType != LightType.Yellow) return;

        StartCoroutine(WaitForSecondThenDestroy());

    }

    public IEnumerator WaitForSecondThenDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
    public void EndInteract(LightType lightType)
    {
        
    }
}
