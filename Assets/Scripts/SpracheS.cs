using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpracheS : MonoBehaviour {

    [TextArea]
    public string de;

    [TextArea]
    public string en;

    public void UpdateText()
    {
        var s = TitleMenu.sprache == "de" ? de : en;
        var t = this.GetComponent<Text>();
        if (t!=null){
            t.text = s;
        }
        var u = this.GetComponent<TextMesh>();
        if (u!=null){
            u.text = s;
        }
    }

    // Use this for initialization
	void Awake () {
        UpdateText();
	}
}
