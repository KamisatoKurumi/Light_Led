using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TranslucentMirror : MonoBehaviour
{
    [SerializeField]
    private Light2D m_playerPoint;

    [SerializeField]
    private Light2D m_ligthBeam;

    [Header("光源和镜面的最大距离")]
    [SerializeField]
    private float m_maxDistance = 10f;

    [Header("光源和镜面的最小距离")]
    [SerializeField]
    private float m_minDistance = 0f;

    [Header("直线射线消失的位置")]
    [SerializeField]
    private float m_lineCloseDistance = 5f;

    #region 聚光灯相关参数
    //控制光线长度 内外半径最好相差1f效果最好
    [Header("最小内半径")]
    [SerializeField]
    private float m_minInnerRadius = 0f;
    [Header("最大内半径")]
    [SerializeField]
    private float m_maxInnerRadius = 9f;


    [Header("最小外半径")]
    private float m_minOuterRadius = 1f;
    [Header("最大外半径")]
    private float m_maxOuterRadius = 10f;

    [Header("最小内聚光灯角度")]
    [SerializeField]
    private float m_minInnerAngle = 0f;
    [Header("最大内聚光灯角度")]
    [SerializeField]
    private float m_maxInnerAngle = 62f;

    [Header("最小外聚光灯角度")]
    [SerializeField]
    private float m_minOuterAngle = 20f;
    [Header("最大外聚光灯角度")]
    [SerializeField]
    private float m_maxOuterAngle = 66f;
    #endregion


    private float m_currentPlayerInnerRadius;
    private float m_currentPlayerOuterRadius;

    [Header("玩家光源放大尺寸")]
    [SerializeField]
    private float m_newPlayerInnerRadius;
    [SerializeField]
    private float m_newPlayerOuterRadius;

    [SerializeField]
    private float targetDistance = 1.5f;

    private float xThreshold = 0.5f;


    private bool isUpLightDistacne;
    private bool IsUpLightDistacne
    {
        get => isUpLightDistacne;
        set
        {
            if (isUpLightDistacne == value) return;
            m_ligthBeam.gameObject.SetActive(!isUpLightDistacne);

            if (isUpLightDistacne)
            {
                UpdatePlayerPointRadius(m_newPlayerInnerRadius, m_newPlayerOuterRadius);
            }
            else
                UpdatePlayerPointRadius(m_currentPlayerInnerRadius, m_currentPlayerOuterRadius);

            isUpLightDistacne = value;
        }
    }

    private bool isChangeColor;
    private bool IsChangeColor
    {
        get => isChangeColor;
        set
        {
            if (isChangeColor == value) return;
            ChangeLightColor();
        }
    }

    private Quaternion m_previousDirection;

    [Header("检测目标tag")]
    [SerializeField]
    private string targetTag;

    [Header("检测射线的个数")]
    [SerializeField]
    private int rayCount;

    private void Awake()
    {
        //Todo 通过代码获取玩家控制角色(暂时注释
        //m_playerPoint = PlayerInput.Instance.CurrentPlayer.GetComponent<Light2D>();
        m_ligthBeam = GetComponentInChildren<Light2D>();
        m_ligthBeam.lightType = Light2D.LightType.Point;

        m_currentPlayerInnerRadius = m_playerPoint.pointLightInnerRadius;
        m_currentPlayerOuterRadius = m_playerPoint.pointLightOuterRadius;
    }

    private void Update()
    {
        if (m_playerPoint != null)
        {
            float yDifferenceCalculator = Mathf.Abs(m_playerPoint.transform.position.y - m_ligthBeam.transform.position.y);
            bool isYAlignHandler = yDifferenceCalculator <= xThreshold;

            m_ligthBeam.gameObject.SetActive(isYAlignHandler);
            if (isYAlignHandler)
            {
                UpdateLightProperties();
            }

            if (m_ligthBeam.gameObject.activeSelf)
            {
                CheckRaycastInLightBeam();
            }
        }
    }

    private void UpdateLightProperties()
    {
        //计算内外半径
        float distace = Mathf.Abs(this.transform.position.x - m_playerPoint.transform.position.x);
        float lerpT = spotlightAngleCalculator(distace);

        float innerRadius = Mathf.Lerp(m_maxInnerRadius, m_minInnerRadius, lerpT);
        float outerRadius = Mathf.Lerp(m_maxOuterRadius, m_minOuterRadius, lerpT);

        float innerAngle;
        float outerAngle = Mathf.Lerp(m_maxOuterAngle, m_minOuterAngle, lerpT); ;
        if (distace < m_lineCloseDistance)
        {
            innerAngle = Mathf.Lerp(m_maxInnerAngle, m_minInnerAngle, lerpT);
        }
        else
        {
            innerAngle = 0;
        }

        m_ligthBeam.pointLightInnerRadius = innerRadius;
        m_ligthBeam.pointLightOuterRadius = outerRadius;

        m_ligthBeam.pointLightInnerAngle = innerAngle;
        m_ligthBeam.pointLightOuterAngle = outerAngle;


        IsUpLightDistacne = targetDistance <= distace;
        IsChangeColor = m_ligthBeam.color != m_playerPoint.color;

        SpotlightTround();
    }

    private void ChangeLightColor()
    {
        m_ligthBeam.color = m_playerPoint.color;
    }

    private float spotlightAngleCalculator(float distace)
    {
        return Mathf.Clamp01((distace - m_minDistance) / (m_maxDistance - m_minDistance));
    }

    private void UpdatePlayerPointRadius(float innerRadius, float outerRadius)
    {
        m_playerPoint.pointLightInnerRadius = innerRadius;
        m_playerPoint.pointLightOuterRadius = outerRadius;
    }

    private void SpotlightTround()
    {
        Vector3 startPoint = this.transform.position - new Vector3(0, 0.5f, 0);
        Vector3 endPoint = this.transform.position + new Vector3(0, 0.5f, 0);

        Vector3 direction = endPoint - startPoint;
        Vector3 crossProduct = Vector3.Cross(direction, m_playerPoint.transform.position - startPoint);

        Quaternion leftAngleEuler = Quaternion.Euler(new Vector3(0, 0, -90));
        Quaternion rightAngleEuler = Quaternion.Euler(new Vector3(0, 0, 90));

        if (crossProduct.z > 0 && m_previousDirection != leftAngleEuler)
        {
            m_ligthBeam.transform.rotation = leftAngleEuler;
        }
        else if (crossProduct.z < 0 && m_previousDirection != rightAngleEuler)
        {   
            m_ligthBeam.transform.rotation = rightAngleEuler;
        }

        m_previousDirection = m_ligthBeam.transform.rotation;
    }

    private void CheckRaycastInLightBeam()
    {
        if (m_ligthBeam.pointLightInnerRadius <= m_lineCloseDistance)
        {
            Vector3 localYAxis = m_ligthBeam.transform.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, localYAxis, m_ligthBeam.pointLightInnerRadius);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag(targetTag))
                {
                    HandleObjctInLight(hit.collider.gameObject);
                }
            }

            Debug.DrawRay(transform.position, localYAxis * m_ligthBeam.pointLightInnerRadius, Color.red);
        }
    }

    private void HandleObjctInLight(GameObject target)
    {
        //Todo 燃烧动画
        target.SetActive(false);
    }
}
