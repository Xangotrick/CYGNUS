using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;


[Serializable]
public class RawInv
{
    public List<rawlibitem> inv { get; set; }
    public float maxmass { get; set; }
    public bool protectsuction { get; set; }

    public float add(int anint, float mass)
    {
        float returner = 0;
        if (mass < maxmass)
        {
            foreach (rawlibitem anitem in inv)
            {
                if (anitem.id == anint)
                {
                    float dif = (maxmass - mass);
                    dif = Mathf.Min(dif, mass);
                    mass = mass - dif;
                    returner = mass;
                    anitem.kg += dif;
                    return returner;
                }
            }
            inv.Add(new rawlibitem(anint, mass));
        }
        return returner;
    }

    public float add(string astring, float mass)
    {
       return add(Raw.getid(astring),mass);
    }



    public void rem(int anint, float mass)
    {
        foreach (rawlibitem anitem in inv)
        {
            if (anitem.id == anint)
            {
                anitem.kg = anitem.kg - mass;
                if (anitem.kg < 0)
                {
                    anitem.kg = 0;
                }
                break;
            }
        }
    }
    public void rem(string astring, float mass)
    {
        foreach (rawlibitem anitem in inv)
        {
            if (Raw.libraw[anitem.id].name == astring)
            {
                anitem.kg = anitem.kg - mass;
                if (anitem.kg < 0)
                {
                    anitem.kg = 0;
                }
                break;
            }
        }
    }

    public float getcount(int id)
    {
        float returner = 0;

        for (int k = 0; k < inv.Count; k++)
        {
            if (inv[k].id == id)
            {
                returner += inv[k].kg;
            }
        }

        return returner;
    }
    public float getcount(string astring)
    {
        float returner = 0;

        for (int k = 0; k < inv.Count; k++)
        {
            if (Raw.libraw[inv[k].id].name == astring)
            {
                returner += inv[k].kg;
            }
        }

        return returner;
    }

    public float mass
    {
        get
        {
            float afloat = 0;
            foreach (rawlibitem anitem in inv)
            {
                afloat += anitem.kg;
            }
            return afloat;
        }
    }
    public float readkg
    {
        get
        {
            return (float)System.Math.Round(mass, 1);
        }
    }

    public RawInv(float max)
    {
        maxmass = max;
        inv = new List<rawlibitem>();
        protectsuction = false;
    }

[Serializable]
    public class rawlibitem
    {
        public int id { get; set; }
        public float kg { get; set; }
        public rawlibitem(int ide, float mass = 1)
        {
            kg = mass;
            id = ide;
        }
        public rawlibitem(string name, float mass = 1)
        {
            id = 0;
            kg = mass;

            for (int k = 0; k < Raw.libraw.Count; k++)
            {
                if (Raw.libraw[k].name == name)
                {
                    id = k;
                    break;
                }
            }
        }

        public Raw raw
        {
            get
            {
                if (id < Raw.libraw.Count)
                {
                    return Raw.libraw[id];
                }
                return Raw.libraw[0];
            }
            set
            {
                id = value.id;
            }
        }

        public float readkg
        {
            get
            {
                return (float)System.Math.Round(kg, 1);
            }
        }
    }
}
