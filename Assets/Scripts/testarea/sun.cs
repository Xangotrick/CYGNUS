using UnityEngine;
using System.Collections;

public class sun : MonoBehaviour {
    public float starttime;
    public Light lighti;
    public Vector3 ColorWanted;
    public Vector3 CurrentColor;
    public Transform thetrans;

    public Color Morning;
    public Color Day;
    public Color Night;
    public LensFlare theflare;
    public float houreees;
    public float brightcoef;

    public float angle;
    public float delay;
    public bool main;
    public float intense;
    public float maplightintense = 0.5f;
    void Start()
    {
        brightcoef = 1;
        starttime = Time.time;
        lighti = GetComponent<Light>();
        theflare = GetComponent<LensFlare>();
        thetrans = transform;

        ColorWanted = new Vector3(255, 255, 255);
        lighti.color = guitools.RGB(ColorWanted.x, ColorWanted.y, ColorWanted.z);
        theflare.color = guitools.RGB(ColorWanted.x, ColorWanted.y, ColorWanted.z);
        CurrentColor = ColorWanted;

        if (delay > houreees / 4f && delay < houreees / 3f)
        {
            ColorWanted = 255* new Vector3(Morning.r, Morning.g, Morning.b);
            print("A");
        }
        if (delay > houreees / 3f && delay < (houreees * 3 / 4f - houreees / 12f))
        {
            ColorWanted = 255 * new Vector3(Day.r, Day.g, Day.b);
            print("B");
        }
        if (delay > (houreees * 3 / 4f - houreees / 12f) && delay < houreees * 3 / 4f)
        {
            ColorWanted = 255 * new Vector3(Night.r, Night.g, Night.b);
            print("C");
        }

        lighti.color = guitools.RGB(ColorWanted.x, ColorWanted.y, ColorWanted.z);
        CurrentColor = ColorWanted;


        StartCoroutine(suni(angle,delay));
    }

    void FixedUpdate()
    {
        if (cam.controlmove)
        {
            //float zzz = worldgen.maininfo.tinstance.transform.position.y;
            //Vector3 cpos = Environment.convmaptoc(worldgen.maininfo.tinstance.transform.position);
            //int zm = Environment.heightmap[Mathf.RoundToInt(cpos.x), Mathf.RoundToInt(cpos.z)];
            //zzz = Mathf.Clamp(zzz, zm-3, zm);
            //zzz = zzz - zm + 3;
            if (main)
            {
                /*
                float dist = 20 + 96 * (zzz / 3f);
                cam.maxrender = dist;
                cam.valbox = zzz / 3f;
                */
            }
        }
    }

    public IEnumerator suni(float angle, float delay)
    {
        while (true)
        {
            float daylength = houreees * 60f;
            float hour = ((Time.time - starttime) % daylength) / (daylength) * houreees + delay;
            hour = hour % daylength;
            CurrentColor = Vector3.Lerp(CurrentColor, ColorWanted, 0.1f * 60f / daylength);
            lighti.color = guitools.RGB(CurrentColor.x, CurrentColor.y, CurrentColor.z);
            theflare.color = guitools.RGB(CurrentColor.x, CurrentColor.y, CurrentColor.z);

            if (main)
            {
                RenderSettings.ambientLight = guitools.RGB(CurrentColor.x, CurrentColor.y, CurrentColor.z);
            }

            if (hour > houreees/4f && hour < houreees * 3/4f)
            {
                lighti.enabled = true;
                thetrans.eulerAngles = new Vector3(90 + 90 * Mathf.Pow(Mathf.Sin((((hour - houreees / 4f) / (houreees / 2f)) - 0.5f) * Mathf.PI), 2) * Mathf.Sign(Mathf.Sin((((hour - houreees / 4f) / (houreees / 2f)) - 0.5f) * Mathf.PI)), angle, 0);
                lighti.intensity = Mathf.Pow(Mathf.Max(0.01f, Mathf.Cos((((hour - houreees/4f) / (houreees/2f)) - 0.5f) * Mathf.PI)), 0.25f) * intense * brightcoef;

                if (main)
                {
                    RenderSettings.ambientIntensity = Mathf.Pow(Mathf.Cos((((hour - houreees / 4f) / (houreees / 2f)) - 0.5f) * Mathf.PI), 1) * maplightintense + 0.0f;
                }
            }
            else
            {
                if (main)
                {
                    RenderSettings.ambientIntensity = 0f;
                }
                thetrans.eulerAngles = new Vector3(270, angle, 0);
                ColorWanted = new Vector3(Morning.r,Morning.b,Morning.g);
                lighti.color = guitools.RGB(ColorWanted.x, ColorWanted.y, ColorWanted.z);
                theflare.color = guitools.RGB(ColorWanted.x, ColorWanted.y, ColorWanted.z);
                CurrentColor = ColorWanted;
                lighti.intensity = 0;
                lighti.enabled = false;
            }
            if (hour > houreees / 4f && hour < houreees / 3f)
            {
                ColorWanted = 255 * new Vector3(Morning.r, Morning.g, Morning.b);
                if (hour < houreees / 4f + 0.2f)
                {
                    if (main && cam.controlmove)
                    {
                        if (!controls.hissoundsyst.music.isPlaying)
                        {
                            controls.hissoundsyst.music.clip = controls.hissoundsyst.Mmorning;
                            controls.hissoundsyst.music.PlayDelayed(20);
                        }
                    }
                }
            }
            if (hour > houreees / 3f && hour < (houreees * 3 / 4f - houreees / 12f))
            {
                ColorWanted = 255 * new Vector3(Day.r, Day.g, Day.b); ;
            }
            if (hour > (houreees * 3 / 4f - houreees / 12f) && hour < houreees * 3 / 4f)
            {
                ColorWanted = 255 * new Vector3(Night.r, Night.g, Night.b);

                if (hour > houreees * 3 / 4f - 1/6f)
                {
                    if (main && cam.controlmove)
                    {
                        if (!controls.hissoundsyst.music.isPlaying)
                        {
                            controls.hissoundsyst.music.clip = controls.hissoundsyst.Mevening;
                            controls.hissoundsyst.music.Play();
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.2f * houreees / 24f);
        }
    }
}
