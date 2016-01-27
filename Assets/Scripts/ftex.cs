using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ftex : MonoBehaviour
{
    public static string dirsession = Application.dataPath ;

    public static void CreateDatum()
    {
        if (!Directory.Exists(dirsession))
        {
            Directory.CreateDirectory(dirsession);
        }
        if (!Directory.Exists(dirsession + "/_lexicon"))
        {
            Directory.CreateDirectory(dirsession + "/_lexicon");
        }
        if (!File.Exists(dirsession + "world"))
        {
            ftex.ArrayToText("points", new string[] { "" });
        }
    }
    public static void WriteLineOfText(string filename, string Line)
    {
        StreamWriter writer = new StreamWriter(dirsession + filename + ".txt");
        writer.WriteLine(Line);
        writer.Close();
    }
    public static void ReadLineOfText(string filename)
    {
        StreamReader reader = new StreamReader(dirsession + filename + ".txt");
        print(reader.ReadLine());
        reader.Close();
    }
    public static string[] TextToArray(string filename)
    {
        if (!File.Exists(dirsession + filename + ".txt"))
        {
            ftex.ArrayToText(filename, new string[] { "" });
            print("me");
        }
        List<string> atemplist = new List<string>();
        string tempstring = "";
        StreamReader reader = new StreamReader(dirsession + filename + ".txt");
    loophere:
        tempstring = reader.ReadLine();
        if (tempstring != null)
        {
            atemplist.Add(tempstring);
            goto loophere;
        }
        string[] returner = new string[atemplist.Count];
        for (int k = 0; k < atemplist.Count; k++)
        {
            returner[k] = atemplist[k];
        }
        reader.Close();
        return returner;
    }
    public static void ArrayToText(string filename, string[] Lines)
    {
        StreamWriter writer = new StreamWriter(dirsession + filename + ".txt");
        for (int k = 0; k < Lines.Length; k++)
        {
            writer.WriteLine(Lines[k]);
        }
        writer.Close();
    }
}
