using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSize : MonoBehaviour {

	public Transform spieler;
	public float mindist = 1.0f;
	public float maxdist = 2.0f;

	public GameObject audios;
	public UnityEngine.Audio.AudioMixer audioMixer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var d = Vector3.Distance(spieler.transform.position,transform.position);
		if (audios!=null){
			if (d<2&& audios.active==false){
				audios.SetActive(true);
			} 
		}
		d = (Mathf.Clamp(d,mindist,maxdist)-mindist)/(maxdist-mindist);
		transform.localScale = Vector3.one*(1-d);
		//audioMixer.SetFloat("GongVol",(1-d)*80.0f-80.0f);
	}
}
