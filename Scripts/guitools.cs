using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class guitools : MonoBehaviour {

    public static float GetPlaneAngle(Vector2 avect)
    {
        float returner = 0;

        returner = Vector2.Angle(new Vector2(1, 0), avect);
        if (Vector3.Cross(new Vector3(1, 0, 0), new Vector3(avect.x, avect.y, 0)).z >= 0)
        {
            returner = 360 - returner;
        }

        return returner;
    }
    public static float GetPlaneAngleFromNorth(Vector2 avect)
    {
        float returner = GetPlaneAngle(avect);

        returner = (90 + returner) % 360;

        while (returner < 0)
        {
            returner += 360;
        }

        returner = returner % 360;

        return returner;
    }

    public static Color Colorific(float intensity = 1f)
    {
        return new Color((float)Math.Abs(Math.Cos(Time.time / (1f * Time.timeScale))) * intensity, (float)Math.Abs(Math.Cos(Time.time / (1f * Time.timeScale) + 45)) * intensity, (float)Math.Abs(Math.Cos(Time.time / (1f * Time.timeScale) + 45 + 45)) * intensity);
    }
    public static Color Colorific(float input,float intensity = 1f)
    {
        return new Color((float)Math.Abs(Math.Cos(input / (1f * Time.timeScale))) * intensity, (float)Math.Abs(Math.Cos(input / (1f * Time.timeScale) + 45)) * intensity, (float)Math.Abs(Math.Cos(input / (1f * Time.timeScale) + 45 + 45)) * intensity);
    }

    public static void DrawPoly(Vector2 poscenter, int N, float radius, Color acolor)
    {
        List<Vector2> Points = new List<Vector2>();
        for (int k = 0; k < N; k++)
        {
            Points.Add(new Vector2(poscenter.x + radius * (float)Math.Cos(2f * k * Math.PI / N), poscenter.y + radius * (float)Math.Sin(2f * k * Math.PI / N)));
        }
        Drawing.DrawLine(Points[0], Points[Points.Count - 1], acolor, 1, false);
        for (int k = 0; k < Points.Count - 1; k++)
        {
            Drawing.DrawLine(Points[k], Points[k + 1], acolor, 1, false);
        }
    }
    public static void DrawPolyAngle(Vector2 poscenter, int N, float radius, Color acolor, float angle, float awidth = 1, bool ananti = false)
    {
        List<Vector2> Points = new List<Vector2>();
        for (int k = 0; k < N; k++)
        {
            Points.Add(new Vector2(poscenter.x + radius * (float)Math.Cos(2f * k * Math.PI / N - angle * Math.PI / 180f), poscenter.y + radius * (float)Math.Sin(2f * k * Math.PI / N - angle * Math.PI / 180f)));
        }
        Drawing.DrawLine(Points[0], Points[Points.Count - 1], acolor, awidth, ananti);
        for (int k = 0; k < Points.Count - 1; k++)
        {
            Drawing.DrawLine(Points[k], Points[k + 1], acolor, awidth, ananti);
        }
        //Drawing.DrawLine(Points[0], poscenter, Color.magenta, 1, false);
    }
    public static void DrawPolyArc(Vector2 poscenter, int N, float radius, Color acolor, float degA, float degB, float awidth = 1)
    {
        List<Vector2> Points = new List<Vector2>();
        for (int k = 0; k < N; k++)
        {
            float anglefutpoint = (2f * k * 180f / N);
            if (anglefutpoint > degA && anglefutpoint < degB)
            {
                Points.Add(new Vector2(poscenter.x + radius * (float)Math.Cos(2f * k * Math.PI / N), poscenter.y - radius * (float)Math.Sin(2f * k * Math.PI / N)));
            }
        }
        if (Points.Count != 0)
        {
            for (int k = 0; k < Points.Count - 1; k++)
            {
                Drawing.DrawLine(Points[k], Points[k + 1], acolor, awidth, false);
            }
        }
    }
    public static void DrawPolyFromPoints(Vector2 poscenter, int N, float radius, Color acolor, Vector2 posA, Vector2 posB)
    {
        List<Vector2> Points = new List<Vector2>();
        for (int k = 0; k < N; k++)
        {
            float anglefutpoint = (2f * k * 180f / N);
            float Am = GetPlaneAngle(posA - poscenter);
            float AM = GetPlaneAngle(posB - poscenter);
            if (Am > AM)
            {
                float At = AM;
                AM = Am;
                Am = At;
            }

            if (anglefutpoint > Am && anglefutpoint < AM)
            {
                Points.Add(new Vector2(poscenter.x + radius * (float)Math.Cos(2f * k * Math.PI / N), poscenter.y - radius * (float)Math.Sin(2f * k * Math.PI / N)));
            }
        }
        if (Points.Count != 0)
        {
            for (int k = 0; k < Points.Count - 1; k++)
            {
                Drawing.DrawLine(Points[k], Points[k + 1], acolor, 1, false);
            }
            Drawing.DrawLine(posA, Points[0], acolor, 1, false);
            Drawing.DrawLine(Points[Points.Count - 1], posB, acolor, 1, false);
        }
    }
    public static void DrawJoiningArcsFromPoints(Vector2 posA, Vector2 posB, int N, Color acolor, float excentric, int width, Color color)
    {
        Vector2 poscenter = new Vector2(0, 0);

        Vector2 tempvectoringcenter1 = posB - posA;
        poscenter = posA + tempvectoringcenter1 / 2;
        tempvectoringcenter1 = new Vector2(-tempvectoringcenter1.y, tempvectoringcenter1.x) / (float)Math.Sqrt(Vector2.SqrMagnitude(tempvectoringcenter1));
        poscenter += excentric * tempvectoringcenter1;

        float radius = (float)Math.Sqrt(Vector2.SqrMagnitude(poscenter - posA));

        //GuiTools.DrawPoly(poscenter, N, radius, GuiTools.RGB(0, 0, 255, 255));
        //Drawing.DrawLine(posA, posB, RGB(0, 0, 255, 255), 1, false);

        List<Vector2> Points = new List<Vector2>();
        for (int k = 0; k < N; k++)
        {
            float startanglepi = GetPlaneAngle(posA - poscenter) * (float)Math.PI / 180f;
            float anglefutpoint = ((2f * k * 180f / N) + startanglepi * 180f / (float)Math.PI);
            float Am = (startanglepi * 180f / (float)Math.PI);
            float AM = (Math.Abs(GetPlaneAngle(posB - poscenter)));
            if (AM < Am)
            {
                AM += 360;
            }

            if (anglefutpoint > Am && anglefutpoint < AM)
            {
                Points.Add(new Vector2(poscenter.x + radius * (float)Math.Cos(2f * k * Math.PI / N + startanglepi), poscenter.y - radius * (float)Math.Sin(2f * k * Math.PI / N + startanglepi)));
            }
        }
        acolor = color;
        if (Points.Count != 0)
        {
            for (int k = 0; k < Points.Count - 1; k++)
            {
                Drawing.DrawLine(Points[k], Points[k + 1], acolor, width, false);
            }
            //if (GetPlaneAngle(posA - poscenter) > GetPlaneAngle(posB - poscenter))
            //{
            //    Drawing.DrawLine(posB, Points[0], ColorRGB(255,255,255,255), 1, false);
            //    Drawing.DrawLine(Points[Points.Count - 1], posA, acolor, 1, false);
            //}
            //else
            //{
            Drawing.DrawLine(posA, Points[0], acolor, width, false);
            Drawing.DrawLine(Points[Points.Count - 1], posB, acolor, width, false);
            //}
        }
    }
    public static void DrawRect(Rect arect, float linewidth, Color thecolor, bool isantialias)
    {
        Drawing.DrawLine(new Vector2(arect.x, arect.y) + new Vector2(0, 0), new Vector2(arect.x, arect.y) + new Vector2(arect.width, 0), thecolor, linewidth, isantialias);
        Drawing.DrawLine(new Vector2(arect.x, arect.y) + new Vector2(arect.width, 0), new Vector2(arect.x, arect.y) + new Vector2(arect.width, arect.height), thecolor, linewidth, isantialias);
        Drawing.DrawLine(new Vector2(arect.x, arect.y) + new Vector2(arect.width, arect.height), new Vector2(arect.x, arect.y) + new Vector2(0, arect.height), thecolor, linewidth, isantialias);
        Drawing.DrawLine(new Vector2(arect.x, arect.y) + new Vector2(0, arect.height), new Vector2(arect.x, arect.y) + new Vector2(0, 0), thecolor, linewidth, isantialias);
    }
    public static void DrawLineFromRect(Rect arectaaa, Color acolor, float awidth, bool atrue)
    {
        Drawing.DrawLine(new Vector2(arectaaa.x, arectaaa.y), new Vector2(arectaaa.x + arectaaa.width, arectaaa.y + arectaaa.height), acolor, awidth, atrue);
    }

    public static void DrawDashed(Vector2 posstart, Vector2 posend, Color color, float width, bool antialias, float length)
    {
        float dist = (posend - posstart).magnitude;
        int lengthint = (int)Math.Round(length);
        int distint = (int)Math.Round(dist);
        int numofdivs = ((distint - (distint % lengthint)) / lengthint);
        bool isdrawing = true;
        Vector2 segmentvect = (posend - posstart).normalized * (lengthint);
        if (numofdivs > 1)
        {
            for (int k = 0; k < numofdivs - 1; k++)
            {
                if (isdrawing && (isinrect(posstart + k * segmentvect, new Rect(0, 0, Screen.width, Screen.height)) || isinrect(posstart + (k + 1) * segmentvect, new Rect(0, 0, Screen.width, Screen.height))))
                {
                    Drawing.DrawLine(posstart + k * segmentvect, posstart + (k + 1) * segmentvect, color, width, antialias);
                }
                isdrawing = !isdrawing;
            }
            if (isdrawing)
            {
                Drawing.DrawLine(posstart + numofdivs * segmentvect, posend, color, width, antialias);
            }
        }
        else
        {
            if (numofdivs == 1)
            {
                Drawing.DrawLine(posstart, posend, color, width, antialias);
            }
            else
            {
                Drawing.DrawLine(posstart, posend, color, width, antialias);
            }
        }
        //Drawing.DrawLine(posstart, posend, color, width, antialias);
    }

    public static Color RGB(float R, float G, float B, float A = 255)
    {
        return new Color(R / 255f, G / 255f, B / 255f, A / 255f);
    }

    public static bool isinrect(Vector2 vect, Rect rectangle)
    {
        if (rectangle.x < vect.x && vect.x < rectangle.x + rectangle.width)
        {
            if (rectangle.y < vect.y && vect.y < rectangle.y + rectangle.height)
            {
                return true;
            }
        }
        return false;
    }
    public static Rect GRectRelative(float posperx, float pospery, float widthper, float heightper, Rect arectaaa, float ratio = 1)
    {
        if (ratio == 1)
        {
            return new Rect(arectaaa.x + posperx * arectaaa.width, arectaaa.y + pospery * arectaaa.height, widthper * arectaaa.width, heightper * arectaaa.height);
        }
        else
        {
            return GRectRelative(ratio,new Rect(arectaaa.x + posperx * arectaaa.width, arectaaa.y + pospery * arectaaa.height, widthper * arectaaa.width, heightper * arectaaa.height));
        }
    }
    public static Rect GRectRelative(float ratio, Rect arectaaa)
    {
        return GRectRelative((1 - ratio) / 2f, (1 - ratio) / 2f, ratio, ratio, arectaaa);
    }
    public static Rect RectFromCenter(Vector2 apos, float width, float height)
    {
        return new Rect(apos.x - width / 2f, apos.y - height / 2f, width, height);
    }


    public static Color FloatToColor(float input, float min, float max)
    {
        float alpha = 35;
        float quarter = (max - min) / 4;
        float transinput = input - min;
        if (transinput < quarter)
        {
            if (transinput < quarter / 15)
            {
                return RGB(0, (transinput) / quarter * 255, 255, alpha * 0);
            }
            else
            {
                return RGB(0, (transinput) / quarter * 255, 255, alpha);
            }
        }
        if (transinput < 2 * quarter && transinput >= quarter)
        {
            return RGB(0, 255, 255 - (transinput - quarter) / quarter * 255, alpha * 2);
        }
        if (transinput < 3 * quarter && transinput >= 2 * quarter)
        {
            return RGB((transinput - 2 * quarter) / quarter * 255, 255, 0, alpha * 3);
        }
        if (transinput >= 3 * quarter && transinput < 4 * quarter)
        {
            return RGB(255, 255 - (transinput - 3 * quarter) / quarter * 255, 0, alpha * 4);
        }
        if (transinput >= 4 * quarter)
        {
            return RGB(0, 0, 0, alpha * 5);
        }
        return RGB(0, 0, 0, alpha * 0);
    }
    public static Color FloatToColor2(float input, float min, float max, float alpha)
    {
        float sextan = (max - min) / 6;
        float transinput = input - min;
        if (transinput < sextan)
        {
            if (transinput < sextan / 15)
            {
                return RGB(0, (transinput) / sextan * 255, 255, alpha);
            }
            else
            {
                return RGB(0, (transinput) / sextan * 255, 255, alpha);
            }
        }
        if (transinput < 2 * sextan && transinput >= sextan)
        {
            return RGB(0, 255, 255 - (transinput - sextan) / sextan * 255, alpha);
        }
        if (transinput < 3 * sextan && transinput >= 2 * sextan)
        {
            return RGB((transinput - 2 * sextan) / sextan * 255, 255, 0, alpha);
        }
        if (transinput >= 3 * sextan && transinput < 4 * sextan)
        {
            return RGB(255, 255 - (transinput - 3 * sextan) / sextan * 255, 0, alpha);
        }
        if (transinput >= 4 * sextan && transinput < 5 * sextan)
        {
            return RGB(255, 0, (transinput - 4 * sextan) / sextan * 255, alpha);
        }
        if (transinput >= 5 * sextan)
        {
            return RGB(255 - (transinput - 5 * sextan) / sextan * 255, 0, 255, alpha);
        }
        return RGB(0, 0, 0, alpha * 0);
    }
    public static Color FloatToColor3(float input, float min, float max, float alpha)
    {
        float sextan = (max - min) / 6;
        float transinput = input - min;
        if (transinput < sextan)
        {
            if (transinput < sextan / 15)
            {
                return RGB(0, (transinput) / sextan * 255, 255, alpha);
            }
            else
            {
                return RGB(0, (transinput) / sextan * 255, 255, alpha);
            }
        }
        if (transinput < 2 * sextan && transinput >= sextan)
        {
            return RGB(0, 255, 255 - (transinput - sextan) / sextan * 255, alpha);
        }
        if (transinput < 3 * sextan && transinput >= 2 * sextan)
        {
            return RGB((transinput - 2 * sextan) / sextan * 255, 255, 0, alpha);
        }
        if (transinput >= 3 * sextan && transinput < 4 * sextan)
        {
            return RGB(255, 255 - (transinput - 3 * sextan) / sextan * 255, 0, alpha);
        }
        if (transinput >= 4 * sextan)
        {
            return RGB(255, 0, (transinput - 4 * sextan) / sextan * 255, alpha);
        }
        return RGB(0, 0, 0, alpha * 0);
    }
    public static Color FloatToColor4(float input, float min, float max, Color Colora, Color Colorb)
    {
        float afloat = (input - min) / (1f * (max - min));
        return new Color(Colora.r + (Colorb.r - Colora.r) * afloat, Colora.g + (Colorb.g - Colora.g) * afloat, Colora.b + (Colorb.b - Colora.b) * afloat, Colora.a + (Colorb.a - Colora.a) * afloat);
    }

    public static void DrawCross(Vector2 center, int width, Color color, float dist, float angle = 0)
    {
        Vector2 A = fextramath.VectFromAngle(center, 0 + angle, dist);
        Vector2 B = fextramath.VectFromAngle(center, 90 + angle, dist);
        Vector2 C = fextramath.VectFromAngle(center, 180 + angle, dist);
        Vector2 D = fextramath.VectFromAngle(center, 270 + angle, dist);

        Drawing.DrawLine(A, C, color, width, false);
        Drawing.DrawLine(B, D, color, width, false);
    }
    public static void DrawCross2(Vector2 center, int width, Color color, float distmin, float distmax, float angle = 0)
    {
        Vector2 A = fextramath.VectFromAngle(center, 0 + angle, distmin);
        Vector2 A2 = fextramath.VectFromAngle(center, 0 + angle, distmax);
        Vector2 B = fextramath.VectFromAngle(center, 90 + angle, distmin);
        Vector2 B2 = fextramath.VectFromAngle(center, 90 + angle, distmax);
        Vector2 C = fextramath.VectFromAngle(center, 180 + angle, distmin);
        Vector2 C2 = fextramath.VectFromAngle(center, 180 + angle, distmax);
        Vector2 D = fextramath.VectFromAngle(center, 270 + angle, distmin);
        Vector2 D2 = fextramath.VectFromAngle(center, 270 + angle, distmax);

        Drawing.DrawLine(A,A2 , color, width, false);
        Drawing.DrawLine(B,B2 , color, width, false);
        Drawing.DrawLine(C,C2 , color, width, false);
        Drawing.DrawLine(D,D2 , color, width, false);
    }

    public static void TextureStyle(GUIStyle astyle, Texture2D atext)
    {
        astyle.active.background = atext;
        astyle.onActive.background = atext;
        astyle.normal.background = atext;
        astyle.onNormal.background = atext;
        astyle.hover.background = atext;
        astyle.onHover.background = atext;
        astyle.focused.background = atext;
        astyle.onFocused.background = atext;
    }

    public static void TextColor(GUIStyle astyle, Color acolor)
    {
        astyle.normal.textColor = acolor;
        astyle.onNormal.textColor = acolor;
        astyle.hover.textColor = acolor;
        astyle.onHover.textColor = acolor;
        astyle.focused.textColor = acolor;
        astyle.onFocused.textColor = acolor;
    }
    public static void TextColorBoW(GUIStyle astyle)
    {
        astyle.normal.textColor = Color.black;
        astyle.onNormal.textColor = Color.white;
        astyle.hover.textColor = Color.black;
        astyle.onHover.textColor = Color.white;
        astyle.focused.textColor = Color.black;
        astyle.onFocused.textColor = Color.white;
    }
    public static void TextSize(GUIStyle astyle, int anint)
    {
        astyle.fontSize = (int)Math.Round(anint * Screen.width / 861f);
    }
    public static void SetStyle(GUIStyle astyle, Color backColor, Color textColor, int fontsize)
    {
        fontsize = Mathf.RoundToInt(fontsize * 0.75f);
        GUI.color = backColor;
        TextColor(astyle, textColor);
        TextSize(astyle, fontsize);
    }
}
