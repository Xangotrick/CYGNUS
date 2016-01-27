using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class effect
{
    public float duration;
    public float stamp;
    public float speedmult;
    public float spellvamp;
    public bool attract = false;
    public float xdir = 0;
    public float ydir = 0;

    public effect(EffectUnity aneff)
    {
        stamp = Time.time;
        duration = aneff.duration;
        speedmult = aneff.speedmult;
        spellvamp = aneff.spellvamp;
        attract = aneff.attract;
        xdir = aneff.xdir;
        ydir = aneff.ydir;
    }
    public effect(float time)
    {
        stamp = Time.time;
        duration = time;
        speedmult = 1;
        spellvamp = 0;
        attract = false;
        xdir = 0;
        ydir = 0;
    }
}
