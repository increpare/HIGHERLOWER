using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour {

	private int index = 0;
	public AudioClip blip;
	public static string sprache="en";
	private bool audiomuted;

	private UnityEngine.UI.Text uitext;

	void Awake(){		
        sprache = PlayerPrefs.GetString("sprache", "en");
		audiomuted = AudioListener.volume>0.5f;
	}
	// Use this for initialization
	void Start () {
		uitext = GetComponent<UnityEngine.UI.Text>();		
	}
	
	float t=0;
	string S(string de, string en){
		if (sprache=="de") 
			return de;
		else	
			return en;
	}
	void doBlip(){		
		AudioSource.PlayClipAtPoint(blip,Vector3.zero,0.05f);
	}
	void wechselSprache(){
		if (sprache=="en"){
			sprache="de";
		} else {
			sprache="en";
		}
		var obs = Object.FindObjectsOfType<SpracheS>();
        foreach (var o in obs){
            o.UpdateText();
        }
	}

	void lockCursor(){		
        Cursor.visible=false;
	}	

	bool animiert = false;

	IEnumerator startanim(){
		animiert=true;
		for (var i=0;i<5;i++){
			yield return new WaitForSeconds(0.1f);
			index= (i%2==0)?-1:0;
		}
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene");
	}

	IEnumerator exitanim(){
		animiert=true;
		for (var i=0;i<5;i++){
			yield return new WaitForSeconds(0.1f);
			index= (i%2==0)?-1:3;
		}
		Application.Quit();		
	}

	void wechselAudio(){
		if (AudioListener.volume>0.5f){
			AudioListener.volume=0;
		} else {
			AudioListener.volume=1;
		}
		audiomuted = AudioListener.volume>0.5f;
	}
	// Update is called once per frame
	void Update () {
		t+=Time.deltaTime;

		if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1)){			
			lockCursor();
		}

		if (!animiert){
			if (Input.GetButtonDown("down")&& index<3){
				lockCursor();
				index++;
				doBlip();
			}
			if (Input.GetButtonDown("up")&& index>0){
				lockCursor();
				index--;
				doBlip();
			}
			if (Input.GetButtonDown("action")){
				lockCursor();
				if (index==0){
					StartCoroutine(startanim());
					doBlip();
				} else if (index==3){
					StartCoroutine(exitanim());
					doBlip();
				}			
			}

			if (Input.GetButtonDown("left")||Input.GetButtonDown("right")||Input.GetButtonDown("action")){
				lockCursor();
				if (index==1){
					wechselSprache();
					doBlip();
				}
				if (index==2){
					wechselAudio();
					doBlip();
				}
			}
		}

		var s = "";
		if (index==0){
			s+="[ Start ]\n";
		} else {
			s+="Start\n";
		}
		if (index==1){
			s+="[ "+S("Sprache (Detusch)","Language (English)")+" ]\n";
		} else {
			s+=S("Sprache (Detusch)","Language (English)")+"\n";
		}
		if (!audiomuted){
			if (index==2){
				s+="[ "+S("Audio (Stumm)","Audio (Mute)")+" ]\n";
			} else {
				s+=S("Audio (Stumm)","Audio (Mute)")+"\n";
			}
		} else {
			if (index==2){
				s+="[ "+S("Audio (Ein)","Audio (On)")+" ]\n";
			} else {
				s+=S("Audio (Ein)","Audio (On)")+"\n";
			}
		}
		if (index==3){
			s+="[ "+S("Beenden","Quit")+" ]\n";
		} else {
			s+=S("Beenden","Quit");
		}

		uitext.text=s;
	}
}
