using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SaveDataFile
{
    public socketIG[, ,] cmap { get; set; }
    public static List<ElectricGrid> elecgrids { get; set; }

    public void Load()
    {
        Environment.cmap = cmap;
        Environment.elecgrids = elecgrids;
    }

    public SaveDataFile()
    {
        cmap = Environment.cmap;
        elecgrids = Environment.elecgrids;
    }
}
