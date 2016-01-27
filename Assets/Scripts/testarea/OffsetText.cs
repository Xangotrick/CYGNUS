using UnityEngine;
using System.Collections;

public class OffsetText : MonoBehaviour {
    public Vector2 uvOffset = Vector2.zero;
    public MeshRenderer rend;
    void Start()
    {
        rend = transform.GetComponent<MeshRenderer>();
    }

    void LateUpdate()
    {
        uvOffset = (new Vector2(0,1) * ((Time.time / 4f) % 1));
        rend.materials[1].SetTextureOffset("_MainTex",uvOffset);  
    }



}
