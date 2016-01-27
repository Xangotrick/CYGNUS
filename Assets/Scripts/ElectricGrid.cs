using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class ElectricGrid {
    public List<socketIG> thelist { get; set; }

    public ElectricGrid(socketIG astart)
    {
        thelist = new List<socketIG>();
        thelist.Add(astart);
    }
    public ElectricGrid(List<socketIG> alist)
    {
        thelist = new List<socketIG>();
        thelist = alist;
    }

    public void refreshoncreate()
    {
        ///connects
        if(thelist.Count != 0)
        {
            for (int kw = 0; kw < thelist.Count; kw++)
            {
                thelist[kw].asock.getelec.resetcon();
                if (thelist.Count > 0)
                {
                    for (int k = 0; k < thelist.Count; k++)
                    {
                        if (thelist[kw].instance != thelist[k].instance)
                        {
                            thelist[kw].asock.getelec.setcon(thelist[kw].location, thelist[k].location, k);
                        }
                    }
                }
                if (thelist[kw].asock.iswire)
                {
                    thelist[kw].resetwires();
                }
            }
        }
    }

    public static List<List<socketIG>> rebuild(List<socketIG> thelist)
    {
        List<List<socketIG>> returner = new List<List<socketIG>>();

        if (thelist.Count > 0)
        {
            for (int k = 0; k < thelist.Count; k++)
            {
                if (returner.Count > 0)
                {
                    List<int> listofconnectedlists = new List<int>();
                    for (int k2 = 0; k2 < returner.Count; k2++)
                    {
                        foreach (socketIG asock in returner[k2])
                        {
                            if (areconnected(asock, thelist[k]))
                            {
                                listofconnectedlists.Add(k2);
                                break;
                            }
                        }
                    }

                    if (listofconnectedlists.Count == 0)
                    {
                        List<socketIG> alist = new List<socketIG>();
                        alist.Add(thelist[k]);
                        returner.Add(alist);
                    }
                    else
                    {
                        if (listofconnectedlists.Count == 1)
                        {
                            returner[listofconnectedlists[0]].Add(thelist[k]);
                        }
                        else
                        {
                            for (int k3 = listofconnectedlists.Count - 1; k3 > 0; k3--)
                            {
                                foreach (socketIG asock in returner[listofconnectedlists[k3]])
                                {
                                    returner[listofconnectedlists[0]].Add(asock);
                                }
                                returner.RemoveAt(listofconnectedlists[k3]);
                            }
                            returner[listofconnectedlists[0]].Add(thelist[k]);
                        }
                    }

                }
                else
                {
                    List<socketIG> alist = new List<socketIG>();
                    alist.Add(thelist[k]);
                    returner.Add(alist);
                }
            }
        }

        return returner;
    }

    




    public static bool isthereitematpos(List<socketIG> thelist, Vector3 avect)
    {
        bool returner = false;

        foreach (socketIG asock in thelist)
        {
            if (asock.location == avect)
            {
                returner = true;
            }
        }

        return returner;
    }
    public static socketIG getitematpos(List<socketIG> thelist, Vector3 avect)
    {
        socketIG returner = null;

        foreach (socketIG asock in thelist)
        {
            if (asock.location == avect)
            {
                returner = asock;
            }
        }

        return returner;
    }
    public static bool areconnected(socketIG A, socketIG B)
    {
        if ((A.location - B.location).magnitude == 1)
        {
            return true;
        }
        return false;
    }
}
