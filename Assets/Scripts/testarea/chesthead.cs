using UnityEngine;
using System.Collections;

public class chesthead : MonoBehaviour {
    Transform head;
    public bool open;
	// Use this for initialization
	void Start () {
        open = false;
        head = transform.FindChild("head");
	}
	
	// Update is called once per frame
	void Update () {
        if (open && head.transform.eulerAngles.z < 30)
        {
            head.transform.eulerAngles = new Vector3(0, 0, head.transform.eulerAngles.z + 30* Time.deltaTime);
        }
        if (!open && head.transform.eulerAngles.z > 0)
        {
            head.transform.eulerAngles = new Vector3(0, 0, head.transform.eulerAngles.z - 30 *Time.deltaTime);
        }
        if (head.transform.eulerAngles.z > 180)
        {
            head.transform.eulerAngles = new Vector3(0, 0, 0);
        }

	}
}
