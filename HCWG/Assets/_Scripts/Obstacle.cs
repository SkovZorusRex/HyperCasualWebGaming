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
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponentInParent<PlayerController>();
        if (player != null) {
            player.Death();
            OnCollision();
        }
    }
}
