using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class fextramath : MonoBehaviour {
    public static Vector2 AAA;
    public static Vector2 BBB;

    public static Vector3 VectFromAngle(Vector3 avect, float anang, float adist)
    {
        return avect + new Vector3(adist * (float)Math.Sin(anang * Math.PI / 180f), adist * (float)Math.Cos(anang * Math.PI / 180f), 0);
    }
    public static Vector2 VectFromAngle(Vector2 avect, float anang, float adist)
    {
        return avect + new Vector2(adist * (float)Math.Sin(anang * Math.PI / 180f), adist * (float)Math.Cos(anang * Math.PI / 180f));
    }

    public static int[] AlgorithmRoute(int initpoint, int exitpoint, List<int>[] net)
    {
        int[] returner = new int[10];
        List<List<int>> answerslist = new List<List<int>>();
        answerslist.Add(new List<int>());
        answerslist[0].Add(initpoint);
    tryagain:
        if (answerslist.Count != 0)
        {
            if (answerslist[0].Count > 20)
            {
                goto success;
            }
            List<List<int>> newlistofanswers = new List<List<int>>();
            for (int k = 0; k < answerslist.Count; k++)
            {
                List<int> alist = answerslist[k];
                if (alist.Count != 0)
                {
                    int lastint = alist[alist.Count - 1];
                    if (net[lastint].Count != 0)
                    {
                        for (int kposs = 0; kposs < net[lastint].Count; kposs++)
                        {
                            //ID of the new option
                            int newpossibility = net[lastint][kposs];
                            if (!isintinthislist(newpossibility, alist))
                            {
                                if (newpossibility == exitpoint)
                                {
                                    for (int k2 = 0; k2 < alist.Count; k2++)
                                    {
                                        returner[k2] = alist[k2];
                                    }
                                    returner[alist.Count] = exitpoint;
                                    goto success;
                                }
                                else
                                {
                                    List<int> tobeadded = new List<int>();//Create the new list in the new list of lists
                                    for (int k2 = 0; k2 < alist.Count; k2++)
                                    {
                                        tobeadded.Add(alist[k2]);
                                    }
                                    tobeadded.Add(newpossibility);
                                    newlistofanswers.Add(tobeadded);
                                }
                            }
                        }
                    }
                }
            }
            answerslist = newlistofanswers;
            goto tryagain;
        }

    success:
        return returner;
    }

    public static bool isintinthislist(int anintput, List<int> array)
    {
        if (array.Count != 0)
        {
            for (int k = 0; k < array.Count; k++)
            {
                if (anintput == array[k])
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static float GetVectAngle(Vector2 avect1, Vector2 avect2)
    {
        float returner = 0;

        float angle1 = guitools.GetPlaneAngle(avect1);
        float angle2 = guitools.GetPlaneAngle(avect2);

        returner = angle2 - angle1;
        returner = returner % 360;

        return returner;
    }

    public static bool ispointinpoly(List<Vector2> poly, Vector2 point)
    {
        Vector2 otherpoint = new Vector2();
        foreach(Vector2 avect in poly)
        {
            otherpoint += (avect) / (poly.Count * 1f);
        }
        //otherpoint = poly[0];
        /*
        float maxdist = 0;
        foreach(Vector2 avect in poly)
        {
            float af =(avect - point).magnitude;
            if(af > maxdist)
            {
                maxdist = af;
            }
        }

        otherpoint = VectFromAngle(point, 0, maxdist * 1.3f);
        */
        AAA = otherpoint;
        BBB = point;
        int numofcross = 0;

        if(interesectseg(point, otherpoint,poly[0],poly[poly.Count -1]))
        {
            numofcross++;
        }


        for (int k = 0; k < poly.Count - 1; k++ )
        {
            if (interesectseg(point, otherpoint, poly[k], poly[k+1]))
            {
                numofcross++;
            }
        }

        //numofcross /= 2;

        if(numofcross% 2 == 0)
        {
            return true;
        }

        return false;
    }
    public static bool interesectseg(Vector2 A1,Vector2 A2,Vector2 B1,Vector2 B2)
    {
        Vector2 VA = A2 - A1;
        Vector2 VB = B2 - B1;

        float t = 0;
        float u = 0;
        float rs = V2DCP(VA,VB);

        bool returner = false;

        if(rs == 0)
        {
            returner = false;
        }
        else
        {
            t = V2DCP(B1 - A1, VB) / rs;
            u = V2DCP(B1 - A1, VA) / rs;

            if(t >= 0 && u >= 0 && t <= 1 && u <=1)
            {
                returner = true;
            }
            else
            {
                returner = false;
            }
        }

        return returner;
    }

    public static float V2DCP(Vector2 A, Vector2 B)
    {
        //return (Vector3.Cross(new Vector3(A.x, A.y), new Vector3(B.x, B.y))).magnitude;
        return A.x * B.y - A.y * B.x;
    }
}
