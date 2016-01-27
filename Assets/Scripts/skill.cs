using UnityEngine;
using System.Reflection;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class skill : MonoBehaviour {
    public static List<skill> act { get; set; }
    public static List<skill> pas { get; set; }
    public string name { get; set; }
    public string dataname { get; set; }
    public bool isact { get; set; }
    public int cooldown { get; set; }

    public int anintval { get; set; }



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public skill(bool isacte, string datname, string rname, int cooldowne, string desc ="")
    {
        name = rname;
        dataname = datname;
        isact = isacte;
        cooldown = cooldowne;
        anintval = 0;
    }

    public static void CreatePas()
    {
        bool abool = false;
        pas = new List<skill>();
        pas.Add(new skill(abool, "arcanemastery", "Arcane Mastery", 0));
        pas.Add(new skill(abool, "windghost", "Wind Ghost", 0));
    }

    public static skill getpasbystring(string astring)
    {
        foreach (skill askill in pas)
        {
            if (askill.name == astring)
            {
                return askill;
            }
        }
        return pas[0];
    }


    public static void CreateAct()
    {
        bool abool = true;
        act = new List<skill>();
        act.Add(new skill(abool, "overload", "Overload", 4));
        act.Add(new skill(abool, "runeprison", "Rune Prison", 14));
        act.Add(new skill(abool, "flash", "Flash", 15));
        act.Add(new skill(abool, "desperatepower", "Desperate Power", 40 - 15));
        act.Add(new skill(abool, "unbreakablewill", "Unbreakable Will", 0));
        act.Add(new skill(abool, "spellflux", "Spell Flux", 7));
        act.Add(new skill(abool, "charge", "Charge",0));
        act.Add(new skill(abool, "pulverize", "Pulverize",0));
        act.Add(new skill(abool, "regrowth", "Regrowth",0));
    }

    
    public Texture2D text
    {
        get
        {
            return (Texture2D)Resources.Load("skills/" + dataname) as Texture2D;
        }
    }
    public AudioClip audio
    {
        get
        {
            return (AudioClip)Resources.Load("skills/sound/" + dataname) as AudioClip;
        }
    }

    public MethodInfo method
    {
        get
        {
            return this.GetType().GetMethod(dataname);
        }
    }
    public void runmethod(object[] param = null)
    {
        method.Invoke(this,param);
    }

    public void overload()
    {
        float range = 12;
        float speed = 1400 / 90f;
        float damage = 160;
        Vector3 pos = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel").FindChild("root").FindChild("palm").transform.position;

        Vector3 A = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * 0;
        Vector3 B = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * (0 + range);

        NetworkerPhoton.RPCPellet(A.x, A.y, A.z, B, speed, damage,"overload",worldgen.maininfo.stats);

        GameObject game = (GameObject)Instantiate((GameObject)Resources.Load("parts/ryze"), pos, Quaternion.identity);
        game.transform.parent = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel");
    }
    public void runeprison()
    {
        float range = 9;
        float speed = 2500 / 90f;
        float damage = 160;
        Vector3 pos = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel").FindChild("root").FindChild("palm").transform.position;

        Vector3 A = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * 0;
        Vector3 B = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * (0 + range);

        NetworkerPhoton.RPCPellet(A.x, A.y, A.z, B, speed, damage, "runeprison", worldgen.maininfo.stats);
        GameObject game = (GameObject)Instantiate((GameObject)Resources.Load("parts/ryze"), pos, Quaternion.identity);
        game.transform.parent = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel");
    }
    public void spellflux()
    {
        Vector3 pos = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel").FindChild("root").FindChild("palm").transform.position;
        worldgen.maininfo.stats.DamageExt(-200, worldgen.maininfo.stats.photonid);
        effect[] arreffect = new effect[] { new effect(3) };
        arreffect[0].spellvamp = -2f;
        arreffect[0].speedmult = 0.25f;
        worldgen.maininfo.stats.AddEffectsLocal(arreffect, worldgen.maininfo.stats.photonid);
        GameObject game = (GameObject)Instantiate((GameObject)Resources.Load("parts/ryze"), pos, Quaternion.identity);
        game.transform.parent = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel");
    }
    public void desperatepower()
    {
        Vector3 pos = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel").FindChild("root").FindChild("palm").transform.position;
        effect[] arreffect = new effect[]{new effect(6)};
        arreffect[0].spellvamp = 0.25f;
        arreffect[0].speedmult = 1.5f;
        worldgen.maininfo.stats.AddEffectsLocal(arreffect, worldgen.maininfo.stats.photonid);
        GameObject game = (GameObject)Instantiate((GameObject)Resources.Load("parts/ryzeult"), pos, Quaternion.identity);
        game.transform.parent = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel");
    }
    public void charge()
    {
    }
    public void pulverize()
    {
    }
    public void regrowth()
    {
    }
    public void unbreakablewill()
    {
    }
    public void flash()
    {
        float range = 12;
        Vector3 pos = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel").FindChild("root").FindChild("palm").transform.position;

        Vector3 A = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * 0;
        Vector3 B = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * (0 + range);
        RaycastHit thehit = new RaycastHit();
        if (Physics.Raycast(new Ray(A, (B-A).normalized), out thehit, range))
        {
            worldgen.maininfo.tinstance.transform.position = thehit.point + new Vector3(0, 2, 0) - 1 * (B - A).normalized;
        }
    }
}
