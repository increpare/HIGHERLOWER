using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour {
	Toggle m_Toggle;
	void Start()
    {
        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate {
                ToggleValueChanged(m_Toggle);
            });
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
		if (m_Toggle.isOn){
			AudioListener.volume=1;
		} else {
			AudioListener.volume=0;
		}
    }
}
