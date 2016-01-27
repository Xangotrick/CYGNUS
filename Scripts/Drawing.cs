using UnityEngine;
using System.Collections;

public class Drawing : MonoBehaviour {

    static Texture2D aaLineTex = null;
    static Texture2D lineTex = null;
    public static void DrawLine(Vector2 start, Vector2 end, Color acolor, float width, bool abool = false)
    {
        Color GUIcolor = GUI.color;
        GUI.color = acolor;
        if (!lineTex)
        {
            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
            lineTex.SetPixel(1, 1, Color.white);
            lineTex.Apply();
        }
        if (!aaLineTex)
        {
            aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, true);
            aaLineTex.SetPixel(0, 0, new Color(1, 1, 1, 0));
            aaLineTex.SetPixel(0, 1, Color.white);
            aaLineTex.SetPixel(0, 2, new Color(1, 1, 1, 0));
            aaLineTex.Apply();
        }
        Vector2 d = end - start;
        float a = Mathf.Rad2Deg * Mathf.Atan(d.y / d.x);
        if (d.x < 0)
            a += 180;

        int width2 = (int)Mathf.Ceil(width / 2);

        GUIUtility.RotateAroundPivot(a, start);
        GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), lineTex);
        GUIUtility.RotateAroundPivot(-a, start);
        GUI.color = GUIcolor;
    }
}
