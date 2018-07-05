using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTooClose : MonoBehaviour {

    private float threshhold = 0.25f;
    private FadeObjectInOut fader = null;
    private bool faded = false;


	// Use this for initialization
	void Start () {
        fader = GetComponent<FadeObjectInOut>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Camera.main == null)
            return;

        float distToCam = (transform.position - Camera.main.transform.position).magnitude;

        if (distToCam < threshhold) {
            if (fader != null && !faded)
                fader.FadeOut();
                faded = true;
        } else {
            if (faded) {
                fader.FadeIn();
                faded = false;
            }
        }
	}
}
