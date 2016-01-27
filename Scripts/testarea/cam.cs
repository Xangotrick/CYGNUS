using UnityEngine;
using System.Collections;

public class cam : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;
    public static bool controlmouse = true;
    public static bool controlkeys = true;
    public static bool controlmove = false;
    //public static float maxrender = 128;
    //public static float valbox = 128;
    public Transform aim;
    public static Camera cama;
    public bool activate = false;


    void Start()
    {
        aim = transform;
    }

    void Update()
    {
        if (activate)
        {
            if (controlmouse)
            {
                if (axes == RotationAxes.MouseXAndY)
                {
                    float rotationX = aim.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                    aim.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                }
                else if (axes == RotationAxes.MouseX)
                {
                    aim.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
                }
                else
                {
                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    aim.localEulerAngles = new Vector3(-rotationY, aim.localEulerAngles.y, 0);
                }
                if (controlmove)
                {
                    /*
                    if (cama == null)
                    {
                        cama = transform.GetComponentInChildren<Camera>();
                    }
                    float max = maxrender;
                    float dist = max;
                    if (aim.localEulerAngles.x < 100)
                    {
                        dist = max * Mathf.Pow((90 - aim.localEulerAngles.x) / 90f, 4) + 10F;
                    }
                    else
                    {
                        if (aim.localEulerAngles.x > 260)
                        {
                            dist = max * Mathf.Pow((aim.localEulerAngles.x - 270) / 90f, 4) + 10f;
                        }
                    }*/
                    //cama.farClipPlane = dist;
                    //RenderSettings.fogEndDistance = cama.farClipPlane * 0.9f;
                    //RenderSettings.skybox.SetFloat("_Exposure", valbox * 0.33f+0.1f);
                }
            }
        }
    }



}