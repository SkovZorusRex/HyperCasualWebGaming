using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class LaunchFeedback : MonoBehaviour
{
    public MMFeedbacks textTurn;
    // Start is called before the first frame update
    void Start()
    {
        textTurn.PlayFeedbacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
