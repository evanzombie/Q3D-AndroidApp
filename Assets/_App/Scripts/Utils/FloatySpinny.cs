using UnityEngine;
using System.Collections;

public class FloatySpinny : MonoBehaviour {

    public Vector3 rotation;
    public float hoverSpeed;
    public float hoverHeight;
    public bool adjustForParent = true;
    public bool useLateUpdate = false;
    public int pauseBetweenMoves = 0;

    private float speedRandom = 0.75f;
    private Vector3 mStartPos;
	private Vector3 mCurrentPos;
    private bool mCurrentActive;
    private int onUpdate;
    private bool paused = false;
    private float internalTime = 0.0f;
	private Rigidbody mRigidbody = null;

    // Use this for initialization
    void Start () {
		mRigidbody = GetComponent<Rigidbody>();
        mStartPos = transform.position;
        hoverSpeed += hoverSpeed * (Random.value * speedRandom);
    }

    // Update is called once per frame
    void FloatySpin () {
		
        onUpdate++;
        if (pauseBetweenMoves > 0 && pauseBetweenMoves == onUpdate)
        {
            onUpdate = 0;
            paused = !paused;
        }

        if (!paused) {
            if (pauseBetweenMoves != 0)
            {
                internalTime += 1.0f / 60.0f;
            } else {
                internalTime = Time.time;
            }

    		Vector3 diff;

	        // capture the delta in case we moved outside of this routine
		    if (adjustForParent && mCurrentActive)
            {
                diff = transform.position - mCurrentPos;
                mStartPos += diff;
            }
		
            if (hoverSpeed > 0.0f)
            {
                Vector3 pos = transform.position;
                pos.y = mStartPos.y + (Mathf.Sin(Time.time * hoverSpeed) * hoverHeight);
				mCurrentPos = pos;
				if (mRigidbody != null)
					mRigidbody.MovePosition(pos);
				
            }

            if (rotation.sqrMagnitude > 0.0f)
            {
				if (mRigidbody != null)
                {
                    Quaternion deltaRotation = Quaternion.Euler(rotation * Time.deltaTime);
					mRigidbody.MoveRotation(mRigidbody.rotation * deltaRotation);
                }
                else
                {
                    transform.Rotate(rotation * Time.deltaTime);
                }
		    }
       	    mCurrentActive = true;
        }
    }

    void FixedUpdate() {
        if (!useLateUpdate)
        {
            FloatySpin();
        }
    }

    void LateUpdate() {
        if (useLateUpdate)
        {
            FloatySpin();
        }
    }
}
