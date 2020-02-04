using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public bool thresholdReached = false;
    public float currentY;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (thresholdReached == false) {
            transform.position = new Vector3(target.position.x, 0, transform.position.z);
        }
        else if (thresholdReached == true) {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}
