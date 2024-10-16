using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TranslucentMirror : MonoBehaviour
{
    [SerializeField]
    private Light2D m_playerPoint;

    [SerializeField]
    private Light2D m_lightBeam;

    [Header("��Դ�;����������")]
    [SerializeField]
    private float m_maxDistance = 10f;

    [Header("��Դ�;������С����")]
    [SerializeField]
    private float m_minDistance = 0f;

    [Header("ֱ��������ʧ��λ��")]
    [SerializeField]
    private float m_lineCloseDistance = 5f;

    #region �۹����ز���
    //���ƹ��߳��� ����뾶������1fЧ�����
    [Header("��С�ڰ뾶")]
    [SerializeField]
    private float m_minInnerRadius = 0f;
    [Header("����ڰ뾶")]
    [SerializeField]
    private float m_maxInnerRadius = 9f;


    [Header("��С��뾶")]
    private float m_minOuterRadius = 1f;
    [Header("�����뾶")]
    private float m_maxOuterRadius = 10f;

    [Header("��С�ھ۹�ƽǶ�")]
    [SerializeField]
    private float m_minInnerAngle = 0f;
    [Header("����ھ۹�ƽǶ�")]
    [SerializeField]
    private float m_maxInnerAngle = 62f;

    [Header("��С��۹�ƽǶ�")]
    [SerializeField]
    private float m_minOuterAngle = 20f;
    [Header("�����۹�ƽǶ�")]
    [SerializeField]
    private float m_maxOuterAngle = 66f;
    #endregion


    private float m_currentPlayerInnerRadius;
    private float m_currentPlayerOuterRadius;

    [Header("��ҹ�Դ�Ŵ�ߴ�")]
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
            m_lightBeam.gameObject.SetActive(!isUpLightDistacne);

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

    [Header("���Ŀ��tag")]
    [SerializeField]
    private string targetTag;

    [Header("������ߵĸ���")]
    [SerializeField]
    private int rayCount;

    #region ����
    private float turnLeftRotationZ = 90f;
    private float turnRightRotationZ = -90f;

    private float mirrorOffset = 0.5f;
    #endregion

    private void Awake()
    {
        //Todo ͨ�������ȡ��ҿ��ƽ�ɫ(��ʱע��
        //m_playerPoint = PlayerInput.Instance.CurrentPlayer.GetComponent<Light2D>();
        m_lightBeam = GetComponentInChildren<Light2D>();
        m_lightBeam.lightType = Light2D.LightType.Point;

        m_currentPlayerInnerRadius = m_playerPoint.pointLightInnerRadius;
        m_currentPlayerOuterRadius = m_playerPoint.pointLightOuterRadius;
    }

    private void Update()
    {
        if (m_playerPoint != null)
        {
            float yDifferenceCalculator = Mathf.Abs(m_playerPoint.transform.position.y - m_lightBeam.transform.position.y);
            bool isYAlignHandler = yDifferenceCalculator <= xThreshold;

            m_lightBeam.gameObject.SetActive(isYAlignHandler);
            if (isYAlignHandler)
            {
                UpdateLightProperties();
                SpotlightTround();
            }

            if (m_lightBeam.gameObject.activeSelf && m_lightBeam.pointLightInnerRadius <= m_lineCloseDistance)
            {
                CheckRaycastInLightBeam();
            }
        }
    }

    private void UpdateLightProperties()
    {
        float distace = Mathf.Abs(this.transform.position.x - m_playerPoint.transform.position.x);
        float lerpT = Mathf.Clamp01((distace - m_minDistance) / (m_maxDistance - m_minDistance));

        UpdateLightRadiusAndAngle(distace, lerpT);
        UpdateLightState(distace);
    }

    private void UpdateLightRadiusAndAngle(float distance, float lerpT)
    {
        float innerRadius = Mathf.Lerp(m_maxInnerRadius, m_minInnerRadius, lerpT);
        float outerRadius = Mathf.Lerp(m_maxOuterRadius, m_minOuterRadius, lerpT);

        float innerAngle = distance < m_lineCloseDistance ? Mathf.Lerp(m_maxInnerAngle, m_minInnerAngle, lerpT) : 0;
        float outerAngle = Mathf.Lerp(m_maxOuterAngle, m_minOuterAngle, lerpT);

        m_lightBeam.pointLightInnerRadius = innerRadius;
        m_lightBeam.pointLightOuterRadius = outerRadius;

        m_lightBeam.pointLightInnerAngle = innerAngle;
        m_lightBeam.pointLightOuterAngle = outerAngle;
    }

    private void UpdateLightState(float distance)
    {
        IsUpLightDistacne = targetDistance <= distance;
        IsChangeColor = m_lightBeam.color != m_playerPoint.color;
    }

    private void ChangeLightColor()
    {
        m_lightBeam.color = m_playerPoint.color;
    }

    private void UpdatePlayerPointRadius(float innerRadius, float outerRadius)
    {
        m_playerPoint.pointLightInnerRadius = innerRadius;
        m_playerPoint.pointLightOuterRadius = outerRadius;
    }

    private void SpotlightTround()
    {
        Vector3 startPoint = this.transform.position - new Vector3(0, mirrorOffset, 0);
        Vector3 endPoint = this.transform.position + new Vector3(0, mirrorOffset, 0);

        Vector3 direction = endPoint - startPoint;
        Vector3 crossProduct = Vector3.Cross(direction, m_playerPoint.transform.position - startPoint);

        Quaternion leftAngleEuler = Quaternion.Euler(new Vector3(0, 0, turnRightRotationZ));
        Quaternion rightAngleEuler = Quaternion.Euler(new Vector3(0, 0, turnLeftRotationZ));

        if (crossProduct.z > 0 && m_previousDirection != leftAngleEuler)
        {
            m_lightBeam.transform.rotation = leftAngleEuler;
        }
        else if (crossProduct.z < 0 && m_previousDirection != rightAngleEuler)
        {   
            m_lightBeam.transform.rotation = rightAngleEuler;
        }

        m_previousDirection = m_lightBeam.transform.rotation;
    }

    private void CheckRaycastInLightBeam()
    {
        Vector3 localYAxis = m_lightBeam.transform.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, localYAxis, m_lightBeam.pointLightInnerRadius);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(targetTag))
            {
                HandleObjctInLight(hit.collider.gameObject);
            }
        }

        Debug.DrawRay(transform.position, localYAxis * m_lightBeam.pointLightInnerRadius, Color.red);
    }

    private void HandleObjctInLight(GameObject target)
    {
        //Todo ȼ�ն���
        target.SetActive(false);
    }
}