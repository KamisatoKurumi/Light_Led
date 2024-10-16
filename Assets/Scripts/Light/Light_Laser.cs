using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Light_Laser : MonoBehaviour, ILightController
{
    private LineRenderer lineRenderer;
    private PlayerController parent;
    public float laserDistance = 10f; // 激光的最大距离
    public float laserHoldTime; // 激光的持续时间
    public LayerMask targetLayer;     // 可检测的目标层
    private bool isLightOn;

    void Start()
    {
        parent = GetComponentInParent<PlayerController>();
        lineRenderer = GetComponent<LineRenderer>();
        TurnOffLight();
    }
    
    void Update()
    {
        if(!isLightOn)
        {
            TurnOffLight();
            return;
        }
        Vector2 laserOrigin = parent.transform.position;
        Vector2 laserDirection = new Vector2(parent.RemineDirXForRight ? 1 : -1, 0);
        // 激光可视化
        if (hit.collider != null)
        {
            // 撞击到物体
            DrawLaser(laserOrigin, hit.point);

            // 对撞击到的物体执行操作
            Debug.Log("Hit: " + hit.collider.name);
            
            if (hit.collider.GetComponent<Item>() != null)
            {
                IInteractWithLight interactItem = hit.collider.GetComponent<IInteractWithLight>();
                interactItem.Interact(parent.lightType);
            }
        }
        else
        {
            // 没有撞击物体时，激光达到最大距离
            DrawLaser(laserOrigin, laserOrigin + laserDirection * laserDistance);
        }
    }

    public void TurnOnLight()
    {
        FireLaser();
    }
    public void TurnOffLight()
    {
        isLightOn = false;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }

    public bool GetIsLightOn()
    {
        
        return isLightOn;
    }
    RaycastHit2D hit;
    private void FireLaser()
    {
        isLightOn = true;
        Vector2 laserOrigin = parent.transform.position;
        Vector2 laserDirection = new Vector2(parent.RemineDirXForRight ? 1 : -1, 0);

        // 发射激光 (Raycast)
        hit = Physics2D.Raycast(laserOrigin, laserDirection, laserDistance, targetLayer);
        
    }

    void DrawLaser(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
