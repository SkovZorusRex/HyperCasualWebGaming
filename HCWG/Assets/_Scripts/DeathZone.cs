using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathZone : MonoBehaviour
{
    [SerializeField] private BoxCollider m_boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        if(m_boxCollider == null)
            m_boxCollider = GetComponent<BoxCollider>();

        m_boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponentInParent<PlayerController>();
            if (player != null)
                player.Death();
        }
    }

    private void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
