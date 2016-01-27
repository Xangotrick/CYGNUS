using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class menuscreen : MonoBehaviour {
    public Transform screen;
    public GUISkin prettyskin;

    float w;
    float h;
    float initgame;

	// Use this for initialization
	void Start () {
        w = Screen.width;
        h = Screen.height;
        initgame = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        screen.transform.Rotate(new Vector3(0, 0, 0.01f));
    }

    void OnGUI()
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);
        guitools.SetStyle(MenuStyle, Color.white, Color.black, (int)Math.Round(18f));

        Rect arect = new Rect(w * 0.25f, h*3 / 4f, w*0.5f, h / 4f);
        Rect therect = guitools.GRectRelative(0.25f, 0, 0.5f, 1, arect);

        if(GUI.Button(guitools.GRectRelative(0, 0.05f, 1, 0.4f, therect), "START", MenuStyle))
        {
            Application.LoadLevel("testarea");
        }
        if(GUI.Button(guitools.GRectRelative(0, 0.55f, 1, 0.4f, therect), "EXIT", MenuStyle))
        {
            Application.Quit();
        }
        if (Time.time - initgame < 2)
        {
            guitools.SetStyle(MenuStyle, Color.black, Color.black, (int)Math.Round(18f));
            GUI.Label(new Rect(0, 0, w, h), "", MenuStyle);
        }
        else
        {
            if (Time.time - initgame < 8)
            {
                print(6 - (Time.time - initgame - 2));
                guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 255 * (6 - (Time.time - initgame - 2))/6f), Color.black, (int)Math.Round(18f));
                GUI.Label(new Rect(0, 0, w, h), "", MenuStyle);
            }
        }
    }
}
