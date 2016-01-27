using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class worldgen : MonoBehaviour {
    public Transform baseitem;
    public GameObject mainplayer;
    public SaveDataFile savefile;
    public static CharInfo maininfo;
    public static List<Transform> playerbase;
    public static List<Stats> characcess;
    public static float clstamp;
    public static bool loachunks;
    public static System.Random randomite = new System.Random();
    public static float RNGcoordx = 0;
    public static float RNGcoordy = 0;

    public Material[] TESe;

    public static Material[] TES;
	// Use this for initialization
    void Start()
    {
        skill.CreateAct();
        skill.CreatePas();

        TES = new Material[TESe.Length];
        for (int k = 0; k < TESe.Length; k++)
        {
            TES[k] = TESe[k];
        }
        randomite = new System.Random(500);
        RNGcoordx = randomite.Next(0,100)/100f;
        RNGcoordy = randomite.Next(0,100)/100f;
        loachunks = false;
        clstamp = Time.time - 5;
        Raw.buildlib();
        ftex.CreateDatum();
        socket.LibConstruct();
        Recipe.Craft();
        Environment.GenEnvironment();

        maininfo = new CharInfo(null, "", null);

        playerbase = new List<Transform>();
        characcess = new List<Stats>();

        socket.LibConstruct();


        //Texture2D prev = AssetPreview.GetAssetPreview((GameObject)Resources.Load("prefab/" + "sm01compressor"));
        //byte[] bytes = prev.EncodeToPNG();

        //File.WriteAllBytes(ftex.dirsession+"/m01compressor.png", bytes);
        /*
        prev = AssetPreview.GetAssetPreview((GameObject)Resources.Load("prefab/" + "sm01treaddiv"));
        bytes = prev.EncodeToPNG();

        File.WriteAllBytes(ftex.dirsession + "/m01treaddiv.png", bytes);*/
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Environment.ElecUpdate();
        Environment.MechUpdate();
        Environment.BuildUpdate();
        Environment.DeathUpdate();

        UpdateChars();


	}
    void Update()
    {
        if (loachunks && Time.time - clstamp > 5)
        {
            ChunkCheck();
        }
    }

    void ChunkCheck()
    {
        int[] pos = new int[] { Mathf.RoundToInt(Environment.convmaptoc(maininfo.tinstance.transform.position).x), Mathf.RoundToInt(Environment.convmaptoc(maininfo.tinstance.transform.position).z) };
        int[] chunkcoords = Environment.findchunk(pos[0], pos[1]);

        if (!cam.controlmove)
        {
            int x = chunkcoords[0];
            int y = chunkcoords[1];
            Environment.loadmap[x, y].abool = true;
            LoadChunk(x, y);
            //Environment.loadmap[x, y].unlock();
            clstamp = Time.time - -4f;
            return;
        }
        else
        {
            
            for (int kx = -1; kx <= 1; kx++)
            {
                for (int ky = -1; ky <= 1; ky++)
                {
                    int x = chunkcoords[0] + kx;
                    int y = chunkcoords[1] + ky;


                    if (x >= 0 && y >= 0)
                    {
                        if (x < Environment.loadmap.GetLength(0) && y < Environment.loadmap.GetLength(1))
                        {
                            if (!Environment.loadmap[x, y].abool)
                            {
                                Environment.loadmap[x, y].abool = true;
                                LoadChunk(x, y);
                                return;
                            }
                        }
                    }
                }
            }
            
        }
    }


    public void LoadChunk(int x, int y)
    {
        Vector3 avect = new Vector3();
        int[] bounds = Environment.boundaries(x, y);
        StartCoroutine(UpdateChunks(bounds, x, y));
    }

    public IEnumerator UpdateChunks(int[] bounds, int x, int y)
    {
        Vector3 avect = new Vector3();
        System.Random therand = new System.Random(Environment.loadmap[x, y].seed);
        float randx = therand.Next();
        float randy = therand.Next();

        float delayedoriginx = bounds[0];
        float delayedoriginy = bounds[2];

        int depths = 0;
        string[, ,] map = new string[16, 128, 16];
        int[,] height = new int[16, 16];

        float heighvarofchunk = (Mathf.PerlinNoise((RNGcoordx + (x) * 1 / 16f) % 1, (RNGcoordy + (y) * 1 / 16f)) % 1);
        while (Math.Abs(heighvarofchunk) < 1)
        {
            heighvarofchunk = heighvarofchunk * 5;
        }

        for (int k2 = 0; k2 < 128; k2++)
        {
            for (int k1 = 0; k1 <= 15; k1++)
            {
                for (int k3 = 0; k3 <= 15; k3++)
                {
                    map[k1, k2, k3] = "";
                }

            }
        }
        for (int k1 = 0; k1 <= 15; k1++)
        {
            for (int k3 = 0; k3 <= 15; k3++)
            {
                int add = 0;
                float step = 1 / (1f * (16));
                float res = (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k1) * step) % 1, (RNGcoordy + (delayedoriginy + k3) * step)) % 1);
                float res2 = (Mathf.PerlinNoise((RNGcoordx + 0.25f + (delayedoriginx + k1) * step * 0.3f) % 1, (RNGcoordy + 0.25f + (delayedoriginy + k3) * step*0.3f)) % 1);
                while (Math.Abs(res) < 1)
                {
                    res = res * 20 * res2;
                }
                //res = res * 7;
                add = Mathf.RoundToInt(Mathf.Min(10,res *  Mathf.Sign(add)));
                //add = Mathf.RoundToInt((0.5f - Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k1) * step) % 1, (RNGcoordy + (delayedoriginy + k3) * step)) % 1) * delta);
                height[k1, k3] = 70 + add;
            }
        }
        for (int k2 = 0; k2 < 128; k2++)
        {
            float randzx = (float)therand.NextDouble();
            float randzy = (float)therand.NextDouble();
            for (int k1 = 0; k1 <= 15; k1++)
            {
                for (int k3 = 0; k3 <= 15; k3++)
                {
                    int heighti = height[k1, k3];
                    int add = 0;
                    int delta = 7;
                    float step = 1 / (16f) * 1 / 2f; ;
                    float sep = 0.0125f * 3f;
                    sep *= 0.25f;

                    float vaadddemist = (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k1) * step * 0.3f) % 1, (RNGcoordy + (delayedoriginy + k2) * step*0.3f)) % 1) * 2;

                    float vaadd = (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k1) * step) % 1, (RNGcoordy + (delayedoriginy + k2) * step)) % 1);
                    vaadd *= (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k2) * step) % 1, (RNGcoordy + (delayedoriginy + k3) * step)) % 1);
                    vaadd *= (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k3) * step) % 1, (RNGcoordy + (delayedoriginy + k1) * step)) % 1);

                    vaadd *= vaadddemist;

                    float vaaddiore = (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k1) * step) % 1, (RNGcoordy + (delayedoriginy + k2) * step)) % 1);
                    vaaddiore *= (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k2) * step) % 1, (RNGcoordy + (delayedoriginy + k3) * step)) % 1);
                    vaaddiore *= (Mathf.PerlinNoise((RNGcoordx + (delayedoriginx + k3) * step) % 1, (RNGcoordy + (delayedoriginy + k1) * step)) % 1);

                    vaaddiore *= vaadddemist;

                    if (k2 <= heighti && k2 > depths)
                    {
                        if (k2 ==heighti)
                        {
                            ///is ground

                            if (map[k1, k2, k3] == "")
                            {
                                //print(k2);
                                map[k1, k2, k3] = "grass";
                            }
                            if (k2 < 61)
                            {
                                map[k1, k2, k3] = "dirt";
                            }

                            float treeboost = (80 - heighti)/10f;
                            if (treeboost < 0)
                            {
                                treeboost = 0;
                            }
                            //treeboost = 1 - treeboost;
                            if ((float)therand.NextDouble() < 0.0117f * 3 * treeboost)
                            {
                                stringstruct.tree(map, k1, k2 + 1, k3, "woodlog", "", Mathf.RoundToInt(7 * treeboost));
                            }

                            if ((float)therand.NextDouble() < 0.0117f / 3f)
                            {
                                if (map[k1, k2, k3] == "grass")
                                {
                                    map[k1, k2, k3] = "basaltsource";
                                }
                            }

                            if ((float)therand.NextDouble() < 0.0117f / 6f)
                            {
                                stringstruct.boulder(map, k1, k2-1, k3, "stone");
                            }

                        }
                        else
                        {
                            if (map[k1, k2, k3] == "")
                            {
                                map[k1, k2, k3] = "dirt";
                            }
                        }
                        delta = 12;
                        step = 0.15f;
                        add = Mathf.RoundToInt((Mathf.PerlinNoise(k1 * step, k3 * step)) * delta);
                        if (k2 < heighti - add)
                        {
                            map[k1, k2, k3] = "stone";
                        }
                    }

                    ///veins
                    if (k2 < 60 && k2 > 40)
                    {
                        step = 0.1f;
                        float mean = Mathf.PerlinNoise(randzx, randzy);
                        float sep2 = 0.015f;
                        float afadd = (Mathf.PerlinNoise((randzx + k1 * step)%1, (randzy + k3 * step)%1));
                        if (afadd < mean + sep2 && afadd > mean - sep2)
                        {
                            map[k1, k2, k3] = "marbleraw";
                        }
                    }
                    if (k2 < heighti && k2 > 50)
                    {
                        float sepo = 0.0125f * 3f;
                        sepo *= 0.08f;
                        float deltas = 0.35f;
                        float vaaddcore = (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k1) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k2) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k2) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k3) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k3) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k1) * step)) % 1);
                        float perc = 1f;
                        Vector3[] thelist = new Vector3[] { };
                        if (vaaddcore < 0.125f + sepo * perc && vaaddcore > 0.125f - sepo * perc)
                        {
                            map[k1, k2, k3] = "orecoal";
                        }
                    }
                    if (k2 < heighti && k2 > 0)
                    {
                        float sepo = 0.0125f * 3f;
                        sepo *= 0.035f;
                        float deltas = 0.1512354f;
                        float vaaddcore = (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k1) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k2) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k2) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k3) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k3) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k1) * step)) % 1);
                        float perc = 1f;
                        Vector3[] thelist = new Vector3[] { };
                        if (vaaddcore < 0.125f + sepo * perc && vaaddcore > 0.125f - sepo * perc)
                        {
                            map[k1, k2, k3] = "orealuminium";
                        }
                    }
                    if (k2 < 60 && k2 > 30)
                    {
                        float sepo = 0.0125f * 3f;
                        sepo *= 0.075f;
                        float deltas = 0.28f;
                        float vaaddcore = (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k1) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k2) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k2) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k3) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k3) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k1) * step)) % 1);
                        float perc = 1f;
                        Vector3[] thelist = new Vector3[] { };
                        if (vaaddcore < 0.125f + sepo * perc && vaaddcore > 0.125f - sepo * perc)
                        {
                            map[k1, k2, k3] = "orecopper";
                        }
                    }
                    if (k2 < 50 && k2 > 20)
                    {
                        float sepo = 0.0125f * 3f;
                        sepo *= 0.05f;
                        float deltas = 0.5f;
                        float vaaddcore = (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k1) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k2) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k2) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k3) * step)) % 1);
                        vaaddcore *= (Mathf.PerlinNoise((deltas + RNGcoordx + (delayedoriginx + k3) * step) % 1, (deltas + RNGcoordy + (delayedoriginy + k1) * step)) % 1);
                        float perc = 1f;
                        Vector3[] thelist = new Vector3[] { };
                        if (vaaddcore < 0.125f + sepo * perc && vaaddcore > 0.125f - sepo * perc)
                        {
                            map[k1, k2, k3] = "oreiron";
                        }
                    }
                    

                    /////cavves
                    if (k2 < 62 && k2 <= heighti && k2 != 0)
                    {
                        if (k2 > heighti-2)
                        {
                            sep *= 0.125f;
                        }
                        if (vaadd < 0.125f + sep && vaadd > 0.125f - sep)
                        {
                            int thex;
                            int they;
                            int thez;
                            string thestringeee = "";
                            float perc = 1f;
                            Vector3[] thelist = new Vector3[] { };
                            map[k1, k2, k3] = thestringeee;
                            if (vaadd < 0.125f + sep * perc && vaadd > 0.125f - sep * perc)
                            {
                                thelist = new Vector3[] { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };
                                for (int k = 0; k < thelist.Length; k++)
                                {
                                    Vector3 vect = thelist[k];
                                    thex = k1 + Mathf.RoundToInt(vect.x);
                                    they = k2 + Mathf.RoundToInt(vect.y);
                                    thez = k3 + Mathf.RoundToInt(vect.z);
                                    thex = Mathf.Min(Mathf.Max(thex, 0), map.GetLength(0) - 1);
                                    they = Mathf.Min(Mathf.Max(they, 0), map.GetLength(1) - 1);
                                    thez = Mathf.Min(Mathf.Max(thez, 0), map.GetLength(2) - 1);
                                    if (they < 62)
                                    {
                                        map[thex, they, thez] = thestringeee;
                                    }
                                }
                                perc = 0.65f;
                                if ((vaadd < 0.125f + sep * perc && vaadd > 0.125f - sep * perc) ||k2 > 56f)
                                {
                                    thelist = new Vector3[] { new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0), new Vector3(1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, 1), new Vector3(-1, 0, -1), new Vector3(0, 01, 01), new Vector3(0, -1, 01), new Vector3(0, 1, -1), new Vector3(0, -1, -1), new Vector3(1, 1, 1), new Vector3(1, 1, -1), new Vector3(-1, 1, 1), new Vector3(-1, 1, -1), new Vector3(1, -1, 1), new Vector3(-1, -1, 1), new Vector3(1, -1, -1), new Vector3(-1, -1, -1) };
                                    for (int k = 0; k < thelist.Length; k++)
                                    {
                                        Vector3 vect = thelist[k];
                                        thex = k1 + Mathf.RoundToInt(vect.x);
                                        they = k2 + Mathf.RoundToInt(vect.y);
                                        thez = k3 + Mathf.RoundToInt(vect.z);
                                        thex = Mathf.Min(Mathf.Max(thex, 0), map.GetLength(0) - 1);
                                        they = Mathf.Min(Mathf.Max(they, 0), map.GetLength(1) - 1);
                                        thez = Mathf.Min(Mathf.Max(thez, 0), map.GetLength(2) - 1);
                                        if (they < 62)
                                        {
                                            map[thex, they, thez] = thestringeee;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int k2 = 0; k2 < 90; k2++)
        {
            float randzx2 = (float)therand.NextDouble();
            for (int k1 = 0; k1 <= 15; k1++)
            {
                for (int k3 = 0; k3 <= 15; k3++)
                {
                    /*
                    map[k1, k2, k3] = "";
                    int add = 0;
                    int delta = 7;
                    float step = 1 / (16f);
                    add = Mathf.PerlinNoise((RNGcoordx + randzx2 + (delayedoriginx + k1) * step) % 1, (RNGcoordy + randzx2 +(delayedoriginy + k3) * step)) % 1);*/
                    if (map[k1, k2, k3] == "stone" && map[k1, k2 + 1, k3] == "")
                    {
                        if (randomite.Next(0, 16) < 1)
                        {
                            map[k1, k2 + 1, k3] = "quartz";
                        }
                    }
                }
            }
        }


        for (int k1 = 0; k1 <= 15; k1++)
        {
            for (int k3 = 0; k3 <= 15; k3++)
            {
                map[k1, depths, k3] = "bedrock";
            }
        }








        /*
        int counter = 0;
        for (int k2 = 0; k2 < 90; k2++)
        {
            for (int k1 = 0; k1 <= 15; k1++)
            {
                for (int k3 = 0; k3 <= 15; k3++)
                {
                    if (map[k1, k2, k3] == "orecoal")
                    {
                        counter++;
                    }
                }
            }
        }
        print(counter);*/

        //////Load it. End Gen before this

        string[, ,] mapA = new string[8, 128, 8];
        string[, ,] mapB = new string[8, 128, 8];//x
        string[, ,] mapC = new string[8, 128, 8];//y
        string[, ,] mapD = new string[8, 128, 8];//xy
        
        for (int k2 = 0; k2 < 128; k2++)
        {
            for (int k1 = 0; k1 <= 15; k1++)
            {
                for (int k3 = 0; k3 <= 15; k3++)
                {
                    if (k1 < 8)
                    {
                        if (k3 < 8)
                        {
                            mapA[k1, k2, k3] = map[k1, k2, k3];
                        }
                        else
                        {
                            mapC[k1, k2, k3-8] = map[k1, k2, k3];
                        }
                    }
                    else
                    {
                        if (k3 < 8)
                        {
                            mapB[k1-8, k2, k3] = map[k1, k2, k3];
                        }
                        else
                        {
                            mapD[k1-8, k2, k3-8] = map[k1, k2, k3];
                        }
                    }
                }
            }
        }
        Environment.loadmap[x, y].chuckA.GetComponent<meshtesting>().LoadUpdate(mapA);
        Environment.loadmap[x, y].chuckB.GetComponent<meshtesting>().LoadUpdate(mapB);
        Environment.loadmap[x, y].chuckC.GetComponent<meshtesting>().LoadUpdate(mapC);
        Environment.loadmap[x, y].chuckD.GetComponent<meshtesting>().LoadUpdate(mapD);
        /*
        GameObject obje = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), new Vector3(bounds[0]-32, 0, bounds[2]-32), Quaternion.identity);
        obje.transform.GetComponent<meshtesting>().LoadUpdate(mapA);
        GameObject objeB = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), new Vector3(bounds[0]-32+ 8, 0, bounds[2]-32), Quaternion.identity);
        objeB.transform.GetComponent<meshtesting>().LoadUpdate(mapB);
        GameObject objeC = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), new Vector3(bounds[0]-32, 0, bounds[2]-32+ 8), Quaternion.identity);
        objeC.transform.GetComponent<meshtesting>().LoadUpdate(mapC);
        GameObject objeD = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), new Vector3(bounds[0]-32 + 8, 0, bounds[2]-32+ 8), Quaternion.identity);
        objeD.transform.GetComponent<meshtesting>().LoadUpdate(mapD);
        */
        if (cam.controlmove)
        {
            yield return new WaitForSeconds(.1f);
        }
        
        for (int k2 = Environment.cmap.GetLength(1) - 1; k2 >= 0; k2--)
        {
            for (int k1 = bounds[0]; k1 <= bounds[1]; k1++)
            {
                for (int k3 = bounds[2]; k3 <= bounds[3]; k3++)
                {
                    string thestring = map[k1 - bounds[0], k2, k3 - bounds[2]];
                    if (thestring != "")
                    {
                        Environment.setmapwmapwg(Environment.convctomap(new Vector3(k1, k2, k3)), thestring);
                        Environment.heightmap[k1, k3] = height[k1 - bounds[0], k3 - bounds[2]];
                    }
                }
            }
            
            if (cam.controlmove)
            {
                yield return new WaitForSeconds(.1f);
            }
        }

        Destroy(Environment.loadmap[x, y].awall);

        if (!cam.controlmove)
        {
            int add = 0;
            int delta = 7;
            float step = 0.075f;
            add = Mathf.RoundToInt((Mathf.PerlinNoise(Environment.convmaptoc(new Vector3()).x * step, Environment.convmaptoc(new Vector3()).x * step)) * delta);
            add++;
            add++;
            add++;
            add++;
            add++;
            Environment.setmapwmap(new Vector3(0, height[0,0]+1, 0), "chest01");
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[0] = new Inventory.InvItem(new socketIG("dirt"), 35);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[1] = new Inventory.InvItem(new socketIG("met"), 50);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[2] = new Inventory.InvItem(new socketIG("gen01"), 30);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[3] = new Inventory.InvItem(new socketIG("base_wire"), 100);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[4] = new Inventory.InvItem(new socketIG("light01"), 20);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[5] = new Inventory.InvItem(new socketIG("m01airgen"), 5);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[6] = new Inventory.InvItem(new socketIG("chest01"), 5);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[7] = new Inventory.InvItem(new socketIG("slopemet"), 15);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[8] = new Inventory.InvItem(new socketIG("m01gensteam"), 10);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[9] = new Inventory.InvItem(new socketIG("m01gencoal"), 10);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[11] = new Inventory.InvItem(new socketIG("m01miner"), 10);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[12] = new Inventory.InvItem(new socketIG("m01crafter"), 20);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[13] = new Inventory.InvItem(new socketIG("m01motor"), 100);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[14] = new Inventory.InvItem(new socketIG("m01tread"), 100);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1, 0)).asock.getinvs.aninv.inv[15] = new Inventory.InvItem(new socketIG("m01treaddiv"), 100);

            Environment.setmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0), "chest01");
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[0] = new Inventory.InvItem(new socketIG("m01furnace"), 35);
            Environment.getmapwmap(new Vector3(0, height[0,0]+1+ 1, 0)).asock.getinvs.aninv.inv[1] = new Inventory.InvItem(new socketIG("m01pulverizer"), 35);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[2] = new Inventory.InvItem(new socketIG("m01compressor"), 35);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[3] = new Inventory.InvItem(new socketIG("phasegun"), 2);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[4] = new Inventory.InvItem(new socketIG("m01batfesi"), 10);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[5] = new Inventory.InvItem(new socketIG("m01copperwire"), 10);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[6] = new Inventory.InvItem(new socketIG("generator"), 2);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[7] = new Inventory.InvItem(new socketIG("fesicapacitor"), 10);
            Environment.getmapwmap(new Vector3(0, height[0, 0] + 1 + 1, 0)).asock.getinvs.aninv.inv[8] = new Inventory.InvItem(new socketIG("m01machine"), 10);
            //cam.controlmove = true;
            Environment.setmapwmap(new Vector3(0, height[0, 0] + 1 + 1 + 1, 0), "m01machine");
            maininfo.tinstance.GetComponentInChildren<Camera>().enabled = true;
            NetworkerPhoton.ProduceWisp(new Vector3(7, 75, 7));
        }
        //Environment.loadmap[x, y].unlock();
    }


    void UpdateChars()
    {
        if (maininfo.tinstance != null)
        {
            if (maininfo.isdrainingox == 0 && false)
            {
                if (maininfo.oxg < 0)
                {
                    maininfo.oxg = 0;
                }
                if (maininfo.oxg > 0)
                {
                //    maininfo.oxg = maininfo.oxg - Time.deltaTime * 0.158f;
                }
            }
            else
            {
                if (maininfo.oxg < maininfo.maxoxg)
                {
                    maininfo.oxg = maininfo.oxg + 2 * Time.deltaTime * 0.158f * 7.5f;
                }
                if (maininfo.oxg > maininfo.maxoxg)
                {
                    maininfo.oxg = maininfo.maxoxg;
                }
            }
        }
    }

    void SaveWorld()
    {
        savefile = new SaveDataFile();
        string astring = fserial.saveasstring(savefile);
        ftex.ArrayToText("world", new string[]{astring});
    }
    public static void LoadWorld()
    {
        //SaveDataFile asavefile = new SaveDataFile();
        //string astring = fserial.saveasstring(asavefile);
        //ftex.ArrayToText("world", new string[] { astring });
        //SaveDataFile newone = (SaveDataFile)fserial.loadasobj(astring);
        
        //newone.Load();
    }
    public static IEnumerator WaitAndPrint(float waitTime)
    {

        SaveDataFile asavefile = new SaveDataFile();
        string astring = fserial.saveasstring(asavefile);
        ftex.ArrayToText("world", new string[] { astring });
        SaveDataFile newone = (SaveDataFile)fserial.loadasobj(astring);
        print("end");

        yield return new WaitForSeconds(waitTime);
    }

    public class stringstruct
    {
        public static void tree(string[, ,] map, int x, int y, int z, string wood, string leaf, int height)
        {
            int spanx = 2;
            int spanz = 2;
            if (leaf == "")
            {
                spanx = 0;
                spanz = 0;
            }



            int toptrunk = y + height - 1;

            if (leaf != "")
            {
                x = Mathf.Max(x, spanx);
                z = Mathf.Max(z, spanz);
                x = Mathf.Min(x, 15 - spanx);
                z = Mathf.Min(z, 15 - spanz);

                map[x, toptrunk + 1, z] = leaf;
                map[x, toptrunk + 1, z + 1] = leaf;
                map[x, toptrunk + 1, z - 1] = leaf;
                map[x + 1, toptrunk + 1, z] = leaf;
                map[x - 1, toptrunk + 1, z] = leaf;


                for (int kx = -2; kx < 3; kx++)
                {
                    for (int kz = -2; kz < 3; kz++)
                    {
                        int relex = x + kx;
                        int relez = z + kz;
                        if (kx < 2 && kx > -2 && kz < 2 && kz > -2)
                        {
                            map[relex, toptrunk, relez] = leaf;
                        }
                        map[relex, toptrunk - 1, relez] = leaf;
                        map[relex, toptrunk - 2, relez] = leaf;
                    }
                }
            }

            for (int k = 0; k < height; k++)
            {
                map[x, y + k, z] = wood;
            }
        }
        public static void boulder(string[, ,] map, int x, int y, int z, string stone)
        {
            int spanx = 2;
            int spanz = 2;

            x = Mathf.Max(x, spanx);
            z = Mathf.Max(z, spanz);
            x = Mathf.Min(x, 15 - spanx);
            z = Mathf.Min(z, 15 - spanz);

            //bool[] abcd = new bool[] { };
            int lazy = 0;
            map[x, y + lazy, z] = stone;
            map[x - 1, y + lazy, z] = stone;
            map[x, y + lazy, z + 1] = stone;

            lazy = 1;
            map[x, y+lazy, z] = stone;
            map[x - 1, y + lazy, z] = stone;
            map[x, y + lazy, z + 1] = stone;
            map[x - 1, y + lazy, z + 1] = stone;

            map[x - 1, y + lazy, z + 2] = stone;
            map[x, y + lazy, z + 2] = stone;
            map[x - 1, y + lazy, z + -1] = stone;
            map[x, y + lazy, z + -1] = stone;

            map[x + 1, y + lazy, z + 1] = stone;
            map[x + 1, y + lazy, z + 0] = stone;
            map[x + -2, y + lazy, z + 1] = stone;
            map[x + -2, y + lazy, z + 0] = stone;

            lazy = 2;
            map[x, y + lazy, z] = stone;
            map[x - 1, y + lazy, z] = stone;
            map[x, y + lazy, z + 1] = stone;
            map[x - 1, y + lazy, z + 1] = stone;

            map[x - 1, y + lazy, z + -1] = stone;

            lazy = 3;
            map[x, y + lazy, z] = stone;
            map[x - 1, y + lazy, z] = stone;

            lazy = 4;
            map[x, y + lazy, z] = stone;
            
        }
    }

    public static bool isvis(string astring)
    {
        List<string> alist = new List<string>();
        alist.Add("ice");
        if (astring == "")
        {
            return true;
        }
        foreach (string tastring in alist)
        {
            if (tastring == astring)
            {
                return true;
            }
        }
        return false;
    }
}
