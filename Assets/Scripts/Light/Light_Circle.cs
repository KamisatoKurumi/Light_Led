using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light_Circle : MonoBehaviour, ILightController
{
    [Header("Component")]
    [SerializeField]private Light2D _light;
    [SerializeField]private CircleCollider2D _collider;
    private PlayerController parent;

    [Header("Property")]
    [SerializeField]private float lightRange;
    [SerializeField]private float lightIntensity;
    [SerializeField]private float lightSoftEdge;
    private bool isLightOn;
    private float lightIntensityTarget;

    void Start()
    {
        parent = GetComponentInParent<PlayerController>();
        _light = GetComponentInChildren<Light2D>();
        _collider = GetComponent<CircleCollider2D>();
        TurnOffLight();
    }

    void Update()
    {
        UpdateLight();
    }


    private void UpdateLight()
    {
        _light.pointLightOuterRadius = lightRange;
        _light.pointLightInnerRadius = lightRange - lightSoftEdge;
        _collider.radius = lightRange - lightSoftEdge/2;

        _light.intensity = Mathf.Lerp(_light.intensity, lightIntensityTarget, 0.5f);
    }

    
    public void TurnOnLight()
    {
        isLightOn = true;
        lightIntensityTarget = 1;
        _collider.enabled = true;
    }

    public void TurnOffLight()
    {
        isLightOn = false;
        lightIntensityTarget = 0;
        _collider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<IInteractWithLight>() != null)
        {
            other.GetComponent<IInteractWithLight>().Interact(parent.lightType);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.GetComponent<IInteractWithLight>() != null)
        {
            other.GetComponent<IInteractWithLight>().EndInteract(parent.lightType);
        }
    }


    public bool GetIsLightOn()
    {
        return isLightOn;
    }

    public float GetLightIntensity()
    {
        return _light.intensity;
    }
    public float GetLightRange()
    {
        return lightRange;
    }
}
