using UnityEngine;
using System.Collections;

public enum UpperFaceAction
{
    RaiseBrow = 0
}

public class UpperFace
{
    public UpperFaceAction action = UpperFaceAction.RaiseBrow;
    public float power = 0.0F;

    private Texture2D neutralTex;
    private Texture2D raiseBrowTex;

    private Rect rect;
    private Texture2D curTex;

    public UpperFace(float x, float y, Texture2D Tex0, Texture2D Tex1)
    {
        neutralTex = Tex0;
        raiseBrowTex = Tex1;

        curTex = raiseBrowTex;
        rect = new Rect(x, y, curTex.width, curTex.height);
    }

    private void Draw()
    {
        rect.width = curTex.width;
        rect.height = curTex.height;
        GUI.DrawTexture(rect, curTex);
    }

    public void OnGUI()
    {
        if (power < 0.1F)
        {
            curTex = neutralTex;
            Draw();
        }
        else
        {
            switch (action)
            {
                case UpperFaceAction.RaiseBrow:
                    curTex = raiseBrowTex;
                    Draw();
                    break;
            }
        }
    }
}

