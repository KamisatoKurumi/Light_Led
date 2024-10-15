using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLightHandler : MonoBehaviour
{
    [Header("Component")]
    [SerializeField]private Light2D _light;
    [SerializeField]private CircleCollider2D _collider;

    [Header("Property")]
    [SerializeField]private float lightRange;
    [SerializeField]private float lightIntensity;
    [SerializeField]private float lightSoftEdge;
    private bool isLightOn;
    private float lightIntensityTarget;

    void Start()
    {
        _light = GetComponentInChildren<Light2D>();
        _collider = GetComponent<CircleCollider2D>();
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

        _light.intensity = math.lerp(_light.intensity, lightIntensityTarget, 0.5f);
    }

    
    public void TurnOnLight()
    {
        isLightOn = true;
        lightIntensityTarget = 1;
    }

    public void TurnOffLight()
    {
        isLightOn = false;
        lightIntensityTarget = 0;
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
