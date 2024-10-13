using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField]private Light2D _light;
    [SerializeField]private bool IsSpot;

    [SerializeField]private List<PlayerLightHandler> playerLights = new List<PlayerLightHandler>();
    [SerializeField]private float lightDuration = 0.6f;
    [SerializeField]private float lightDiffuseOffset = 0;
    [SerializeField]private float lightDiffuseIntensity = 1;
    [SerializeField]private Vector3[] poses = new Vector3[4];

    private float lightIntensityTarget;
    private bool isLightOn;

    [SerializeField]private float focusDistance;
    Vector3[] shape;
    Vector3[] newShape;
    float lightDir;

    void Start()
    {
        _light = GetComponentInChildren<Light2D>();
        if(_light.lightType == Light2D.LightType.Freeform)
        {
            if(IsSpot)
            {
                focusDistance = _light.shapePath[1].x;
                Vector3[] spotShape = new Vector3[4]
                {
                    _light.shapePath[0],
                    new Vector3(focusDistance, 0),
                    new Vector3(focusDistance, 0),
                    _light.shapePath[3]
                };
                _light.SetShapePath(spotShape);
            }
            shape = _light.shapePath;
        }
    }

    void Update()
    {
        UpdateLight();
        if(playerLights.Count > 0 && !isLightOn)
        {
            TurnOnLight();
        }
        else if(playerLights.Count <= 0 && isLightOn)
        {
            TurnOffLight();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Light") && other.GetComponent<PlayerLightHandler>())
        {
            playerLights.Add(other.GetComponent<PlayerLightHandler>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Light") && other.GetComponent<PlayerLightHandler>())
        {
            playerLights.Remove(other.GetComponent<PlayerLightHandler>());
        }
    }
    private void UpdateLight()
    {
        _light.intensity = Mathf.Lerp(_light.intensity, lightIntensityTarget, lightDuration);

        if(playerLights.Count <= 0) return;
        float dir = playerLights[0].transform.position.x - transform.position.x;
        lightDir = dir<=0?1:-1;
        transform.localScale = new Vector3(lightDir, transform.localScale.y, transform.localScale.z);
        //控制强度
        float allLightIntensity = 0;
        foreach(var light in playerLights)
        {
            float a = CheckDistance(light, false);
            allLightIntensity += light.GetLightIntensity() * (1 - a);
        }
        lightIntensityTarget = allLightIntensity;

        //控制散射角度
        if(IsSpot)
        {
            SpotLight();
        }
        else
        {
            ScatterLight();
        }
    }

    private void SpotLight()
    {
        var light = playerLights[0];
        float a = CheckDistance(light, true)*lightDiffuseIntensity + lightDiffuseOffset;

        if(shape != null)
        {
            newShape = new Vector3[4];
            for(int i = 0;i<newShape.Length;++i)
            {
                newShape[i] = shape[i] + poses[i];
            }
            float newFocusDistance = focusDistance - a;
            newShape[1] = new Vector3(newFocusDistance, 0);
            newShape[2] = new Vector3(newFocusDistance, 0);
            _light.SetShapePath(newShape);
        }
    }

    private void ScatterLight()
    {
        var light = playerLights[0];
        float a = CheckDistance(light, true)*lightDiffuseIntensity + lightDiffuseOffset;
        
        if(shape != null)
        {
            newShape = new Vector3[4];
            for(int i = 0;i<newShape.Length;++i)
            {
                newShape[i] = shape[i] + poses[i];
            }
            newShape[1] = shape[1] - new Vector3(0,a,0);
            newShape[2] = shape[2] + new Vector3(0,a,0);

            _light.SetShapePath(newShape);
        }
    }

    private float CheckDistance(PlayerLightHandler light, bool IsReversal)
    {
        float distanse = Vector2.Distance(light.transform.position, transform.position);
        float a = distanse / light.GetLightRange() > 1 ? 1 : distanse / light.GetLightRange();
        return IsReversal ? 1-a:a;
    }

    private void TurnOnLight()
    {
        isLightOn = true;
        //lightIntensityTarget = 1;
    }

    private void TurnOffLight()
    {
        isLightOn = false;
        lightIntensityTarget = 0;
    }

}
