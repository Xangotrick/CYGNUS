using UnityEngine;
using System.Collections;

public class controls : MonoBehaviour {
    public Transform chara;
    public Transform transcube;
    public static Transform hiscam;
    public static Sounds hissoundsyst;
    Light hislight;
    CharacterController cont;
    public float speed;
	float acc;
    bool isonfloor;
    float lastz;
    float blastz;
    Vector3 lastMouseCoordinate = new Vector3();
    public static int seltoolbar;
    public static Inventory.InvItem[] toolbar;
    public static Vector3 aimedpoint;
    public static float downpresstime;
    public static float downpressmax;
    public static bool buttondown;
    public static Color randforcolor = Color.red;
    public static bool isfightingmode;
    public bool isonline;
    BoxCollider thefoot;

    public static socket[, ,] socks;
	// Use this for initialization
	void Start () {
        isonline = false;
        downpressmax = 0.67f;
        buttondown = false;
        aimedpoint = new Vector3();
        seltoolbar = 0;
        toolbar = new Inventory.InvItem[10];
        for (int k = 0; k < toolbar.Length; k++)
        {
            toolbar[k] = new Inventory.InvItem();
        }

        isfightingmode = true;

        isonfloor = false;
		acc = -9.81f;
        blastz = 0;
        lastz = 1;
        isonfloor = false;
	}

    public void RefreshLoadout()
    {
        hissoundsyst = chara.GetComponentInChildren<Sounds>();
        gui.soundsys = hissoundsyst;
        hiscam = chara.FindChild("camhook");
        hislight = chara.GetComponentInChildren<Light>();
        cont = chara.GetComponent<CharacterController>();
        isonline = true;
    }

	// Update is called once per frame
    void Update()
    {
        if (isonline)
        {
            isonfloor = (Mathf.Abs(blastz - lastz) == 0);
            Vector3 movedir = new Vector3();
            ////Mouse
            if (cam.controlmouse)
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        seltoolbar += 1;
                        seltoolbar = seltoolbar % 10;
                    }
                    else
                    {
                        seltoolbar += 9;
                        seltoolbar = seltoolbar % 10;
                    }
                    if (selsock.isgun)
                    {
                        NetworkerPhoton.RPCAnim(false, true, false, false, worldgen.maininfo.photonid);
                    }
                    else
                    {
                        NetworkerPhoton.RPCAnim(false, false, false, false, worldgen.maininfo.photonid);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (aimedpoint.y > -800)
                    {
                        if (selsock.isobject)
                        {
                        }
                        else
                        {
                            if (selsock.name != "" && toolbar[seltoolbar].num != 0)
                            {
                                Vector3 theangle = hiscam.eulerAngles;
                                theangle = new Vector3(theangle.x / 90f, theangle.y / 90f, theangle.z / 90f);
                                theangle = new Vector3(Mathf.RoundToInt(theangle.x), Mathf.RoundToInt(theangle.y), Mathf.RoundToInt(theangle.z));
                                theangle = 90 * theangle;
                                if (PhotonNetwork.inRoom)
                                {
                                    string thestring = fserial.saveasstring(selsock);
                                    NetworkerPhoton.RPCModTerrain(aimedpoint.x, aimedpoint.y, aimedpoint.z, thestring, theangle);
                                }
                                else
                                {
                                    Environment.MarkBuild(aimedpoint, selsock, theangle);
                                }
                                toolbar[seltoolbar].num--;
                                if (toolbar[seltoolbar].num == 0)
                                {
                                    toolbar[seltoolbar] = new Inventory.InvItem();
                                }
                                hissoundsyst.stoaudio(selsock.sound).Play();
                            }
                            else
                            {
                                buttondown = true;
                                downpresstime = Time.time;
                            }
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    buttondown = false;
                    hissoundsyst.SWeld.Stop();
                }
                if (Input.GetMouseButton(0))
                {
                    if (selsock.isgun)
                    {
                        if (Time.time - selsock.getgun.stamp > selsock.getgun.reset)
                        {
                            selsock.getgun.stamp = Time.time;
                            NetworkerPhoton.RPCSound("shoot", worldgen.maininfo.photonid);
                            NetworkerPhoton.RPCAnim(false, true, true, false, worldgen.maininfo.photonid);

                            Vector3 pos = worldgen.maininfo.tinstance.transform.FindChild("camhook").FindChild("handskel").transform.position;

                            Vector3 A = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * 2;
                            Vector3 B = pos + worldgen.maininfo.tinstance.transform.FindChild("camhook").forward * (2 + selsock.getgun.range);

                            NetworkerPhoton.RPCPellet(A.x, A.y, A.z, B, 50, 150, "pellet", worldgen.maininfo.stats);
                        }
                    }
                    if (aimedpoint.y > -800)
                    {
                        if (selsock.name != "")
                        {
                        }
                        else
                        {
                            if (selsock.isobject)
                            {
                            }
                            else
                            {
                                if (!hissoundsyst.SWeld.isPlaying)
                                {
                                    hissoundsyst.SWeld.Play();
                                }
                                if (Time.time - downpresstime > downpressmax)
                                {
                                    ///SOLVE!
                                    if (Environment.getmapwmap(aimedpoint) != null)
                                    {
                                        if (Environment.getmapwmap(aimedpoint).asock.breakable)
                                        {
                                            if (PhotonNetwork.inRoom)
                                            {
                                                NetworkerPhoton.RPCAddDeath(aimedpoint.x, aimedpoint.y, aimedpoint.z);
                                            }
                                            else
                                            {
                                                Environment.MarkDead(aimedpoint);
                                            }
                                            if (Environment.getmapwmap(aimedpoint).asock.mineable)
                                            {
                                                gui.thelistofcaptures.Add("+1 " + Environment.getmapwmap(aimedpoint).asock.disname);
                                                int thenewint = -1;
                                                for (int k = 0; k < worldgen.maininfo.inv.inv.Length; k++)
                                                {
                                                    if (worldgen.maininfo.inv.inv[k].socket.asock.name != "")
                                                    {
                                                        if (worldgen.maininfo.inv.inv[k].socket.asock.name == Environment.getmapwmap(aimedpoint).asock.name)
                                                        {
                                                            worldgen.maininfo.inv.inv[k].num += 1;
                                                            thenewint = -2;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (thenewint < 0)
                                                        {
                                                            thenewint = k;
                                                        }
                                                        thenewint = k;
                                                    }
                                                }

                                                if (thenewint >= 0)
                                                {
                                                    worldgen.maininfo.inv.inv[thenewint] = new Inventory.InvItem(new socketIG(socket.copysocket(Environment.getmapwmap(aimedpoint).asock), new Vector3(), null), 1);
                                                    if (thenewint > 29)
                                                    {
                                                        controls.toolbar[thenewint % 10] = worldgen.maininfo.inv.inv[thenewint];
                                                    }
                                                }
                                            }
                                            if (Environment.getmapwmap(aimedpoint).asock.isdrop)
                                            {
                                                foreach (RawInv.rawlibitem item in Environment.getmapwmap(aimedpoint).asock.getdrop.aninv.inv)
                                                {
                                                    worldgen.maininfo.rawinv.add(item.id, item.kg);
                                                    gui.thelistofcaptures.Add("+" + item.kg + "kg of " + item.raw.name);

                                                }
                                            }
                                            NetworkerPhoton.RPCDestroyBlock(aimedpoint.x, aimedpoint.y, aimedpoint.z);
                                            downpresstime = Time.time;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        hissoundsyst.SWeld.Stop();
                    }
                }
            }

            if (cam.controlkeys)
            {
                /// Keyboard
                if (Input.GetKeyDown(KeyCode.V))
                {
                    NetworkerPhoton.RPCLight(worldgen.maininfo.photonid);
                }
                if (Input.GetKeyDown(KeyCode.R) && !isfightingmode)
                {
                    if (!selsock.isgun)
                    {
                        NetworkerPhoton.RPCAnim(true, false, false, false, worldgen.maininfo.photonid);
                    }
                }
                if (Input.GetKeyDown(KeyCode.U))
                {

                    socket[, ,] lib = new socket[16, 128, 16];
                    for (int k2 = lib.GetLength(1) - 1; k2 >= 0; k2--)
                    {
                        for (int k1 = 0; k1 <= 15; k1++)
                        {
                            for (int k3 = 0; k3 <= 15; k3++)
                            {
                                socketIG thenew = Environment.getmapwmap(new Vector3(k1, k2, k3));
                                if (thenew != null)
                                {
                                    lib[k1, k2, k3] = socket.copysocket(thenew.asock);
                                }
                            }
                        }
                    }


                    string astring = fserial.saveasstring(lib);

                    socks = (socket[, ,])fserial.loadasobj(astring);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {


                    StartCoroutine(UpdateChunks());
                ///    
                ///    UpdateChunkss();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    randforcolor = guitools.Colorific();
                }
                if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
                {
                    movedir += hiscam.transform.forward * speed;
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    movedir += hiscam.transform.forward * speed * -1f;
                }

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    movedir += hiscam.transform.right * speed;
                }

                if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
                {
                    movedir += hiscam.transform.right * speed * -1f;
                }

                bool spellcast = false;

                if (Input.GetKeyDown(KeyCode.A) && isfightingmode)
                {
                    if (worldgen.maininfo.askill.actmembers[0] != -1)
                    {
                        skill askill = worldgen.maininfo.askill.act0;
                        if (Time.time - worldgen.maininfo.askill.actcool[0] > askill.cooldown)
                        {
                            worldgen.maininfo.askill.actcool[0] = Time.time;
                            NetworkerPhoton.RPCAnim(false, false, false, true, worldgen.maininfo.photonid);
                            NetworkerPhoton.RPCSkillSound(askill.dataname, worldgen.maininfo.photonid);
                            askill.runmethod();
                            spellcast = true;
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.E) && isfightingmode)
                {
                    if (worldgen.maininfo.askill.actmembers[1] != -1)
                    {
                        skill askill = worldgen.maininfo.askill.act1;
                        if (Time.time - worldgen.maininfo.askill.actcool[1] > askill.cooldown)
                        {
                            worldgen.maininfo.askill.actcool[1] = Time.time;
                            NetworkerPhoton.RPCAnim(false, false, false, true, worldgen.maininfo.photonid);
                            NetworkerPhoton.RPCSkillSound(askill.dataname, worldgen.maininfo.photonid);
                            askill.runmethod();
                            spellcast = true;
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && isfightingmode)
                {
                    if (worldgen.maininfo.askill.actmembers[2] != -1)
                    {
                        skill askill = worldgen.maininfo.askill.act2;
                        if (Time.time - worldgen.maininfo.askill.actcool[2] > askill.cooldown)
                        {
                            worldgen.maininfo.askill.actcool[2] = Time.time;
                            NetworkerPhoton.RPCAnim(false, false, false, true, worldgen.maininfo.photonid);
                            NetworkerPhoton.RPCSkillSound(askill.dataname, worldgen.maininfo.photonid);
                            askill.runmethod();
                            spellcast = true;
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.F) && isfightingmode)
                {
                    if (worldgen.maininfo.askill.actmembers[3] != -1)
                    {
                        skill askill = worldgen.maininfo.askill.act3;
                        if (Time.time - worldgen.maininfo.askill.actcool[3] > askill.cooldown)
                        {
                            worldgen.maininfo.askill.actcool[3] = Time.time;
                            NetworkerPhoton.RPCAnim(false, false, false, true, worldgen.maininfo.photonid);
                            NetworkerPhoton.RPCSkillSound(askill.dataname, worldgen.maininfo.photonid);
                            askill.runmethod();
                            spellcast = true;
                        }
                    }
                }
                if (spellcast)
                {

                    skill.getpasbystring("arcanemastery").anintval++;
                    for (int k = 0; k < 4; k++)
                    {
                        if (worldgen.maininfo.askill.pasmembers[k] != -1)
                        {
                            if (skill.pas[worldgen.maininfo.askill.pasmembers[k]].dataname == "arcanemastery")
                            {
                                if (skill.getpasbystring("arcanemastery").anintval == 4)
                                {

                                    skill.getpasbystring("arcanemastery").anintval = 0;
                                    worldgen.maininfo.askill.actcool[0] -= 4;
                                    worldgen.maininfo.askill.actcool[1] -= 4;
                                    worldgen.maininfo.askill.actcool[2] -= 4;
                                    worldgen.maininfo.askill.actcool[3] -= 4;
                                }
                                break;
                            }
                        }
                    }
                }

            }

            if (cam.controlmove)
            {
                
                if(worldgen.maininfo.stats.dir != new Vector2(0,0))
                {
                    movedir = new Vector3(worldgen.maininfo.stats.dir.normalized.x,0,worldgen.maininfo.stats.dir.normalized.y) * speed;

                }
                movedir = movedir * Time.deltaTime;
                movedir *= worldgen.maininfo.stats.coefmouv;

                float jumpSpeed = 8.0F;
                float gravity = 10f;
                float max = -9.81f;
                max = -5;

                if (Input.GetKey(KeyCode.Space))
                {
                    if ((worldgen.maininfo.oxg > 1f))
                    {
                        worldgen.maininfo.oxg = worldgen.maininfo.oxg - Time.deltaTime * 0.158f * 40 * 4f;
                        acc = 3 * 3f;
                        if (!hissoundsyst.SJetPack.isPlaying)
                        {
                            hissoundsyst.SJetPack.Play();
                        }
                    }
                    else
                    {
                        if (isonfloor)
                        {
                            acc = 6;
                        }
                        worldgen.maininfo.oxg = 0f;
                    }
                }
                else
                {
                    if (hissoundsyst.SJetPack.isPlaying)
                    {
                        hissoundsyst.SJetPack.Stop();
                    }
                }
                if (isonfloor)
                {
                    movedir.y = 0;

                }
                else
                {
                    if (acc > max)
                    {
                        acc -= 15 * Time.deltaTime;
                    }
                }
                if (Mathf.Abs(acc) < 0.1f)
                {
                    acc = -1;
                }

                movedir.y = acc * Time.deltaTime;

                blastz = lastz;
                cont.Move(movedir);
                lastz = transform.position.y;

            }

            Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
            lastMouseCoordinate = Input.mousePosition;
            RaycastHit thehit = new RaycastHit();
            if (selsock.isgun)
            {
                aimedpoint = new Vector3(0, -1000, 0);
                transcube.position = new Vector3(0, -1000, 0);
            }
            else
            {
                if (Physics.Raycast(new Ray(hiscam.transform.position, hiscam.transform.forward), out thehit, 5))
                {

                    if (selsock.name != "" && !selsock.isobject)
                    {
                        Vector3 pos = thehit.point - hiscam.transform.forward * 0.1f;
                        pos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
                        aimedpoint = pos;
                        transcube.position = pos;
                    }
                    else
                    {
                        Vector3 pos = thehit.point + hiscam.transform.forward * 0.1f;
                        pos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
                        aimedpoint = pos;
                        transcube.position = pos;
                    }
                }
                else
                {
                    aimedpoint = new Vector3(0, -1000, 0);
                    transcube.position = new Vector3(0, -1000, 0);
                }
            }
        }
	}


    public IEnumerator UpdateChunks()
    {
        for (int k2 = socks.GetLength(1) - 1; k2 >= 0; k2--)
        {
            for (int k1 = 0; k1 <= 15; k1++)
            {
                for (int k3 = 0; k3 <= 15; k3++)
                {
                    if (socks[k1,k2,k3] != null)
                    {
                        Environment.setmapwmap(new Vector3(k1, k2, k3), socket.copysocket(socks[k1,k2,k3]));
                    }
                }
            }
            print("test");
            yield return new WaitForSeconds(.1f);
        }
    }
    public void UpdateChunkss()
    {

        for (int k2 = socks.GetLength(1) - 1; k2 >= 0; k2--)
        {
            for (int k1 = 0; k1 <= 15; k1++)
            {
                for (int k3 = 0; k3 <= 15; k3++)
                {
                    if (socks[k1, k2, k3] != null)
                    {
                        Environment.setmapwmap(Environment.convctomap(new Vector3(k1, k2, k3)), socket.copysocket(socks[k1, k2, k3]));
                        print("doing");
                    }
                }
            }
        }
    }

    void OnGUI()
    {
    }

    public socket selsock
    {
        get
        {
            return controls.toolbar[controls.seltoolbar].socket.asock;
        }
    }


    public static void printie(string astring)
    {
        print(astring);
    }
}
