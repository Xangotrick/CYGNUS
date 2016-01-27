using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Environment : MonoBehaviour
{
    public static socketIG[,,] cmap { get; set; }
    public static loadmapitem[,] loadmap { get; set; }
    public static int[,] heightmap { get; set; }
    public static List<ElectricGrid> elecgrids { get; set; }
    public static List<MechGrid> mechgrids { get; set; }
    public static List<parcel> parcellist { get; set; }
    public static List<Vector3> markdeath { get; set; }
    public static List<Vector3> markbuild { get; set; }
    public static List<socket> marksocket { get; set; }
    public static List<Vector3> markrot { get; set; }

    public static float refresh;
    public static float genrefreshstamp;

    public static float poorref = Time.time;

    public static void GenEnvironment()
    {
        refresh = 0.5f;
        cmap = new socketIG[512,128,512];
        loadmap = new loadmapitem[cmap.GetLength(0) / 16, cmap.GetLength(2) / 16];
        heightmap = new int[cmap.GetLength(0), cmap.GetLength(02)];
        elecgrids = new List<ElectricGrid>();
        mechgrids = new List<MechGrid>();
        parcellist = new List<parcel>();
        markdeath = new List<Vector3>();
        markbuild = new List<Vector3>();
        marksocket = new List<socket>();
        markrot = new List<Vector3>();

        Vector3 avect = new Vector3();


        Vector3 cpos = convmaptoc(new Vector3(0, 0, 0));
        int[] thecoords = new int[] { Mathf.RoundToInt(cpos.x), Mathf.RoundToInt(cpos.y), Mathf.RoundToInt(cpos.z) };

        int[] chunkcoords = findchunk(thecoords[0], thecoords[2]);

        for (int k1 = 0; k1 < loadmap.GetLength(0); k1++)
        {
            for (int k2 = 0; k2 < loadmap.GetLength(1); k2++)
            {
                loadmap[k1, k2] = new loadmapitem(k1, k2, false);
            }
        }
        for (int k1 = 0; k1 < heightmap.GetLength(0); k1++)
        {
            for (int k2 = 0; k2 < heightmap.GetLength(1); k2++)
            {
                heightmap[k1, k2] = 75;
            }
        }


        for (int k1 = 0; k1 < loadmap.GetLength(0); k1++)
        {
            int[] bounds = boundaries(k1, loadmap.GetLength(1));
            Instantiate((GameObject)Resources.Load("prefab/" + "sinvwall2"), convctomap(new Vector3(bounds[0], 0, bounds[2])), Quaternion.identity);
            bounds = boundaries(k1, -1);
            Instantiate((GameObject)Resources.Load("prefab/" + "sinvwall2"), convctomap(new Vector3(bounds[0], 0, bounds[2])), Quaternion.identity);
        }
        for (int k1 = 0; k1 < loadmap.GetLength(1); k1++)
        {
            int[] bounds = boundaries(loadmap.GetLength(0), k1);
            Instantiate((GameObject)Resources.Load("prefab/" + "sinvwall2"), convctomap(new Vector3(bounds[0], 0, bounds[2])), Quaternion.identity);
            bounds = boundaries(-1, k1);
            Instantiate((GameObject)Resources.Load("prefab/" + "sinvwall2"), convctomap(new Vector3(bounds[0], 0, bounds[2])), Quaternion.identity);
        }
        /*
        for (int k1 = 0; k1 < cmap.GetLength(0); k1++)
        {
            for (int k2 = 0; k2 < cmap.GetLength(1); k2++)
            {
                for (int k3 = 0; k3 < cmap.GetLength(2); k3++)
                {
                    cmap[k1, k2, k3] = new socketIG(socket.socketlib[0], avect, null);
                }
            }
        }

        for (int kx = -7; kx < 7; kx++)
        {
            for (int kz = -7; kz < 7; kz++)
            {
                for (int ky = 0; ky < 65; ky++)
                {
                    //setmapwmap(new Vector3(kx, 0, kz), "dirt");
                    setmapwmap(new Vector3(kx, ky, kz), socket.socketlib[randomite.Next(2) + 1]);
                    setmapwmap(new Vector3(kx, ky, kz), socket.socketlib[randomite.Next(2) + 1]);
                }
            }
        }*/

        //LoadChunk(chunkcoords[0], chunkcoords[1]);


        
        
        
        worldgen.LoadWorld();
        //print("hei");
        /*
        for (int k1 = 0; k1 < cmap.GetLength(0); k1++)
        {
            for (int k2 = 0; k2 < cmap.GetLength(1); k2++)
            {
                for (int k3 = 0; k3 < cmap.GetLength(2); k3++)
                {
                    //setmapwmap(convctomap(new Vector3(k1, k2, k3)), cmap[k1, k2, k3].asock.name);
                }
            }
        }
        */
    }

    public static int[] boundaries(int x, int y)
    {
        return new int[] { x * 16, x * 16 + 15, y * 16, y * 16 + 15 };
    }
    public static int[] findchunk(int x, int y)
    {
        int restx = x % 16;
        int resty = y % 16;

        int modx = (x - restx) / 16;
        int mody = (y - resty) / 16;

        return new int[]{modx, mody};
    }
    /*
    public static void LoadChunk(int x, int y)
    {
        if (!loadmap[x, y].abool)
        {
            int[] bounds = boundaries(x, y);
            Vector3 avect = new Vector3();
            System.Random therand = new System.Random(Environment.loadmap[x, y].seed);

            for (int k2 = Environment.cmap.GetLength(1) - 1; k2 >= 0; k2--)
            {
                for (int k3 = bounds[2]; k3 <= bounds[3]; k3++)
                {
                    for (int k1 = bounds[0]; k1 <= bounds[1]; k1++)
                    {
                        Environment.cmap[k1, k2, k3] = new socketIG(socket.socketlib[0], avect, null);
                        if (k2 < 65 && k2 > 50)
                        {
                            Environment.setmapwmap(Environment.convctomap(new Vector3(k1, k2, k3)), socket.socketlib[therand.Next(2) + 1]);
                        }
                        if (k2 == 50)
                        {
                            Environment.setmapwmap(Environment.convctomap(new Vector3(k1, k2, k3)), "bedrock");
                        }
                    }
                }
            }
            loadmap[x, y].unlock();
        }
    }
    */
    public static void ParcelUpdate()
    {
        if (parcellist.Count > 0)
        {
            for (int kp = 0; kp < parcellist.Count; kp++)
            {
                parcel daparcel = parcellist[kp];
                if (!daparcel.killme)
                {
                    daparcel.nextstep();
                }
            }

        tryagain:
            for (int kp = 0; kp < parcellist.Count; kp++)
            {
                parcel daparcel = parcellist[kp];
                if (daparcel.killme)
                {
                    float left = daparcel.quantity;
                    print(left + elecgrids[daparcel.grid].thelist[daparcel.genint].asock.name);
                    if (left > 0 && elecgrids[daparcel.grid].thelist[daparcel.genint].asock.getelec.maxbuffer > 0)
                    {
                        elecgrids[daparcel.grid].thelist[daparcel.genint].asock.getelec.buffer = Mathf.Min(elecgrids[daparcel.grid].thelist[daparcel.genint].asock.getelec.maxbuffer, elecgrids[daparcel.grid].thelist[daparcel.genint].asock.getelec.buffer + left);
                    }
                    parcellist.RemoveAt(kp);
                    goto tryagain;
                }
            }
        }
    }
    public static void ElecUpdate()
    {
        foreach (ElectricGrid agrid in elecgrids)
        {
            foreach (socketIG Tsock in agrid.thelist)
            {
                if (Tsock.asock.ismech)
                {
                    if (Tsock.asock.getmech.production != 0)
                    {
                        Tsock.asock.getelec.energyuse = Tsock.asock.getmech.production / Tsock.asock.getmech.convratio;
                        Tsock.asock.getelec.intake = Tsock.asock.getelec.energyuse * 3;
                        if (Tsock.asock.getmech.powered)
                        {
                            if (Tsock.asock.getelec.buffer < Tsock.asock.getelec.energyuse)
                            {
                                Tsock.asock.getmech.powered = false;
                                Tsock.asock.getmech.hisgrid(Tsock.location).refreshpower();
                            }
                        }
                        else
                        {
                            if (Tsock.asock.getelec.buffer > 2 * Tsock.asock.getelec.energyuse)
                            {
                                Tsock.asock.getmech.powered = true;
                                Tsock.asock.getmech.hisgrid(Tsock.location).refreshpower();
                            }
                        }
                    }
                }
                if (Tsock.asock.iscraft)
                {
                    if (Tsock.asock.getcraft.autos.Count != 0)
                    {
                        if (Tsock.asock.getcraft.vfloat == -1)
                        {
                            foreach (int anint in Tsock.asock.getcraft.autos)
                            {
                                Recipe rec = Tsock.asock.getcraft.Recipes[anint];
                                bool candoit = true;

                                if (Tsock.asock.getelec.buffer < rec.eleccost)
                                {
                                    candoit = false;
                                }

                                foreach (Inventory.InvItem item in rec.Items)
                                {
                                    if (Tsock.asock.getinvs.aninv.getcount(item.socket.asock.name) < item.num)
                                    {
                                        candoit = false;
                                    }
                                }
                                foreach (RawInv.rawlibitem item in rec.RawItems)
                                {
                                    if (Tsock.asock.getrawinvs.aninv.getcount(item.id) < item.kg)
                                    {
                                        candoit = false;
                                    }
                                }
                                if (candoit)
                                {
                                    Tsock.asock.getelec.buffer -= rec.eleccost;
                                    foreach (Inventory.InvItem item in rec.Items)
                                    {
                                        Tsock.asock.getinvs.aninv.remamount(item.socket.asock.name, item.num);
                                    }
                                    foreach (RawInv.rawlibitem item in rec.RawItems)
                                    {
                                        Tsock.asock.getrawinvs.aninv.rem(item.id, item.kg);
                                    }

                                    Tsock.asock.getcraft.vfloat = Time.time;
                                    Tsock.asock.getcraft.vfloat2 = rec.time;
                                    Tsock.asock.getcraft.Result = rec.Result;
                                    Tsock.asock.getcraft.ResultRaw = rec.ResultRaw;
                                }
                            }
                        }
                        else
                        {
                            if (Time.time - Tsock.asock.getcraft.vfloat > Tsock.asock.getcraft.vfloat2)
                            {
                                ///make
                                foreach (Inventory.InvItem item in Tsock.asock.getcraft.Result)
                                {
                                    for (int k = 0; k < 4; k++)
                                    {
                                        if (Tsock.asock.getinvs.aninv.inv[k].socket.asock.name == item.socket.asock.name)
                                        {
                                            Tsock.asock.getinvs.aninv.inv[k].num += item.num;
                                            break;
                                        }
                                        else
                                        {
                                            if (Tsock.asock.getinvs.aninv.inv[k].num == 0)
                                            {
                                                Tsock.asock.getinvs.aninv.inv[k] = new Inventory.InvItem(item.socket, item.num);
                                                break;
                                            }
                                        }
                                    }
                                }
                                foreach (RawInv.rawlibitem item in Tsock.asock.getcraft.ResultRaw)
                                {
                                    Tsock.asock.getrawinvs.aninv.add(item.id, item.kg);
                                }
                                Tsock.asock.getcraft.vfloat = -1;
                            }
                        }
                    }
                }
                //print(Tsock.asock.getelec.buffer);
                if (Tsock.asock.getelec.energyuse > 0 && Tsock.asock.getelec.energyoutput == 0)
                {
                    print("heree)");
                    if (Tsock.asock.getelec.buffer > Tsock.asock.getelec.maxbuffer)
                    {
                        Tsock.asock.getelec.buffer = Tsock.asock.getelec.maxbuffer;
                    }
                    switch (Tsock.asock.name)
                    {
                        case "light01":
                            Light01Run(Tsock,1,30);
                            break;
                        case "light02":
                            Light01Run(Tsock, 4, 50);
                            break;
                        case "m01miner":
                            M01Miner(Tsock);
                            break;
                        default:
                            if (Tsock.asock.isoxgen)
                            {
                                Tsock.asock.getoxgen.ElecHandling(Tsock);
                            }
                            else
                            {
                                if (Tsock.asock.getelec.buffer > 0)
                                {
                                    Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.energyuse * Time.deltaTime;
                                }
                                else
                                {
                                    Tsock.asock.getelec.buffer = 0;
                                }
                            }



                            break;
                    }


                    //  print(Tsock.asock.getelec.buffer);
                }
            }
        }

        if (Time.time - genrefreshstamp > refresh)
        {
            genrefreshstamp = Time.time;

            ParcelUpdate();

            if (elecgrids.Count > 0)
            {
                for (int kg = 0; kg < elecgrids.Count; kg++)
                {
                    if (elecgrids[kg].thelist.Count > 0)
                    {
                        for (int ks = 0; ks < elecgrids[kg].thelist.Count; ks++)
                        {
                            socketIG Tsock = elecgrids[kg].thelist[ks];
                            
                            switch (Tsock.asock.name)
                            {
                                case "m01gencoal":
                                    M01COALGEN(Tsock);
                                    if (elecgrids[kg].thelist[ks].asock.getelec.energyoutput > 0 && elecgrids[kg].thelist[ks].asock.getvals.vbool)
                                    {
                                        parcellist.Add(new parcel(kg, elecgrids[kg].thelist[ks].asock.getelec.energyoutput * refresh * elecgrids[kg].thelist[ks].asock.getvals.vfloat, ks, new List<int>(),ks));
                                    }
                                    break;
                                case "m01gensteam":
                                    M01STEAMGEN(Tsock);
                                    if (elecgrids[kg].thelist[ks].asock.getelec.energyoutput > 0 && elecgrids[kg].thelist[ks].asock.getvals.vbool)
                                    {
                                        parcellist.Add(new parcel(kg, elecgrids[kg].thelist[ks].asock.getelec.energyoutput * refresh * elecgrids[kg].thelist[ks].asock.getvals.vfloat, ks, new List<int>(), ks));
                                    }
                                    break;
                                case "m01batfesi":
                                    if (Tsock.asock.getelec.buffer > Tsock.asock.getelec.energyoutput && elecgrids[kg].thelist[ks].asock.getvals.vbool)
                                    {
                                        parcellist.Add(new parcel(kg, elecgrids[kg].thelist[ks].asock.getelec.energyoutput * refresh, ks, new List<int>(), ks));
                                        Tsock.asock.getelec.buffer -= elecgrids[kg].thelist[ks].asock.getelec.energyoutput * refresh;
                                    }
                                    break;
                                default:
                                    if (elecgrids[kg].thelist[ks].asock.getelec.energyoutput > 0 && elecgrids[kg].thelist[ks].asock.getvals.vbool)
                                    {
                                        parcellist.Add(new parcel(kg, elecgrids[kg].thelist[ks].asock.getelec.energyoutput * refresh * elecgrids[kg].thelist[ks].asock.getvals.vfloat, ks, new List<int>(), ks));
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
    public static void MechUpdate()
    {
        if (Time.time - poorref > 1)
        {
            poorref = Time.time;

            if (mechgrids.Count > 0)
            {
                for (int kg = 0; kg < mechgrids.Count; kg++)
                {
                    if (mechgrids[kg].thelist.Count > 0)
                    {
                        for (int ks = 0; ks < mechgrids[kg].thelist.Count; ks++)
                        {
                            socketIG Tsock = mechgrids[kg].thelist[ks];
                            if (Tsock.asock.getmech.powered)
                            {
                                switch (Tsock.asock.name)
                                {
                                    case "m01tread":
                                        M01TREAD(Tsock);
                                        break;
                                    case "m01treaddiv":
                                        M01TREADDIV(Tsock);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static void DeathUpdate()
    {
        foreach (Vector3 avect in markdeath)
        {
            RemBlock(avect);
        }
        markdeath = new List<Vector3>();
    }
    public static void BuildUpdate()
    {
        if (markbuild.Count > 0)
        {
            for (int k = 0; k < markbuild.Count; k++)
            {
                AddBlock(markbuild[k], marksocket[k], markrot[k]);
            }
        }
        markbuild = new List<Vector3>();
        marksocket = new List<socket>();
        markrot = new List<Vector3>();
    }


    public static void MarkDead(Vector3 mappos)
    {
        markdeath.Add(mappos);
    }
    public static void MarkBuild(Vector3 mappos, socket asocket, Vector3 vectrot)
    {
        markbuild.Add(mappos);
        marksocket.Add(asocket);
        markrot.Add(vectrot);
    }

    public static void AddBlock(Vector3 mappos, socket tsock, Vector3 eulerrot)
    {
        mappos = new Vector3(Mathf.RoundToInt(mappos.x), Mathf.RoundToInt(mappos.y), Mathf.RoundToInt(mappos.z));
        socket newsocket = socket.copysocket(tsock);
        setmapwmap(mappos, newsocket);
        GameObject theinstance = getmapwmap(mappos).instance;
        if (!tsock.iswire)
        {
            if (tsock.name == "m01tread")
            {
                theinstance.transform.eulerAngles = new Vector3(0, eulerrot.y, eulerrot.z);
            }
            else
            {
                theinstance.transform.eulerAngles = eulerrot;
            }

        }
        socketIG result = new socketIG(newsocket, mappos, theinstance);

        if (newsocket.iselec)
        {
            HandleElecGridAdd(mappos, tsock, result);
            if (newsocket.name == "m01gensteam")
            {
                socketIG under = getmapwmap(mappos - new Vector3(0, 1, 0));
                if (under != null)
                {
                    if (under.asock.name == "basaltsource")
                    {
                        newsocket.getvals.vbool = true;
                    }
                }
            }
            if (newsocket.name == "m01miner")
            {
                socketIG under = getmapwmap(mappos - new Vector3(0, 1, 0));
                if (under != null)
                {
                    if (under.asock.name == "basalt")
                    {
                        newsocket.getvals.vbool = true;
                    }
                }
            }
        }

        if (newsocket.ismech)
        {
            HandleMechGridAdd(mappos, tsock, result);
        }
    }
    public static void HandleElecGridAdd(Vector3 mappos, socket tsock, socketIG result)
    {
        List<int> foundgrid = new List<int>();

        if (elecgrids.Count > 0)
        {
            for (int kg = 0; kg < elecgrids.Count; kg++)
            {
                foreach (socketIG asock in elecgrids[kg].thelist)
                {
                    bool thebool = asock.location == mappos + new Vector3(01, 0, 0) || asock.location == mappos + new Vector3(0, 01, 0) || asock.location == mappos + new Vector3(0, 0, 01) || asock.location == mappos + new Vector3(-1, 0, 0) || asock.location == mappos + new Vector3(0, -1, 0) || asock.location == mappos + new Vector3(0, 0, -1);

                    if (thebool)
                    {
                        foundgrid.Add(kg);
                        break;
                    }
                }
            }
        }


        if (foundgrid.Count == 0)
        {
            ElectricGrid thegrid = new ElectricGrid(result);
            elecgrids.Add(thegrid);
            thegrid.refreshoncreate();
            parcellist = new List<parcel>();
        }

        if (foundgrid.Count == 1)
        {
            elecgrids[foundgrid[0]].thelist.Add(result);
            elecgrids[foundgrid[0]].refreshoncreate();
            parcellist = new List<parcel>();
        }

        if (foundgrid.Count > 1)
        {
            List<socketIG> alist = new List<socketIG>();
            for (int k = 0; k < foundgrid.Count; k++)
            {
                for (int k2 = 0; k2 < elecgrids[foundgrid[k]].thelist.Count; k2++)
                {
                    alist.Add(elecgrids[foundgrid[k]].thelist[k2]);
                }
            }
            alist.Add(result);

            for (int k = elecgrids.Count - 1; k >= 0; k--)
            {
                bool isequal = false;
                foreach (int anint in foundgrid)
                {
                    if (k == anint)
                    {
                        isequal = true;
                    }
                }
                if (isequal)
                {
                    elecgrids.RemoveAt(k);
                }
            }

            elecgrids.Add(new ElectricGrid(alist));
            elecgrids[elecgrids.Count - 1].refreshoncreate();
        }
    }
    public static void HandleMechGridAdd(Vector3 mappos, socket tsock, socketIG result)
    {
        List<int> foundgrid = new List<int>();

        if (mechgrids.Count > 0)
        {
            for (int kg = 0; kg < mechgrids.Count; kg++)
            {
                foreach (socketIG asock in mechgrids[kg].thelist)
                {
                    bool thebool = asock.location == mappos + new Vector3(01, 0, 0) || asock.location == mappos + new Vector3(0, 01, 0) || asock.location == mappos + new Vector3(0, 0, 01) || asock.location == mappos + new Vector3(-1, 0, 0) || asock.location == mappos + new Vector3(0, -1, 0) || asock.location == mappos + new Vector3(0, 0, -1);

                    if (thebool)
                    {
                        foundgrid.Add(kg);
                        break;
                    }
                }
            }
        }


        if (foundgrid.Count == 0)
        {
            MechGrid thegrid = new MechGrid(result);
            mechgrids.Add(thegrid);
            thegrid.refreshoncreate();
            parcellist = new List<parcel>();
        }

        if (foundgrid.Count == 1)
        {
            mechgrids[foundgrid[0]].thelist.Add(result);
            mechgrids[foundgrid[0]].refreshoncreate();
            parcellist = new List<parcel>();
        }

        if (foundgrid.Count > 1)
        {
            List<socketIG> alist = new List<socketIG>();
            for (int k = 0; k < foundgrid.Count; k++)
            {
                for (int k2 = 0; k2 < mechgrids[foundgrid[k]].thelist.Count; k2++)
                {
                    alist.Add(mechgrids[foundgrid[k]].thelist[k2]);
                }
            }
            alist.Add(result);

            for (int k = mechgrids.Count - 1; k >= 0; k--)
            {
                bool isequal = false;
                foreach (int anint in foundgrid)
                {
                    if (k == anint)
                    {
                        isequal = true;
                    }
                }
                if (isequal)
                {
                    mechgrids.RemoveAt(k);
                }
            }

            mechgrids.Add(new MechGrid(alist));
            mechgrids[mechgrids.Count - 1].refreshoncreate();
        }
    }


    public static void RemBlock(Vector3 mappos)
    {
        int[] pos = new int[] { Mathf.RoundToInt(Environment.convmaptoc(mappos).x), Mathf.RoundToInt(Environment.convmaptoc(mappos).z) };
        int[] chunkcoords = Environment.findchunk(pos[0], pos[1]);
        if(loadmap[chunkcoords[0], chunkcoords[1]].abool)
        {
            HandleElecGridRem(mappos);
            HandleMechRem(mappos);
            socketIG thesock = getmapwmap(mappos);

            if (thesock.instance != null)
            {
                if (thesock.instance.transform.parent != null)
                {
                    Destroy(thesock.instance.gameObject.transform.parent.gameObject);
                }
                else
                {
                    Destroy(thesock.instance.gameObject);
                }
            }
            setmapwmap(mappos, "");
        }


    }
    public static void HandleElecGridRem(Vector3 mappos)
    {
        int anelecid = -1;
        int anid = -1;
        if (elecgrids.Count != 0)
        {
            for (int k2 = 0; k2 < elecgrids.Count; k2++)
            {
                if (elecgrids[k2].thelist.Count > 0)
                {
                    for (int k = 0; k < elecgrids[k2].thelist.Count; k++)
                    {
                        if (mappos == elecgrids[k2].thelist[k].location)
                        {
                            anid = k;
                        }
                    }
                }

                if (anid != -1)
                {
                    anelecid = k2;


                    break;
                }
            }
        }

        if (anelecid != -1)
        {

            elecgrids[anelecid].thelist.RemoveAt(anid);

            List<List<socketIG>> theanswer = ElectricGrid.rebuild(elecgrids[anelecid].thelist);
            foreach (List<socketIG> alist in theanswer)
            {
                elecgrids.Add(new ElectricGrid(alist));
            }

            for (int k = elecgrids.Count - 1; k >= elecgrids.Count - theanswer.Count; k--)
            {
                elecgrids[k].refreshoncreate();
            }

            elecgrids.RemoveAt(anelecid);

            parcellist = new List<parcel>();
        }
    }
    public static void HandleMechRem(Vector3 mappos)
    {
        int anmecchid = -1;
        int anid = -1;
        if (mechgrids.Count != 0)
        {
            for (int k2 = 0; k2 < mechgrids.Count; k2++)
            {
                if (mechgrids[k2].thelist.Count > 0)
                {
                    for (int k = 0; k < mechgrids[k2].thelist.Count; k++)
                    {
                        if (mappos == mechgrids[k2].thelist[k].location)
                        {
                            anid = k;
                        }
                    }
                }

                if (anid != -1)
                {
                    anmecchid = k2;


                    break;
                }
            }
        }

        if (anmecchid != -1)
        {

            mechgrids[anmecchid].thelist.RemoveAt(anid);

            List<List<socketIG>> theanswer = ElectricGrid.rebuild(mechgrids[anmecchid].thelist);
            foreach (List<socketIG> alist in theanswer)
            {
                mechgrids.Add(new MechGrid(alist));
            }

            for (int k = mechgrids.Count - 1; k >= mechgrids.Count - theanswer.Count; k--)
            {
                mechgrids[k].refreshoncreate();
            }

            mechgrids.RemoveAt(anmecchid);

            parcellist = new List<parcel>();
        }
    }

    public static socketIG getmapwmap(Vector3 avect)
    {
        Vector3 cvect = convmaptoc(avect);

        return cmap[Mathf.Clamp(Mathf.RoundToInt(cvect.x), 0, cmap.GetLength(0)-1), Mathf.Clamp(Mathf.RoundToInt(cvect.y), 0, cmap.GetLength(1)-1), Mathf.Clamp(Mathf.RoundToInt(cvect.z), 0, cmap.GetLength(2)-1)];
    }

    public static void setmapwmap(Vector3 avect, socket thesock)
    {
        Vector3 cvect = convmaptoc(avect);
        socketIG oldsock = cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)];
        if (oldsock != null)
        {
            if (oldsock.instance != null)
            {
                Destroy(oldsock.instance);
            }
        }
        GameObject obje = (GameObject)Instantiate(thesock.prefab, avect, Quaternion.identity);
        cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)] = new socketIG(thesock, avect, obje);
    }

    public static socketIG setmapwmap(Vector3 avect, string thesock)
    {
        Vector3 cvect = convmaptoc(avect);
        socketIG oldsock = cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)];
        if (oldsock != null)
        {
            if (oldsock.instance != null)
            {
                Destroy(oldsock.instance);
            }
        }


        socket thesocke = new socket();
        foreach (socket asock in socket.socketlib)
        {
            if (asock.name == thesock)
            {
                if (asock.name != "")
                {
                    if (asock.props.Count != 0)
                    {
                        thesocke = socket.copysocket(asock);
                    }
                    else
                    {
                        thesocke = asock;
                    }

                    GameObject obje = (GameObject)Instantiate(thesocke.prefab, avect, Quaternion.identity);
                    return cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)] = new socketIG(thesocke, avect, obje);

                    

                    /*if (thesocke.vis)
                    {
                        Vector3[] vectsarount = new Vector3[] { avect + new Vector3(1, 0, 0), avect + new Vector3(-1, 0, 0), avect + new Vector3(0, 1, 0), avect + new Vector3(0, -1, 0), avect + new Vector3(0, 0, 1), avect + new Vector3(0, 0, -1) };
                        for (int k = 0; k < vectsarount.Length; k++)
                        {
                            socketIG nextsock = getmapwmap(vectsarount[k]);
                            try
                            {
                                if (nextsock.instance == null)
                                {
                                    obje.transform.GetComponent<MeshRenderer>().enabled = true;
                                    break;
                                }
                            }
                            catch (NullReferenceException)
                            {
                                obje.transform.GetComponent<MeshRenderer>().enabled = true;
                                break;
                            }
                        }
                    }*/

                    /*if (thesocke.vis)
                    {
                        socketIG nextsock = getmapwmap(avect + new Vector3(0, 1, 0));
                        if (nextsock == null)
                        {
                            obje.transform.GetComponent<MeshRenderer>().enabled = true;
                        }
                    }*/
                }
                else
                {
                    cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)] = null;
                    return null;
                }
            }
        }
        return null;
    }
    public static socketIG setmapwmapwg(Vector3 avect, string thesock)
    {
        Vector3 cvect = convmaptoc(avect);
        socketIG oldsock = cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)];
        if (oldsock != null)
        {
            if (oldsock.instance != null)
            {
                Destroy(oldsock.instance);
            }
        }


        socket thesocke = new socket();
        foreach (socket asock in socket.socketlib)
        {
            if (asock.name == thesock)
            {
                if (asock.name != "")
                {
                    if (asock.props.Count != 0)
                    {
                        thesocke = socket.copysocket(asock);
                    }
                    else
                    {
                        thesocke = asock;
                    }

                    if (asock.name == "quartz" ||asock.name == "basaltsource")
                    {
                        GameObject obje = (GameObject)Instantiate(thesocke.prefab, avect, Quaternion.identity);
                        return cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)] = new socketIG(thesocke, avect, obje);
                    }
                    else
                    {
                        return cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)] = new socketIG(thesocke, avect, null);
                    }
                }
                else
                {
                    cmap[Mathf.RoundToInt(cvect.x), Mathf.RoundToInt(cvect.y), Mathf.RoundToInt(cvect.z)] = null;
                    return null;
                }
            }
        }
        return null;
    }
    public static Vector3 convctomap(Vector3 core)
    {
        return new Vector3(Mathf.RoundToInt(core.x) - cmap.GetLength(0) / 2, core.y, Mathf.RoundToInt(core.z) - cmap.GetLength(2) / 2);
    }
    public static Vector3 convmaptoc(Vector3 core)
    {
        return new Vector3(Mathf.RoundToInt(core.x) + cmap.GetLength(0) / 2, core.y, Mathf.RoundToInt(core.z) + cmap.GetLength(2) / 2);
    }

    public class parcel
    {
        public int grid { get; set; }
        public int genint { get; set; }
        public float quantity { get; set; }
        public int position { get; set; }
        public List<int> pastpos { get; set; }
        public bool killme { get; set; }

        public parcel(int grida, float quantitya, int posa, List<int> past, int geninte)
        {
            killme = false;
            grid = grida;
            quantity = quantitya;
            position = posa;
            pastpos = past;
            genint = geninte;
        }

        public void nextstep()
        {
            if (elecgrids.Count > grid)
            {
                ElectricGrid thegrid = elecgrids[grid];
                List<int> nextposition = new List<int>();

                nextposition = thegrid.thelist[position].asock.getelec.connecteddef;

                socketIG thesock = thegrid.thelist[position];
                quantity = quantity * thesock.asock.getelec.loss;



                if (thesock.asock.getelec.intake != 0 && thesock.asock.getelec.buffer < thesock.asock.getelec.maxbuffer && position != genint)
                {
                    float rintake = (float)Math.Min(thesock.asock.getelec.intake, Math.Abs(thesock.asock.getelec.maxbuffer - thesock.asock.getelec.intake));


                    if (quantity < rintake * refresh)
                    {
                        //print(quantity);
                        thesock.asock.getelec.buffer += quantity;
                        quantity = 0;
                        nextposition = new List<int>();
                    }
                    else
                    {
                        //print(thesock.asock.getelec.intake * refresh);
                        thesock.asock.getelec.buffer += rintake * refresh;
                        quantity = quantity - rintake * refresh;
                    }
                }
                if (thesock.asock.iswire)
                {
                    thesock.asock.getelec.buffer = quantity;
                }



                List<int> secondlist = new List<int>();
                foreach (int anint in nextposition)
                {
                    bool isequal = false;
                    foreach (int anint2 in pastpos)
                    {
                        if (anint == anint2)
                        {
                            isequal = true;
                        }
                    }
                    if (!isequal)
                    {
                        secondlist.Add(anint);
                    }
                }



                pastpos.Add(position);

                if (secondlist.Count == 1)
                {
                    position = secondlist[0];
                }
                if (secondlist.Count > 1)
                {
                    for (int k = 1; k < secondlist.Count; k++)
                    {
                        parcellist.Insert(0, new parcel(grid, quantity / secondlist.Count, secondlist[k], pastpos,genint));
                    }
                    position = secondlist[0];
                    quantity = quantity / secondlist.Count;
                }
                if (secondlist.Count == 0)
                {
                    killme = true;
                }

            }
        }
    }

    public class loadmapitem
    {
        public bool abool {get;set;}
        public GameObject awall { get; set; }
        public GameObject chuckA { get; set; }
        public GameObject chuckB { get; set; }
        public GameObject chuckC { get; set; }
        public GameObject chuckD { get; set; }
        public int seed { get; set; }

        public loadmapitem(int k1, int k2, bool tabool)
        {
            int[] bounds = boundaries(k1, k2);
            GameObject obje = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "sinvwall"), convctomap(new Vector3(bounds[0], 0, bounds[2])), Quaternion.identity);
            chuckA = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), convctomap(new Vector3(bounds[0], 0, bounds[2])), Quaternion.identity);
            chuckB = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), convctomap(new Vector3(bounds[0] + 8, 0, bounds[2])), Quaternion.identity);
            chuckC = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), convctomap(new Vector3(bounds[0], 0, bounds[2] + 8)), Quaternion.identity);
            chuckD = (GameObject)Instantiate((GameObject)Resources.Load("prefab/" + "chucky"), convctomap(new Vector3(bounds[0] + 8, 0, bounds[2] + 8)), Quaternion.identity);
            awall = obje;
            abool = tabool;
        }

        public void removeat(int xm, int ym, int zm)
        {
            int x = xm%16;
            int y = ym;
            int z = zm % 16;
            if (x < 8)
            {
                if (z < 8)
                {
                    x = x % 8;
                    z = z % 8;
                    chuckA.GetComponent<meshtesting>().DestroyCube(x, y, z);
                }
                else
                {
                    x = x % 8;
                    z = z % 8;
                    chuckC.GetComponent<meshtesting>().DestroyCube(x, y, z);
                }
            }
            else
            {
                if (z < 8)
                {
                    x = x % 8;
                    z = z % 8;
                    chuckB.GetComponent<meshtesting>().DestroyCube(x, y, z);
                }
                else
                {
                    x = x % 8;
                    z = z % 8;
                    chuckD.GetComponent<meshtesting>().DestroyCube(x, y, z);
                }
            }
        }

        public void unlock()
        {
            if (awall != null)
            {
                print("test");
                DestroyObject(awall.gameObject);
            }

            abool = true;
        }
    }


    public static void Light01Run(socketIG Tsock, float inte, float range)
    {

        if (Tsock.asock.getvals.vbool)
        {
            if (Tsock.asock.getelec.buffer > 0)
            {

                Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.energyuse * Time.deltaTime;

                if (Tsock.asock.getelec.buffer > 2 * Tsock.asock.getelec.energyuse * Time.deltaTime)
                {
                    Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.energyuse * Time.deltaTime;
                }
                else
                {
                    Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.buffer / 10f;
                }
            }
            else
            {
                Tsock.asock.getelec.buffer = 0;
            }

            //print(Tsock.asock.getelec.energyuse * Time.deltaTime);  
            Light child = Tsock.instance.transform.GetComponentInChildren<Light>();
            ///NO SOLVE
            if (child != null)
            {
                if (Tsock.asock.getelec.buffer > 0)
                {

                    float thefloat = Math.Min(1, (Tsock.asock.getelec.buffer) / (1f * Tsock.asock.getelec.intake));
                    child.intensity = inte * thefloat;
                    child.range = range * thefloat;
                    Tsock.instance.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", guitools.RGB(Tsock.asock.getvals.vVector3.x * thefloat, Tsock.asock.getvals.vVector3.y * thefloat, Tsock.asock.getvals.vVector3.z * thefloat, 255));

                }
                else
                {
                    child.intensity = 0;
                    Tsock.instance.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", guitools.RGB(0, 0, 0, 255));
                }
            }
        }
    }


    public static void M01Calefactor(socketIG Tsock)
    {
        if (Tsock.asock.getvals.vbool)
        {
            if (Tsock.asock.getelec.buffer > 0)
            {
                Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.energyuse * Time.deltaTime;
            }
            else
            {
                Tsock.asock.getelec.buffer = 0;
            }
        }
        sprop.vals val = Tsock.asock.getvals;
        val.vbool = (val.vfloat != 0);
        if (val.vfloat == 0)
        {
            if (Tsock.asock.getinvs.aninv.inv[0].num != 0)
            {
                val.vfloat = 1;
                switch (Tsock.asock.getinvs.aninv.inv[0].socket.asock.name)
                {
                    case "woodlog":
                        val.vint = 3;
                        break;
                    case "woodplank":
                        val.vint = 2;
                        break;
                    default:
                        val.vint = 1;
                        break;
                }

                Tsock.asock.getinvs.aninv.inv[0].num--;
                val.vfloat = 1;
                if (Tsock.asock.getinvs.aninv.inv[0].num == 0)
                {
                    Tsock.asock.getinvs.aninv.inv[0] = new Inventory.InvItem();
                }
            }
        }
        else
        {
            if (Tsock.asock.getelec.buffer > 0)
            {
                val.vfloat = val.vfloat - val.vint * Time.deltaTime * 1 / 60f;
                Tsock.asock.getrawinvs.aninv.add("Rough Coal", val.vint * Time.deltaTime * 1 / 60f);
            }
        }

        if (val.vfloat < 0)
        {
            val.vfloat = 0;
        }
    }
    public static void M01COALGEN(socketIG Tsock)
    {
        sprop.vals val = Tsock.asock.getvals;
        sprop.rawinvs raw = Tsock.asock.getrawinvs;
        val.vbool = (val.vfloat > 0);
        if (val.vfloat == 0)
        {
            val.vfloat = raw.aninv.getcount("Rough Coal");
            if (val.vfloat != 0)
            {
                val.vint = 1;
                return;
            }
            val.vfloat = raw.aninv.getcount("Coal");
            if (val.vfloat != 0)
            {
                val.vint = 2;
                return;
            }
            val.vfloat = raw.aninv.getcount("Graphite");
            if (val.vfloat != 0)
            {
                val.vint = 3;
                return;
            }
        }
        else
        {
            switch (val.vint)
            {
                case 1:
                    raw.aninv.rem("Rough Coal", refresh / 60f);
                    val.vfloat = raw.aninv.getcount("Rough Coal");
                    break;
                case 2:
                    raw.aninv.rem("Coal", refresh / 120f);
                    val.vfloat = raw.aninv.getcount("Coal");
                    break;
                case 3:
                    raw.aninv.rem("Graphite", refresh / 180f);
                    val.vfloat = raw.aninv.getcount("Graphite");
                    break;
            }
        }
    }
    public static void M01STEAMGEN(socketIG Tsock)
    {
        sprop.vals val = Tsock.asock.getvals;
        sprop.rawinvs raw = Tsock.asock.getrawinvs;
        if (val.vbool)
        {
            raw.aninv.add("Sulfur", refresh / 240f);
        }
    }

    public static void M01Miner(socketIG Tsock)
    {
        if (Tsock.asock.getelec.buffer > 0 && Tsock.asock.getvals.vbool)
        {
            Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.energyuse * Time.deltaTime;
            float rand = charnetworkbehavior.arand.Next(100);
            rand = rand / 100f;
            if (rand < 0.5f)
            {
                if (rand < 0.2f)
                {
                    Tsock.asock.getrawinvs.aninv.add("Rough Coal", Time.deltaTime * 2 / 60f);
                }
                else
                {
                    if (rand < 0.35f)
                    {
                    }
                    else
                    {
                        if (rand < 0.45f)
                        {
                            Tsock.asock.getrawinvs.aninv.add("Silicon", Time.deltaTime * 2 / 60f);
                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
                Tsock.asock.getrawinvs.aninv.add("Iron Oxide", Time.deltaTime * 4 / 60f);
            }
        }
        else
        {
            Tsock.asock.getelec.buffer = 0;
        }
    }

    
    public static void M01TREAD(socketIG Tsock)
    {
        socketIG front = Environment.getmapwmap(Tsock.location + Tsock.instance.transform.forward);
        socketIG back = Environment.getmapwmap(Tsock.location - Tsock.instance.transform.forward);

        if (front != null)
        {
            if (front.asock.isinv)
            {
                for (int k = 0; k < Tsock.asock.getinvs.aninv.inv.Length; k++)
                {
                    if (Tsock.asock.getinvs.aninv.inv[k].num != 0)
                    {
                        front.asock.getinvs.aninv.add(Tsock.asock.getinvs.aninv.inv[k].socket.asock.name, Tsock.asock.getinvs.aninv.inv[k].num);

                        Tsock.asock.getinvs.aninv.inv[k] = new Inventory.InvItem();
                        break;
                    }
                }
            }
            if (front.asock.israwinv)
            {
                if (Tsock.asock.getrawinvs.aninv.inv.Count != 0)
                {
                    front.asock.getrawinvs.aninv.add(Tsock.asock.getrawinvs.aninv.inv[0].id, Tsock.asock.getrawinvs.aninv.inv[0].kg);
                    Tsock.asock.getrawinvs.aninv.inv.RemoveAt(0);
                }
            }
        }

        if (back != null)
        {
            if (back.asock.isinv)
            {
                if (!back.asock.getinvs.aninv.protectsuction)
                {
                    for (int k = 0; k < back.asock.getinvs.aninv.inv.Length; k++)
                    {
                        if (back.asock.getinvs.aninv.inv[k].num != 0)
                        {
                            Tsock.asock.getinvs.aninv.add(back.asock.getinvs.aninv.inv[k].socket.asock.name, 1);

                            if (back.asock.getinvs.aninv.inv[k].num == 1)
                            {
                                back.asock.getinvs.aninv.inv[k] = new Inventory.InvItem();
                            }
                            else
                            {
                                back.asock.getinvs.aninv.inv[k].num--;
                            }
                            break;
                        }
                    }
                }
            }
            if (back.asock.israwinv)
            {
                if (!back.asock.getrawinvs.aninv.protectsuction)
                {
                    if (back.asock.getrawinvs.aninv.inv.Count != 0)
                    {
                        if (back.asock.getrawinvs.aninv.inv[0].kg < 1)
                        {
                            Tsock.asock.getrawinvs.aninv.add(back.asock.getrawinvs.aninv.inv[0].id, 1);
                            back.asock.getrawinvs.aninv.inv.RemoveAt(0);
                        }
                        else
                        {
                            Tsock.asock.getrawinvs.aninv.add(back.asock.getrawinvs.aninv.inv[0].id, 1);
                            back.asock.getrawinvs.aninv.inv[0].kg--;
                        }
                    }
                }
            }
        }
    }
    public static void M01TREADDIV(socketIG Tsock)
    {
        socketIG front = Environment.getmapwmap(Tsock.location + Tsock.instance.transform.forward);
        socketIG right = Environment.getmapwmap(Tsock.location + Tsock.instance.transform.right);
        socketIG left = Environment.getmapwmap(Tsock.location - Tsock.instance.transform.right);
        socketIG back = Environment.getmapwmap(Tsock.location - Tsock.instance.transform.forward);

        int raw = 0;
        int real = 0;

        if (front != null)
        {
            if (front.asock.israwinv)
            {
                raw++;
            }
            if (front.asock.isinv)
            {
                real++;
            }
        }
        if (right != null)
        {
            if (right.asock.israwinv)
            {
                raw++;
            }
            if (right.asock.isinv)
            {
                real++;
            }
        }
        if (left != null)
        {
            if (left.asock.israwinv)
            {
                raw++;
            }
            if (left.asock.isinv)
            {
                real++;
            }
        }
        int num = 0;

        for (int k = 0; k < Tsock.asock.getinvs.aninv.inv.Length; k++)
        {
            if (Tsock.asock.getinvs.aninv.inv[k].num != 0)
            {
                num = Tsock.asock.getinvs.aninv.inv[k].num;
            }
        }

        int[] disthem = new int[real];
        int rest = 0;
        if (real > 1)
        {
            for (int k = 0; k < real - 1; k++)
            {
                disthem[k] = num / real;
                rest += num / real;
            }
            disthem[real - 1] = num - rest;
        }
        else
        {
            disthem[0] = num;
        }


        int id = 0;

        if (front != null)
        {
            if (front.asock.isinv)
            {
                for (int k = 0; k < Tsock.asock.getinvs.aninv.inv.Length; k++)
                {
                    if (Tsock.asock.getinvs.aninv.inv[k].num != 0)
                    {
                        front.asock.getinvs.aninv.add(Tsock.asock.getinvs.aninv.inv[k].socket.asock.name, disthem[id]);
                        id++;
                        break;
                    }
                }
            }
            if (front.asock.israwinv)
            {
                if (Tsock.asock.getrawinvs.aninv.inv.Count != 0)
                {
                    front.asock.getrawinvs.aninv.add(Tsock.asock.getrawinvs.aninv.inv[0].id, Tsock.asock.getrawinvs.aninv.inv[0].kg/ (raw * 1f));
                }
            }
        }
        if (left != null)
        {
            if (left.asock.isinv)
            {
                for (int k = 0; k < Tsock.asock.getinvs.aninv.inv.Length; k++)
                {
                    if (Tsock.asock.getinvs.aninv.inv[k].num != 0)
                    {
                        left.asock.getinvs.aninv.add(Tsock.asock.getinvs.aninv.inv[k].socket.asock.name, disthem[id]);
                        id++;
                        break;
                    }
                }
            }
            if (left.asock.israwinv)
            {
                if (Tsock.asock.getrawinvs.aninv.inv.Count != 0)
                {
                    left.asock.getrawinvs.aninv.add(Tsock.asock.getrawinvs.aninv.inv[0].id, Tsock.asock.getrawinvs.aninv.inv[0].kg / (raw * 1f));
                }
            }
        }
        if (right != null)
        {
            if (right.asock.isinv)
            {
                for (int k = 0; k < Tsock.asock.getinvs.aninv.inv.Length; k++)
                {
                    if (Tsock.asock.getinvs.aninv.inv[k].num != 0)
                    {
                        right.asock.getinvs.aninv.add(Tsock.asock.getinvs.aninv.inv[k].socket.asock.name, disthem[id]);
                        id++;
                        break;
                    }
                }
            }
            if (right.asock.israwinv)
            {
                if (Tsock.asock.getrawinvs.aninv.inv.Count != 0)
                {
                    right.asock.getrawinvs.aninv.add(Tsock.asock.getrawinvs.aninv.inv[0].id, Tsock.asock.getrawinvs.aninv.inv[0].kg / (raw * 1f));
                }
            }
        }


        for (int k = 0; k < Tsock.asock.getinvs.aninv.inv.Length; k++)
        {
            if (Tsock.asock.getinvs.aninv.inv[k].num != 0)
            {
                Tsock.asock.getinvs.aninv.inv[k] = new Inventory.InvItem();
                break;
            }
        }
        if (Tsock.asock.getrawinvs.aninv.inv.Count != 0)
        {
            Tsock.asock.getrawinvs.aninv.inv.RemoveAt(0);
        }


        if (back != null)
        {
            if (back.asock.isinv)
            {
                if (!back.asock.getinvs.aninv.protectsuction)
                {
                    for (int k = 0; k < back.asock.getinvs.aninv.inv.Length; k++)
                    {
                        if (back.asock.getinvs.aninv.inv[k].num != 0)
                        {
                            Tsock.asock.getinvs.aninv.add(back.asock.getinvs.aninv.inv[k].socket.asock.name, back.asock.getinvs.aninv.inv[k].num);

                            back.asock.getinvs.aninv.inv[k] = new Inventory.InvItem();
                            break;
                        }
                    }
                }
            }
            if (back.asock.israwinv)
            {
                if (!back.asock.getrawinvs.aninv.protectsuction)
                {
                    if (back.asock.getrawinvs.aninv.inv.Count != 0)
                    {
                        Tsock.asock.getrawinvs.aninv.add(back.asock.getrawinvs.aninv.inv[0].id, back.asock.getrawinvs.aninv.inv[0].kg);
                        back.asock.getrawinvs.aninv.inv.RemoveAt(0);
                    }
                }
            }
        }
    }
}
