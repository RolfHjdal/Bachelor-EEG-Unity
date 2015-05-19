using UnityEngine;
using System.Collections;

public enum LowerFaceAction
{
    Smile = 0,
    Clench = 1,
    SmirkLeft = 2,
    SmirkRight = 3,
    Laugh = 4
}

public class LowerFace
{
    public LowerFaceAction action = LowerFaceAction.Smile;
    public float power = 0.0F;

    private Texture2D neutralTex;
    private Texture2D smileTex;
    private Texture2D clenchTex;
    private Texture2D smirkLeftTex;
    private Texture2D smirkRightTex;
    private Texture2D laughTex;

    private Rect rect;
    private Texture2D curTex;

    public LowerFace(float x, float y, Texture2D Tex0, Texture2D Tex1, Texture2D Tex2, Texture2D Tex3, Texture2D Tex4, Texture2D Tex5)
    {
        neutralTex = Tex0;
        smileTex = Tex1;
        clenchTex = Tex2;
        smirkLeftTex = Tex3;
        smirkRightTex = Tex4;
        laughTex = Tex5;

        curTex = smileTex;
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
                case LowerFaceAction.Smile:
                    curTex = smileTex;
                    Draw();
                    break;
                case LowerFaceAction.Clench:
                    curTex = clenchTex;
                    Draw();
                    break;
                case LowerFaceAction.SmirkLeft:
                    curTex = smirkLeftTex;
                    Draw();
                    break;
                case LowerFaceAction.SmirkRight:
                    curTex = smirkRightTex;
                    Draw();
                    break;
                case LowerFaceAction.Laugh:
                    curTex = laughTex;
                    Draw();
                    break;
            }
        }
    }
}
