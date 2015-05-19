using UnityEngine;
using System.Collections;
using System;

public class LabelDraw : MonoBehaviour {

    public String str_Information;

    public bool enable_LabelInformation;

    float time_count;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI ()
    {
        if (enable_LabelInformation)
        {
            GUI.Label(new Rect(Screen.width - str_Information.Length * 6, Screen.height - 20, Screen.width, Screen.height), str_Information);
            if ((Time.time - time_count) > 5)
            {
                enable_LabelInformation = false;
            }
        }
    }

    void DrawLabel (String message)
    {
        str_Information = message;
        enable_LabelInformation = true;
        time_count = Time.time;   
    }
}
