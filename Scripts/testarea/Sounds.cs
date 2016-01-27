using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {
    public AudioSource music;

    public AudioClip Mmorning;
    public AudioClip Mevening;
    public AudioSource SWeld;
    public AudioSource SWTap;
    public AudioSource SScrew;
    public AudioSource SClang;
    public AudioSource SOxLeak;
    public AudioSource SJetPack;
    public AudioSource Sack;
    public AudioSource Scancel;
    public AudioSource Shoot;
    public AudioSource SHon;

	// Use this for initialization
	void Start () {
        music = transform.FindChild("Music").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public AudioSource stoaudio(string astring)
    {
        switch(astring)
        {
            case "weld":
                return SWeld;
            case "wtap":
                return SWTap;
            case "screw":
                return SScrew;
            case "mclang":
                return SClang;
            case "oxleak":
                return SOxLeak;
            case "jetpack":
                return SJetPack;
            case "ack":
                return Sack;
            case "cancel":
                return Scancel;
            case "Hon":
                return SHon;
            case "shoot":
                return Shoot;
        }
        return SWTap;
    }
}
