using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick(){
        Application.LoadLevel("MainScene");
    }
}
