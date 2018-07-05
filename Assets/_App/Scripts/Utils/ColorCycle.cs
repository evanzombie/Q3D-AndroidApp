using UnityEngine;
using System.Collections;

public class ColorCycle : MonoBehaviour {

    public float cycleDuration = 1.0f;
    public Color[] colors;

    private int current = 0;
    private int next = 1;
    private float currentT = 0.0f;
    private Renderer render;


    // Use this for initialization
    void Start () {
        render = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {

        currentT += Time.deltaTime;
        float t = currentT / cycleDuration;
        
        render.material.color = Color.Lerp(colors[current], colors[next], t);

        if (t > 1.0f) {

            current = next;
            next++;
            if (next >= colors.Length)
                next = 0;

            currentT = 0.0f;
        }
    }
}
