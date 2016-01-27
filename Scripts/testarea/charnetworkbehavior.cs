using UnityEngine;
using System.Collections;

public class charnetworkbehavior : Photon.MonoBehaviour {
    Vector3 recpos = new Vector3();
    Quaternion reqquat = Quaternion.identity;
    Quaternion reqcam = Quaternion.identity;
    public Transform cam;
    public PhotonView viw;
    public static System.Random arand;

    void Start()
    {
        viw = GetComponent<PhotonView>();
        worldgen.playerbase.Add(transform);            
        
        print(viw.viewID);

        arand = new System.Random();


        float rand = arand.Next(100)/100f*360f;
        
        Vector3 color = new Vector3(guitools.Colorific(rand, 1).r, guitools.Colorific(rand, 1).g, guitools.Colorific(rand, 1).b);

        NetworkerPhoton.RPCColoration(color, viw.viewID);
        if (photonView.isMine)
        {
            worldgen.maininfo.photonid = viw.viewID;
            print(viw.viewID);
        }
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, recpos, 0.25f);
            transform.rotation = Quaternion.Lerp(transform.rotation, reqquat, 0.25f);
            cam.localRotation = Quaternion.Lerp(cam.localRotation, reqcam, 0.25f);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Quaternion rot2 = cam.localRotation;
        if (stream.isWriting)
        {
            //stream.Serialize(ref pos);
            //stream.Serialize(ref rot);
            stream.SendNext(pos);
            stream.SendNext(rot);
            stream.SendNext(rot2);
        }
        else
        {
            //stream.Serialize(ref pos);
            //stream.Serialize(ref rot);
            recpos = (Vector3)stream.ReceiveNext();
            reqquat = (Quaternion)stream.ReceiveNext();
            reqcam = (Quaternion)stream.ReceiveNext();
        }
    }


}
