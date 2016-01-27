using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Stats : MonoBehaviour
{
    public Transform thetransvis;
    public Transform theactualpos;
    public string name;
    float maxhealth;
    float health;
    float damage;

    List<effect> effects;

    public float speed;
    public float spellvamp;
    public Vector2 dir;

    public AudioClip music;
    public AudioSource scream;
    public AudioSource death;
    public bool isactive;
    public string state;
    public int inputlifeunity;
    public int inputlifeunityr;
    public int photonid;
    public bool isplayer;
    public bool owner;
    

	// Use this for initialization
	void Start () {
        health = inputlifeunityr;
        maxhealth = inputlifeunity;
        photonid = transform.GetComponent<PhotonView>().viewID;
        worldgen.characcess.Add(this);
        effects = new List<effect>();
        calceffects();
	}
	
	// Update is called once per frame
	void Update () {

        back:
        foreach (effect eff in effects)
        {
            if (Time.time - eff.stamp > eff.duration)
            {
                effects.Remove(eff);
                calceffects();
                goto back;
            }
        }

        if (damage >= 0)
        {
            if (damage > 1)
            {
                health = health - Time.deltaTime * damage;
                damage = damage - Time.deltaTime * damage;
            }
            else
            {
                health = health - damage;
                damage = 0;
            }
        }
        else
        {
            health = health - damage;
            damage = 0;
        }
	}

    public float coefmouv
    {
        get
        {
            return speed;
        }
    }
    public void DamageLocal(float damagee, int senderid)
    {
        damage = damage + damagee;
    }
    public void DamageExt(float damagee, int senderid)
    {
        NetworkerPhoton.thenetw.RPC("DamageRPC", PhotonTargets.All, photonid, damagee, senderid);
    }

    public void AddEffectsLocal(effect[] eff, int senderid)
    {
        for (int k = 0; k < eff.Length; k++)
        {
            eff[k].stamp = Time.time;
            effects.Add(eff[k]);
        }
        calceffects();
    }
    public void AddEffectsExt(effect[] eff, int senderid)
    {
        if (eff.Length == 0)
        {
        }
        else
        {
            string astring = fserial.saveasstring(eff);
            NetworkerPhoton.thenetw.RPC("EffectRPC", PhotonTargets.All, photonid, astring, senderid);
        }
    }

    public void calceffects()
    {
        speed = 1;
        spellvamp = 0;
        dir = new Vector2();
        foreach (effect eff in effects)
        {
            speed *= eff.speedmult;
            spellvamp += eff.spellvamp;
            dir += new Vector2(eff.xdir, eff.ydir);
        }
    }

    public float healthperc
    {
        get
        {
            if (health < 0)
            {
                return 0;
            }
            return health / (1f * maxhealth);
        }
    }

    public int maxhealthdivs
    {
        get
        {
            return Mathf.RoundToInt(maxhealth / 100f);
        }
    }
    public int maxhealthi
    {
        get
        {
            return Mathf.RoundToInt(maxhealth);
        }
    }
    public float healthpercdama
    {
        get
        {
            if (health - damage >= 0)
            {
                return (health - damage) / (1f * maxhealth);
            }
            else
            {
                return 0;
            }

        }
    }

    public Color col
    {
        get
        {
            return guitools.RGB(255, 215, 0);
        }
    }

}
