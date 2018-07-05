using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {

    public Transform target = null;
    public float lookSpeedDegreesDelta = 1.0f;


    // Use this for initialization
    void Start () {
        if (target == null)
            target = Camera.main.transform;
	}
	
	void LateUpdate () {
        if (target != null) {
            Quaternion targetRot = Quaternion.LookRotation(transform.position - target.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, lookSpeedDegreesDelta);
        }
    }
}
