using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class snakebehave : Photon.MonoBehaviour  {
    Vector3 initpos;
    Stats stats;
    public Vector3 aim;
    public float speed;
    public float localrspeed;
    public float stamp = Time.time;
    public float delay = 8;
    float speedr;
    public Transform head;
    public Transform b0;
    public Transform b1;
    public Transform b2;
    public Transform b3;
    public Transform b4;

    Vector3 recpos = new Vector3();
    Vector3 recaim = new Vector3();
    string recstate = "";
    Quaternion reqquat = Quaternion.identity;

    List<Stats> engaged;

    public PhotonView viw;

    float ballcool = 5;
    float chargecool = 3;
    float frostcool = 25;
    float flashcool = 45;

    float ballst = Time.time;
    float chargest = Time.time;
    float frostst = Time.time;
    float flashst = Time.time;

    float ballrange = 12;
    float chargerange = 15;
    float frostrange = 9;

    public bool iscomboing = false;


    /*
     * flash
     * 
     * charge
     * 
     * frost
     * 
    */



    // Use this for initialization
    void Start()
    {
        engaged = new List<Stats>();
        viw = GetComponent<PhotonView>();
        stats = transform.GetComponent<Stats>();
        speedr = 0.005f;
        localrspeed = 30f;
        initpos = transform.position;
        aim = initpos;
	}
	
	// Update is called once per frame




	void Update () {
        if (cam.controlmove)
        {
            setvals();

            if (!photonView.isMine)
            {
                network();
            }
            else
            {
                switch (stats.state)
                {
                    case "anger":
                        behavioranger();
                        break;
                    case "attack":
                        behaviorattack();
                        break;
                    default:
                        behavioridle();
                        break;
                }
            }
            movement();
        }
	}

    public void setvals()
    {
        switch (stats.state)
        {
            case "":
                speed = 1 * stats.speed;
                localrspeed = 30f * stats.speed;
                speedr = 0.005f * stats.speed;
                delay = 8 * stats.speed;

                break;
            case "anger":
                delay = 8 / 5f * stats.speed;
                speed = 1 * 5f * stats.speed;
                localrspeed = 30f * 5f * stats.speed;
                speedr = 0.005f * 10f * stats.speed;
                break;
            case "attack":
                float maj = 2f;
                delay = 8 / 10f * stats.speed * maj;
                speed = 1 * 10f * stats.speed * maj;
                localrspeed = 30f * 10 * stats.speed * maj;
                speedr = 0.005f * 20f * stats.speed * 10 * maj;
                break;
        }
    }
    public void network()
    {
        float maj = 1;
        switch (stats.state)
        {
            case "":
                break;
            case "anger":
                maj = 5;
                break;
            case "attack":
                maj = -1;
                break;
        }
        if (maj == -1)
        {
            head.position = Vector3.Lerp(head.position, recpos, 0.5f);
            head.rotation = reqquat;
            stats.state = recstate;
        }
        else
        {
            if ((head.position - recpos).magnitude > 1 * maj)
            {
                head.position = Vector3.Lerp(head.position, recpos, 0.5f);
                head.rotation = reqquat;
                stats.state = recstate;
            }
        }
        aim = recaim;
    }

    public void behavioridle()
    {
        if (stats.healthperc < 1)
        {
            stats.state = "anger";
        }
        if ((aim - head.position).magnitude > 0.1f && Time.time - stamp < delay)
        {
        }
        else
        {
            stamp = Time.time;
            Vector3 rand = initpos + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            Vector3 cpos = Environment.convmaptoc(rand);
            int zm = Environment.heightmap[Mathf.RoundToInt(cpos.x), Mathf.RoundToInt(cpos.z)];
            aim = new Vector3(Environment.convctomap(cpos).x, Random.Range(3, 6) + zm, Environment.convctomap(cpos).z);
        }
    }

    public void behavioranger()
    {
        if (controls.hissoundsyst.music.clip != stats.music)
        {
            controls.hissoundsyst.music.clip = stats.music;
            controls.hissoundsyst.music.Play();
        }
        if (engaged.Count != 0)
        {
            string pos = "";
            if (hasflash || hasfrost)
            {
                if (hasfrost && hasflash)
                {
                    pos = "aggrofrost";
                }
                else
                {
                    pos = "away";
                }
            }
            else
            {
                pos = "aggro";
            }

            if (stats.theactualpos.GetComponent<aoe>() != null)
            {
                pos = "hold";
            }

            if (pos == "aggro")
            {
                Stats theaime = engaged[0];
                foreach (Stats astat in engaged)
                {
                    if ((stats.theactualpos.position - astat.theactualpos.position).magnitude < (stats.theactualpos.position - theaime.theactualpos.position).magnitude)
                    {
                        theaime = astat;
                    }
                }
                float dist = (stats.theactualpos.position - theaime.theactualpos.position).magnitude;
                if (dist < 0.8f * chargerange && hascharge)
                {
                    NetworkerPhoton.RPCSnakeSkill("charge",stats.theactualpos.position + (theaime.theactualpos.position - stats.theactualpos.position).normalized * chargerange,stats.photonid);
                }
                else
                {
                    if (dist < 0.8f * ballrange && hasball)
                    {
                        RaycastHit thehit = new RaycastHit();
                        if (Physics.Raycast(new Ray(transform.FindChild("b4").position, (theaime.theactualpos.position - transform.FindChild("b4").position).normalized), out thehit, ballrange * 0.8f))
                        {
                        }
                        else
                        {
                            NetworkerPhoton.RPCSnakeSkill("ball",transform.FindChild("b4").position + (theaime.theactualpos.position - transform.FindChild("b4").position).normalized * ballrange,stats.photonid);
                        }
                    }
                    else
                    {
                        aim = theaime.theactualpos.position;
                    }
                }

            }

            if (pos == "away")
            {
                Stats theaime = engaged[0];
                foreach (Stats astat in engaged)
                {
                    if (stats.healthperc * stats.maxhealthi < theaime.healthperc * theaime.maxhealthi)
                    {
                        theaime = astat;
                    }
                }
                float dist = (stats.theactualpos.position - theaime.theactualpos.position).magnitude;
                if ((aim - head.position).magnitude < 1.5f)
                {
                    Vector3 rand = theaime.theactualpos.position + (stats.theactualpos.position - theaime.theactualpos.position).normalized * 0.8f * ballrange;
                    rand += Vector3.Cross(new Vector3(0, 1, 0), (stats.theactualpos.position - theaime.theactualpos.position).normalized) * Random.Range(-1.5f, 1.5f);
                    Vector3 cpos = Environment.convmaptoc(rand);
                    int zm = Environment.heightmap[Mathf.RoundToInt(cpos.x), Mathf.RoundToInt(cpos.z)];
                    aim = new Vector3(Environment.convctomap(cpos).x, Random.Range(3, 6) + zm, Environment.convctomap(cpos).z);
                }
                if (dist < 0.8f * ballrange && hasball)
                {
                    NetworkerPhoton.RPCSnakeSkill("ball", transform.FindChild("b4").position + (theaime.theactualpos.position - transform.FindChild("b4").position).normalized * ballrange, stats.photonid);
                }
            }
            if (pos == "aggrofrost")
            {
                Stats theaime = engaged[0];
                foreach (Stats astat in engaged)
                {
                    if (stats.healthperc * stats.maxhealthi < theaime.healthperc * theaime.maxhealthi)
                    {
                        theaime = astat;
                    }
                }
                float dist = (theaime.theactualpos.position - stats.theactualpos.position).magnitude;
                Vector3 pose = theaime.theactualpos.position + (theaime.theactualpos.position - stats.theactualpos.position).normalized * 1.5f;
                Vector3 cpos = Environment.convmaptoc(pose);
                int zm = Environment.heightmap[Mathf.RoundToInt(cpos.x), Mathf.RoundToInt(cpos.z)];
                pose = new Vector3(Environment.convctomap(cpos).x, Random.Range(3, 6) + zm, Environment.convctomap(cpos).z);
                aim = pose;

                NetworkerPhoton.RPCSnakeSkill("flash", pose, stats.photonid);
                NetworkerPhoton.RPCSnakeSkill("froststorm", new Vector3(), stats.photonid);

                if (hasball)
                {
                    NetworkerPhoton.RPCSnakeSkill("ball", transform.FindChild("b4").position + (theaime.theactualpos.position - transform.FindChild("b4").position).normalized * ballrange, stats.photonid);
                }
                //if (hascharge)
                //{
                //    chargeuse(stats.theactualpos.position + (theaime.theactualpos.position - stats.theactualpos.position).normalized * chargerange);
                //}
            }

            if (pos == "hold")
            {
                Stats theaime = engaged[0];
                foreach (Stats astat in engaged)
                {
                    if ((stats.theactualpos.position - astat.theactualpos.position).magnitude < (stats.theactualpos.position - theaime.theactualpos.position).magnitude)
                    {
                        theaime = astat;
                    }
                }
                float dist = (theaime.theactualpos.position - stats.theactualpos.position).magnitude;
                if ((aim - head.position).magnitude < 1.5f)
                {
                    Vector3 rand = theaime.theactualpos.position;
                    rand += new Vector3(Random.Range(-2f, 2f),0,Random.Range(-2f, 2f));
                    Vector3 cpos = Environment.convmaptoc(rand);
                    int zm = Environment.heightmap[Mathf.RoundToInt(cpos.x), Mathf.RoundToInt(cpos.z)];
                    aim = new Vector3(Environment.convctomap(cpos).x, Random.Range(3, 6) + zm, Environment.convctomap(cpos).z);
                }
                if (dist < 0.8f * ballrange && hasball)
                {
                    NetworkerPhoton.RPCSnakeSkill("ball", transform.FindChild("b4").position + (theaime.theactualpos.position - transform.FindChild("b4").position).normalized * ballrange, stats.photonid);
                }
            }

        }
        else
        {
            foreach (Stats astat in worldgen.characcess)
            {
                if (astat.isplayer)
                {
                    engaged.Add(astat);
                }
            }
        }
    }


    public void behaviorattack()
    {
        if ((aim - head.position).magnitude < 0.5f )
        {
            stats.state = "anger";
        }
    }



    public void movement()
    {
        head.position += head.forward * speed * Time.deltaTime;
        head.rotation = Quaternion.Lerp(head.rotation, Quaternion.LookRotation((aim - head.position).normalized), speedr);
        UpdateTrans(b0);
        UpdateTrans(b1);
        UpdateTrans(b2);
        UpdateTrans(b3);
        UpdateTrans(b4);
    }

    public Transform myhead(Transform atrans)
    {
        if (atrans == b0)
        {
            return head;
        }
        if (atrans == b1)
        {
            return b0;
        }
        if (atrans == b2)
        {
            return b1;
        }
        if (atrans == b3)
        {
            return b2;
        }
        if (atrans == b4)
        {
            return b3;
        }
        return null;
    }
    public void UpdateTrans(Transform atrans)
    {
        Transform par = myhead(atrans);
        atrans.position = Vector3.Lerp(atrans.position, par.position, 0.0225f * speed);


        atrans.rotation = Quaternion.Lerp(atrans.rotation, Quaternion.LookRotation((par.position - atrans.position).normalized), speedr * 10);
        atrans.GetChild(0).localEulerAngles += new Vector3(0, 0, Time.deltaTime * localrspeed);
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector3 pos = head.position;
        Quaternion rot = head.rotation;
        Vector3 aime = aim;
        string statee = stats.state ;
        if (stream.isWriting)
        {
            stream.SendNext(pos);
            stream.SendNext(rot);
            stream.SendNext(aim);
            stream.SendNext(statee);
        }
        else
        {
            recpos = (Vector3)stream.ReceiveNext();
            reqquat = (Quaternion)stream.ReceiveNext();
            recaim = (Vector3)stream.ReceiveNext();
            recstate = (string)stream.ReceiveNext();
        }
    }

    bool hascharge
    {
        get
        {
            return (Time.time - chargest > chargecool);
        }
    }
    bool hasball
    {
        get
        {
            return (Time.time - ballst > ballcool);
        }
    }
    bool hasfrost
    {
        get
        {
            return (Time.time - frostst > frostcool);
        }
    }
    bool hasflash
    {
        get
        {
            return (Time.time - flashst > flashcool);
        }
    }

    public void chargeuse(Vector3 pos)
    {
        playaudio("snakepounce", 0);
        NetworkerPhoton.RPCSnakePart(stats.photonid,"snakepush");
        chargest = Time.time;
        aim = pos;
        stats.state = "attack";
    }
    public void balluse(Vector3 pos)
    {
        ballst = Time.time;
        float range = ballrange;
        float speed = 2000 / 90f;
        float damage = 40;
        Vector3 posini = transform.FindChild("b4").position;

        Vector3 A = posini;
        Vector3 B = pos;

        playaudio("snakelove", 1);
        NetworkerPhoton.RPCPellet(A.x, A.y, A.z, B, speed, damage, "snakelove", stats);
    }
    public void frostuse()
    {
        frostst = Time.time;
        NetworkerPhoton.RPCSnakePart(stats.photonid, "snakeaoe");
        playaudio("froststorm", 2);
        aoe anaoe = stats.theactualpos.gameObject.AddComponent<aoe>();
        anaoe.stats = stats;
        anaoe.damage = 30;
        anaoe.range = frostrange;
        anaoe.length = 6;
        anaoe.owner = true;

        effect[] arreffect = new effect[] { new effect(0.02f) };
        arreffect[0].spellvamp = 0f;
        arreffect[0].speedmult = 0.75f;
        anaoe.effects = arreffect;
    }
    public void flashuse(Vector3 pos)
    {
        flashst = Time.time;
        head.transform.rotation = Quaternion.LookRotation(pos - head.transform.position);
        playaudio("flash", 3);
        Vector3 legacy = head.transform.position;
        Vector3 dir = pos - legacy;
        head.transform.position = pos;
        for (int k = 0; k < 10; k++)
        {
            Vector3 pospart = legacy + k * (dir) / 10f;
            PhotonNetwork.Instantiate("parts/flash", pospart, Quaternion.identity, 0);
        }
    }

    public void playaudio(string sound, int num)
    {
        AudioSource child = stats.theactualpos.GetComponents<AudioSource>()[num];
        child.clip = (AudioClip)Resources.Load("skills/sound/" + sound) as AudioClip;
        child.Play();
    }
}
