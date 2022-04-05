using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private SplineFollower m_follower;
    [SerializeField] private PathGenerator m_path;

    private Touch m_touch;
    private Vector2 m_touchStartPosition, m_touchEndPosition;
    private Vector2 m_direction;
    private float m_pathSize;
    // Start is called before the first frame update
    void Start()
    {
        if(m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.useGravity = false;

        if(m_follower == null)
            m_follower = GetComponent<SplineFollower>();
        
        if(m_path == null)
            m_path = FindObjectOfType<PathGenerator>();
        m_pathSize = m_path.size;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            m_touch = Input.GetTouch(0);

            if(m_touch.phase == TouchPhase.Began)
            {
                m_touchStartPosition = m_touch.position;
            }
            else if(m_touch.phase == TouchPhase.Moved || m_touch.phase == TouchPhase.Ended)
            {
                m_touchEndPosition = m_touch.position;

                float x = m_touchEndPosition.x - m_touchStartPosition.x;

                if(Mathf.Abs(x) >= 0.1f)
                {
                    m_direction.y = Mathf.Clamp(x, -1f, 1f);
                    var d = m_follower.motion.offset + new Vector2(0, m_direction.y * Time.deltaTime * m_speed);
                    if (Mathf.Abs(d.y) > (m_pathSize / 2) + 1)
                    {
                        m_follower.follow = false;
                        m_rigidbody.useGravity = true;
                    }
                    else
                    {
                        d.y = Mathf.Clamp(d.y, -m_pathSize, m_pathSize);
                        m_follower.motion.offset = d;
                    }
                }
            }
        }
    }
}
