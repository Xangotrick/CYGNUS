using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NetworkerPhoton : Photon.MonoBehaviour
{
    public GUISkin prettyskin;
    public GUISkin baseskin;

    public Texture2D TWhite;

    /////Server data:
    public static string regservername = "Control Simulator 2016";
    public static HostData[] hostData;
    public static string ipaddress = "192.168.10.101";
    public static float w = Screen.width;
    public static float h = Screen.height;
    public static PhotonView thenetw;
    public Transform transcube;
    public Transform hiscam;
    public Transform controler;
    public Transform sound;

    public static bool locked = false;
    public static bool lobyover = false;
    public static bool lobylocked = false;
    public static bool IG = false;
    public static int lobbyheroint = 0;

    public static List<string> log = new List<string>();
    public static string aliasstamp = System.Environment.UserName;
    public static string stringstamp = "";


    public static List<string> envpackstamp = new List<string>();


    private bool ConnectInUpdate = true;

    // Use this for initialization
    void Start()
    {
        w = Screen.width;
        h = Screen.height;
        ipaddress = Network.player.ipAddress;
        thenetw = GetComponent<PhotonView>();
        MasterServer.port = 23476;
        Network.natFacilitatorPort = 50005;
    }

    // Update is called once per frame
    void OnGUI()
    {
    }
    void Update()
    {

        if (ConnectInUpdate && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings("testmydck");
            
        }

        if (PhotonNetwork.inRoom)
        {
            if (worldgen.maininfo.tinstance == null)
            {
                GameObject obje = (GameObject)PhotonNetwork.Instantiate("prefab/" + "schar", new Vector3(0, 85, 1), Quaternion.identity, 0);
                //(achar.name);
                hiscam.parent = obje.transform.FindChild("camhook");
                controler.parent = obje.transform.FindChild("camhook");
                sound.parent = obje.transform;
                //obje.GetComponent<charnetworkbehavior>().cam = obje.transform.FindChild("camhook");
                hiscam.GetComponent<cam>().aim = obje.transform.FindChild("camhook");
                hiscam.GetComponent<cam>().activate = true;
                cam.cama = hiscam.GetComponent<Camera>();
                hiscam.localPosition = new Vector3(0, 0f, 0);
                hiscam.localEulerAngles = new Vector3();

                controler.GetComponent<controls>().chara = obje.transform;
                controler.GetComponent<controls>().RefreshLoadout();

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                worldgen.maininfo.tinstance = obje;
                worldgen.maininfo.stats = obje.transform.GetComponent<Stats>();

                worldgen.loachunks = true;

            }
        }
    }

    public void EnterServer()
    {
        Rect arect = new Rect(w - w / 12f, 0, w / 12f, h);
        float corrector = (arect.width / (w / 12f));
        GUIStyle MenuStyle = new GUIStyle(prettyskin.button);
        GUIStyle imgstyle = new GUIStyle(prettyskin.label);
        guitools.TextureStyle(MenuStyle, TWhite);
        MenuStyle = new GUIStyle(baseskin.label);
        guitools.SetStyle(MenuStyle, Color.white, Color.black, (int)Math.Round(18f));
        float ww = w / 1.2f;
        float hh = h / 1.2f;
        Rect NetworkWindow = new Rect((w - ww) / 2f, (h - hh) / 2f, ww, hh);
        GUI.Label(NetworkWindow, "", MenuStyle);


        Rect uprect = guitools.GRectRelative(0, 0, 1, 0.35f, NetworkWindow);
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(guitools.GRectRelative(0.25f, 0, 0.5f, 0.5f, uprect, 0.9f), "NAME:", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
        aliasstamp = GUI.TextField(guitools.GRectRelative(0.25f, 0.5f, 0.5f, 0.5f, uprect, 0.9f), aliasstamp, MenuStyle);
        ipaddress = GUI.TextField(guitools.GRectRelative(0.5f - 0.15f, 0.5f - 0.05f, 0.3f, 0.1f, NetworkWindow), ipaddress, MenuStyle);

        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
        if (GUI.Button(guitools.GRectRelative(0.5f - 0.15f + 0.3f, 0.5f - 0.05f - 0.1f, 0.3f, 0.1f, NetworkWindow), "Create server", MenuStyle))
        {
            StartServer();
            //PlayerJoinRPC(aliasstamp);
        }
        if (GUI.Button(guitools.GRectRelative(0.5f - 0.15f - 0.3f, 0.5f - 0.05f - 0.1f, 0.3f, 0.1f, NetworkWindow), "Refresh", MenuStyle))
        {
            StartCoroutine("RefreshHostList");
        }

        MasterServer.ipAddress = ipaddress;
        MasterServer.port = 23466;
        Network.natFacilitatorIP = ipaddress;
        Network.natFacilitatorPort = 50005;

        ww = w / 1.2f;
        hh = h / 1.2f;
        if (hostData != null)
        {
            for (int k = 0; k < hostData.Length; k++)
            {
                if (GUI.Button(guitools.GRectRelative(1 / 4f, 1 / 2f + k / (hostData.Length * 2f), 1 / 2f, 1 / (hostData.Length * 2f), NetworkWindow), hostData[k].gameName, MenuStyle))
                {
                    Network.Connect(hostData[k]);
                }
            }
        }
    }





    private void StartServer()
    {
        Network.InitializeServer(16, 25002, true);
        MasterServer.RegisterHost(regservername, aliasstamp);
    }




    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 4 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
    }


    ////
    public static void ProduceWisp(Vector3 pos)
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject abj = PhotonNetwork.Instantiate("creatures/" + "cwisp", pos, Quaternion.identity, 0);
            abj.transform.FindChild("head").FindChild("snakehead").FindChild("coll").GetComponent<bodydamage>().owner = true;
        }
    }


    /////////////////////////
    public static void RPCModTerrain(float x, float y, float z, string serial, Vector3 rot)
    {
        thenetw.RPC("ModTerrainRPC", PhotonTargets.All, x, y, z, serial, rot.x, rot.y, rot.z);
    }
    public static void RPCPellet(float x, float y, float z, Vector3 rot, float speed, float damage, string name, Stats sender)
    {
        thenetw.RPC("PelletRPC", PhotonTargets.Others, false, x, y, z, rot.x, rot.y, rot.z, speed, damage, name, sender.photonid);
        thenetw.RPC("PelletRPC", PhotonNetwork.player, true, x, y, z, rot.x, rot.y, rot.z, speed, damage, name, sender.photonid);
    }
    public static void RPCAddDeath(float x, float y, float z)
    {
        thenetw.RPC("AddDeathRPC", PhotonTargets.All, x, y, z);
    }
    public static void RPCDestroyBlock(float x, float y, float z)
    {
        thenetw.RPC("DestroyBlockRPC", PhotonTargets.All, x, y, z);
    }
    public static void RPCColoration(Vector3 a, int id)
    {
        thenetw.RPC("ColorationRPC", PhotonTargets.All, a.x, a.y, a.z, id);
    }
    public static void RPCAnim(bool shake, bool gun, bool recoil, bool spell, int id)
    {
        thenetw.RPC("AnimRPC", PhotonTargets.All, shake, gun, recoil, spell , id);
    }
    public static void RPCLight(int id)
    {
        thenetw.RPC("LightRPC", PhotonTargets.All, id);
    }
    public static void RPCSound(string astring, int id)
    {
        thenetw.RPC("SoundRPC", PhotonTargets.All, astring, id);
    }
    public static void RPCSkillSound(string astring, int id)
    {
        thenetw.RPC("SkillSoundRPC", PhotonTargets.All, astring, id);
    }
    public static void RPCSetInteract(Vector3 a, bool abool, string astring)
    {
        thenetw.RPC("SetInteractRPC", PhotonTargets.All, a.x, a.y, a.z, abool, astring);
    }
    public static void RPCSetInteractRotate(Vector3 a, Vector3 rot)
    {
        thenetw.RPC("SetInteractRotateRPC", PhotonTargets.All, a.x, a.y, a.z, rot.x, rot.y, rot.z);
    }
    public static void RPCSetColorBlock(Vector3 a, Color acol)
    {
        thenetw.RPC("SetColorBlockRPC", PhotonTargets.All, a.x, a.y, a.z, acol.r,acol.g,acol.b);
    }

    

    [PunRPC]
    public void ModTerrainRPC(float x, float y, float z, string serial, float xr, float yr, float zr)
    {
        socket newone = (socket)fserial.loadasobj(serial);
        Vector3 thevect = new Vector3(x, y, z);
        Vector3 thevect2 = new Vector3(xr, yr, zr);

        Environment.MarkBuild(thevect, newone, thevect2);
    }
    [PunRPC]
    public void AddDeathRPC(float x, float y, float z)
    {
        Vector3 thevect = new Vector3(x, y, z);

        Environment.MarkDead(thevect);
        //Environment.MarkDeadLocal(thevect);
    }
    [PunRPC]
    public void PelletRPC(bool abool, float x, float y, float z, float xa, float ya, float za, float aspeed, float adamage, string aname, int id)
    {
        Vector3 thevect = new Vector3(x, y, z);

        Vector3 thevect2 = new Vector3(xa, ya, za);

        GameObject oobj = (GameObject)Instantiate((GameObject)Resources.Load("proj/" +"s" +aname), thevect, Quaternion.identity);


        oobj.transform.GetComponent<pellet>().aim = thevect2;

        oobj.transform.GetComponent<pellet>().owner = abool;
        oobj.transform.GetComponent<pellet>().damage = adamage;
        oobj.transform.GetComponent<pellet>().speed = aspeed;
        oobj.transform.GetComponent<pellet>().id = id;
    }


    [PunRPC]
    public void DestroyBlockRPC(float x, float y, float z)
    {
        Vector3 vect = new Vector3(x,y,z);
        int[] pos = new int[] { Mathf.RoundToInt(Environment.convmaptoc(vect).x), Mathf.RoundToInt(Environment.convmaptoc(vect).z) };
        int[] chunkcoords = Environment.findchunk(pos[0], pos[1]);
        Vector3 thevect = new Vector3(x, y, z);
        Environment.loadmap[chunkcoords[0], chunkcoords[1]].removeat(pos[0], Mathf.RoundToInt(Environment.convmaptoc(vect).y), pos[1]);
    }
    [PunRPC]
    public void ColorationRPC(float x, float y, float z, int id)
    {
        foreach (Transform atrans in worldgen.playerbase)
        {
            if (id == atrans.GetComponent<charnetworkbehavior>().viw.viewID)
            {
                Material mat = new Material(atrans.GetComponent<MeshRenderer>().materials[0]);
                atrans.GetComponent<MeshRenderer>().materials[0] = mat;
                Vector3 color = new Vector3(x, y, z);
                Transform child = atrans.FindChild("camhook");
                color = color / (color.magnitude);
                child.GetComponentInChildren<Light>().color = new Color(x, y, z, 1);
                child.GetComponentInChildren<Light>().color = new Color(1, 1, 1, 1);
                atrans.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor",  new Color(x , y, z, 1) * 4);

            }
        }
    }
    [PunRPC]
    public void AnimRPC(bool shake, bool gun, bool recoil, bool spell, int id)
    {
        foreach (Transform atrans in worldgen.playerbase)
        {
            if (id == atrans.GetComponent<charnetworkbehavior>().viw.viewID)
            {
                hand child = atrans.FindChild("camhook").FindChild("handskel").GetComponent<hand>();
                child.isshake = shake;
                child.isgun = gun;
                child.isrecoil = recoil;
                child.isspell = spell;
                child.UpdateMe();

            }
        }
    }


    [PunRPC]
    public void SoundRPC(string sound, int id)
    {
        /*
        foreach (Transform atrans in worldgen.playerbase)
        {
            if (id == atrans.GetComponent<charnetworkbehavior>().viw.viewID)
            {
                hand child = atrans.FindChild("camhook").FindChild("handskel").GetComponent<hand>();
                child.isshake = shake;
                child.isgun = gun;
                child.isrecoil = recoil;
                child.UpdateMe();

            }
        }*/
        controls.hissoundsyst.stoaudio(sound).Play();
    }
    [PunRPC]
    public void SkillSoundRPC(string sound, int id)
    {
        
        foreach (Transform atrans in worldgen.playerbase)
        {
            if (id == atrans.GetComponent<charnetworkbehavior>().viw.viewID)
            {
                AudioSource child = atrans.FindChild("skillsource").GetComponent<AudioSource>();
                child.clip = (AudioClip)Resources.Load("skills/sound/" + sound) as AudioClip;
                child.Play();
            }
        }
    }

    [PunRPC]
    public void LightRPC(int id)
    {
        foreach (Transform atrans in worldgen.playerbase)
        {
            if (id == atrans.GetComponent<charnetworkbehavior>().viw.viewID)
            {
                Transform child = atrans.FindChild("camhook");
                child.GetComponentInChildren<Light>().enabled = !child.GetComponentInChildren<Light>().enabled;
            }
        }
    }

    [PunRPC]
    public void SetInteractRPC(float x, float y, float z, bool tbool, string astring)
    {
        Vector3 thevect = new Vector3(x, y, z);

        socketIG thesock = Environment.getmapwmap(thevect);

        if (!tbool)
        {
            List<sprop> alist = (List<sprop>)fserial.loadasobj(astring);
            thesock.asock.props = new List<sprop>();
            foreach (sprop aprop in alist)
            {
                thesock.asock.props.Add(sprop.copysprop(aprop));
                if(aprop is sprop.invs)
                {   
                    sprop.invs inv = (sprop.invs)aprop;
                    for(int k = 0; k < inv.aninv.inv.Length; k++)
                    {
                        thesock.asock.getinvs.aninv.inv[k] = new Inventory.InvItem(inv.aninv.inv[k].socket, inv.aninv.inv[k].num);
                    }
                }
            }
        }

        thesock.asock.getinteract.abool = tbool;

        //if (thesock.asock.name == "chest01")
        //{
        //    thesock.instance.GetComponent<chesthead>().open = tbool;
        // }
        //Environment.MarkDeadLocal(thevect);
    }
    [PunRPC]
    public void SetInteractRotateRPC(float x, float y, float z, float xr, float yr, float zr)
    {
        Vector3 thevect = new Vector3(x, y, z);

        socketIG thesock = Environment.getmapwmap(thevect);

        if (thesock != null)
        {
            if (thesock.instance != null)
            {
                thesock.instance.transform.eulerAngles = new Vector3(xr, yr, zr);
                thesock.asock.getinteract.abool = false;
            }
        }
    }

    [PunRPC]
    public void SetColorBlockRPC(float x, float y, float z, float xc, float yc, float zc)
    {
        Color thecol = new Color(xc, yc, zc);
        Vector3 thevect = new Vector3(x, y, z);

        socketIG thesock = Environment.getmapwmap(thevect);

        if (thesock.instance != null)
        {
            MeshRenderer mesh = thesock.instance.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                for (int k = 0; k < mesh.materials.Length; k++)
                {
                    mesh.materials[k].color = thecol;
                }
            }
            else
            {
                mesh = thesock.instance.GetComponentInChildren<MeshRenderer>();
                if (mesh != null)
                {
                    for (int k = 0; k < mesh.materials.Length; k++)
                    {
                        mesh.materials[k].color = thecol;
                    }
                }
            }
        }
    }


    [PunRPC]
    void DamageRPC(int id, float damagee, int senderid)
    {
        foreach (Stats astat in worldgen.characcess)
        {
            if (astat.photonid == id)
            {
                astat.DamageLocal(damagee, senderid);
            }
        }
    }

    [PunRPC]
    void EffectRPC(int id, string serial, int senderid)
    {
        foreach (Stats astat in worldgen.characcess)
        {
            if (astat.photonid == id)
            {
                effect[] alist = (effect[])fserial.loadasobj(serial);

                astat.AddEffectsLocal(alist, senderid);
            }
        }
    }


    public static void RPCSnakePart(int id, string part)
    {
        thenetw.RPC("SnakePartRPC", PhotonTargets.All, id, part);
    }
    [PunRPC]
    public void SnakePartRPC(int id,string part)
    {
        foreach (Stats astat in worldgen.characcess)
        {
            if (astat.photonid == id)
            {
                GameObject game = (GameObject)Instantiate((GameObject)Resources.Load("parts/"+part), astat.transform.FindChild("head").position,astat.transform.FindChild("head").rotation);
                game.transform.parent = astat.transform.FindChild("head");
            }
        }
    }


    public static void RPCSkillSoundStatees(string astring, int id, int num)
    {
        thenetw.RPC("SkillSoundStatesRPC", PhotonTargets.All, astring, id, num);
    }
    [PunRPC]
    public void SkillSoundStatesRPC(string sound, int id, int num)
    {
        foreach (Stats astat in worldgen.characcess)
        {
            if (astat.photonid == id)
            {
                AudioSource child = astat.theactualpos.GetComponents<AudioSource>()[num];
                child.clip = (AudioClip)Resources.Load("skills/sound/" + sound) as AudioClip;
                child.Play();
            }
        }
    }

    public static void RPCSnakeSkill(string skill, Vector3 vect, int id)
    {
        thenetw.RPC("SnakeSkillRPC", PhotonTargets.All, skill, vect.x, vect.y, vect.z, id);
    }
    [PunRPC]
    public void SnakeSkillRPC(string skill, float x, float y, float z, int id)
    {
        foreach (Stats astat in worldgen.characcess)
        {
            if (astat.photonid == id)
            {
                snakebehave snake = astat.transform.GetComponent<snakebehave>();
                Vector3 pos = new Vector3(x, y, z);
                switch (skill)
                {
                    case "charge":
                        snake.chargeuse(pos);
                        break;
                    case "ball":
                        snake.balluse(pos);
                        break;
                    case "flash":
                        snake.flashuse(pos);
                        break;
                    case "froststorm":
                        snake.frostuse();
                        break;
                }
            }
        }
    }
}
