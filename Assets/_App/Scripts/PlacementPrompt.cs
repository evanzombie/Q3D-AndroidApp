using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementPrompt : MonoBehaviour {

  public GameObject groundMarker;

  private Vector3 targetPosition;
  private Quaternion targetRotation = Quaternion.identity;
  private Quaternion currentRotation = Quaternion.identity;
  private float smoothTime = 0.5f;
  private Vector3 velocity = Vector3.one;

  private Camera cam;

  // Use this for initialization
  void Start () {
      cam = Camera.main;
	}

  public void SetHit(Vector3 hitPoint) {
    targetPosition = hitPoint;
   }
	
	// Update is called once per frame
	void Update () {
      Vector3 fwd = targetPosition - cam.transform.position;
      fwd.y = 0.0f;
      fwd.Normalize();
      targetRotation = Quaternion.LookRotation(fwd, Vector3.up);

      transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
      currentRotation = Quaternion.Slerp(currentRotation, targetRotation, 0.1f);
      transform.rotation = currentRotation;
      groundMarker.SetActive(true);
	}
}
