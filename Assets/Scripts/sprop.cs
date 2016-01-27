using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class sprop
{
    public string name { get; set; }
    public sprop()
    {
        name = "";
    }

    public static sprop copysprop(sprop aprop)
    {

        if (aprop is wire)
        {
            sprop.wire returner2 = new sprop.wire();
            returner2.name = aprop.name;
            return returner2;
        }
        if (aprop is interact)
        {
            sprop.interact gelec = aprop as interact;
            sprop.interact returner2 = new sprop.interact();
            returner2.name = gelec.name;
            returner2.abool = gelec.abool;
            return returner2;
        }
        if (aprop is oxgen)
        {
            sprop.oxgen gelec = aprop as oxgen;
            sprop.oxgen returner2 = new sprop.oxgen();
            returner2.name = aprop.name;
            returner2.range = gelec.range;
            return returner2;
        }
        if (aprop is elec)
        {
            sprop.elec gelec = aprop as elec;
            sprop.elec returner2 = new sprop.elec(0);
            returner2.name = aprop.name;
            returner2.maxpower = gelec.maxpower;
            returner2.energyoutput = gelec.energyoutput;
            returner2.energyuse = gelec.energyuse;
            returner2.maxbuffer = gelec.maxbuffer;
            returner2.buffer = gelec.buffer;
            returner2.intake = gelec.intake;
            returner2.loss = gelec.loss;
            for (int k = 0; k < 6; k++)
            {
                returner2.connected[k] = gelec.connected[k];
            }
            foreach (int anint in gelec.connecteddef)
            {
                returner2.connecteddef.Add(anint);
            }
            return returner2;
        }
        if (aprop is mech)
        {
            sprop.mech gelec = aprop as mech;
            sprop.mech returner2 = new sprop.mech(gelec.load, gelec.production,gelec.maxprod,gelec.convratio);
            returner2.name = aprop.name;
            for (int k = 0; k < 6; k++)
            {
                returner2.connected[k] = gelec.connected[k];
            }
            foreach (int anint in gelec.connecteddef)
            {
                returner2.connecteddef.Add(anint);
            }
            return returner2;
        }
        if (aprop is gun)
        {
            sprop.gun gelec = aprop as gun;
            sprop.gun returner2 = new sprop.gun();
            returner2.name = aprop.name;
            returner2.range = gelec.range;
            returner2.stamp = gelec.stamp;
            returner2.reset = gelec.reset;
            return returner2;
        }
        if (aprop is vals)
        {
            vals gvals = aprop as vals;
            vals returner2 = new vals();
            returner2.name = aprop.name;
            returner2.vstring = gvals.vstring;
            returner2.vfloat = gvals.vfloat;
            returner2.vint = gvals.vint;
            returner2.vVector3 = gvals.vVector3;
            returner2.vbool = gvals.vbool;
            return returner2;
        }
        if (aprop is invs)
        {
            invs ginvs = aprop as invs;
            invs returner2 = new invs(ginvs.aninv.inv.Length);
            for (int k = 0; k < ginvs.aninv.inv.Length; k++)
            {
                returner2.aninv.inv[k] = new Inventory.InvItem();
            }
            returner2.aninv.protectsuction = ginvs.aninv.protectsuction;
            return returner2;
        }
        if (aprop is rawinvs)
        {
            rawinvs ginvs = aprop as rawinvs;
            rawinvs returner2 = new rawinvs(ginvs.aninv.maxmass);
            returner2.aninv.protectsuction = ginvs.aninv.protectsuction;
            foreach (RawInv.rawlibitem anint in ginvs.aninv.inv)
            {
                returner2.aninv.add(anint.id, anint.kg);
            }

            return returner2;
        }
        if (aprop is drops)
        {
            drops ginvs = aprop as drops;
            drops returner2 = new drops();
            returner2.aninv.protectsuction = ginvs.aninv.protectsuction;
            foreach (RawInv.rawlibitem anint in ginvs.aninv.inv)
            {
                returner2.aninv.add(anint.id, anint.kg);
            }

            return returner2;
        }
        if (aprop is craft)
        {
            craft ginvs = aprop as craft;
            craft returner2 = new craft(ginvs.RecipeList);

            returner2.vint = ginvs.vint;
            returner2.vfloat = ginvs.vfloat;
            returner2.vfloat2 = ginvs.vfloat2;

            foreach (int anint in ginvs.autos)
            {
                returner2.autos.Add(anint);
            }


            foreach (RawInv.rawlibitem anint in ginvs.ResultRaw)
            {
                returner2.ResultRaw.Add(new RawInv.rawlibitem(anint.id, anint.kg));
            }
            foreach (Inventory.InvItem anint in ginvs.Result)
            {
                returner2.Result.Add(new Inventory.InvItem(anint.socket,anint.num));
            }

            return returner2;
        }
        
        
        sprop returner = new sprop();

        returner.name = aprop.name;

        return returner;
    }



[Serializable]
    public class elec:sprop
    {
        ///rel to fixed update!
        public float maxpower { get; set; }
        public float energyoutput { get; set; }
        public float energyuse { get; set; }
        public float maxbuffer { get; set; }
        public float buffer { get; set; }
        public float intake { get; set; }
        public float loss { get; set; }
        public bool[] connected { get; set; }
        public List<int> connecteddef { get; set; }

        public elec(float maxp, float lossy = 1, float oute = 0, float ini = 0, float euse = 0, float maxbuffere = 0, float buffere = 0)
        {
            name = "elec";
            maxpower = maxp;
            energyoutput = oute;
            buffer = buffere;
            loss = lossy;
            intake = ini;
            energyuse = euse;
            maxbuffer = maxbuffere;

            if (intake != 0 && maxbuffer == 0)
            {
                maxbuffer = 10 * intake;
            }

            connected = new bool[6];
            connecteddef = new List<int>();
            for (int k = 0; k < 6; k++)
            {
                connected[k] = false;
            }
        }

        public void resetcon()
        {
            for (int k = 0; k < 6; k++)
            {
                connected[k] = false;
            }
            connecteddef = new List<int>();
        }
        public void setcon(Vector3 A, Vector3 Other, int otherid)
        {
            string astring = "result ";
            if ((Other - A) == new Vector3(1, 0, 0))
            {
                connected[0] = true;
                connecteddef.Add(otherid);
            }
            else
            {
                if ((Other - A) == new Vector3(-1, 0, 0))
                {
                    connected[1] = true;
                    connecteddef.Add(otherid);
                }
                else
                {
                    if ((Other - A) == new Vector3(0, 1, 0))
                    {
                        connected[2] = true;
                        connecteddef.Add(otherid);
                    }
                    else
                    {
                        if ((Other - A) == new Vector3(0, -1, 0))
                        {
                            connected[3] = true;
                            connecteddef.Add(otherid);
                        }
                        else
                        {
                            if ((Other - A) == new Vector3(0, 0, 01))
                            {
                                connected[4] = true;
                                connecteddef.Add(otherid);
                            }
                            else
                            {
                                if ((Other - A) == new Vector3(0, 0, -1))
                                {
                                    connected[5] = true;
                                    connecteddef.Add(otherid);
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                }
            }

            astring += (Other - A).x.ToString() + "  " + (Other - A).y.ToString() + "   " + (Other - A).z.ToString();
            //controls.printi(astring);
        }
    }
[Serializable]
public class mech : sprop
{
    ///rel to fixed update!
    public float load { get; set; }
    public float production { get; set; }
    public float maxprod { get; set; }
    public float convratio { get; set; }
    public bool powered { get; set; }
    public bool[] connected { get; set; }
    public List<int> connecteddef { get; set; }

    public mech(float loa, float prod,float maxprode, float convratioe)
    {
        name = "mech";
        load = loa;
        production = prod;
        maxprod = maxprode;
        convratio = convratioe;
        powered = false;
        if (load > 0)
        {
            powered = true;
        }

        connected = new bool[6];
        connecteddef = new List<int>();
        for (int k = 0; k < 6; k++)
        {
            connected[k] = false;
        }

    }

    public MechGrid hisgrid(Vector3 mappos)
    {
        if (Environment.mechgrids.Count != 0)
        {
            for (int k2 = 0; k2 < Environment.mechgrids.Count; k2++)
            {
                if (Environment.mechgrids[k2].thelist.Count > 0)
                {
                    for (int k = 0; k < Environment.mechgrids[k2].thelist.Count; k++)
                    {
                        if (mappos == Environment.mechgrids[k2].thelist[k].location)
                        {
                            return Environment.mechgrids[k2];
                        }
                    }
                }
            }
        }

        return null;
    }

    public void resetcon()
    {
        for (int k = 0; k < 6; k++)
        {
            connected[k] = false;
        }
        connecteddef = new List<int>();
    }
    public void setcon(Vector3 A, Vector3 Other, int otherid)
    {
        string astring = "result ";
        if ((Other - A) == new Vector3(1, 0, 0))
        {
            connected[0] = true;
            connecteddef.Add(otherid);
        }
        else
        {
            if ((Other - A) == new Vector3(-1, 0, 0))
            {
                connected[1] = true;
                connecteddef.Add(otherid);
            }
            else
            {
                if ((Other - A) == new Vector3(0, 1, 0))
                {
                    connected[2] = true;
                    connecteddef.Add(otherid);
                }
                else
                {
                    if ((Other - A) == new Vector3(0, -1, 0))
                    {
                        connected[3] = true;
                        connecteddef.Add(otherid);
                    }
                    else
                    {
                        if ((Other - A) == new Vector3(0, 0, 01))
                        {
                            connected[4] = true;
                            connecteddef.Add(otherid);
                        }
                        else
                        {
                            if ((Other - A) == new Vector3(0, 0, -1))
                            {
                                connected[5] = true;
                                connecteddef.Add(otherid);
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
        }

        astring += (Other - A).x.ToString() + "  " + (Other - A).y.ToString() + "   " + (Other - A).z.ToString();
        //controls.printi(astring);
    }
}

[Serializable]
    public class wire:sprop
    {
        public wire()
        {

        }
    }

[Serializable]
public class interact : sprop
{
    public bool abool { get; set; }
    public interact()
    {
        abool = false;
    }
}

[Serializable]
    public class vals : sprop
    {
        public string vstring { get; set; }
        public int vint { get; set; }
        public float vfloat { get; set; }
        public float vfloat2 { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public bool vbool { get; set; }

        public vals(bool ebool = false, Vector3 evect = new Vector3(), int eint = 0, float efloat = 1, string astring = "")
        {
            vstring = astring;
            vint = eint;
            vVector3 = evect;
            vbool = ebool;
            vfloat = efloat;
            vfloat2 = 0;
        }


        public Vector3 vVector3
        {
            get
            {
                return new Vector3(x, y, z);
            }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }
    }

[Serializable]
    public class invs : sprop
    {
        public Inventory aninv { get; set; }
        public invs(int size)
        {
            aninv = new Inventory(size);
        }
    }

[Serializable]
    public class rawinvs : sprop
    {
        public RawInv aninv { get; set; }
        public rawinvs(float max)
        {
            aninv = new RawInv(max);
        }
    }

[Serializable]
    public class oxgen : sprop
    {
        public float range { get; set; }
        public oxgen()
        {
            range = 10;
        }
        public oxgen(float anint)
        {
            range = anint;
        }
        public void ElecHandling(socketIG Tsock)
        {
            if (Tsock.asock.getvals.vbool)
            {
                if (Tsock.asock.getelec.buffer > 0)
                {

                   /// Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.energyuse * Time.deltaTime;

                    if (Tsock.asock.getelec.buffer > 2 * Tsock.asock.getelec.energyuse * Time.deltaTime * Tsock.asock.getvals.vfloat)
                    {
                        Tsock.asock.getelec.buffer = Tsock.asock.getelec.buffer - Tsock.asock.getelec.energyuse * Time.deltaTime * Tsock.asock.getvals.vfloat;
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
            }

            float thefloat = Math.Min(1, (Tsock.asock.getelec.buffer) / (1f * Tsock.asock.getelec.intake)) * Tsock.asock.getvals.vfloat;

            if (!Tsock.asock.getvals.vbool)
            {
                thefloat = 0;
            }

            Tsock.instance.GetComponentInChildren<oxsource>().range = range * thefloat;
        }
    }

[Serializable]
public class gun : sprop
{
    public float range { get; set; }
    public float reset { get; set; }
    public float stamp { get; set; }
    public gun()
    {
        range = 30;
        reset = 0.3f;
        stamp = Time.time;
    }
}

[Serializable]
    public class craft : sprop
{
    public string RecipeList { get; set; }
    public List<int> autos { get; set; }
    public int vint { get; set; }
    public float vfloat { get; set; }
    public float vfloat2 { get; set; }
    public List<Inventory.InvItem> Result { get; set; }
    public List<RawInv.rawlibitem> ResultRaw { get; set; }

    public craft(string RecipeListe)
    {
        vint = 0;
        vfloat = -1;
        vfloat2 = 0;
        Result = new List<Inventory.InvItem>();
        ResultRaw = new List<RawInv.rawlibitem>();
        autos = new List<int>();
        //foreach (Recipe arecipe in RecipeList)
        //{
        //    RecipeList.Add(arecipe);
        //}

        RecipeList = RecipeListe;
    }
    public List<Recipe> Recipes
    {
        get
        {
            switch (RecipeList)
            {
                case "m01crafter":
                    return Recipe.M01Crafter;
                case "m01furnace":
                    return Recipe.M01Furnace;
                case "m01pulverizer":
                    return Recipe.M01Pulverizer;
                case "m01compressor":
                    return Recipe.M01Compressor;
                case "":
                    break;
            }
            return null;
        }
    }
}
[Serializable]
    public class drops : sprop
    {
        public RawInv aninv { get; set; }
        public drops()
        {
            aninv = new RawInv(100);
        }
    }
}
