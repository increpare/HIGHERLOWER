using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Sprache : MonoBehaviour
{

    public Text buttonLabel;
    public static string sprache = "DE";

    // Use this for initialization
    void Start()
    {
        sprache = PlayerPrefs.GetString("sprache", "DE");
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