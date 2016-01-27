using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class gui : MonoBehaviour {
    public GUISkin prettyskin;
    public GUISkin prettyskinloseend;

    public Texture2D Twhite;
    public Texture2D Twhiteround;
    public Texture2D Twhiteroundbord;
    public Texture2D Tsquareh;
    public Texture2D Tsquareth;
    public Texture2D Tvoid;
    public Texture2D TArrowRight;
    public static Sounds soundsys;

    Rect ToolRect;
    Rect HealthRect;
    Rect LabelRect;
    Rect SmallerRect;
    float w;
    float h;
    float initgame;

    float he;
    float we;

    float hl;
    float wl;

    public static bool start;

    public socketIG workingon;
    public Inventory.InvItem MouseInvHeld;
    public static Lexiverse lexicon;
    
    public bool ischangingname;
    public bool isinteracting;
    public static bool isinvopen;
    public string changingnamestring;
    public string inventoryname;
    public int page;
    public int currentrecipe;
    public static List<string> thelistofcaptures;
    public float staampcapture = -1;
    public int mouseskillselect = -1;
    public bool mouseskillact = false;
    public int skillmenuselect = -1;
    public bool skillmenuact = false;

    public float skillactslidf = 0;
    public float skillpasslidf = 0;

    public static List<Stats> listofEnemy;
    public static List<float> enemydamagestamp;
    /// 0 inv
    /// 1 raw elements
    ///
    public static int invsetting;

    void Start()
    {
        lexicon = new Lexiverse(prettyskinloseend, prettyskin);
        Lexiverse.Tvoid = Tvoid;
        Lexiverse.Twhite = Twhite;
        Lexiverse.Tsquareh = Tsquareh;
        staampcapture = -1;
        thelistofcaptures = new List<string>();
        invsetting = 0;
        inventoryname = "";
        MouseInvHeld = new Inventory.InvItem();
        ischangingname = false;
        isinvopen = false;
        w = Screen.width;
        h = Screen.height;
        initgame = Time.time;

        he = h / 10f;
        we = 10 * he;
        hl = h / 20f;
        wl = 5 * he;

        ToolRect = new Rect((w - we) / 2f, h - he, we, he);
        HealthRect = new Rect((w - we) * 3 / 4f + we, h - he, (w - we) / 4f, he);
        LabelRect = new Rect((w - wl) / 2f,0, wl, hl);
        SmallerRect = new Rect(0,hl,w,h-hl-he);
        start = true;

        listofEnemy = new List<Stats>();
        enemydamagestamp = new List<float>();
    }
    void Update()
    {
        Inputs();
    }

    void OnGUI()
    {
        if (cam.controlmove)
        {
            if (controls.isfightingmode)
            {
                DrawCrossHair();
                DrawHealthBar();
                DrawPowerUps();

                DrawEnemyHealth();

            }
            else
            {
                DrawCrossHair();
                DrawSocketMenu();
                DrawCapture();

                DrawEnemyHealth();

                if (!isinvopen)
                {
                    DrawToolBar();
                }

                DrawHealthBar();

                DrawWorkSpace();

                DrawInventory();

                lexicon.DrawLexiverse();
            }
        }
    }

    void DrawCrossHair()
    {

        if (Time.time - controls.downpresstime < controls.downpressmax && controls.buttondown)
        {
            guitools.DrawPolyArc(new Vector2(w / 2f, h / 2f), 30, 22, Color.magenta, 0, 360 * ((Time.time - controls.downpresstime)/(1f* controls.downpressmax)), 2);
        }

        Vector2 offset = new Vector2();
        if (controls.isfightingmode)
        {
            offset = new Vector2(w / 30f, w / 30f)*0.5f;
        }
        guitools.DrawCross(new Vector2(w / 2f, h / 2f)+offset, 1, Color.white, 25);

        //guitools.DrawCross2(new Vector2(w / 2f, h / 2f), 2, Color.white, 15, 30);
    }
    void DrawCapture()
    {
        if (thelistofcaptures.Count != 0)
        {
            if (staampcapture == -1)
            {
                staampcapture = Time.time;
            }
            GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255,255 * (2 - (Time.time -staampcapture))/2f), Color.white, (int)Math.Round(18f));

            guitools.TextureStyle(MenuStyle, Tvoid);

            GUI.Label(guitools.GRectRelative(0, 1, 1,  1, LabelRect), thelistofcaptures[0], MenuStyle);
            if ((Time.time - staampcapture) > 2)
            {
                staampcapture = -1;
                thelistofcaptures.RemoveAt(0);
            }
        }
    }
    void DrawSocketMenu()
    {
        if (controls.aimedpoint.y > -800 && controls.toolbar[controls.seltoolbar].socket.asock.name == "")
        {
            socketIG thesock = Environment.getmapwmap(controls.aimedpoint);

            try
            {
                if (thesock.asock != null)
                {
                    GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

                    guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(14f));
                    GUI.Label(guitools.GRectRelative(0, 0 / 4f, 1, 3 / 4f, LabelRect), thesock.asock.disname, MenuStyle);
                    if (thesock.asock.props.Count != 0)
                    {
                        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(8f));
                        GUI.Label(guitools.GRectRelative(0, 2.5f / 4f, 1, 1.5f / 4f, LabelRect), "(press 'B' to modify)", MenuStyle);
                    }
                    Rect arect = new Rect(0, hl, (h - he - hl) / 3f, (h - he - hl) / 3f);
                    float i = 0;


                    if (thesock.asock.iselec)
                    {
                        arect = new Rect(0, hl + i, (h - he - hl) / 3f, (h - he - hl) / 3f);
                        DrawElecMenu(arect, thesock, thesock.asock.getelec);
                        i += (h - he - hl) / 3f;
                    }
                    if (thesock.asock.ismech)
                    {
                        arect = new Rect(0, hl + i, (h - he - hl) / 3f, (h - he - hl) / 6f);
                        DrawMechMenu(arect, thesock, thesock.asock.getmech);
                        i += (h - he - hl) / 6f;
                    }


                }
            }
            catch (NullReferenceException)
            {
    
            }
        }
    }
    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && !lexicon.ison && !controls.isfightingmode)
        {
            gui.isinvopen = !gui.isinvopen;

            if (gui.isinvopen)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                cam.controlmouse = false;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                cam.controlmouse = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.H) && !isinteracting && !ischangingname && !isinvopen)
        {
            lexicon.ison = !lexicon.ison;
            if (lexicon.ison)
            {
                soundsys.SHon.Play();
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                soundsys.Scancel.Play();
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = lexicon.ison;
            Cursor.lockState = CursorLockMode.None;
            cam.controlmouse = !lexicon.ison;
            cam.controlkeys = !lexicon.ison;

        }
        if (controls.aimedpoint.y > -800 && controls.toolbar[controls.seltoolbar].socket.asock.name == "")
        {
            socketIG thesock = Environment.getmapwmap(controls.aimedpoint);

            try
            {
                if (thesock.asock != null)
                {
                    if (thesock.asock.props.Count != 0)
                    {
                        if (Input.GetKeyDown(KeyCode.B) && !isinteracting && !lexicon.ison)
                        {
                            ischangingname = true;
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.None;
                            cam.controlmouse = false;
                            cam.controlkeys = false;
                            workingon = thesock;
                        }
                        if (Input.GetMouseButtonDown(1) && !ischangingname)
                        {
                            if (thesock.asock.isinteract)
                            {
                                if (!thesock.asock.getinteract.abool)
                                {
                                    controls.hissoundsyst.SScrew.Play();
                                    page = 0;
                                    currentrecipe = 0;
                                    isinteracting = true;
                                    NetworkerPhoton.RPCSetInteract(thesock.location, true, "");
                                    Cursor.visible = true;
                                    Cursor.lockState = CursorLockMode.None;
                                    cam.controlmouse = false;
                                    cam.controlkeys = false;
                                    workingon = thesock;
                                    lexicon.ison = false;
                                    if (thesock.asock.isinv || thesock.asock.israwinv)
                                    {
                                        isinvopen = true;
                                    }
                                }
                            }
                        }
                    }
                    if (Input.GetMouseButtonDown(2))
                    {
                        NetworkerPhoton.RPCSetColorBlock(thesock.location, controls.randforcolor);
                    }
                }
            }
            catch (NullReferenceException)
            {
            }
        }
    }

    void DrawElecMenu(Rect arect, socketIG thesock, sprop.elec elec)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(arect, "", MenuStyle);

        List<string> alist = new List<string>();
        alist.Add("Max Power: " + elec.maxpower+ " EU/s");
        alist.Add("Max Power Output: " + elec.energyoutput + " EU/s");
        alist.Add("Power Intake: " + elec.intake + " EU/s");
        alist.Add("Power Usage: " + elec.energyuse + " EU/s");
        alist.Add("Buffer: " + Math.Round(thesock.asock.getelec.buffer) + " EU");
        alist.Add("Max Buffer: " + elec.maxbuffer + " EU");
        alist.Add("Efficiency: " + elec.loss*100 + "%");

        guitools.TextureStyle(MenuStyle, Tsquareh);

        for (int k = -2; k < alist.Count; k++)
        {
            if (k != -2)
            {
                if (k == -1)
                {
                    Rect newrect = guitools.GRectRelative(0, 0, 1, 2 / (alist.Count + 2f), arect);
                    guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 255), Color.black, (int)Math.Round(10f));
                    GUI.Label(guitools.GRectRelative(0, 0, 1, 1, newrect), "Electric Panel", MenuStyle);
                }
                else
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 255), Color.black, (int)Math.Round(7f));
                    Rect newrect = guitools.GRectRelative(0, (2 + k) * 1 / (alist.Count + 2f), 1, 1 / (alist.Count + 2f), arect);
                    GUI.Label(guitools.GRectRelative(0, 0, 0.2f, 1, newrect), k + ".", MenuStyle);
                    MenuStyle.alignment = TextAnchor.MiddleLeft;
                    GUI.Label(guitools.GRectRelative(0.2f, 0, 0.8f, 1, newrect), alist[k], MenuStyle);
                    MenuStyle.alignment = TextAnchor.MiddleCenter;
                }
            }
        }
    }
    void DrawMechMenu(Rect arect, socketIG thesock, sprop.mech mech)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(arect, "", MenuStyle);

        List<string> alist = new List<string>();
        alist.Add("ONLINE: " + mech.powered );
        alist.Add("Power Intake: " + mech.load + " MU");
        alist.Add("Power Output: " + mech.production + " MU");
        alist.Add("Max Power Output: " + mech.maxprod + " MU");
        alist.Add("Conversion Rate: " + mech.convratio * 100 + "%");

        guitools.TextureStyle(MenuStyle, Tsquareh);

        for (int k = -2; k < alist.Count; k++)
        {
            if (k != -2)
            {
                if (k == -1)
                {
                    Rect newrect = guitools.GRectRelative(0, 0, 1, 2 / (alist.Count + 2f), arect);
                    guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 255), Color.black, (int)Math.Round(10f));
                    GUI.Label(guitools.GRectRelative(0, 0, 1, 1, newrect), "Mechanical Panel", MenuStyle);
                }
                else
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 255), Color.black, (int)Math.Round(7f));
                    Rect newrect = guitools.GRectRelative(0, (2 + k) * 1 / (alist.Count + 2f), 1, 1 / (alist.Count + 2f), arect);
                    GUI.Label(guitools.GRectRelative(0, 0, 0.2f, 1, newrect), k + ".", MenuStyle);
                    MenuStyle.alignment = TextAnchor.MiddleLeft;
                    GUI.Label(guitools.GRectRelative(0.2f, 0, 0.8f, 1, newrect), alist[k], MenuStyle);
                    MenuStyle.alignment = TextAnchor.MiddleCenter;
                }
            }
        }
    }

    void DrawToolBar()
    {

        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 100), Color.black, (int)Math.Round(18f));
        GUI.Label(ToolRect, "", MenuStyle);

        for (int k = 0; k < 10; k++)
        {
            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 100), Color.black, (int)Math.Round(18f));
            guitools.TextureStyle(MenuStyle, Tsquareh);
            Rect current = new Rect(ToolRect.x + k * ToolRect.height, ToolRect.y, ToolRect.height, ToolRect.height);
            if (k == controls.seltoolbar)
            {
                guitools.SetStyle(MenuStyle, controls.randforcolor, Color.black, (int)Math.Round(18f));
                guitools.TextureStyle(MenuStyle, Tsquareth);
                GUI.Label(guitools.GRectRelative(1,current), "", MenuStyle);
                guitools.TextureStyle(MenuStyle, Tsquareh);
            }
            GUI.Label(current, "", MenuStyle);

            //guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
            //guitools.TextureStyle(MenuStyle, controls.toolbar[k].socket.asock.icon);
            //GUI.Label(guitools.GRectRelative(0.8f, current), "", MenuStyle);

            controls.toolbar[k].DrawItem(current, MenuStyle, Tvoid);
        }

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.white, (int)Math.Round(18f));
        guitools.TextureStyle(MenuStyle, Tvoid);
        GUI.Label(guitools.GRectRelative(0, -0.5f, 1, 0.5f, ToolRect), controls.toolbar[controls.seltoolbar].socket.asock.disname, MenuStyle);
    }
    void DrawHealthBar()
    {

        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(5f));
        GUI.Label(guitools.GRectRelative(1.05f,HealthRect), "", MenuStyle);
        Rect therect = guitools.GRectRelative(0,0,1,0.5f,HealthRect);

        guitools.SetStyle(MenuStyle, guitools.RGB(100, 0, 0, 255), Color.black, (int)Math.Round(10f));
        GUI.Label(therect, "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 0, 0, 255), Color.black, (int)Math.Round(10f));
        //GUI.Label(guitools.GRectRelative(0,0.1f,1,0.8f,guitools.GRectRelative(0, 0, worldgen.maininfo.health/100f, 1, therect)), "", MenuStyle);
        //guitools.TextureStyle(MenuStyle, Tvoid);
        //GUI.Label(therect, "HEALTH : "+ Mathf.RoundToInt(worldgen.maininfo.health)+"%", MenuStyle);
        Rect life = guitools.GRectRelative(0, 0.1f, 1, 0.8f, guitools.GRectRelative(0, 0, 1, 1, therect));
        guitools.TextureStyle(MenuStyle, Twhiteround);
        guitools.SetStyle(MenuStyle, guitools.RGB(75, 0, 0, 255 ), Color.white, (int)Math.Round(16f));
        GUI.Label(guitools.GRectRelative(0, 0, worldgen.maininfo.stats.healthperc, 1, life), "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 25, 25, 255), Color.white, (int)Math.Round(16f));
        GUI.Label(guitools.GRectRelative(0, 0, worldgen.maininfo.stats.healthpercdama, 1, life), "", MenuStyle);
        int anint = worldgen.maininfo.stats.maxhealthdivs;
        float afloat = worldgen.maininfo.stats.maxhealthi / 100f;
        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.white, (int)Math.Round(16f));
        guitools.TextureStyle(MenuStyle, Twhite);
        float wii = 0.01f;
        if (anint != 0)
        {
            for (int k2 = 0; k2 < anint; k2++)
            {
                if (k2 != anint - 1)
                {
                    GUI.Label(guitools.GRectRelative((1 + k2) / (1f * afloat) - wii / 2f, 0.5f, wii, 0.5f, life), "", MenuStyle);
                }
            }
        }




        therect = guitools.GRectRelative(0, 0.5f, 1, 0.5f, HealthRect);
        guitools.TextureStyle(MenuStyle, Twhite);

        guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 100, 255), Color.black, (int)Math.Round(10f));
        GUI.Label(therect, "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 255, 255), Color.black, (int)Math.Round(10f));
        GUI.Label(guitools.GRectRelative(0,0.05f,1,0.9f,guitools.GRectRelative(0, 0, worldgen.maininfo.oxg /(1f * worldgen.maininfo.maxoxg), 1, therect)), "", MenuStyle);
        guitools.TextureStyle(MenuStyle, Tvoid);
        GUI.Label(therect, "AIR : " + System.Math.Round(worldgen.maininfo.oxg,1) + "g OF " + System.Math.Round(worldgen.maininfo.maxoxg,1)+"g", MenuStyle);



        guitools.TextureStyle(MenuStyle, Twhite);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(10f));
        //GUI.Label(guitools.GRectRelative(0, 0.5f - 0.02f, 1, 0.04f, HealthRect), "", MenuStyle);

    }

    void DrawWorkSpace()
    {
        if (ischangingname)
        {
            socketIG thesock = workingon;

            GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

            Rect therect = guitools.RectFromCenter(new Vector2(w / 2f, h / 2f), w / 4f, h / 4f);
            Rect therectup = guitools.GRectRelative(0,0,1,1/2f,therect);
            Rect therectdown = guitools.GRectRelative(0,1/2f,1,1/2f,therect);



            GUI.Label(therect, "", MenuStyle);
            guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
            changingnamestring = GUI.TextField(guitools.GRectRelative(0.8f, therectup), changingnamestring, 15, MenuStyle);
            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

            if (GUI.Button(guitools.GRectRelative(0.8f, therectdown), "OK", MenuStyle))
            {
                thesock.asock.disname = changingnamestring;
                ischangingname = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                cam.controlmouse = true;
                cam.controlkeys = true;
            }
        }
        if (isinteracting)
        {
            socketIG thesock = workingon;

            GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

            Rect therect = guitools.RectFromCenter(new Vector2(w / 2f, h / 2f), w / 4f, h / 1.5f);

            switch (thesock.asock.name)
            {
                case "gen01":
                    DrawGen01Menu(therect, thesock, thesock.asock.getelec, thesock.asock.getvals);
                    break;
                case "light01":
                    DrawLight01Menu(therect, thesock, thesock.asock.getelec, thesock.asock.getvals);
                    break;
                case "light02":
                    DrawLight01Menu(therect, thesock, thesock.asock.getelec, thesock.asock.getvals);
                    break;
                case "chest01":
                    DrawChest01Menu(thesock);
                    break;
                case "m01crafter":
                    DrawM01Crafter(thesock);
                    break;
                case "m01furnace":
                    DrawM01Crafter(thesock);
                    break;
                case "m01pulverizer":
                    DrawM01Crafter(thesock);
                    break;
                case "m01compressor":
                    DrawM01Crafter(thesock);
                    break;
                case "m01calefactor":
                    DrawSingleSloatChest(thesock);
                    DrawCaleFactor(thesock);
                    break;
                case "m01gensteam":
                    break;
                case "m01miner":
                    break;
                case "m01gencoal":
                    break;
                case "m01machine":
                    DrawCharBuilder();
                    break;
                case "m01tread":
                    isinvopen = false;
                    thesock.instance.transform.eulerAngles = thesock.instance.transform.eulerAngles + new Vector3(0, 90, 0);
                    isinteracting = false;
                    NetworkerPhoton.RPCSetInteractRotate(thesock.location, thesock.instance.transform.eulerAngles);
                    thesock.asock.getinteract.abool = false;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    cam.controlmouse = true;
                    cam.controlkeys = true;
                    break;
                case "m01treaddiv":
                    isinvopen = false;
                    thesock.instance.transform.eulerAngles = thesock.instance.transform.eulerAngles + new Vector3(0, 90, 0);
                    isinteracting = false;
                    NetworkerPhoton.RPCSetInteractRotate(thesock.location, thesock.instance.transform.eulerAngles);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    cam.controlmouse = true;
                    cam.controlkeys = true;
                    break;
                case "m01airgen":
                    DrawAirGen01Menu(therect, thesock, thesock.asock.getelec, thesock.asock.getvals);
                    break;
                default:
                    isinteracting = false;
                    NetworkerPhoton.RPCSetInteract(thesock.location, false, fserial.saveasstring(thesock.asock.props));
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    cam.controlmouse = true;
                    cam.controlkeys = true;
                    break;
            }
            if (thesock.asock.israwinv)
            {
                Rect arect = new Rect(w - (h - he - hl) / 3f, hl, (h - he - hl) / 3f, (h - he - hl) / 3f );
                DrawRawInv(thesock.asock.getrawinvs.aninv,thesock,arect);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                isinvopen = false;
                isinteracting = false;
                NetworkerPhoton.RPCSetInteract(thesock.location, false, fserial.saveasstring(thesock.asock.props));
                Cursor.visible = false;
                cam.controlmouse = true;
                cam.controlkeys = true;
                Cursor.lockState = CursorLockMode.Locked;
                //if (thesock.asock.name == "chest01")
               // {
                //    thesock.instance.GetComponent<chesthead>().open = false;
                //}
            }
        }
    }
    void DrawGen01Menu(Rect arect, socketIG thesock, sprop.elec elec, sprop.vals valis)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

        GUI.Label(arect, "", MenuStyle);

        if (GUI.Button(guitools.GRectRelative(0,0.9f,1,0.1f, arect), "OK", MenuStyle))
        {
            isinteracting = false;
            NetworkerPhoton.RPCSetInteract(thesock.location, false, fserial.saveasstring(thesock.asock.props));
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cam.controlmouse = true;
            cam.controlkeys = true;
        }

        Rect OffOnn = guitools.GRectRelative(0, 0.1f, 1, 0.1f, arect);

        if (valis.vbool)
        {
            GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, arect), "STATUS : ONLINE", MenuStyle);
            valis.vbool = GUI.Toggle(guitools.GRectRelative(0.8f, (guitools.GRectRelative(0, 0, 1, 1, OffOnn))), valis.vbool, "TURN OFF", MenuStyle);
        }
        else
        {
            GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, arect), "STATUS : OFFLINE", MenuStyle);
            valis.vbool = GUI.Toggle(guitools.GRectRelative(0.8f, (guitools.GRectRelative(0, 0, 1, 1, OffOnn))), valis.vbool, "TURN ON", MenuStyle);
        }

        Rect PowerEmmission = guitools.GRectRelative(0, 0.2f, 1, 0.1f, arect);
        GUI.Label(PowerEmmission, "POWER PRODUCTION: " + Mathf.RoundToInt(valis.vfloat * 100) + "%", MenuStyle);
        PowerEmmission = guitools.GRectRelative(0, 0.3f, 1, 0.1f, arect);
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
        GUIStyle MenuStyle2 = new GUIStyle(prettyskin.label);
        guitools.SetStyle(MenuStyle2, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
        valis.vfloat = GUI.HorizontalSlider(guitools.GRectRelative(0.8f, PowerEmmission), valis.vfloat, 0, 1, MenuStyle, MenuStyle2);

    }
    void DrawAirGen01Menu(Rect arect, socketIG thesock, sprop.elec elec, sprop.vals valis)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

        GUI.Label(arect, "", MenuStyle);

        if (GUI.Button(guitools.GRectRelative(0, 0.9f, 1, 0.1f, arect), "OK", MenuStyle))
        {
            isinteracting = false;
            NetworkerPhoton.RPCSetInteract(thesock.location, false, fserial.saveasstring(thesock.asock.props));
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cam.controlmouse = true;
            cam.controlkeys = true;
        }

        Rect OffOnn = guitools.GRectRelative(0, 0.1f, 1, 0.1f, arect);

        if (valis.vbool)
        {
            GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, arect), "STATUS : ONLINE", MenuStyle);
            valis.vbool = GUI.Toggle(guitools.GRectRelative(0.8f, (guitools.GRectRelative(0, 0, 1, 1, OffOnn))), valis.vbool, "TURN OFF", MenuStyle);
        }
        else
        {
            GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, arect), "STATUS : OFFLINE", MenuStyle);
            valis.vbool = GUI.Toggle(guitools.GRectRelative(0.8f, (guitools.GRectRelative(0, 0, 1, 1, OffOnn))), valis.vbool, "TURN ON", MenuStyle);
        }

        Rect PowerEmmission = guitools.GRectRelative(0, 0.2f, 1, 0.1f, arect);
        GUI.Label(PowerEmmission, "sAIR PRODUCTION: " + Mathf.RoundToInt(valis.vfloat * 100) + "%", MenuStyle);
        PowerEmmission = guitools.GRectRelative(0, 0.3f, 1, 0.1f, arect);
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
        GUIStyle MenuStyle2 = new GUIStyle(prettyskin.label);
        guitools.SetStyle(MenuStyle2, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
        valis.vfloat = GUI.HorizontalSlider(guitools.GRectRelative(0.8f, PowerEmmission), valis.vfloat, 0, 1, MenuStyle, MenuStyle2);

    }
    void DrawLight01Menu(Rect arect, socketIG thesock, sprop.elec elec, sprop.vals valis)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

        GUI.Label(arect, "", MenuStyle);

        if (GUI.Button(guitools.GRectRelative(0, 0.9f, 1, 0.1f, arect), "OK", MenuStyle))
        {
            isinteracting = false;
            NetworkerPhoton.RPCSetInteract(thesock.location, false, fserial.saveasstring(thesock.asock.props));
            Cursor.visible = false;
            cam.controlmouse = true;
            cam.controlkeys = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Rect OffOnn = guitools.GRectRelative(0, 0.1f, 1, 0.1f, arect);

        if (valis.vbool)
        {
            GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, arect), "STATUS : ONLINE", MenuStyle);
            valis.vbool = GUI.Toggle(guitools.GRectRelative(0.8f, (guitools.GRectRelative(0, 0, 1, 1, OffOnn))), valis.vbool, "TURN OFF", MenuStyle);
        }
        else
        {
            GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, arect), "STATUS : OFFLINE", MenuStyle);
            valis.vbool = GUI.Toggle(guitools.GRectRelative(0.8f, (guitools.GRectRelative(0, 0, 1, 1, OffOnn))), valis.vbool, "TURN ON", MenuStyle);
        }

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(12f));
        Rect Colore = guitools.GRectRelative(0, 0.2f, 1, 0.1f, arect);
        GUI.Label(Colore, "COLOR: ", MenuStyle);
        Colore = guitools.GRectRelative(0, 0.3f, 0.2f, 0.1f, arect);
        GUI.Label(Colore, "R:" + Mathf.RoundToInt(valis.vVector3.x), MenuStyle);
        Colore = guitools.GRectRelative(0, 0.4f, 0.2f, 0.1f, arect);
        GUI.Label(Colore, "G:" + Mathf.RoundToInt(valis.vVector3.y), MenuStyle);
        Colore = guitools.GRectRelative(0, 0.5f, 0.2f, 0.1f, arect);
        GUI.Label(Colore, "B:" + Mathf.RoundToInt(valis.vVector3.z), MenuStyle);

        guitools.SetStyle(MenuStyle, guitools.RGB(valis.vVector3.x, valis.vVector3.y, valis.vVector3.z, 255), Color.black, (int)Math.Round(18f));
        Rect Colores = guitools.GRectRelative(0.8f, 0.3f, 0.2f, 0.3f, arect);
        GUI.Label(Colores, "", MenuStyle);


        float A = 0;
        float B = 0;
        float C = 0;
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
        GUIStyle MenuStyle2 = new GUIStyle(prettyskin.label);
        guitools.SetStyle(MenuStyle2, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
        Colore = guitools.GRectRelative(0.2f, 0.3f, 0.6f, 0.1f, arect);
        A = GUI.HorizontalSlider(guitools.GRectRelative(0.8f, Colore), valis.vVector3.x, 0, 255, MenuStyle, MenuStyle2);
        Colore = guitools.GRectRelative(0.2f, 0.4f, 0.6f, 0.1f, arect);
        B = GUI.HorizontalSlider(guitools.GRectRelative(0.8f, Colore), valis.vVector3.y, 0, 255, MenuStyle, MenuStyle2);
        Colore = guitools.GRectRelative(0.2f, 0.5f, 0.6f, 0.1f, arect);
        C = GUI.HorizontalSlider(guitools.GRectRelative(0.8f, Colore), valis.vVector3.z, 0, 255, MenuStyle, MenuStyle2);

        Transform child = thesock.instance.transform.Find("light");
        child.GetComponent<Light>().color = guitools.RGB(valis.vVector3.x, valis.vVector3.y, valis.vVector3.z, 255);

        child.GetComponent<Light>().enabled = valis.vbool;
        if (!valis.vbool)
        {
            thesock.instance.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", guitools.RGB(0, 0, 0, 255));
        }


        valis.vVector3 = new Vector3(A, B, C);

    }

    void DrawChest01Menu(socketIG thesock)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

        Rect darect = guitools.GRectRelative(0.25f, 0f, 0.5f, 0.5f, SmallerRect);
        darect = guitools.GRectRelative(0.9f, darect);
        darect = guitools.RectFromCenter(new Vector2(darect.x + darect.width/2f, darect.y + darect.height / 2f ), darect.height, darect.height);


        GUI.Label(darect, "", MenuStyle);

        darect = guitools.GRectRelative(0.8f, darect);
        Rect line1 = guitools.GRectRelative(0, 0, 1, 0.25f, darect);
        Rect line2 = guitools.GRectRelative(0, 0.25f, 1, 0.25f, darect);
        Rect line3 = guitools.GRectRelative(0, 0.5f, 1, 0.25f, darect);
        Rect line4 = guitools.GRectRelative(0, 0.75f, 1, 0.25f, darect);
        Rect currect = line1;



        guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
        guitools.TextureStyle(MenuStyle, Twhite);

        guitools.TextureStyle(MenuStyle, Tvoid);
        Inventory theinv = thesock.asock.getinvs.aninv;
        for (int k = 0; k < theinv.inv.Length; k++)
        {
            currect = line1;
            if (k > 3)
            {
                currect = line2;
            }
            if (k > 7)
            {
                currect = line3;
            }
            if (k > 11)
            {
                currect = line4;
            }
            int mink = k % 4;
            currect = guitools.GRectRelative(mink / 4f, 0, 1 / 4f, 1, currect);
            Inventory.InvItem curritem = theinv.inv[k];



            guitools.TextureStyle(MenuStyle, Twhite);
            guitools.SetStyle(MenuStyle, guitools.RGB(220, 220, 220, 255), Color.black, (int)Math.Round(18f));
            GUI.Label(guitools.GRectRelative(0.9f, currect), "", MenuStyle);
            if (curritem.socket.asock.name != "")
            {
                Vector2 mousepos = Converters.MouseToScreen(Input.mousePosition);
                curritem.DrawItem(currect, MenuStyle, Tvoid);
                if (guitools.isinrect(mousepos, currect))
                {
                    inventoryname = curritem.socket.asock.disname;
                }
            }

            guitools.TextureStyle(MenuStyle, Tvoid);
            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
            if (GUI.Button(guitools.GRectRelative(0.9f, currect), "", MenuStyle))
            {
                MouseInvHeld = curritem.InteractItem(MouseInvHeld, theinv, k);
            }

        }
    }
    void DrawSingleSloatChest(socketIG thesock)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

        Rect darect = guitools.GRectRelative(0.25f, 0f, 0.5f, 0.5f, SmallerRect);
        darect = guitools.GRectRelative(0.9f, darect);
        darect = guitools.RectFromCenter(new Vector2(darect.x + darect.width / 2f, darect.y + darect.height / 2f), darect.height, darect.height);


        GUI.Label(darect, "", MenuStyle);

        darect = guitools.GRectRelative(0.25f, darect);
        Rect currect = darect;



        guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
        guitools.TextureStyle(MenuStyle, Twhite);

        guitools.TextureStyle(MenuStyle, Tvoid);
        Inventory theinv = thesock.asock.getinvs.aninv;
        currect = guitools.GRectRelative(0.9f, currect);
        Inventory.InvItem curritem = theinv.inv[0];



        guitools.TextureStyle(MenuStyle, Twhite);
        guitools.SetStyle(MenuStyle, guitools.RGB(220, 220, 220, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(guitools.GRectRelative(0.9f, currect), "", MenuStyle);
        if (curritem.socket.asock.name != "")
        {
            Vector2 mousepos = Converters.MouseToScreen(Input.mousePosition);
            curritem.DrawItem(currect, MenuStyle, Tvoid);
            if (guitools.isinrect(mousepos, currect))
            {
                inventoryname = curritem.socket.asock.disname;
            }
        }

        guitools.TextureStyle(MenuStyle, Tvoid);
        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
        if (GUI.Button(guitools.GRectRelative(0.9f, currect), "", MenuStyle))
        {
            MouseInvHeld = curritem.InteractItem(MouseInvHeld, theinv, 0);
        }
    }
    void DrawCaleFactor(socketIG thesock)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskin.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

        Rect darect = guitools.GRectRelative(0.25f, 0f, 0.5f, 0.5f, SmallerRect);
        darect = guitools.RectFromCenter(new Vector2(darect.x + darect.width / 2f, darect.y + darect.height / 2f), darect.height, darect.height);


        GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, darect), "BURNING: " + Mathf.RoundToInt(thesock.asock.getvals.vfloat * 100) + "%", MenuStyle);
    }
    
    void DrawM01Crafter(socketIG thesock)
    {
        bool crafting = false;
        float craftlevel = 0;
        if (thesock.asock.getcraft.vfloat != -1)
        {
            if (Time.time - thesock.asock.getcraft.vfloat < thesock.asock.getcraft.vfloat2)
            {
                //currentrecipe = thesock.asock.getcraft.vint;
                craftlevel = (Time.time - thesock.asock.getcraft.vfloat) / (1f * thesock.asock.getcraft.vfloat2);
                crafting = true;
            }
            else
            {
                foreach (Inventory.InvItem item in thesock.asock.getcraft.Result)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (thesock.asock.getinvs.aninv.inv[k].socket.asock.name == item.socket.asock.name)
                        {
                            thesock.asock.getinvs.aninv.inv[k].num += item.num;
                            break;
                        }
                        else
                        {
                            if (thesock.asock.getinvs.aninv.inv[k].num == 0)
                            {
                                thesock.asock.getinvs.aninv.inv[k] = new Inventory.InvItem(item.socket, item.num);
                                break;
                            }
                        }
                    }
                }
                foreach (RawInv.rawlibitem item in thesock.asock.getcraft.ResultRaw)
                {
                    thesock.asock.getrawinvs.aninv.add(item.id, item.kg);
                }
                thesock.asock.getcraft.vfloat = -1;
            }
        }


        GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

        Rect darect = guitools.GRectRelative(0.25f, 0f, 0.5f, 0.5f, SmallerRect);
        darect = guitools.GRectRelative(-0.125f, 0, 1.25f, 1, darect);
        //darect = guitools.GRectRelative(0.9f, darect);
        //darect = guitools.RectFromCenter(new Vector2(darect.x + darect.width/2f, darect.y + darect.height / 2f ), darect.height, darect.height);
        Rect rectA = guitools.GRectRelative(0, 0, 0.2f, 1, darect,0.9f);
        Rect rectB = guitools.GRectRelative(0.2f, 0, 0.4f, 1, darect, 0.9f);
        Rect rectC = guitools.GRectRelative(0.6f, 0.45f, 0.2f, 0.1f, darect, 0.9f);
        rectC = guitools.GRectRelative(0, -0.5f, 1, 1.5f, rectC);
        Rect rectD = guitools.GRectRelative(0.8f, 0, 0.2f, 1, darect, 0.9f);


        GUI.Label(rectA, "", MenuStyle);
        GUI.Label(rectB, "", MenuStyle);
        GUI.Label(guitools.GRectRelative(0,0,1,1.5f,rectC), "", MenuStyle);
        //GUI.Label(darect, "", MenuStyle);

        ///A Recipes
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(8f));
        List<Recipe> reciplist = thesock.asock.getcraft.Recipes;
        int numofpages = (((reciplist.Count) - (reciplist.Count % 9))/9);
        GUI.Label(guitools.GRectRelative(0.25f,0,0.5f,0.1f,rectA,0.95f), "PAGE "+(page+1)+"/"+(numofpages+1), MenuStyle);
        if (GUI.Button(guitools.GRectRelative(0, 0, 0.25f, 0.1f, rectA, 0.95f), "<<", MenuStyle))
        {
            if (page > 0)
            {
                page--;
            }
        }
        if (GUI.Button(guitools.GRectRelative(0.75f, 0, 0.25f, 0.1f, rectA, 0.95f), ">>", MenuStyle))
        {
            if (page < numofpages)
            {
                page++;
            }
        }
        for (int k = 0; k < 9; k++)
        {
            int rk = k + 9 * page;
            if (rk < reciplist.Count)
            {
                if (GUI.Button(guitools.GRectRelative(0, 0.1f * (k + 1), 0.8f, 0.1f, rectA, 0.95f), reciplist[rk].name, MenuStyle) && !crafting)
                {
                    currentrecipe = rk;
                }
                int res = -1;
                foreach (int anint in thesock.asock.getcraft.autos)
                {
                    if (rk == anint)
                    {
                        res = rk;
                        break;
                    }
                }
                if (res != -1)
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(150, 250, 150, 255), Color.black, (int)Math.Round(8f));

                    if (GUI.Button(guitools.GRectRelative(0.8f, 0.1f * (k + 1), 0.2f, 0.1f, rectA, 0.95f), "A", MenuStyle) && !crafting)
                    {
                        thesock.asock.getcraft.autos.Remove(rk);
                    }
                }
                else
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(250, 150, 150, 255), Color.black, (int)Math.Round(8f));
                    if (GUI.Button(guitools.GRectRelative(0.8f, 0.1f * (k + 1), 0.2f, 0.1f, rectA, 0.95f), "A", MenuStyle) && !crafting)
                    {
                        thesock.asock.getcraft.autos.Add(rk);
                    }
                }
                guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(8f));
            }
        }


        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(8f));
        ///B
        if (currentrecipe < reciplist.Count)
        {
            GUI.Label(guitools.GRectRelative(0.25f, 0, 0.5f, 0.1f, rectB, 0.95f), reciplist[currentrecipe].name, MenuStyle);
        }
        else
        {
            GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, rectB, 0.95f), "RECIPE:", MenuStyle);
        }
        GUI.Label(guitools.GRectRelative(0, 0.1f, 0.5f, 0.1f, rectB, 0.95f), "REQUIRES:", MenuStyle);
        GUI.Label(guitools.GRectRelative(0.5f, 0.1f, 0.5f, 0.1f, rectB, 0.95f), "CRAFTS:", MenuStyle);

        List<string> write = new List<string>();
        foreach (Inventory.InvItem item in reciplist[currentrecipe].Items)
        {
            write.Add(item.socket.asock.disname + ": " + item.num);
        }
        foreach (RawInv.rawlibitem item in reciplist[currentrecipe].RawItems)
        {
            write.Add(item.raw.name + ": " + item.kg+"kg");
        }

        MenuStyle.alignment = TextAnchor.MiddleLeft;
        if (write.Count != 0)
        {
            for (int k = 0; k < write.Count; k++)
            {
                GUI.Label(guitools.GRectRelative(0, 0.2f + (0.8f) / (write.Count * 1f) * k, 0.5f, (0.8f) / (write.Count * 1f), rectB, 0.95f), write[k], MenuStyle);
            }
        }


        write = new List<string>();
        foreach (Inventory.InvItem item in reciplist[currentrecipe].Result)
        {
            write.Add(item.socket.asock.disname + ": " + item.num);
        }
        foreach (RawInv.rawlibitem item in reciplist[currentrecipe].ResultRaw)
        {
            write.Add(item.raw.name + ": " + item.kg + "kg");
        }
        if (write.Count != 0)
        {
            for (int k = 0; k < write.Count; k++)
            {
                GUI.Label(guitools.GRectRelative(0.5f, 0.2f + (0.8f) / (write.Count * 1f) * k, 0.5f, (0.8f) / (write.Count * 1f), rectB, 0.95f), write[k], MenuStyle);
            }
        }

        MenuStyle.alignment = TextAnchor.MiddleCenter;

        ///C
        GUI.Label(guitools.GRectRelative(0, 0, 1, 0.5f, rectC, 0.8f), "COST: " + reciplist[currentrecipe].eleccost + "EU", MenuStyle);
        GUI.Label(guitools.GRectRelative(0, 1, 1, 0.5f, rectC, 0.8f), "TIME: " + reciplist[currentrecipe].time + "", MenuStyle);
        if (crafting)
        {
            guitools.TextureStyle(MenuStyle, Twhite);
            guitools.SetStyle(MenuStyle, guitools.RGB(180, 180, 180, 255), Color.black, (int)Math.Round(8f));
            GUI.Label(guitools.GRectRelative(0, 0.5f, 1, 0.5f, rectC, 0.8f), "", MenuStyle);
            guitools.SetStyle(MenuStyle, guitools.RGB(120, 120, 120, 255), Color.black, (int)Math.Round(8f));
            GUI.Label(guitools.GRectRelative(0, 0.5f, 1* craftlevel, 0.5f, rectC, 0.8f), "", MenuStyle);


            guitools.TextureStyle(MenuStyle, Tvoid);
            GUI.Label(guitools.GRectRelative(0, 0.5f, 1, 0.5f, rectC, 0.8f), "CRAFT", MenuStyle);
            MenuStyle = new GUIStyle(prettyskinloseend.label);
        }
        else
        {
            if (GUI.Button(guitools.GRectRelative(0, 0.5f, 1, 0.5f, rectC, 0.8f), "CRAFT", MenuStyle))
            {
                bool candoit = true;

                if (thesock.asock.getelec.buffer < reciplist[currentrecipe].eleccost)
                {
                    candoit = false;
                }

                foreach (Inventory.InvItem item in reciplist[currentrecipe].Items)
                {
                    if (worldgen.maininfo.inv.getcount(item.socket.asock.name) < item.num)
                    {
                        candoit = false;
                    }
                }
                foreach (RawInv.rawlibitem item in reciplist[currentrecipe].RawItems)
                {
                    if (worldgen.maininfo.rawinv.getcount(item.id) < item.kg)
                    {
                        candoit = false;
                    }
                }
                if (candoit)
                {
                    thesock.asock.getelec.buffer -= reciplist[currentrecipe].eleccost;
                    foreach (Inventory.InvItem item in reciplist[currentrecipe].Items)
                    {
                        worldgen.maininfo.inv.remamount(item.socket.asock.name, item.num);
                    }
                    foreach (RawInv.rawlibitem item in reciplist[currentrecipe].RawItems)
                    {
                        worldgen.maininfo.rawinv.rem(item.id, item.kg);
                    }

                    thesock.asock.getcraft.vfloat = Time.time;
                    thesock.asock.getcraft.vfloat2 = reciplist[currentrecipe].time;
                    thesock.asock.getcraft.Result = reciplist[currentrecipe].Result;
                    thesock.asock.getcraft.ResultRaw = reciplist[currentrecipe].ResultRaw;

                    controls.hissoundsyst.stoaudio("mclang").Play();
                }
            }
        }
         
        
        ///D
        

        Inventory theinv = thesock.asock.getinvs.aninv;
        for (int k = 0; k < theinv.inv.Length; k++)
        {
            Rect therect = guitools.RectFromCenter(new Vector2(rectD.x + rectD.width / 2f, rectD.y + rectD.height / 2f), rectD.height / 4f,rectD.height);
            therect = guitools.GRectRelative(0.9f, therect);
            GUI.Label(therect, "", MenuStyle);



            guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
            guitools.TextureStyle(MenuStyle, Twhite);
            guitools.TextureStyle(MenuStyle, Tvoid);
            int mink = k;
            Rect currect = guitools.GRectRelative(0,mink / 4f, 1, 1 / 4f, therect,0.75f);
            Inventory.InvItem curritem = theinv.inv[k];



            guitools.TextureStyle(MenuStyle, Twhite);
            guitools.SetStyle(MenuStyle, guitools.RGB(220, 220, 220, 255), Color.black, (int)Math.Round(18f));
            GUI.Label(guitools.GRectRelative(0.9f, currect), "", MenuStyle);
            if (curritem.socket.asock.name != "")
            {
                Vector2 mousepos = Converters.MouseToScreen(Input.mousePosition);
                curritem.DrawItem(currect, MenuStyle, Tvoid);
                if (guitools.isinrect(mousepos, currect))
                {
                    inventoryname = curritem.socket.asock.disname;
                }
            }

            guitools.TextureStyle(MenuStyle, Tvoid);
            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
            if (GUI.Button(guitools.GRectRelative(0.9f, currect), "", MenuStyle))
            {
                MouseInvHeld = curritem.InteractItem(MouseInvHeld, theinv, k);
            }

        }
    }
    void DrawInventory(bool force = false)
    {
        if (isinvopen || force)
        {
            GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);
            Rect darect = guitools.GRectRelative(0.25f, 0.5f, 0.5f, 0.5f, SmallerRect);
            Rect line1 = guitools.GRectRelative(0, 0, 1, 0.15f, darect);
            Rect line2 = guitools.GRectRelative(0, 0.15f, 1, 0.2f, darect);
            Rect line3 = guitools.GRectRelative(0, 0.15f + 0.2f, 1, 0.2f, darect);
            Rect line4 = guitools.GRectRelative(0, 0.15f + 0.4f, 1, 0.2f, darect);
            Rect line5 = guitools.GRectRelative(0, 0.15f + 0.6f, 1, 0.05f, darect);
            Rect line6 = guitools.GRectRelative(0, 0.8f, 1, 0.2f, darect);

            Rect selectinv = guitools.GRectRelative(0, 0.1f, 0.25f, 0.8f, line1);



            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));

            GUI.Label(darect, "", MenuStyle);

            if(GUI.Button(guitools.GRectRelative(0,0,1/4f,1,selectinv),"I",MenuStyle))
            {
                invsetting = 0;
            }
            if(GUI.Button(guitools.GRectRelative(1/4f,0,1/4f,1,selectinv),"R",MenuStyle))
            {
                invsetting = 1;
            }

            if (invsetting == 0)
            {
                Rect currect = line2;

                guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
                guitools.TextureStyle(MenuStyle, Twhite);


                GUI.Label(guitools.GRectRelative(0.25f, 0.1f, 0.5f, 0.8f, line1), inventoryname, MenuStyle);
                GUI.Label(guitools.GRectRelative(0.8f, line5), "", MenuStyle);

                guitools.TextureStyle(MenuStyle, Tvoid);
                Inventory theinv = worldgen.maininfo.inv;
                Vector2 mousepos = Converters.MouseToScreen(Input.mousePosition);
                for (int k = 0; k < theinv.inv.Length; k++)
                {
                    currect = line2;
                    if (k > 9)
                    {
                        currect = line3;
                    }
                    if (k > 19)
                    {
                        currect = line4;
                    }
                    if (k > 29)
                    {
                        currect = line6;
                    }
                    int mink = k % 10;
                    currect = guitools.GRectRelative(mink / 10f, 0, 1 / 10f, 1, currect);
                    Inventory.InvItem curritem = theinv.inv[k];
                    if (curritem.num == 0)
                    {
                        theinv.inv[k].socket = new socketIG();
                    }

                    if (k > 29)
                    {
                        controls.toolbar[mink] = theinv.inv[k];
                    }


                    guitools.TextureStyle(MenuStyle, Twhite);
                    guitools.SetStyle(MenuStyle, guitools.RGB(220, 220, 220, 255), Color.black, (int)Math.Round(18f));
                    GUI.Label(guitools.GRectRelative(0.9f, currect), "", MenuStyle);
                    if (curritem.socket.asock.name != "")
                    {
                        curritem.DrawItem(currect, MenuStyle, Tvoid);
                        if (guitools.isinrect(mousepos, currect))
                        {
                            inventoryname = curritem.socket.asock.disname;
                        }
                    }

                    guitools.TextureStyle(MenuStyle, Tvoid);
                    guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
                    if (GUI.Button(guitools.GRectRelative(0.9f, currect), "", MenuStyle))
                    {
                        MouseInvHeld = curritem.InteractItem(MouseInvHeld, theinv, k);

                        controls.toolbar[controls.seltoolbar] = theinv.inv[30 + controls.seltoolbar];

                        if (controls.toolbar[controls.seltoolbar].socket.asock.isgun)
                        {
                            NetworkerPhoton.RPCAnim(false, true, false, false, worldgen.maininfo.photonid);
                        }
                        else
                        {
                            NetworkerPhoton.RPCAnim(false, false, false, false, worldgen.maininfo.photonid);
                        }
                    }
                }
            }

            if (invsetting == 1)
            {
                RawInv theinv = worldgen.maininfo.rawinv;
                Rect lineend = guitools.GRectRelative(0, 0.85f, 1, 0.15f, darect);
                guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(18f));
                GUI.Label(guitools.GRectRelative(0.25f, 0.1f, 0.5f, 0.8f, line1), "Raw Ressource", MenuStyle);
                guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(12f));
                GUI.Label(guitools.GRectRelative(0, 0.1f, 1/3f, 0.8f, lineend, 0.9f), "MASS: " + theinv.readkg + "kg OF "+theinv.maxmass+"kg", MenuStyle);

                if (theinv.inv.Count < 30)
                {
                    for (int k = 0; k < 30; k++)
                    {
                        Rect workingrect = guitools.GRectRelative(0, 0.15f, 1 / 3f, 0.7f, darect);
                        if (k > 9)
                        {
                            workingrect = guitools.GRectRelative(1 / 3f, 0.15f, 1 / 3f, 0.7f, darect);
                        }
                        if (k > 19)
                        {
                            workingrect = guitools.GRectRelative(2 / 3f, 0.15f, 1 / 3f, 0.7f, darect);
                        }
                        int mink = k % 10;
                        Rect slotrect = guitools.GRectRelative(0, mink / 10f, 1, 1 / 10f, workingrect,0.9f);

                        guitools.SetStyle(MenuStyle, guitools.RGB(230, 230, 230, 255), Color.black, (int)Math.Round(10f));
                        GUI.Label(slotrect, "", MenuStyle);

                        if(k < theinv.inv.Count)
                        {
                            guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(8f));
                            GUI.Label(slotrect, "", MenuStyle);
                            guitools.TextureStyle(MenuStyle, Tvoid);
                            MenuStyle.alignment = TextAnchor.MiddleLeft;
                            GUI.Label(guitools.GRectRelative(0, 0, 2 / 3f, 1, slotrect), theinv.inv[k].raw.name + ": ", MenuStyle);
                            MenuStyle.alignment = TextAnchor.MiddleRight;
                            GUI.Label(guitools.GRectRelative(0, 0, 2 / 3f, 1, slotrect),theinv.inv[k].readkg + "kg", MenuStyle);

                            MenuStyle.alignment = TextAnchor.MiddleCenter;
                            guitools.TextureStyle(MenuStyle, TArrowRight);
                            guitools.SetStyle(MenuStyle, guitools.RGB(100, 100, 100, 255), Color.black, (int)Math.Round(10f));
                            GUI.Label(guitools.GRectRelative(0.2f,0,0.6f,1,guitools.GRectRelative(2 / 3f, 0, 1 / 6f, 1, slotrect, 0.9f)), "", MenuStyle);
                            MenuStyle = new GUIStyle(prettyskinloseend.label);
                            guitools.SetStyle(MenuStyle, guitools.RGB(175, 175, 175, 100), Color.black, (int)Math.Round(10f));
                            if (GUI.Button(guitools.GRectRelative(2 / 3f, 0, 1 / 6f, 1, slotrect), "", MenuStyle))
                            {
                                if (isinteracting)
                                {
                                    if (workingon.asock.israwinv)
                                    {
                                        float rest = workingon.asock.getrawinvs.aninv.add(theinv.inv[k].id, theinv.inv[k].kg);
                                        foreach (RawInv.rawlibitem item in theinv.inv)
                                        {
                                            if (item.id == theinv.inv[k].id)
                                            {
                                                item.kg = rest;
                                            }
                                        }
                                    reset:
                                        if (theinv.inv.Count > 0)
                                        {
                                            for (int k2 = 0; k2 < theinv.inv.Count; k2++)
                                            {
                                                if (theinv.inv[k2].kg <= 0)
                                                {
                                                    theinv.inv.RemoveAt(k2);
                                                    goto reset;
                                                }
                                            }
                                        }
                                    }
                                }
                            }



                            MenuStyle.alignment = TextAnchor.MiddleCenter;
                            guitools.TextureStyle(MenuStyle, Twhite);
                        }

                    }
                }
            }

            if (MouseInvHeld.socket.asock.name != "")
            {
                Vector2 mousepos = Converters.MouseToScreen(Input.mousePosition);

                Rect arect = new Rect(mousepos.x, mousepos.y, line2.height, line2.height);

                MouseInvHeld.DrawItem(arect, MenuStyle, Tvoid);
            }

        }
    }

    void DrawRawInv(RawInv araw, socketIG thesock, Rect darect)
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);
        GUI.Label( darect, "", MenuStyle);
        RawInv theinv = araw;
        Rect lineend = guitools.GRectRelative(0, 0.85f, 1, 0.15f, darect);
        guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(10f));
        GUI.Label(guitools.GRectRelative(0, 0, 1, 0.1f, darect,0.9f), "Raw Ressource", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 150, 255), Color.black, (int)Math.Round(10f));
        GUI.Label(guitools.GRectRelative(0, 0.9f, 1, 0.1f, darect, 0.9f), "MASS: " + theinv.readkg + "kg OF " + theinv.maxmass + "kg", MenuStyle);

        if (theinv.inv.Count < 10)
        {
            for (int k = 0; k < 10; k++)
            {
                Rect workingrect = guitools.GRectRelative(0, 0.1f, 1, 0.8f, darect);
                int mink = k % 10;
                Rect slotrect = guitools.GRectRelative(0, mink / 10f, 1, 1 / 10f, workingrect, 0.9f);

                guitools.SetStyle(MenuStyle, guitools.RGB(230, 230, 230, 255), Color.black, (int)Math.Round(7f));
                GUI.Label(slotrect, "", MenuStyle);

                if (k < theinv.inv.Count)
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(5f));
                    GUI.Label(slotrect, "", MenuStyle);
                    guitools.TextureStyle(MenuStyle, Tvoid);
                    MenuStyle.alignment = TextAnchor.MiddleLeft;
                    GUI.Label(guitools.GRectRelative(0, 0, 2 / 3f, 1, slotrect), theinv.inv[k].raw.name + ": ", MenuStyle);
                    MenuStyle.alignment = TextAnchor.MiddleRight;
                    GUI.Label(guitools.GRectRelative(0, 0, 2 / 3f, 1, slotrect), theinv.inv[k].readkg + "kg", MenuStyle);

                    MenuStyle.alignment = TextAnchor.MiddleCenter;
                    guitools.TextureStyle(MenuStyle, TArrowRight);
                    guitools.SetStyle(MenuStyle, guitools.RGB(100, 100, 100, 255), Color.black, (int)Math.Round(7f));
                    GUI.Label(guitools.GRectRelative(0.2f, 0, 0.6f, 1, guitools.GRectRelative(2 / 3f, 0, 1 / 6f, 1, slotrect, 0.9f)), "", MenuStyle);
                    MenuStyle = new GUIStyle(prettyskin.label);
                    guitools.SetStyle(MenuStyle, guitools.RGB(175, 175, 175, 100), Color.black, (int)Math.Round(7f));
                    if (GUI.Button(guitools.GRectRelative(2 / 3f, 0, 1 / 6f, 1, slotrect), "", MenuStyle))
                    {
                        float rest = worldgen.maininfo.rawinv.add(theinv.inv[k].id, theinv.inv[k].kg);
                        foreach (RawInv.rawlibitem item in theinv.inv)
                        {
                            if (item.id == theinv.inv[k].id)
                            {
                                item.kg = rest;
                            }
                        }
                    reset:
                        if (theinv.inv.Count > 0)
                        {
                            for (int k2 = 0; k2 < theinv.inv.Count; k2++)
                            {
                                if (theinv.inv[k2].kg <= 0)
                                {
                                    theinv.inv.RemoveAt(k2);
                                    goto reset;
                                }
                            }
                        }
                    }



                    MenuStyle.alignment = TextAnchor.MiddleCenter;
                    guitools.TextureStyle(MenuStyle, Twhite);
                }

            }
        }
    }

    void DrawCharBuilder()
    {
        CharInfo.skillinfo skillz = worldgen.maininfo.askill;
        float width = w * 2 / 3f;
        float xv = 1 / 15f;
        float yv = 1 / 8f;
        Rect greatrect = guitools.RectFromCenter(new Vector2(w / 2f, h / 2f), width, width * 8 / 15f);

        GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);

        GUI.Label(greatrect, "", MenuStyle);
        guitools.TextureStyle(MenuStyle, Twhite);
        guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(guitools.GRectRelative(xv, yv, xv * 3, yv * 4, greatrect,0.95f), "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(guitools.GRectRelative(2 * xv, 5 * yv, xv, yv, greatrect,0.95f), "Z", MenuStyle);
        GUI.Label(guitools.GRectRelative(1 * xv, 6 * yv, xv, yv, greatrect, 0.95f), "Q", MenuStyle);
        GUI.Label(guitools.GRectRelative(2 * xv, 6 * yv, xv, yv, greatrect, 0.95f), "S", MenuStyle);
        GUI.Label(guitools.GRectRelative(3 * xv, 6 * yv, xv, yv, greatrect, 0.95f), "D", MenuStyle);

        ///Harm
        guitools.SetStyle(MenuStyle, guitools.RGB(255, 215, 0, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(guitools.GRectRelative(1 * xv, 5 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        GUI.Label(guitools.GRectRelative(3 * xv, 5 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        GUI.Label(guitools.GRectRelative(4 * xv, 5 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        GUI.Label(guitools.GRectRelative(4 * xv, 6 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
        if (skillz.actmembers[0] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.act0.text);
            GUI.Label(guitools.GRectRelative(1 * xv, 5 * yv, xv, yv, greatrect, 0.90f), "", MenuStyle);
        }
        if (skillz.actmembers[1] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.act1.text);
            GUI.Label(guitools.GRectRelative(3 * xv, 5 * yv, xv, yv, greatrect, 0.90f), "", MenuStyle);
        }
        if (skillz.actmembers[2] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.act2.text);
            GUI.Label(guitools.GRectRelative(4 * xv, 5 * yv, xv, yv, greatrect, 0.90f), "", MenuStyle);
        }
        if (skillz.actmembers[3] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.act3.text);
            GUI.Label(guitools.GRectRelative(4 * xv, 6 * yv, xv, yv, greatrect, 0.90f), "", MenuStyle);
        }
        guitools.TextureStyle(MenuStyle, Tvoid);
        int rel = -2;
        int index = -1;
        if (GUI.Button(guitools.GRectRelative(1 * xv, 5 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.actmembers[0];
            index = 0;
        }
        if (GUI.Button(guitools.GRectRelative(3 * xv, 5 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.actmembers[1];
            index = 1;
        }
        if (GUI.Button(guitools.GRectRelative(4 * xv, 5 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.actmembers[2];
            index = 2;
        }
        if (GUI.Button(guitools.GRectRelative(4 * xv, 6 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.actmembers[3];
            index = 3;
        }
        if (Event.current.button == 0)
        {
            if (rel != -2)
            {
                if (mouseskillselect == -1)
                {
                    if (rel != -1)
                    {
                        skillz.actmembers[index] = -1;
                        mouseskillact = true;
                        mouseskillselect = rel;
                    }
                }
                else
                {
                    if (mouseskillact)
                    {
                        if (rel == -1)
                        {
                            skillz.actmembers[index] = mouseskillselect;
                            mouseskillselect = -1;
                        }
                        else
                        {
                            skillz.actmembers[index] = mouseskillselect;
                            mouseskillact = true;
                            mouseskillselect = rel;
                        }
                    }
                }
            }
        }
        else
        {
            if (rel != -2)
            {
                skillz.actmembers[index] = -1;
            }
        }

        ///Defend
        
        guitools.TextureStyle(MenuStyle, Twhite);
        guitools.SetStyle(MenuStyle, guitools.RGB(0, 35, 102, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(guitools.GRectRelative(4 * xv, 1 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        GUI.Label(guitools.GRectRelative(4 * xv, 2 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        GUI.Label(guitools.GRectRelative(4 * xv, 3 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        GUI.Label(guitools.GRectRelative(4 * xv, 4 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
        if (skillz.pasmembers[0] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.pas0.text);
            GUI.Label(guitools.GRectRelative(4 * xv, 1 * yv, xv, yv, greatrect, 0.9f), "", MenuStyle);
        }
        if (skillz.pasmembers[1] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.pas1.text);
            GUI.Label(guitools.GRectRelative(4 * xv, 2 * yv, xv, yv, greatrect, 0.9f), "", MenuStyle);
        }
        if (skillz.pasmembers[2] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.pas2.text);
            GUI.Label(guitools.GRectRelative(4 * xv, 3 * yv, xv, yv, greatrect, 0.9f), "", MenuStyle);
        }
        if (skillz.pasmembers[3] != -1)
        {
            guitools.TextureStyle(MenuStyle, skillz.pas3.text);
            GUI.Label(guitools.GRectRelative(4 * xv, 4 * yv, xv, yv, greatrect, 0.9f), "", MenuStyle);
        }
        guitools.TextureStyle(MenuStyle, Tvoid);
        rel = -2;
        index = -1;
        if (GUI.Button(guitools.GRectRelative(4 * xv, 1 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.pasmembers[0];
            index = 0;
        }
        if (GUI.Button(guitools.GRectRelative(4 * xv, 2 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.pasmembers[1];
            index = 1;
        }
        if (GUI.Button(guitools.GRectRelative(4 * xv, 3 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.pasmembers[2];
            index = 2;
        }
        if (GUI.Button(guitools.GRectRelative(4 * xv, 4 * yv, xv, yv, greatrect, 0.95f), "", MenuStyle))
        {
            rel = skillz.pasmembers[3];
            index = 3;
        }

        if (Event.current.button == 0)
        {
            if (rel != -2)
            {
                if (mouseskillselect == -1)
                {
                    if (rel != -1)
                    {
                        skillz.pasmembers[index] = -1;
                        mouseskillact = false;
                        mouseskillselect = rel;
                    }
                }
                else
                {
                    if (!mouseskillact)
                    {
                        if (rel == -1)
                        {
                            skillz.pasmembers[index] = mouseskillselect;
                            mouseskillselect = -1;
                        }
                        else
                        {
                            skillz.pasmembers[index] = mouseskillselect;
                            mouseskillact = false;
                            mouseskillselect = rel;
                        }
                    }
                }
            }
        }
        else
        {
            if (rel != -2)
            {
                skillz.pasmembers[index] = -1;
            }
        }
        ///Right side
        List<int> actlist = new List<int>();
        List<int> paslist = new List<int>();
        for (int k = 0; k < skill.act.Count; k++)
        {
            actlist.Add(k);
        }
        if (skillz.actmembers[0] != -1)
        {
            actlist.Remove(skillz.actmembers[0]);
        }
        if (skillz.actmembers[1] != -1)
        {
            actlist.Remove(skillz.actmembers[1]);
        }
        if (skillz.actmembers[2] != -1)
        {
            actlist.Remove(skillz.actmembers[2]);
        }
        if (skillz.actmembers[3] != -1)
        {
            actlist.Remove(skillz.actmembers[3]);
        }

        for (int k = 0; k < skill.pas.Count; k++)
        {
            paslist.Add(k);
        }
        if (skillz.pasmembers[0] != -1)
        {
            paslist.Remove(skillz.pasmembers[0]);
        }
        if (skillz.pasmembers[1] != -1)
        {
            paslist.Remove(skillz.pasmembers[1]);
        }
        if (skillz.pasmembers[2] != -1)
        {
            paslist.Remove(skillz.pasmembers[2]);
        }
        if (skillz.pasmembers[3] != -1)
        {
            paslist.Remove(skillz.pasmembers[3]);
        }

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
        guitools.TextureStyle(MenuStyle, Twhite);
        GUI.Label(guitools.GRectRelative(5 * xv, 1 * yv, xv, 6 * yv, greatrect, 0.95f), "", MenuStyle);
        GUI.Label(guitools.GRectRelative(6 * xv, 3.5f * yv, 8 * xv, 1 * yv, greatrect, 0.95f), "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(guitools.GRectRelative(6 * xv, 3.5f * yv, 4 * xv, 1 * yv, greatrect, 0.85f), "PROCESS", MenuStyle);
        GUI.Label(guitools.GRectRelative(10 * xv, 3.5f * yv, 4 * xv, 1 * yv, greatrect, 0.85f), "INFLUENCE", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(245, 245, 245, 255), Color.black, (int)Math.Round(18f));

        Rect act = guitools.GRectRelative(6 * xv, 4.5f * yv, 4f * xv, 3f * yv, greatrect);
        Rect pass = guitools.GRectRelative(10 * xv, 4.5f * yv, 4f * xv, 3f * yv, greatrect);
        Rect skille = guitools.GRectRelative(6 * xv, 4.5f * yv - 4 * yv, 8f * xv, 3f * yv, greatrect);

        SkillList(actlist, true, act);
        SkillList(paslist, false, pass);
        if (skillmenuselect != -1)
        {
            SelSkil(skille);
        }


        ////Mouse
        if (mouseskillselect != -1)
        {
            Vector2 mousepos = Converters.MouseToScreen(Input.mousePosition);
            skillmenuact = mouseskillact;
            skillmenuselect = mouseskillselect;
            Rect arect = new Rect(mousepos.x - width / 30f, mousepos.y - width / 30f, width / 15f, width / 15f);
            guitools.SetStyle(MenuStyle, guitools.RGB(0, 35, 102, 255), Color.black, (int)Math.Round(18f));
            if (mouseskillact)
            {
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 215, 0, 255), Color.black, (int)Math.Round(18f));
            }
            guitools.TextureStyle(MenuStyle, Twhite);
            GUI.Label(guitools.GRectRelative(0.95f, arect), "", MenuStyle);

            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
            if (mouseskillact)
            {
                guitools.TextureStyle(MenuStyle, skill.act[mouseskillselect].text);
            }
            else
            {
                guitools.TextureStyle(MenuStyle, skill.pas[mouseskillselect].text);
            }
            GUI.Label(guitools.GRectRelative(0.9f, arect), "", MenuStyle);
        }
    }

    public void SkillList(List<int> alist, bool thebool, Rect arect)
    {
        int skillactslid = Mathf.RoundToInt(skillactslidf);
        int skillpasslid = Mathf.RoundToInt(skillpasslidf);
        /*
        guitools.SetStyle(MenuStyle, guitools.RGB(0, 35, 102, 255), Color.black, (int)Math.Round(18f));
        guitools.SetStyle(MenuStyle, guitools.RGB(255, 215, 0, 255), Color.black, (int)Math.Round(18f));
         */
        float wz = 1 / 4f;
        float wx = 1 / 3f;
        GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);
        GUIStyle MenuStyleKnob = new GUIStyle(prettyskin.verticalSliderThumb);
        guitools.TextureStyle(MenuStyle, Twhite);
        guitools.SetStyle(MenuStyle, guitools.RGB(245, 245, 245, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(arect, "", MenuStyle);
        //guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));


        skill selskill = null;
        if (skillmenuselect != -1)
        {
            if (skillmenuact)
            {
                selskill = skill.act[skillmenuselect];
            }
            else
            {
                selskill = skill.pas[skillmenuselect];
            }
        }
        for (int k = 0; k < 3; k++)
        {
            guitools.TextureStyle(MenuStyle, Twhite);
            skill askill = null;
            if (thebool)
            {
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 215, 0, 255), Color.black, (int)Math.Round(10f));
                if (k + skillactslid >= alist.Count)
                {
                    break;
                }
                askill = skill.act[alist[k + skillactslid]];
            }
            else
            {
                guitools.SetStyle(MenuStyle, guitools.RGB(0, 35, 102, 255), Color.black, (int)Math.Round(10f));
                if (k + skillpasslid >= alist.Count)
                {
                    break;
                }
                askill = skill.pas[alist[k + skillpasslid]];
            }
            GUI.Label(guitools.GRectRelative(0, wx * k, wz, wx, arect, 0.95f), "", MenuStyle);
            guitools.SetStyle(MenuStyle, guitools.RGB(245, 245, 245, 255), Color.black, (int)Math.Round(10f));
            if (skillmenuselect != -1)
            {
                if (skillmenuact)
                {
                    selskill = skill.act[skillmenuselect];
                }
                else
                {
                    selskill = skill.pas[skillmenuselect];
                }
                if (selskill.name == askill.name)
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(10f));
                }
            }
            if (GUI.Button(guitools.GRectRelative(wz, wx * k, 3 * wz, wx, arect, 0.95f), askill.name, MenuStyle))
            {
                skillmenuselect = alist[k + skillpasslid];
                skillmenuact = thebool;
            }
            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(10f));
            guitools.TextureStyle(MenuStyle, askill.text);
            GUI.Label(guitools.GRectRelative(0, wx * k, wz, wx, arect, 0.9f), "", MenuStyle);
            guitools.TextureStyle(MenuStyle, Twhite);
        }

        if (alist.Count > 3)
        {
            if (thebool)
            {
                skillactslidf = Mathf.RoundToInt(GUI.VerticalSlider(guitools.GRectRelative(1 - wz / 8f, 0, wz / 8f, 1, arect, 1), skillactslidf, 0, alist.Count - 3, MenuStyle, MenuStyleKnob));
            }
            else
            {
                skillpasslidf = Mathf.RoundToInt(GUI.VerticalSlider(guitools.GRectRelative(1 - wz / 8f, 0, wz / 8f, 1, arect, 1), skillpasslidf, 0, alist.Count - 3, MenuStyle, MenuStyleKnob));
            }
        }
        else
        {
            skillactslid = 0;
        }
    }
    public void SelSkil(Rect arect)
    {
        CharInfo.skillinfo skillz = worldgen.maininfo.askill;
        float wx = 1 / 8f;
        float wy = 1 / 3f;
        GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);
        guitools.TextureStyle(MenuStyle, Twhite);
        guitools.SetStyle(MenuStyle, guitools.RGB(245, 245, 245, 255), Color.black, (int)Math.Round(18f));
        GUI.Label(arect, "", MenuStyle);
        skill selskill = null;
        if (skillmenuselect != -1)
        {
            if (skillmenuact)
            {
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 215, 0, 255), Color.black, (int)Math.Round(10f));
                selskill = skill.act[skillmenuselect];
            }
            else
            {
                guitools.SetStyle(MenuStyle, guitools.RGB(0, 35, 102, 255), Color.black, (int)Math.Round(10f));
                selskill = skill.pas[skillmenuselect];
            }
        }
        GUI.Label(guitools.GRectRelative(0, 0, wx, wy, arect, 0.95f), "", MenuStyle);
        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(10f));
        guitools.TextureStyle(MenuStyle, selskill.text);
        GUI.Label(guitools.GRectRelative(0, 0, wx, wy, arect, 0.9f), "", MenuStyle);
        guitools.TextureStyle(MenuStyle, Twhite);
        if (GUI.Button(guitools.GRectRelative(0, wy, wx, wy, arect, 0.9f), "SELECT", MenuStyle))
        {
            if (skillmenuact)
            {
                bool isnotthere = true;
                if (skillmenuselect == skillz.actmembers[0] || skillmenuselect == skillz.actmembers[1] || skillmenuselect == skillz.actmembers[2] || skillmenuselect == skillz.actmembers[3])
                {
                    isnotthere = false;
                }
                if (isnotthere)
                {
                    if (skillz.actmembers[0] == -1)
                    {
                        skillz.actmembers[0] = skillmenuselect;
                    }
                    else
                    {
                        if (skillz.actmembers[1] == -1)
                        {
                            skillz.actmembers[1] = skillmenuselect;
                        }
                        else
                        {
                            if (skillz.actmembers[2] == -1)
                            {
                                skillz.actmembers[2] = skillmenuselect;
                            }
                            else
                            {
                                if (skillz.actmembers[3] == -1)
                                {
                                    skillz.actmembers[3] = skillmenuselect;
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                bool isnotthere = true;
                if (skillmenuselect == skillz.pasmembers[0] || skillmenuselect == skillz.pasmembers[1] || skillmenuselect == skillz.pasmembers[2] || skillmenuselect == skillz.pasmembers[3])
                {
                    isnotthere = false;
                }
                if (isnotthere)
                {
                    if (skillz.pasmembers[0] == -1)
                    {
                        skillz.pasmembers[0] = skillmenuselect;
                    }
                    else
                    {
                        if (skillz.pasmembers[1] == -1)
                        {
                            skillz.pasmembers[1] = skillmenuselect;
                        }
                        else
                        {
                            if (skillz.pasmembers[2] == -1)
                            {
                                skillz.pasmembers[2] = skillmenuselect;
                            }
                            else
                            {
                                if (skillz.pasmembers[3] == -1)
                                {
                                    skillz.pasmembers[3] = skillmenuselect;
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
        if (GUI.Button(guitools.GRectRelative(0, 2 * wy, wx, wy, arect, 0.9f), "+", MenuStyle))
        {

        }
    }


    void DrawPowerUps()
    {

        GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);
        Rect rect = guitools.GRectRelative(0.3f, 0, 0.4f, 1, ToolRect);
        guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 100), Color.black, (int)Math.Round(18f));
        GUI.Label(rect, "", MenuStyle);



        for (int k = 0; k < 4; k++)
        {
            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 100), Color.black, (int)Math.Round(18f));
            guitools.TextureStyle(MenuStyle, Tsquareh);
            Rect current = new Rect(rect.x + k * rect.height, rect.y, rect.height, rect.height);
            GUI.Label(current, "", MenuStyle);
            bool isfalse = false;
            skill askill = null;
            if (worldgen.maininfo.askill.actmembers[k] != -1)
            {
                askill = skill.act[worldgen.maininfo.askill.actmembers[k]];
                isfalse = true;
            }
            if (isfalse)
            {
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
                guitools.TextureStyle(MenuStyle, askill.text);
                GUI.Label(current, "", MenuStyle);

                guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 100), Color.black, (int)Math.Round(18f));
                guitools.TextureStyle(MenuStyle, Twhite);
                GUI.Label(guitools.GRectRelative(0.7f, 0.7f, 0.3f, 0.3f, current), "", MenuStyle);
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.white, (int)Math.Round(10f));
                guitools.TextureStyle(MenuStyle, Tvoid);
                string a = "A";
                if(k ==1) a = "E";
                if(k ==2) a = "R";
                if(k ==3) a = "F";
                GUI.Label(guitools.GRectRelative(0.7f, 0.7f, 0.3f, 0.3f, current), a, MenuStyle);

                int timeleft = Mathf.RoundToInt((askill.cooldown) - (Time.time - worldgen.maininfo.askill.actcool[k]));
                if (timeleft > 0)
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 100), Color.black, (int)Math.Round(18f));
                    guitools.TextureStyle(MenuStyle, Twhite);
                    GUI.Label(guitools.GRectRelative(0.8f, current), "", MenuStyle);
                    guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.white, (int)Math.Round(10f));
                    guitools.TextureStyle(MenuStyle, Tvoid);
                    GUI.Label(guitools.GRectRelative(0.8f, current), timeleft.ToString(), MenuStyle);
                }
            }

        }

        guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.white, (int)Math.Round(16f));
        guitools.TextureStyle(MenuStyle, Tvoid);
    }
    void DrawEnemyHealth()
    {
        GUIStyle MenuStyle = new GUIStyle(prettyskinloseend.label);
        guitools.TextureStyle(MenuStyle, Twhite);
        start:
        if (listofEnemy.Count != 0)
        {
            for (int k = 0; k < listofEnemy.Count; k++)
            {
                if (Time.time - enemydamagestamp[k] > 10f)
                {
                    listofEnemy.RemoveAt(k);
                    enemydamagestamp.RemoveAt(k);
                    goto start;
                }
                
                guitools.TextureStyle(MenuStyle, Twhiteround);
                guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 0.5f * 255 * (10 - (Time.time - enemydamagestamp[k])) / 10f), Color.white, (int)Math.Round(16f));
                Vector2 pos = Converters.CamToScreen(listofEnemy[k].thetransvis,cam.cama);
                Rect arect = guitools.RectFromCenter(pos, w / 30f * 3, h / 50f * 4);
                GUI.Label(arect, "", MenuStyle);
                guitools.TextureStyle(MenuStyle, Twhiteroundbord);
                guitools.SetStyle(MenuStyle, guitools.RGB(listofEnemy[k].col.r*255, listofEnemy[k].col.g*255, listofEnemy[k].col.b*255, 255 * (10 - (Time.time - enemydamagestamp[k])) / 10f), Color.white, (int)Math.Round(16f));
                GUI.Label(guitools.GRectRelative(1,arect), "", MenuStyle);
                guitools.TextureStyle(MenuStyle, Tvoid);
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255 * (10 - (Time.time - enemydamagestamp[k])) / 10f), Color.white, (int)Math.Round(16f));
                GUI.Label(guitools.GRectRelative(0, 0, 1, 0.5f, arect), listofEnemy[k].name, MenuStyle);
                guitools.TextureStyle(MenuStyle, Twhiteround);
                Rect life = guitools.GRectRelative(0, 0.55f, 1, 0.4f, arect, 0.9f);
                guitools.TextureStyle(MenuStyle, Twhiteround);
                guitools.SetStyle(MenuStyle, guitools.RGB(75, 0, 0, 255 * (10 - (Time.time - enemydamagestamp[k])) / 10f), Color.white, (int)Math.Round(16f));
                GUI.Label(guitools.GRectRelative(0, 0, listofEnemy[k].healthperc, 1, life), "", MenuStyle);
                guitools.SetStyle(MenuStyle, guitools.RGB(0200, 25, 25, 255 * (10 - (Time.time - enemydamagestamp[k])) / 10f), Color.white, (int)Math.Round(16f));
                GUI.Label(guitools.GRectRelative(0, 0, listofEnemy[k].healthpercdama, 1, life), "", MenuStyle);
                int anint = listofEnemy[k].maxhealthdivs;
                float afloat = listofEnemy[k].maxhealthi / 100f ;
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255 * (10 - (Time.time - enemydamagestamp[k])) / 10f), Color.white, (int)Math.Round(16f));
                guitools.TextureStyle(MenuStyle, Twhite);
                float wii = 0.01f;
                if (anint != 0)
                {
                    for (int k2 = 0; k2 < anint; k2++)
                    {
                        if (k2 != anint - 1)
                        {
                            GUI.Label(guitools.GRectRelative((1 + k2) / (1f * afloat) - wii / 2f, 0.5f, wii, 0.5f, life), "", MenuStyle);
                        }
                    }
                }
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255 * (10 - (Time.time - enemydamagestamp[k])) / 10f), Color.white, (int)Math.Round(16f));
                guitools.TextureStyle(MenuStyle, Twhiteroundbord);
                GUI.Label(guitools.GRectRelative(0, 0, 1, 1, life), "", MenuStyle);
            }
        }
    }

    public class Lexiverse
    {
        public static float w;
        public static float h;
        public List<Page> Book { get; set; }
        public Page current { get; set; }
        public Chapter chap { get; set; }
        public bool ison { get; set; }
        public static GUISkin prettyskin;
        public static GUISkin prettyskinprot;
        public static Texture2D Tvoid;
        public static Texture2D Twhite;
        public static Texture2D Tsquareh;
        int status { get; set; }
        int sus1 { get; set; }
        int sus2 { get; set; }
        string indexsearch { get; set; }
        int anintforscroll { get; set; }

        public Lexiverse(GUISkin astyle,GUISkin astyle2)
        {
            prettyskin = astyle;
            prettyskinprot = astyle2;
            w = Screen.width;
            h = Screen.height;
            ison = false;
            indexsearch = "";
            Book = Build();
            current = Book[0];
            status = 0;
            chap = new Chapter("");
            sus1 = 0;
            sus2 = 0;
            anintforscroll = 0;
        }

        public void DrawLexiverse()
        {
            if (ison)
            {
                GUIStyle MenuStyle = new GUIStyle(prettyskin.label);
                GUIStyle MenuStyleKnob = new GUIStyle(prettyskin.verticalSliderThumb);

                guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
                Rect TotRect = guitools.GRectRelative(0.25f, 0.25f, 0.5f, 0.5f, new Rect(0, 0, w, h));
                TotRect = guitools.GRectRelative(0.25f, -0.125f, 0.5f, 1.125f, TotRect);
                TotRect = guitools.RectFromCenter(new Vector2(w / 2f, h / 2f), TotRect.width, TotRect.width * 11 / 7f);
                Rect MainRect = guitools.GRectRelative(1 / 7f, 1/11f, 6 / 7f, 10 / 11f, TotRect,0.95f);
                Rect TopBar = guitools.GRectRelative(1 / 7f, 0, 6 / 7f, 1 / 11f, TotRect, 0.95f);
                Rect SideBar = guitools.GRectRelative(0, 1 / 11f, 1 / 7f, 10 / 11f, TotRect, 0.95f);

                GUI.Label(guitools.GRectRelative(1, TotRect), "", MenuStyle);
                guitools.SetStyle(MenuStyle, guitools.RGB(075, 075, 075, 255), Color.black, (int)Math.Round(18f));
                GUI.Label(guitools.GRectRelative(0, 0, 1 / 7f, 1 / 11f, TotRect, 0.9f), "", MenuStyle);
                guitools.SetStyle(MenuStyle, guitools.RGB(235, 235, 235, 255), Color.black, (int)Math.Round(18f));
                GUI.Label(guitools.GRectRelative(1, SideBar), "", MenuStyle);
                GUI.Label(guitools.GRectRelative(1, TopBar), "", MenuStyle);
                GUI.Label(guitools.GRectRelative(1,MainRect), "", MenuStyle);

                guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
                if(GUI.Button(guitools.GRectRelative(0,0,1,1/10f, SideBar,0.85f), "I", MenuStyle))
                {
                    status = 1;
                }
                if (GUI.Button(guitools.GRectRelative(0, 1/10f, 1, 1 / 10f, SideBar, 0.85f), "P", MenuStyle))
                {
                    status = 2;
                }


                if(status == 0)
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
                    GUI.Label(guitools.GRectRelative(0.9f, TopBar), "HELP", MenuStyle);
                }
                if (status == 1)
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(18f));
                    GUI.Label(guitools.GRectRelative(0.9f, TopBar), "INDEX SEARCH", MenuStyle);
                    indexsearch = GUI.TextField(guitools.GRectRelative(1 / 6f, 0 / 10f, 4 / 6f, 1 / 10f, MainRect,0.95f), indexsearch, MenuStyle);
                    List<Page> alist = new List<Page>();
                    if (indexsearch != "")
                    {
                        foreach (Page apage in Book)
                        {
                            bool isin = true;

                            if (indexsearch.ToCharArray().Length != 0)
                            {
                                for (int k = 0; k < indexsearch.Length; k++)
                                {
                                    if (k < apage.DispName.Length)
                                    {
                                        if (indexsearch[k].ToString().ToUpper() != apage.DispName[k].ToString().ToUpper())
                                        {
                                            isin = false;
                                        }
                                    }
                                    if (apage.DispName == "")
                                    {
                                        isin = false;
                                    }
                                }
                            }
                            if (isin)
                            {
                                alist.Add(apage);
                            }
                        }
                    }
                    for (int k = 0; k < 9; k++)
                    {
                        if (k < alist.Count)
                        {
                            if (GUI.Button(guitools.GRectRelative(1 / 6f, (1 + k) / 10f, 4 / 6f, 1 / 10f, MainRect,0.95f), alist[k].DispName, MenuStyle))
                            {
                                soundsys.Sack.Play();
                                current = alist[k];
                                status = 2;
                                chap = new Chapter("");
                                sus1 = 0;
                            }
                        }
                    }
                }
                if (status == 2)
                {
                    guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(12f));
                    GUI.Label(guitools.GRectRelative(0.9f, TopBar), "PAGE: "+current.DispName, MenuStyle);
                    if (GUI.Button(guitools.GRectRelative(0 / 3f, 0 / 10f, 1 / 3f, 1 / 10f, MainRect, 0.95f), "DESC.", MenuStyle))
                    {
                        sus1 = 0;
                        soundsys.Sack.Play();
                    }
                    if (current.isinv || current.israw)
                    {
                        if (GUI.Button(guitools.GRectRelative(1 / 3f, 0 / 10f, 1 / 3f, 1 / 10f, MainRect, 0.95f), "CRAFTS", MenuStyle))
                        {
                            sus1 = 1;
                            sus2 = 0;
                            soundsys.Sack.Play();
                        }
                    }
                    switch (sus1)
                    {
                        case 0:
                            if (chap.name == "")
                            {
                                GUI.Label(guitools.GRectRelative(0 / 3f, 1 / 10f, 3 / 3f, 1 / 10f, MainRect, 0.95f), "CHAPTERS:", MenuStyle);
                                if (current.Chapters.Count != 0)
                                {
                                    for (int k = 0; k < 8; k++)
                                    {
                                        if (k < current.Chapters.Count)
                                        {
                                            string astring = "";

                                            switch (current.Chapters[k].name[0].ToString())
                                            {
                                                case "d":
                                                    astring = "Description";
                                                    break;
                                                case "w":
                                                    astring = "Wiki Link";
                                                    break;
                                            }

                                            if (GUI.Button(guitools.GRectRelative(0 / 3f, (2 + k) / 10f, 3 / 3f, 1 / 10f, MainRect, 0.95f), astring, MenuStyle))
                                            {
                                                if (astring == "Wiki Link")
                                                {
                                                    Application.OpenURL(current.Chapters[k].Content[0]);
                                                }
                                                else
                                                {
                                                    soundsys.Sack.Play();
                                                    chap = current.Chapters[k];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (GUI.Button(guitools.GRectRelative(0 / 3f, 1 / 10f, 3 / 3f, 1 / 10f, MainRect, 0.95f), "BACK", MenuStyle))
                                {
                                    soundsys.Scancel.Play();
                                    chap = new Chapter("");
                                }
                                guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(7f));
                                guitools.TextureStyle(MenuStyle, Tvoid);
                                MenuStyle.alignment = TextAnchor.MiddleLeft;
                                if (chap.Content.Count != 0)
                                {
                                    for (int k = 0; k < 28; k++)
                                    {
                                        if (k + anintforscroll < chap.Content.Count)
                                        {
                                            GUI.Label(guitools.GRectRelative(0 / 3f, 2f / 10f + 8 / 10f * ((k) / 30f), 11 / 12f, 8 / 10f * 1 / 30f, MainRect, 1), chap.Content[k + anintforscroll], MenuStyle);
                                        }
                                    }
                                }
                                guitools.TextureStyle(MenuStyle, Twhite);
                                MenuStyle.alignment = TextAnchor.MiddleCenter;
                                if (chap.Content.Count > 28)
                                {
                                    anintforscroll = Mathf.RoundToInt(GUI.VerticalSlider(guitools.GRectRelative(11 / 12f, 0, 1 / 12f, 1, MainRect, 1), anintforscroll, 0, chap.Content.Count - 28, MenuStyle, MenuStyleKnob));
                                }
                                else
                                {
                                    anintforscroll = 0;
                                }

                            }

                            break;
                        case 1:
                            if (GUI.Button(guitools.GRectRelative(0f / 6f, 1 / 10f, 1 / 2f, 1 / 10f, MainRect, 0.95f), "USED IN", MenuStyle))
                            {
                                sus2 = 0;
                            }
                            if (GUI.Button(guitools.GRectRelative(3f / 6f, 1 / 10f, 1 / 2f, 1 / 10f, MainRect, 0.95f), "MADE WITH", MenuStyle))
                            {
                                sus2 = 1;
                            }
                            List<Recipe> reclist = new List<Recipe>();
                            foreach (Recipe arec in Recipe.M01ALL)
                            {
                                bool ising = (sus2 == 0);
                                if (ising && current.israw)
                                {
                                    foreach (RawInv.rawlibitem item in arec.RawItems)
                                    {
                                        if (item.raw.name == current.DispName)
                                        {
                                            reclist.Add(arec);
                                        }
                                    }
                                }
                                if (ising && current.isinv)
                                {
                                    foreach (Inventory.InvItem item in arec.Items)
                                    {
                                        if (item.socket.asock.disname == current.DispName)
                                        {
                                            reclist.Add(arec);
                                        }
                                    }

                                }
                                if (!ising && current.israw)
                                {
                                    foreach (RawInv.rawlibitem item in arec.ResultRaw)
                                    {
                                        if (item.raw.name == current.DispName)
                                        {
                                            reclist.Add(arec);
                                        }
                                    }
                                }
                                if (!ising && current.isinv)
                                {
                                    foreach (Inventory.InvItem item in arec.Result)
                                    {
                                        if (item.socket.asock.disname == current.DispName)
                                        {
                                            reclist.Add(arec);
                                        }
                                    }
                                }
                            }
                            guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(9f));
                            for (int k = 0; k < 8; k++)
                            {
                                if (k + anintforscroll < reclist.Count)
                                {
                                    Rect rect = guitools.GRectRelative(0 / 3f, 2f / 10f + k / 10f, 11 / 12f, 1 / 10f, MainRect, 1);
                                    Vector2 mousepos = Converters.MouseToScreen(Input.mousePosition);
                                    if (guitools.isinrect(mousepos,rect))
                                    {
                                        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(9f));
                                        Rect recret = new Rect(MainRect.x + MainRect.width, MainRect.y, MainRect.width, MainRect.width);
                                        //UI.Label(recret, "", MenuStyle);
                                        DrawRecipeMenu(recret,reclist[k]);

                                        guitools.SetStyle(MenuStyle, guitools.RGB(150, 150, 200, 255), Color.black, (int)Math.Round(9f));
                                    }
                                    GUI.Label(rect,reclist[k].station+": "+ reclist[k].name, MenuStyle);
                                    guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(9f));
                                }
                            }


                            if (reclist.Count > 8)
                            {
                                anintforscroll = Mathf.RoundToInt(GUI.VerticalSlider(guitools.GRectRelative(11 / 12f, 0, 1 / 12f, 1, MainRect, 1), anintforscroll, 0, chap.Content.Count - 8, MenuStyle, MenuStyleKnob));
                            }
                            else
                            {
                                anintforscroll = 0;
                            }

                            break;
                    }
                }
            }
        }
        void DrawRecipeMenu(Rect arect, Recipe recipe)
        {
            GUIStyle MenuStyle = new GUIStyle(prettyskinprot.label);

            guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
            GUI.Label(arect, "", MenuStyle);

            List<string> alist = new List<string>();
            alist.Add("Energy cost: " + recipe.eleccost+"EU");
            alist.Add("Time cost: "+recipe.time);
            alist.Add("Requires:");
            foreach (RawInv.rawlibitem item in recipe.RawItems)
            {
                alist.Add(item.raw.name +": " + item.kg + "kg");
            }
            foreach (Inventory.InvItem item in recipe.Items)
            {
                alist.Add(item.socket.asock.disname + ": " + item.num);
            }
            alist.Add("Produces:");
            foreach (RawInv.rawlibitem item in recipe.ResultRaw)
            {
                alist.Add(item.raw.name + ": " + item.kg + "kg");
            }
            foreach (Inventory.InvItem item in recipe.Result)
            {
                alist.Add(item.socket.asock.disname + ": " + item.num);
            }

            guitools.TextureStyle(MenuStyle, Tsquareh);

            for (int k = -2; k < alist.Count; k++)
            {
                if (k != -2)
                {
                    if (k == -1)
                    {
                        Rect newrect = guitools.GRectRelative(0, 0, 1, 2 / (alist.Count + 2f), arect);
                        guitools.SetStyle(MenuStyle, guitools.RGB(0, 0, 0, 255), Color.black, (int)Math.Round(10f));
                        GUI.Label(guitools.GRectRelative(0, 0, 1, 1, newrect), recipe.station + ": " + recipe.name, MenuStyle);
                        MenuStyle.alignment = TextAnchor.MiddleCenter;
                        guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(10f));
                    }
                    else
                    {
                        guitools.TextureStyle(MenuStyle,Twhite);
                        Rect newrect = guitools.GRectRelative(0, (2 + k) * 1 / (alist.Count + 2f), 1, 1 / (alist.Count + 2f), arect);
                        MenuStyle.alignment = TextAnchor.MiddleLeft;
                        switch (alist[k])
                        {
                            case "Requires:":
                                MenuStyle.alignment = TextAnchor.MiddleCenter;
                                guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(10f));
                                GUI.Label(guitools.GRectRelative(0, 0, 1f, 1, newrect, 0.9f), alist[k], MenuStyle);
                                guitools.SetStyle(MenuStyle, guitools.RGB(200, 150, 150, 255), Color.black, (int)Math.Round(10f));
                                break;
                            case "Produces:":
                                MenuStyle.alignment = TextAnchor.MiddleCenter;
                                guitools.SetStyle(MenuStyle, guitools.RGB(200, 200, 200, 255), Color.black, (int)Math.Round(10f));
                                GUI.Label(guitools.GRectRelative(0, 0, 1f, 1, newrect, 0.9f), alist[k], MenuStyle);
                                guitools.SetStyle(MenuStyle, guitools.RGB(150, 200, 150, 255), Color.black, (int)Math.Round(10f));
                                break;
                            default:
                                GUI.Label(guitools.GRectRelative(0, 0, 1f, 1, newrect,0.9f), alist[k], MenuStyle);
                                break;
                        }
                    }
                }
                MenuStyle.alignment = TextAnchor.MiddleCenter;
            }
        }
        public List<Page> Build()
        {
            List<Page> returner = new List<Page>();
            Page apage;

            apage = new Page("");

            returner.Add(apage);

            apage = new Page("Iron Ingot",true,false);
            apage.Chapters.Add(new Chapter("dironingot"));

            returner.Add(apage);
            apage = new Page("");

            apage = new Page("Iron Dust", true, false);
            apage.Chapters.Add(new Chapter("dirondust"));

            returner.Add(apage);
            apage = new Page("");

            apage = new Page("Iron Ore", true, false);
            apage.Chapters.Add(new Chapter("dironore"));
            apage.Chapters.Add(new Chapter("wironore"));

            returner.Add(apage);
            apage = new Page("");

            return returner;
        }

        public class Page
        {
            public string DispName { get; set; }
            public List<Chapter> Chapters { get; set; }
            public bool israw { get; set; }
            public bool isinv { get; set; }
            public Page(string name, bool r = false, bool i=false)
            {
                DispName = name;
                Chapters = new List<Chapter>();
                israw = r;
                isinv = i;
            }
        }

        public class Chapter
        {
            public string name { get; set; }
            public List<string> Content { get; set; }
            public Chapter(string aname)
            {
                name = aname;
                Content = new List<string>();
                string[] array = ftex.TextToArray("/_lexicon/" + aname);
                if (array.Length != 0)
                {
                    for (int k = 0; k < array.Length; k++)
                    {
                        Content.Add(array[k]);
                    }
                }
            }
        }
    }
}
