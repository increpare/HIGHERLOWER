using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Sprache : MonoBehaviour
{

    public Text buttonLabel;
    public static string sprache = "EN";

    // Use this for initialization
    void Awake()
    {
        sprache = PlayerPrefs.GetString("sprache", "EN");
        buttonLabel.text = sprache;
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        Debug.Log("A");
        switch (sprache){
            case "DE":
                sprache = "EN";
                break;
            default:
                sprache = "DE";
                break;
        }
        buttonLabel.text = sprache;

        var obs = Object.FindObjectsOfType<SpracheS>();
        foreach (var o in obs){
            o.UpdateText();
        }
    }
}