using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Raw
{
    public static List<Raw> libraw { get; set; }
    public string name { get; set; }
    public int id { get; set; }

    public Raw(string astring, int anin)
    {
        name = astring;
        id = anin;
    }
    
    public static void buildlib()
    {
        libraw = new List<Raw>();
        libraw.Add(new Raw("Hydrogen", libraw.Count));
        libraw.Add(new Raw("Carbon", libraw.Count));
        libraw.Add(new Raw("Oxygen", libraw.Count));
        libraw.Add(new Raw("Iron Ingot", libraw.Count));
        libraw.Add(new Raw("Rough Coal", libraw.Count));
        libraw.Add(new Raw("Sulfur", libraw.Count));
        libraw.Add(new Raw("Iron Ore", libraw.Count));
        libraw.Add(new Raw("Iron Dust", libraw.Count));
        libraw.Add(new Raw("Silicon", libraw.Count));
        libraw.Add(new Raw("Quartz Crystal", libraw.Count));
        libraw.Add(new Raw("Silicon Dust", libraw.Count));
        libraw.Add(new Raw("Copper Ore", libraw.Count));
        libraw.Add(new Raw("Copper Dust", libraw.Count));
        libraw.Add(new Raw("Copper Ingot", libraw.Count));
        libraw.Add(new Raw("Copper Coil", libraw.Count));
        libraw.Add(new Raw("Aluminium Ore", libraw.Count));
        libraw.Add(new Raw("Aluminium Dust", libraw.Count));
        libraw.Add(new Raw("Aluminium Plate", libraw.Count));
        libraw.Add(new Raw("Thermite", libraw.Count));
        libraw.Add(new Raw("FeSi Dust", libraw.Count));
    }

    public static int getid(string thestring)
    {
        int returner = 0;
        foreach (Raw araw in libraw)
        {
            if (araw.name == thestring)
            {
                return araw.id;
            }
        }
        return returner;
    }
}
