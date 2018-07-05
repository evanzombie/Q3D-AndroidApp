using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCamera : MonoBehaviour {

  public float speed = 0.75f;
  public Vector3 targetOffset = new Vector3(0.25f, 0.25f, 0.5f);
  private Vector3 targetPosition;
  //private Vector3 currentRotation;
  private Vector3 velocity = Vector3.zero;
  private Camera cam;

  // Use this for initialization
  void Start () {
    cam = Camera.main;
  }
    

  // Update is called once per frame
  void Update () {
    Transform camXForm = cam.gameObject.transform;
    targetPosition = camXForm.position;
    targetPosition += camXForm.forward * targetOffset.z;
    targetPosition += camXForm.up * targetOffset.y;
    targetPosition += camXForm.right * targetOffset.x;
    /**
    Vector3 fwd = targetPosition - cam.transform.position;
    fwd.y = 0.0f;
    fwd.Normalize();
    targetRotation = Quaternion.LookRotation(fwd, Vector3.up);
    currentRotation = Quaternion.Slerp(currentRotation, targetRotation, 0.1f);
    transform.rotation = currentRotation;
    **/

    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, speed);
  }
}
