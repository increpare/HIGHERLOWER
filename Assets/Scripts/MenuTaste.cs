using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTaste : MonoBehaviour {

	// Use this for initialization
	void onClick(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
	}
	void Start () {
		GetComponent<Button>().onClick.AddListener(onClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
