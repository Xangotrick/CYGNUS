using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class socketIG {
    public socket asock { get; set; }
    //public GameObject instance { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public socketIG(socket thesock, Vector3 pos, GameObject theobj)
    {

        asock = thesock;
        location = pos;
        //instance = theobj;
    }

    public socketIG()
    {
        if (socket.socketlib.Count == 0)
        {
            asock = new socket();
        }
        else
        {
            asock = socket.socketlib[0];
        }
        location = new Vector3();
        //instance = null;
    }

    public socketIG(string thestring)
    {
        //instance = null;
        location = new Vector3();
        asock = socket.socketlib[0];
        foreach (socket asocek in socket.socketlib)
        {
            if (asocek.name == thestring)
            {
                asock = socket.copysocket(asocek);
            }
        }
    }

    public void hardresetwires()
    {
        asock.getelec.resetcon();
    }
    public void resetwires()
    {
        if (asock.iswire)
        {
            


            instance.transform.Find("c").transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            instance.transform.Find("X").GetComponent<MeshRenderer>().enabled = asock.getelec.connected[0];
            instance.transform.Find("-X").GetComponent<MeshRenderer>().enabled = asock.getelec.connected[1];
            instance.transform.Find("Y").GetComponent<MeshRenderer>().enabled = asock.getelec.connected[2];
            instance.transform.Find("-Y").GetComponent<MeshRenderer>().enabled = asock.getelec.connected[3];
            instance.transform.Find("Z").GetComponent<MeshRenderer>().enabled = asock.getelec.connected[4];
            instance.transform.Find("-Z").GetComponent<MeshRenderer>().enabled = asock.getelec.connected[5];

            //controls.printi(asock.getelec.connected[0] + " " + asock.getelec.connected[01] + " " + asock.getelec.connected[02] + " " + asock.getelec.connected[03] + " " + asock.getelec.connected[04] + " " + asock.getelec.connected[05]  );
        }
    }

    public Vector3 location
    {
        get
        {
            return new Vector3(x, y, z);
        }
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    public GameObject instance
    {
        get
        {
            Collider[] hitColliders = Physics.OverlapSphere(location, 0.25f);
            if (hitColliders.Length != 0)
            {
                GameObject obje = hitColliders[0].gameObject;
                if (hitColliders[0].gameObject.layer == 2)
                {
                    if (hitColliders.Length > 1)
                    {
                        obje = hitColliders[1].gameObject;
                    }
                    else
                    {
                        return null;
                    }
                }

                while (obje.transform.parent != null)
                {
                    obje = obje.transform.parent.gameObject;
                }


                return obje;
            }
            return null;
        }
    }
}
