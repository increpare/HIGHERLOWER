using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

    public AudioSource asource;
    public AudioClip boop;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    public float gongvol=0.1f;
    public void TaskOnClick(){
        asource.PlayOneShot(boop,gongvol);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    void Update(){
        if (Input.GetButtonDown("action")){
            TaskOnClick();
        }
    }
}
