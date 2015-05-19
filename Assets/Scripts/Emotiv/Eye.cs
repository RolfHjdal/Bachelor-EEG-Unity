using UnityEngine;
using System.Collections;

public enum EyeAction
{
    Neutral = 0,
    Blink = 1,
    WinkLeft = 2,
    WinkRight = 3,
    LookLeft = 4,
    LookRight = 5
}

public class Eye
{
    public EyeAction action = EyeAction.Neutral;

    private Texture2D neutralTex;
    private Texture2D blinkTex;
    private Texture2D winkLeftTex;
    private Texture2D winkRightTex;
    private Texture2D lookLeftTex;
    private Texture2D lookRightTex;
    private Texture2D raiseBrowTex;

    private Rect rect;
    private Texture2D curTex;

    public Eye(float x, float y, Texture2D Tex0, Texture2D Tex1, Texture2D Tex2, Texture2D Tex3, Texture2D Tex4, Texture2D Tex5)
    {
        neutralTex = Tex0;
        blinkTex = Tex1;
        winkLeftTex = Tex2;
        winkRightTex = Tex3;
        lookLeftTex = Tex4;
        lookRightTex = Tex5;

        curTex = neutralTex;
        rect = new Rect(x, y, curTex.width, curTex.height);
    }

    public void Draw()
    {
        rect.width = curTex.width;
        rect.height = curTex.height;
        GUI.DrawTexture(rect, curTex);
    }

    public void OnGUI()
    {
        switch (action)
        {
            case EyeAction.Neutral:
                curTex = neutralTex;
                Draw();
                break;
            case EyeAction.Blink:
                curTex = blinkTex;
                Draw();
                break;
            case EyeAction.LookLeft:
                curTex = lookLeftTex;
                Draw();
                break;
            case EyeAction.LookRight:
                curTex = lookRightTex;
                Draw();
                break;
            case EyeAction.WinkLeft:
                curTex = winkLeftTex;
                Draw();
                break;
            case EyeAction.WinkRight:
                curTex = winkRightTex;
                Draw();
                break;
        }
    }
}
