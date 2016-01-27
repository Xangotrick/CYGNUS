using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;


[Serializable]
public class socket {
    public string name { get; set; }
    public string prefabs { get; set; }
    //public Texture2D icon { get; set; }
    public string sound { get; set; }
    public string disname { get; set; }
    public bool breakable { get; set; }
    public bool isobject { get; set; }
    public bool mineable { get; set; }
    public List<sprop> props { get; set; }
    public static List<socket> socketlib = new List<socket>();

    public socket()
    {
        name = "";
        props = new List<sprop>();
        breakable = true;
        mineable = true;
    }
    public socket(string anid, string prefa, string dispname = "", string sounde = "")
    {
        isobject = false;
        name = anid;
        prefabs = prefa;
        props = new List<sprop>();
        sound = sounde;
        disname = dispname;
        if (disname == "")
        {
            disname = name;
        }
        if (sound == "")
        {
            sound = "wtap";
        }
        breakable = true;
        mineable = true;
    }

    public Texture2D icon
    {
        get
        {
            return gettext(name);
        }
    }

    ///Construct List
    public static void LibConstruct()
    {
        socketlib = new List<socket>();
        BaseConstruct();
        NatureConstruct();
        MK1Construct();
    }
    public static void BaseConstruct()
    {

        socket thesocket = new socket();

        socketlib.Add(thesocket);

        thesocket = new socket("met", "smet", "Metal Plate", "mclang");

        socketlib.Add(thesocket);
        thesocket = new socket("slopemet", "sslopemet", "Metal Slope");

        socketlib.Add(thesocket);

        thesocket = new socket("woodplank", "swoodplank", "Oak Plank");
        socketlib.Add(thesocket);
        /*
        thesocket = new socket();

        socketlib.Add(thesocket);
        */
    }
    public static void NatureConstruct()
    {

        socket thesocket = new socket();

        socketlib.Add(thesocket);


        thesocket = new socket("dirt", "sdirt", "Dirt");
        thesocket.mineable = false;

        socketlib.Add(thesocket);

        thesocket = new socket("bedrock", "sbedrock", "DevBlock");
        thesocket.breakable = false;

        socketlib.Add(thesocket);
        thesocket = new socket("basalt", "sbasalt", "Ore");
        thesocket.props.Add(new sprop.drops());
        thesocket.getdrop.aninv.add("Iron Ore", 4f);
        thesocket.mineable = false;

        socketlib.Add(thesocket);
        thesocket = new socket("basaltsource", "sbasaltsource", "Geyser ");
        thesocket.breakable = false;

        socketlib.Add(thesocket);

        thesocket = new socket("grass", "sgrass", "Snow");
        thesocket.mineable = false;

        socketlib.Add(thesocket);

        thesocket = new socket("stone", "sstone", "Stone");
        thesocket.mineable = false;

        socketlib.Add(thesocket);

        thesocket = new socket("marbleraw", "smarbleraw", "Raw Marble");

        socketlib.Add(thesocket);

        thesocket = new socket("woodleaf", "swoodleaf", "Oak Leaf");
        thesocket.mineable = false;
        socketlib.Add(thesocket);

        thesocket = new socket("woodlog", "swoodlog", "Alpine Log");
        socketlib.Add(thesocket);

        thesocket = new socket("quartz", "squartz", "Quartz");
        thesocket.props.Add(new sprop.drops());
        thesocket.mineable = false;
        thesocket.getdrop.aninv.add("Quartz Crystal", 0.5f);
        socketlib.Add(thesocket);

        thesocket = new socket("oreiron", "soreiron", "Iron Ore");
        thesocket.props.Add(new sprop.drops());
        thesocket.getdrop.aninv.add("Iron Ore", 4f);
        thesocket.mineable = false;
        socketlib.Add(thesocket);

        thesocket = new socket("orecopper", "sorecopper", "Copper Ore");
        thesocket.props.Add(new sprop.drops());
        thesocket.getdrop.aninv.add("Copper Ore", 4f);
        thesocket.mineable = false;
        socketlib.Add(thesocket);

        thesocket = new socket("orecoal", "sorecoal", "Coal Ore");
        thesocket.props.Add(new sprop.drops());
        thesocket.getdrop.aninv.add("Rough Coal", 4f);
        thesocket.mineable = false;
        socketlib.Add(thesocket);

        thesocket = new socket("orealuminium", "sorealuminium", "Aluminium Ore");
        thesocket.props.Add(new sprop.drops());
        thesocket.getdrop.aninv.add("Aluminium Ore", 4f);
        thesocket.mineable = false;
        socketlib.Add(thesocket);


        thesocket = new socket("ice", "sice", "Ice");
        thesocket.mineable = false;
        socketlib.Add(thesocket);

        thesocket = new socket("debug", "sdebug", "debug");
        thesocket.mineable = false;
        socketlib.Add(thesocket);
        /*
        thesocket = new socket();

        socketlib.Add(thesocket);
        */
    }

    public static void MK1Construct()
    {

        socket thesocket = new socket();

        thesocket = new socket("m01machine", "sm01machine", "Machine Block");
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);

        thesocket = new socket("chest01", "schest01", "16 Slot Chest");
        thesocket.props.Add(new sprop.invs(16));
        thesocket.props.Add(new sprop.rawinvs(16));
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);


        thesocket = new socket("base_wire", "sbwire", "Basic Wire", "mclang");
        thesocket.props.Add(new sprop.elec(100, 1));
        thesocket.props.Add(new sprop.wire());
        socketlib.Add(thesocket);

        thesocket = new socket("m01copperwire", "sm01copperwire", "Copper Wire", "mclang");
        thesocket.props.Add(new sprop.elec(100, 0.9f));
        thesocket.props.Add(new sprop.wire());
        socketlib.Add(thesocket);

        thesocket = new socket();
        thesocket = new socket("gen01", "sgen01", "Infinite Generator", "mclang");
        thesocket.props.Add(new sprop.elec(100, 1, 400));
        thesocket.props.Add(new sprop.vals(true, new Vector3(), 1, 1));
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);

        thesocket = new socket();
        thesocket = new socket("light01", "slight01", "Lamp", "mclang");
        thesocket.props.Add(new sprop.elec(100, 1, 0, 100, 25, 300));
        thesocket.props.Add(new sprop.vals(true, new Vector3(255, 255, 255)));
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);


        //
        thesocket = new socket();
        thesocket = new socket("light02", "slight02", "Power Lamp", "mclang");
        thesocket.props.Add(new sprop.elec(100, 1, 0, 800, 400, 2000));
        thesocket.props.Add(new sprop.vals(true, new Vector3(255, 255, 255)));
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);

        //
        thesocket = new socket();
        thesocket = new socket("m01airgen", "sm01airgen", "Basic Air Supply", "oxleak");
        thesocket.props.Add(new sprop.elec(100, 1, 0, 800, 400, 2000));
        thesocket.props.Add(new sprop.oxgen(15));
        thesocket.props.Add(new sprop.vals(true, new Vector3(), 1, 1));
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);

        thesocket = new socket("m01crafter", "sm01crafter", "Crafting Station");
        thesocket.props.Add(new sprop.invs(4));
        thesocket.props.Add(new sprop.rawinvs(30));
        thesocket.props.Add(new sprop.interact());
        thesocket.props.Add(new sprop.elec(100, 1, 0, 100, 0, 400));
        thesocket.props.Add(new sprop.craft("m01crafter"));
        socketlib.Add(thesocket);

        thesocket = new socket("m01gencoal", "sm01gencoal", "Coal Generator");
        thesocket.props.Add(new sprop.rawinvs(30));
        thesocket.getrawinvs.aninv.protectsuction = true;
        thesocket.props.Add(new sprop.interact());
        thesocket.props.Add(new sprop.elec(100, 1, 1200));
        thesocket.props.Add(new sprop.vals(false, new Vector3(), 1, 0));
        socketlib.Add(thesocket);

        thesocket = new socket("m01gensteam", "sm01gensteam", "Steam Generator");
        thesocket.props.Add(new sprop.elec(100, 1, 600));
        thesocket.props.Add(new sprop.vals(false, new Vector3(), 1, 1));
        thesocket.props.Add(new sprop.interact());
        thesocket.props.Add(new sprop.rawinvs(15));
        socketlib.Add(thesocket);

        //
        thesocket = new socket("m01miner", "sm01miner", "Miner");
        ///temp
        thesocket.props.Add(new sprop.rawinvs(5));
        thesocket.props.Add(new sprop.interact());
        thesocket.props.Add(new sprop.elec(100, 1, 0, 150, 50, 500));
        thesocket.props.Add(new sprop.vals(false, new Vector3(), 1, 1));
        socketlib.Add(thesocket);


        thesocket = new socket("m01motor", "sm01motor", "Motor");
        thesocket.props.Add(new sprop.mech(0, 0, 200, 1));
        thesocket.props.Add(new sprop.elec(100, 1, 0, 0, 0, 800));
        socketlib.Add(thesocket);

        thesocket = new socket("m01tread", "sm01tread", "Conveyor");
        thesocket.props.Add(new sprop.mech(10, 0, 0, 1));
        thesocket.props.Add(new sprop.rawinvs(10));
        thesocket.props.Add(new sprop.invs(10));
        thesocket.props.Add(new sprop.interact());
        thesocket.getrawinvs.aninv.protectsuction = true;
        thesocket.getinvs.aninv.protectsuction = true;
        socketlib.Add(thesocket);

        thesocket = new socket("m01treaddiv", "sm01treaddiv", "Conveyor Divider");
        thesocket.props.Add(new sprop.mech(10, 0, 0, 1));
        thesocket.props.Add(new sprop.rawinvs(10));
        thesocket.props.Add(new sprop.invs(10));
        thesocket.getrawinvs.aninv.protectsuction = true;
        thesocket.getinvs.aninv.protectsuction = true;
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);


        thesocket = new socket("m01furnace", "sm01furnace", "Ore Furnace");
        thesocket.props.Add(new sprop.invs(4));
        thesocket.props.Add(new sprop.rawinvs(30));
        thesocket.props.Add(new sprop.interact());
        thesocket.props.Add(new sprop.elec(100, 1, 0, 50, 0, 1000));
        thesocket.props.Add(new sprop.craft("m01furnace"));
        socketlib.Add(thesocket);

        thesocket = new socket("m01pulverizer", "sm01pulverizer", "Pulverizer");
        thesocket.props.Add(new sprop.invs(4));
        thesocket.props.Add(new sprop.rawinvs(30));
        thesocket.props.Add(new sprop.interact());
        thesocket.props.Add(new sprop.elec(100, 1, 0, 50, 0, 1000));
        thesocket.props.Add(new sprop.craft("m01pulverizer"));
        socketlib.Add(thesocket);

        thesocket = new socket("m01compressor", "sm01compressor", "Compressor");
        thesocket.props.Add(new sprop.invs(4));
        thesocket.props.Add(new sprop.rawinvs(30));
        thesocket.props.Add(new sprop.interact());
        thesocket.props.Add(new sprop.elec(100, 1, 0, 50, 0, 1000));
        thesocket.props.Add(new sprop.craft("m01compressor"));
        socketlib.Add(thesocket);

        thesocket = new socket("phasegun", "sphasegun", "Alpine Blaster");
        thesocket.props.Add(new sprop.drops());
        thesocket.props.Add(new sprop.gun());
        thesocket.isobject = true;
        socketlib.Add(thesocket);

        thesocket = new socket("m01batfesi", "sm01batfesi", "FeSi Battery");
        thesocket.props.Add(new sprop.elec(100, 1, 100, 100, 0, 36000, 0));
        thesocket.props.Add(new sprop.vals(true, new Vector3(), 1, 1));
        thesocket.props.Add(new sprop.interact());
        socketlib.Add(thesocket);
        /*
        thesocket = new socket();

        socketlib.Add(thesocket);
        */
        /////Parts

        thesocket = new socket("generator", "", "Generator");
        thesocket.isobject = true;
        socketlib.Add(thesocket);

        thesocket = new socket("fesicapacitor", "", "FeSi Capacitor");
        thesocket.isobject = true;
        socketlib.Add(thesocket);

        thesocket = new socket("ironcog", "", "Iron Cog");
        thesocket.isobject = true;
        socketlib.Add(thesocket);

    }


    /// Functions
    public static GameObject getprefab(string astring)
    {
        return (GameObject)Resources.Load("prefab/"+astring) as GameObject;
    }
    public static Texture2D gettext(string astring)
    {
        return (Texture2D)Resources.Load("icons/" + astring) as Texture2D;
    }
    public GameObject prefab
    {
        get
        {
            return (GameObject)Resources.Load("prefab/"+prefabs) as GameObject;
        }
    }
    public static socket sfromstring(string astring)
    {
        foreach (socket asock in socketlib)
        {
            if (asock.name == astring)
            {
                return asock;
            }
        }
        return new socket();
    }

    public bool iswire
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.wire)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool iselec
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.elec)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool isinv
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.invs)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool israwinv
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.rawinvs)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool isoxgen
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.oxgen)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool isinteract
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.interact)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool iscraft
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.craft)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool ismech
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.mech)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool isdrop
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.drops)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool isgun
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.gun)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public sprop.elec getelec
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.elec)
                {
                    return aprop as sprop.elec;
                }
            }
            return null;
        }
    }
    public sprop.vals getvals
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.vals)
                {
                    return aprop as sprop.vals;
                }
            }
            return null;
        }
    }
    public sprop.invs getinvs
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.invs)
                {
                    return aprop as sprop.invs;
                }
            }
            return null;
        }
    }
    public sprop.rawinvs getrawinvs
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.rawinvs)
                {
                    return aprop as sprop.rawinvs;
                }
            }
            return null;
        }
    }
    public sprop.oxgen getoxgen
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.oxgen)
                {
                    return aprop as sprop.oxgen;
                }
            }
            return null;
        }
    }
    public sprop.interact getinteract
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.interact)
                {
                    return aprop as sprop.interact;
                }
            }
            return null;
        }
    }
    public sprop.craft getcraft
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.craft)
                {
                    return aprop as sprop.craft;
                }
            }
            return null;
        }
    }
    public sprop.mech getmech
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.mech)
                {
                    return aprop as sprop.mech;
                }
            }
            return null;
        }
    }
    public sprop.drops getdrop
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.drops)
                {
                    return aprop as sprop.drops;
                }
            }
            return null;
        }
    }
    public sprop.gun getgun
    {
        get
        {
            foreach (sprop aprop in props)
            {
                if (aprop is sprop.gun)
                {
                    return aprop as sprop.gun;
                }
            }
            return null;
        }
    }

    public static socket copysocket(socket asock)
    {
        socket returner = new socket();

        returner.name = asock.name;
        returner.prefabs = asock.prefabs;
        returner.sound = asock.sound;
        returner.disname = asock.disname;
        returner.breakable = asock.breakable;
        returner.mineable = asock.mineable;

        foreach (sprop aprop in asock.props)
        {
            returner.props.Add(sprop.copysprop(aprop));
        }
        

        return returner;
    }

}
