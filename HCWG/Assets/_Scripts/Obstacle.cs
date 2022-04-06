using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Obstacle : MonoBehaviour, IObstacle
{
    public MMFeedbacks hitFeedback;
    public void OnCollision()
    {
        hitFeedback.PlayFeedbacks();
        Handheld.Vibrate();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponentInParent<PlayerController>();
        if (player != null) {
            player.OnDeath();
            OnCollision();
        }
    }
}
