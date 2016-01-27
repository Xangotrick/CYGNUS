using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class pellet : MonoBehaviour {
    public int id;
    public float speed;
    public float damage;
    public string partname;
    public bool owner = false;
    public Vector3 aim = new Vector3();

    public EffectUnity[] effectfalses;
    public effect[] effects;
    public AudioSource onhit;
    public ParticleSystem part;

    public float isdead = 0;

    void Start()
    {
        part = transform.GetComponentInChildren<ParticleSystem>();
        onhit = transform.GetComponent<AudioSource>();

        effectfalses = transform.GetComponents<EffectUnity>();
        if (effectfalses.Length != 0)
        {
            effects = new effect[effectfalses.Length];
            for (int k = 0; k < effectfalses.Length; k++)
            {
                effects[k] = new effect(effectfalses[k]);
            }
        }
    }

    void Update()
    {
        if (isdead > 0)
        {
            if (onhit != null)
            {
                if (onhit.isPlaying)
                {
                }
                else
                {
                    Destroy(transform.gameObject);
                }
            }
            else
            {
                if (Time.time - isdead > 0.5f)
                {
                    Destroy(transform.gameObject);
                }
            }
        }
        else
        {
            if (aim != new Vector3())
            {
                if ((transform.position - aim).magnitude > 1)
                {
                    transform.position += (aim - transform.position).normalized * speed * Time.deltaTime;
                }
                else
                {
                    transform.GetComponent<MeshRenderer>().enabled = false;
                    transform.GetComponent<Light>().enabled = false;
                    if (part != null)
                    {
                        part.emissionRate = 0;
                    }
                    isdead = Time.time;
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (isdead > 0)
        {

        }
        else
        {
            Transform obje = other.gameObject.transform;
            while (obje.parent != null)
            {
                obje = obje.parent;
            }
            Stats then = obje.GetComponent<Stats>();
            if (then != null)
            {
                if (then.photonid == id)
                {
                    return;
                }
                else
                {
                    if (owner)
                    {
                        then.DamageExt(damage,id);
                        if (effects.Length != 0)
                        {
                            for (int k = 0; k < effects.Length; k++)
                            {
                                if (effects[k].attract)
                                {
                                    Vector2 avect = new Vector2();
                                    foreach (Stats astat in worldgen.characcess)
                                    {
                                        if (astat.photonid == id)
                                        {
                                            avect = new Vector2(astat.theactualpos.position.x, astat.theactualpos.position.y);
                                            break;
                                        }
                                    }
                                    avect = new Vector2(then.theactualpos.position.x, then.theactualpos.position.y) - avect;
                                    avect = -avect.normalized;
                                    effects[k].xdir = avect.x;
                                    effects[k].ydir = avect.y;
                                }
                            }
                            then.AddEffectsExt(effects, id);
                        }
                        gui.listofEnemy.Add(then);
                        gui.enemydamagestamp.Add(Time.time);
                        if (worldgen.maininfo.stats.spellvamp != 0)
                        {
                            float life = -damage * worldgen.maininfo.stats.spellvamp;
                            worldgen.maininfo.stats.DamageExt(life, id);
                        }
                    }
                    if (onhit != null)
                    {
                        onhit.Play();
                    }
                }
            }
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetComponent<Light>().enabled = false;
            if (part != null)
            {
                part.emissionRate = 0;
            }
            if (partname != "")
            {
                Vector3 impact = transform.position + (other.transform.position - transform.position) / 2f;
                Vector3 norm = (other.transform.position - transform.position).normalized;
                //GameObject game = (GameObject)Instantiate((GameObject)Resources.Load("parts/" + partname), new Vector3(other.contacts[0].point.x * 1f, other.contacts[0].point.y * 1f, other.contacts[0].point.z * 1f), Quaternion.LookRotation( other.contacts[0].normal));
                GameObject game = (GameObject)Instantiate((GameObject)Resources.Load("parts/" + partname),impact,Quaternion.LookRotation(norm) );
            }
            isdead = Time.time;
        }
    }
}
