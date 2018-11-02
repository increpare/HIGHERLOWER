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

    public void TaskOnClick(){
        asource.PlayOneShot(boop,0.2f);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("MeshGen");
    }
}
