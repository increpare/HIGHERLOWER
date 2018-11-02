using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWiggle : MonoBehaviour {

	public float freq=1.0f;
	public float mag = 1.0f;
	public Vector3 axis = Vector3.right;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float a = Mathf.Cos(freq*Time.time)*mag;
		transform.localRotation=Quaternion.AxisAngle(axis,a);
	}
}
