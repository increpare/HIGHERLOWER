using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSize : MonoBehaviour {

	public Transform spieler;
	public float mindist = 1.0f;
	public float maxdist = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var d = Vector3.Distance(spieler.transform.position,transform.position);
		d = (Mathf.Clamp(d,mindist,maxdist)-mindist)/(maxdist-mindist);
		transform.localScale = Vector3.one*(1-d);
	}
}
