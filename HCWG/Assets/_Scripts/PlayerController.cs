using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public enum ControlType
{
    HoldAndDrag,
    TapAndHold
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private ControlType m_controlType = ControlType.TapAndHold;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private GameObject m_child;
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private SplineFollower m_follower;
    [SerializeField] private PathGenerator m_path;

    [SerializeField] private float m_errorMargin = .5f;
    [SerializeField] private SceneHandler m_sceneHandler;

    private bool m_enableInput = true;

    private Touch m_touch;
    private Vector2 m_touchStartPosition, m_touchEndPosition;
    private Vector3 m_direction;
    private float m_pathSize;

    private RuntimePlatform appPlatform;
    // Start is called before the first frame update
    void Start()
    {
        appPlatform = Application.platform;
        if (m_rigidbody == null)
            m_rigidbody = m_child.GetComponent<Rigidbody>();
        m_rigidbody.useGravity = false;

        if (m_follower == null)
            m_follower = GetComponent<SplineFollower>();

        if (m_path == null)
            m_path = FindObjectOfType<PathGenerator>();
        m_pathSize = m_path.size;

        Debug.Log(Application.platform);
    }

    // Update is called once per frame
    void Update()
    {
        if (appPlatform == RuntimePlatform.WebGLPlayer || appPlatform == RuntimePlatform.WindowsEditor)
        {
            if (m_enableInput)
            {
                m_direction.y = Input.GetAxis("Horizontal");
                m_child.transform.Translate(m_direction * m_speed * Time.deltaTime);
                if (Mathf.Abs(m_child.transform.localPosition.y) > (m_pathSize / 2) + m_errorMargin)
                {
                    OnDeath();
                } 
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && m_enableInput)
            {
                m_touch = Input.GetTouch(0);

                if (m_controlType == ControlType.TapAndHold)
                {
                    if (m_touch.phase == TouchPhase.Began)
                    {
                        if (m_touch.position.x > Screen.width / 2)
                        {
                            m_direction.y = 1f;
                        }
                        else if (m_touch.position.x < Screen.width / 2)
                        {
                            m_direction.y = -1f;
                        }
                    }
                    else if (m_touch.phase == TouchPhase.Stationary)
                    {
                        m_child.transform.Translate(m_direction * m_speed * Time.deltaTime);
                        if (Mathf.Abs(m_child.transform.localPosition.y) > (m_pathSize / 2) + m_errorMargin)
                        {
                            OnDeath();
                        }
                    }
                }

                if (m_controlType == ControlType.HoldAndDrag)
                {
                    if (m_touch.phase == TouchPhase.Began)
                    {
                        m_touchStartPosition = m_touch.position;
                    }
                    else if (m_touch.phase == TouchPhase.Moved || m_touch.phase == TouchPhase.Ended || m_touch.phase == TouchPhase.Stationary)
                    {
                        m_touchEndPosition = m_touch.position;

                        float x = m_touchEndPosition.x - m_touchStartPosition.x;

                        if (Mathf.Abs(x) >= 0.1f)
                        {
                            m_direction.y = Mathf.Clamp(x, -1f, 1f);
                            m_child.transform.Translate(m_direction * m_speed * Time.deltaTime);
                            if (Mathf.Abs(m_child.transform.localPosition.y) > (m_pathSize / 2) + m_errorMargin)
                            {
                                OnDeath();
                            }
                        }
                    }
                }
            }
        }

        //Debug.Log(m_follower.GetPercent());
    }

    public void OnDeath()
    {
        Debug.Log("Death");
        m_enableInput = false;
        m_follower.follow = false;
        m_rigidbody.useGravity = true;
        m_sceneHandler.RestartCurrentLevel(5f);
    }

    public void OnReachEnd()
    {
        m_follower.follow = false;
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = true;
        m_child.transform.parent = null;
        m_child.transform.rotation = Quaternion.identity;
    }
}
