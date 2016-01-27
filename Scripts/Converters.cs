using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Converters : MonoBehaviour {
    public static float nmtopixel = 5.038f;
    public static float nmtom = 1852;
    public static Vector2 nmoffset = new Vector2(0, 0);

    public static Vector2 nmtoscreen(Vector2 avectnm)
    {
        Vector2 aworkingvectnm = new Vector2(avectnm.x, avectnm.y);

        Vector2 deltafromcenternm = aworkingvectnm - nmoffset;

        Vector2 returner = new Vector2(Screen.width / 2f + deltafromcenternm.x * nmtopixel, Screen.height / 2f - deltafromcenternm.y * nmtopixel);
        return returner;
    }


    public static Vector2 MouseToScreen(Vector2 mousepos)
    {
        Vector2 returner = new Vector2(mousepos.x, Screen.height - mousepos.y);
        return returner;
    }

    public static Vector2 CamToScreen(Transform atrans, Camera OurCam)
    {
        return MouseToScreen(OurCam.WorldToScreenPoint(atrans.position));
    }
    public static Vector2 CamToScreen(Vector3 apos, Camera OurCam)
    {
        return MouseToScreen(OurCam.WorldToScreenPoint(apos));
    }
    public static Vector2 screentocam(Vector2 avectscreen)
    {
        Vector2 r = Vector2.zero;

        Vector2 deltanm = avectscreen - new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 workingdelta = new Vector2(deltanm.x / (1f * nmtopixel), -deltanm.y / (1f * nmtopixel));

        r = workingdelta + nmoffset;

        return r;
    }
    

    public static string ArtoS(string[] arrstring, string key)
    {
        string returner = "";
        for (int k = 0; k < arrstring.Length; k++)
        {
            returner = returner + arrstring[k];
            returner = returner + key;
        }
        return returner;
    }
    public static string[] StoAr(string astring)
    {
        int index = 0;
        char key = astring[astring.Length - 1];
        string tempstring = "";
        List<string> atemplist = new List<string>();
        while (index < astring.Length)
        {
            if (astring[index] != key)
            {
                tempstring = tempstring + astring[index];
            }
            if (astring[index] == key)
            {
                atemplist.Add(tempstring);
                tempstring = "";
            }
            index++;
        }


        string[] returner = new string[atemplist.Count];
        for (int k = 0; k < atemplist.Count; k++)
        {
            returner[k] = atemplist[k];
        }
        return returner;
    }

    public static string FListtoS(List<float> alist, string akey)
    {
        if (alist.Count == 0)
        {
            return "void";
        }
        string[] anarr = new string[alist.Count];
        for (int k = 0; k < alist.Count; k++)
        {
            anarr[k] = alist[k].ToString();
        }
        return ArtoS(anarr, akey);
    }
    public static List<float> StoFList(string astring)
    {
        if (astring == "void")
        {
            return new List<float>();
        }

        string[] answer = StoAr(astring);
        List<float> r = new List<float>();
        for (int k = 0; k < answer.Length; k++)
        {
            r.Add(float.Parse(answer[k]));
        }

        return r;
    }

    public static object StoDyn(string astring, string akey)
    {
        switch (akey)
        {
            case "System.String":
                return astring;
            case "System.Int32":
                return Int32.Parse(astring);
            case "System.Boolean":
                return Boolean.Parse(astring);
            case "System.Single":
                return float.Parse(astring);
            case "1111":
                return "";
            case "11111":
                return "";
        }
        return "";
    }

    public static int StoI(string astring)
    {
        int returner = 0;
        if (astring != "")
        {
            string astringans = "";
            for (int k = 0; k < astring.Length; k++)
            {
                astringans += ((int)astring[k] - 48).ToString();
            }
            return int.Parse(astringans);
        }
        return returner;
    }

    public static string[] DatetoArray(DateTime atime)
    {
        string[] r = new string[3];

        string ahour = atime.Hour.ToString();
        string aminu = atime.Minute.ToString();
        string aseco = atime.Second.ToString();
    adding:
        if (ahour.Length < 2)
        {
            ahour = "0" + ahour;
            goto adding;
        }
        if (aminu.Length < 2)
        {
            aminu = "0" + aminu;
            goto adding;
        }
        if (aseco.Length < 2)
        {
            aseco = "0" + aseco;
            goto adding;
        }

        r[0] = ahour;
        r[1] = aminu;
        r[2] = aseco;

        return r;
    }

    public static float vecttolineycoord(Vector2 avect, float anx)
    {
        return (avect.x * anx + avect.y);
    }

    public static string alttofl(float afloat)
    {

        string fl = "";

        fl = ((int)Math.Round(afloat / 100)).ToString();
        while (fl.Length < 3)
        {
            fl = "0" + fl;
        }

        return fl;
    }
}
