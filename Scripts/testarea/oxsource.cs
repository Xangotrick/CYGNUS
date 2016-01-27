using UnityEngine;
using System.Collections;

public class oxsource : MonoBehaviour {
    public float range = 10f;

    void Update()
    {
        if (Mathf.Abs(transform.localScale.z - range) > 0.5f)
        {
            transform.localScale -= Mathf.Sign(transform.localScale.z - range) * Time.deltaTime * 2 * new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(range, range, range);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<charnetworkbehavior>() != null)
        {
            worldgen.maininfo.isdrainingox += 1;
        }

    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<charnetworkbehavior>() != null)
        {
            worldgen.maininfo.isdrainingox -= 1;
        }
    }
    void OnDestroy()
    {
    
        {
            worldgen.maininfo.isdrainingox -= 1;
        }
    }
}
