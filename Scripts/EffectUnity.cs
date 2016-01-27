using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class EffectUnity : MonoBehaviour
{
    public float duration= 0;
    public float stamp=0;
    public float speedmult=1;
    public float spellvamp = 0;
    public bool attract = false;
    public float xdir = 0;
    public float ydir = 0;

    public EffectUnity()
    {
        stamp = Time.time;
        duration = 0;
        speedmult = 1;
        spellvamp = 0;
        attract = false;
        xdir = 0;
        ydir = 0;
    }
}
