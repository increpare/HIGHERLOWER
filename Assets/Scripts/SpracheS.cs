using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpracheS : MonoBehaviour {

    public string de;
    public string en;

    public void UpdateText()
    {
        var t = this.GetComponent<Text>();
        t.text = Sprache.sprache == "DE" ? de : en;
    }

    // Use this for initialization
	void Awake () {
        UpdateText();
	}
}
