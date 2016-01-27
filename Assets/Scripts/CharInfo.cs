using UnityEngine;
using System.Collections;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class CharInfo{
    public string name { get; set; }
    public Stats stats { get; set; }
    public float oxg { get; set; }
    public float maxoxg { get; set; }
    public Inventory inv { get; set; }
    public RawInv rawinv { get; set; }
    public GameObject tinstance { get; set; }
    public skillinfo askill { get; set; }
    public int isdrainingox { get; set; }
    public int photonid { get; set; }



    public CharInfo(GameObject objecte, string aname, Stats stats)
    {
        name = aname;
        stats = new Stats();
        inv = new Inventory(40);
        rawinv = new RawInv(50);
        rawinv.add(0, 3);
        rawinv.add("Iron Ore", 12);
        rawinv.add(4, 1);
        rawinv.add("Sulfur", 3);
        tinstance = objecte;
        oxg = 0;
        maxoxg = 7.5f;
        isdrainingox = 0;
        photonid = 0;
        askill = new skillinfo();
    }

    public class skillinfo
    {
        public int[] pasmembers { get; set; }
        public int[] actmembers { get; set; }
        public float[] actcool { get; set; }

        public skillinfo()
        {
            actmembers = new int[] { 0, 1, 2, 3 };
            pasmembers = new int[] { 0, -1, -1, -1 };
            actcool = new float[] { 0, 0, 0, Time.time - 40 };
        }

        public bool isonline(int anint)
        {
            if (actmembers[anint] != -1)
            {
                return (Time.time - actcool[anint] > skill.act[actmembers[anint]].cooldown);
            }
            return false;
        }

        public skill act0
        {
            get
            {
                return skill.act[actmembers[0]];
            }
        }
        public skill act1
        {
            get
            {
                return skill.act[actmembers[1]];
            }
        }
        public skill act2
        {
            get
            {
                return skill.act[actmembers[2]];
            }
        }
        public skill act3
        {
            get
            {
                return skill.act[actmembers[3]];
            }
        }

        public skill pas0
        {
            get
            {
                return skill.pas[pasmembers[0]];
            }
        }
        public skill pas1
        {
            get
            {
                return skill.pas[pasmembers[1]];
            }
        }
        public skill pas2
        {
            get
            {
                return skill.pas[pasmembers[2]];
            }
        }
        public skill pas3
        {
            get
            {
                return skill.pas[pasmembers[3]];
            }
        }
    }
}
