using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioScale : MonoBehaviour
{
	public float sensitivity 	= 100.0f;
	public float baseScale 		= 1.0f;
	public float smoothing 		= 0.05f;

	private float targetScale 	= 1.0f;
	private Vector3 velocity 	= Vector3.zero;
	private float[] spectrum 	= new float[128];
	private Vector3 target 		= Vector3.zero;

	void Update()
	{
		AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

		for (int i = 1; i < spectrum.Length - 1; i++)
		{
			targetScale = spectrum[i];
		}

		target = (Vector3.one * baseScale) + (Vector3.one * (targetScale * sensitivity));
		transform.localScale =  Vector3.SmoothDamp(transform.localScale, target, ref velocity, smoothing);
	}
}