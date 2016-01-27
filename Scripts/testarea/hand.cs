using UnityEngine;
using System.Collections;

public class hand : MonoBehaviour {
    public bool isshake;
    public bool isgun;
    public bool isrecoil;
    public bool isspell;
    public Animator anim;
    public weaponvis phasegun;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        isshake = false;
        isgun = false;
        isrecoil = false;
        isspell = false;
        phasegun = transform.FindChild("root").FindChild("palm").FindChild("phasegun").GetComponent<weaponvis>();
        UpdateMe();
	}
	
	// Update is called once per frame
    void Update()
    {
        /*
        isrecoil = false;
        isshake = false;
        anim.SetBool("Recoil", isrecoil);
        anim.SetBool("IsShaking", isshake);
	*/
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && anim.GetCurrentAnimatorStateInfo(0).speed != 1)
        {
            isrecoil = false;
            isshake = false;
            isspell = false;
            anim.SetBool("Recoil", isrecoil);
            anim.SetBool("IsShaking", isshake);
            anim.SetBool("Spell", isspell); 
        }
	}
    public void UpdateMe()
    {
        anim.SetBool("IsShaking", isshake);
        anim.SetBool("HasGun", isgun);
        anim.SetBool("Recoil", isrecoil);
        anim.SetBool("Spell", isspell);
        phasegun.SetVis(isgun);
    }
}
