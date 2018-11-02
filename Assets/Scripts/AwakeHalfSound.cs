using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeHalfSound : MonoBehaviour {


	IEnumerator boop(){
		AudioSource a_s = GetComponent<AudioSource>();
		yield return new WaitForSeconds(a_s.clip.length/2);
		a_s.Play();		
	}
	
	// Use this for initialization
	void Start () {
		StartCoroutine("boop");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
