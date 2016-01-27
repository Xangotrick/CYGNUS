using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class Recipe
{
    public static List<Recipe> M01ALL;
    public static List<Recipe> M01Crafter;
    public static List<Recipe> M01Furnace;
    public static List<Recipe> M01Pulverizer;
    public static List<Recipe> M01Compressor;
    public string name { get; set; }
    public string station { get; set; }
    public float eleccost { get; set; }
    public float time { get; set; }
    public List<RawInv.rawlibitem> RawItems { get; set; }
    public List<Inventory.InvItem> Items { get; set; }
    public List<Inventory.InvItem> Result { get; set; }
    public List<RawInv.rawlibitem> ResultRaw { get; set; }

    void Start()
    {
    }

    public Recipe(string stationd, float cost = 0, float timee = 0)
    {
        RawItems = new List<RawInv.rawlibitem>();
        Items = new List<Inventory.InvItem>();
        Result = new List<Inventory.InvItem>();
        ResultRaw = new List<RawInv.rawlibitem>();
        eleccost = cost;
        time = timee;
        station = stationd;
    }

    public static void Craft()
    {
        BM01Crafter();
        BM01Furnace();
        BM01Pulverizer();
        BM01Compressor();
        BM01ALL();
    }

    public static void BM01Crafter()
    {
        string stat = "Crafting Station";
        M01Crafter = new List<Recipe>();
        Recipe rec = new Recipe(stat, 0, 0);


        rec = new Recipe(stat, 100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Coil", 9));
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ingot", 9));
        rec.Result.Add(new Inventory.InvItem("m01machine", 1));
        rec.name = "Mk1 Machine Block";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 200, 8);
        rec.Items.Add(new Inventory.InvItem("m01machine", 1));
        rec.RawItems.Add(new RawInv.rawlibitem("Thermite", 2));
        rec.RawItems.Add(new RawInv.rawlibitem("Generator", 1));
        rec.Result.Add(new Inventory.InvItem("m01gencoal", 1));
        rec.name = "Coal Generator";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 200, 8);
        rec.Items.Add(new Inventory.InvItem("m01machine", 1));
        rec.Items.Add(new Inventory.InvItem("ironcog", 2));
        rec.RawItems.Add(new RawInv.rawlibitem("Generator", 1));
        rec.Result.Add(new Inventory.InvItem("m01gensteam", 1));
        rec.name = "Steam Generator";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 200, 8);
        rec.Items.Add(new Inventory.InvItem("m01machine", 1));
        rec.RawItems.Add(new RawInv.rawlibitem("Thermite", 2));
        rec.Items.Add(new Inventory.InvItem("ironcog", 2));
        rec.Result.Add(new Inventory.InvItem("m01furnace", 1));
        rec.name = "Furnace";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 200, 8);
        rec.Items.Add(new Inventory.InvItem("m01machine", 1));
        rec.Items.Add(new Inventory.InvItem("ironcog", 2));
        rec.Result.Add(new Inventory.InvItem("m01crafter", 1));
        rec.name = "Crafting Station";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 200, 8);
        rec.Items.Add(new Inventory.InvItem("m01machine", 1));
        rec.RawItems.Add(new RawInv.rawlibitem("Generator", 1));
        rec.Items.Add(new Inventory.InvItem("ironcog", 10));
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ingot", 2));
        rec.Result.Add(new Inventory.InvItem("m01crafter", 1));
        rec.name = "Compressor";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 200, 8);
        rec.Items.Add(new Inventory.InvItem("m01machine", 1));
        rec.Items.Add(new Inventory.InvItem("fesicapacitor", 3));
        rec.Result.Add(new Inventory.InvItem("m01batfesi", 1));
        rec.name = "FeSi Battery";
        M01Crafter.Add(rec);


        ////Crafts part
        rec = new Recipe(stat, 100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("FeSi Dust", 1));
        rec.RawItems.Add(new RawInv.rawlibitem("Aluminium Plate", 4));
        rec.Result.Add(new Inventory.InvItem("fesicapacitor", 1));
        rec.name = "FeSi Capacitor";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Ingot", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Copper Coil", 9));
        rec.name = "Copper Coil (Raw)";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Ingot", 1));
        rec.Result.Add(new Inventory.InvItem("m01copperwire", 9));
        rec.name = "Copper Wire";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 0, 0.1f);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Dust", 1));
        rec.RawItems.Add(new RawInv.rawlibitem("Silicon Dust", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("FeSi Dust", 2));
        rec.name = "Iron Silica Dust";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 0, 0.1f);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Dust", 1));
        rec.RawItems.Add(new RawInv.rawlibitem("Aluminium Dust", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Thermite", 2));
        rec.name = "Iron Silica Dust";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 100, 1f);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Coil", 9));
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ingot", 1));
        rec.Items.Add(new Inventory.InvItem("fesicapacitor", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Generator", 1));
        rec.name = "Generator (Raw)";
        M01Crafter.Add(rec);

        rec = new Recipe(stat,100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ore", 4));
        rec.RawItems.Add(new RawInv.rawlibitem("Sulfur", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Iron Ingot", 2));
        rec.name = "Iron Ingot";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ingot", 2));
        rec.Result.Add(new Inventory.InvItem("met", 10));
        rec.name = "Metal Plate";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ingot", 1));
        rec.Result.Add(new Inventory.InvItem("slopemet", 10));
        rec.name = "Metal Slope";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 100, 1);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ingot", 4));
        rec.RawItems.Add(new RawInv.rawlibitem("Silicon", 2));
        rec.RawItems.Add(new RawInv.rawlibitem("Sulfur", 1));
        rec.Result.Add(new Inventory.InvItem("light02", 6));
        rec.name = "Power Lamp";
        M01Crafter.Add(rec);

        rec = new Recipe(stat, 100, 1);
        rec.Items.Add(new Inventory.InvItem("woodlog", 1));
        rec.Result.Add(new Inventory.InvItem("woodplank", 6));
        rec.name = "Process Oak Log";
        M01Crafter.Add(rec);
    }
    public static void BM01Furnace()
    {
        string stat = "Furnace";
        M01Furnace = new List<Recipe>();
        Recipe rec = new Recipe(stat, 0, 0);

        rec = new Recipe(stat, 200, 10);
        rec.Items.Add(new Inventory.InvItem("woodlog", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Rough Coal", 1));
        rec.name = "Burn Wood";

        rec = new Recipe(stat, 200, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Aluminium Ore", 10));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Aluminium Plate", 4));
        rec.name = "Smelt Aluminium";
        M01Furnace.Add(rec);
        rec = new Recipe(stat, 100, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Aluminium Dust", 2));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Aluminium Plate", 4));
        rec.name = "Smelt Aluminium Dust";

        rec = new Recipe(stat, 200, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Ore", 10));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Copper Ingot", 1));
        rec.name = "Smelt Copper";
        M01Furnace.Add(rec);
        rec = new Recipe(stat, 100, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Dust", 2));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Copper Ingot", 1));
        rec.name = "Smelt Copper Dust";

        rec = new Recipe(stat, 100, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ore", 10));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Iron Ingot", 1));
        rec.name = "Smelt Iron Ore";
        M01Furnace.Add(rec);

        rec = new Recipe(stat, 100, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ore", 10));
        rec.Result.Add(new Inventory.InvItem("ironcog", 1));
        rec.name = "Smelt Iron Ore to Cog";
        M01Furnace.Add(rec);

        rec = new Recipe(stat, 200, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Dust", 10));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Iron Ingot", 1));
        rec.name = "Smelt Iron Dust";
        M01Furnace.Add(rec);

        rec = new Recipe(stat, 200, 10);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Dust", 2));
        rec.Result.Add(new Inventory.InvItem("ironcog", 1));
        rec.name = "Smelt Iron Dust to Cog";
        M01Furnace.Add(rec);
    }
    public static void BM01Pulverizer()
    {
        string stat = "Pulverizer";
        M01Pulverizer = new List<Recipe>();
        Recipe rec = new Recipe(stat, 0, 0);

        rec = new Recipe(stat, 75, 5);
        rec.RawItems.Add(new RawInv.rawlibitem("Aluminium Ore", 2));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Aluminium Dust", 1));
        rec.name = "Crush Aluminium";
        M01Pulverizer.Add(rec);

        rec = new Recipe(stat, 75, 5);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Ore", 2));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Copper Dust", 1));
        rec.name = "Crush Copper";
        M01Pulverizer.Add(rec);

        rec = new Recipe(stat, 75, 5);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Ore", 2));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Iron Dust", 1));
        rec.name = "Crush Iron";
        M01Pulverizer.Add(rec);
    }
    public static void BM01Compressor()
    {
        string stat = "Compressor";
        M01Compressor = new List<Recipe>();
        Recipe rec = new Recipe(stat, 0, 0);

        rec = new Recipe(stat, 275, 25);
        rec.RawItems.Add(new RawInv.rawlibitem("Aluminium Dust", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Aluminium Plate", 4));
        rec.name = "Condense Aluminium";
        M01Compressor.Add(rec);

        rec = new Recipe(stat, 275, 25);
        rec.RawItems.Add(new RawInv.rawlibitem("Copper Dust", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Copper Ingot", 1));
        rec.name = "Condense Copper";
        M01Compressor.Add(rec);

        rec = new Recipe(stat, 275, 25);
        rec.RawItems.Add(new RawInv.rawlibitem("Iron Dust", 1));
        rec.ResultRaw.Add(new RawInv.rawlibitem("Iron Ingot", 1));
        rec.name = "Condense Iron";
        M01Compressor.Add(rec);
    }
    public static void BM01ALL()
    {
        M01ALL = new List<Recipe>();
        concactenate(M01ALL, M01Crafter);
        concactenate(M01ALL, M01Furnace);
        concactenate(M01ALL, M01Pulverizer);
        concactenate(M01ALL, M01Compressor);
    }

    public static void concactenate(List<Recipe> subject, List<Recipe> add)
    {
        foreach (Recipe arec in add)
        {
            subject.Add(arec);
        }
    }
}
