using System.Collections;
using System.Collections.Generic;
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
