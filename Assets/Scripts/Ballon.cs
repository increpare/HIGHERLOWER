using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour {

    public float mag=1;
    public float freq=1;

    private Vector3 start;
	// Use this for initialization
	void Start () {
        start = transform.localPosition;
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        var px = Mathf.PerlinNoise(freq * Time.timeSinceLevelLoad, 0);
        var py = Mathf.PerlinNoise(0,freq * Time.timeSinceLevelLoad);
        var x = start.x + mag * 2 * (px - 0.5f);
        var y = start.y + mag * 2 * (py - 0.5f);
        transform.localPosition = new Vector3(x,y, start.z);
	}
}
