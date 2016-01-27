using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class Inventory {
    public InvItem[] inv { get; set; }
    public bool protectsuction { get; set; }



    public Inventory(int size)
    {
        inv = new InvItem[size];
        for (int k = 0; k < size; k++)
        {
            inv[k] = new InvItem();
        }
        protectsuction = false;
    }
    
    public int getcount(string name)
    {
        int returner = 0;

        for (int k = 0; k < inv.Length; k++)
        {
            if (inv[k].socket.asock.name == name)
            {
                returner += inv[k].num;
            }
        }

        return returner;
    }

    public void remamount(string name, int quant)
    {
        for (int k = 0; k < inv.Length; k++)
        {
            if (inv[k].socket.asock.name == name)
            {
                int amoutn = inv[k].num;
                if (amoutn > quant)
                {
                    inv[k].num = amoutn - quant;
                    return;
                }
                else
                {
                    quant = quant - amoutn;
                    inv[k].num = 0;
                }
            }
        }
    }

    public void add(string aname, int quant)
    {
        int i = -1;
        for (int k = 0; k < inv.Length; k++)
        {
            if (inv[k].socket.asock.name == aname)
            {
                inv[k].num += quant;
                return;
            }
            if (i == -1 && inv[k].num == 0)
            {
                i = k;
            }
        }

        if (i != -1)
        {
            inv[i] = new InvItem(aname, quant);
        }
    }


[Serializable]
    public class InvItem
    {
        public socketIG socket { get; set; }
        public int num { get; set; }
        public InvItem()
        {
            socket = new socketIG();
            num = 0;
        }
        public InvItem(socketIG sock, int numio)
        {
            socket = sock;
            num = numio;
        }
        public InvItem(string name, int numio)
        {
            socket = new socketIG(name);
            num = numio;
        }

        public void DrawItem(Rect currect, GUIStyle MenuStyle, Texture2D Tvoid)
        {
            if (socket.asock.name != "")
            {
                guitools.SetStyle(MenuStyle, guitools.RGB(255, 255, 255, 255), Color.black, (int)Math.Round(18f));
                guitools.TextureStyle(MenuStyle, socket.asock.icon);
                GUI.Label(guitools.GRectRelative(0.9f, currect), "", MenuStyle);
                guitools.TextureStyle(MenuStyle, Tvoid);
                guitools.SetStyle(MenuStyle, guitools.RGB(220, 220, 220, 255), guitools.Colorific(), (int)Math.Round(10f));
                //GUI.Label(guitools.GRectRelative(0.55f, 0.55f, 0.5f, 0.6f, guitools.GRectRelative(0.9f, currect)), num.ToString(), MenuStyle);
                MenuStyle.alignment = TextAnchor.LowerRight;
                GUI.Label(guitools.GRectRelative(0.9f, currect), num.ToString(), MenuStyle);
                MenuStyle.alignment = TextAnchor.MiddleCenter;
            }
        }

        public InvItem InteractItem(InvItem MouseInvHeld, Inventory theinv, int k)
        {
            if (Event.current.button == 1)
            {
                if (MouseInvHeld.num == 0)
                {
                    if (socket.asock.name == "")
                    {
                        //nil
                    }
                    else
                    {
                        int taken = num / 2;
                        MouseInvHeld = new Inventory.InvItem(socket, taken);
                        theinv.inv[k] = new Inventory.InvItem(socket, num - taken);
                    }
                }
                else
                {
                    if (socket.asock.name == "")
                    {
                        int taken = MouseInvHeld.num / 2;
                        theinv.inv[k] = new Inventory.InvItem(MouseInvHeld.socket, taken);
                        MouseInvHeld = new Inventory.InvItem(MouseInvHeld.socket, MouseInvHeld.num - taken);
                    }
                    else
                    {
                        if (MouseInvHeld.socket.asock.name == socket.asock.name)
                        {
                            theinv.inv[k] = new Inventory.InvItem(MouseInvHeld.socket, num + MouseInvHeld.num);
                            MouseInvHeld = new Inventory.InvItem();
                        }
                        else
                        {
                            Inventory.InvItem BUFF = new Inventory.InvItem(MouseInvHeld.socket, MouseInvHeld.num);
                            MouseInvHeld = new Inventory.InvItem(socket, num);
                            theinv.inv[k] = new Inventory.InvItem(BUFF.socket, BUFF.num);
                        }
                    }
                }
            }
            else
            {
                if (MouseInvHeld.num == 0)
                {
                    if (socket.asock.name == "")
                    {
                        //nil
                    }
                    else
                    {
                        MouseInvHeld = new Inventory.InvItem(socket, num);
                        theinv.inv[k] = new Inventory.InvItem();
                    }
                }
                else
                {
                    if (socket.asock.name == "")
                    {
                        theinv.inv[k] = new Inventory.InvItem(MouseInvHeld.socket, MouseInvHeld.num);
                        MouseInvHeld = new Inventory.InvItem();
                    }
                    else
                    {
                        if (MouseInvHeld.socket.asock.name == socket.asock.name)
                        {
                            theinv.inv[k] = new Inventory.InvItem(MouseInvHeld.socket, num + MouseInvHeld.num);
                            MouseInvHeld = new Inventory.InvItem();
                        }
                        else
                        {
                            Inventory.InvItem BUFF = new Inventory.InvItem(MouseInvHeld.socket, MouseInvHeld.num);
                            MouseInvHeld = new Inventory.InvItem(socket, num);
                            theinv.inv[k] = new Inventory.InvItem(BUFF.socket, BUFF.num);
                        }
                    }
                }
            }
            return MouseInvHeld;
        }
    }

}
