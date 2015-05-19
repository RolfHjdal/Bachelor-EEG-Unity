using UnityEngine;
using System.Collections;

public class Face : MonoBehaviour
{
    public Texture2D eyeNeutralTex;
    public Texture2D blinkTex;
    public Texture2D winkLeftTex;
    public Texture2D winkRightTex;
    public Texture2D lookLeftTex;
    public Texture2D lookRightTex;
    public Texture2D raiseBrowTex;

    public Texture2D lowerFaceNeutralTex;
    public Texture2D smileTex;
    public Texture2D clenchTex;
    public Texture2D smirkLeftTex;
    public Texture2D smirkRightTex;
    public Texture2D laughTex;

    public Texture2D upperFaceTex;
    public Texture2D midFaceTex;

    private EyeAction eyeAction;

    private LowerFaceAction lowerFaceAction;
    private float lowerFacePower;

    private UpperFaceAction upperFaceAction;
    private float upperFacePower;

    private Eye eye;
    private UpperFace upperFace;
    private LowerFace lowerFace;

    private bool show = false;
    private float x = 650;
    private float y = 170;

    private Rect upperFaceRect;
    private Rect midFaceRect;

    public void Start()
    {
        float tempX = x;
        float tempY = y;

        upperFaceRect = new Rect(tempX, tempY, upperFaceTex.width, upperFaceTex.height);

        tempY += upperFaceTex.height;

        if (eye == null)
            eye = new Eye(tempX, tempY, eyeNeutralTex, blinkTex, winkLeftTex, winkRightTex, lookLeftTex, lookRightTex);

        if (upperFace == null)
            upperFace = new UpperFace(tempX, tempY, eyeNeutralTex, raiseBrowTex);
        
        tempY += eyeNeutralTex.height;

        midFaceRect = new Rect(tempX, tempY, midFaceTex.width, midFaceTex.height);

        tempY += midFaceRect.height;

        if (lowerFace == null)
            lowerFace = new LowerFace(tempX, tempY, lowerFaceNeutralTex, smileTex, clenchTex, smirkLeftTex, smirkRightTex, laughTex);
    }

    public void Show()
    {
        show = true;
    }

    public void Hide()
    {
        show = false;
    }

    public void CheckEye()
    {
        if (EmoExpressiv.isBlink)
        {
            eyeAction = EyeAction.Blink;
            return;
        }
        if (EmoExpressiv.isLeftWink)
        {
            eyeAction = EyeAction.WinkLeft;
            return;
        }
        if (EmoExpressiv.isRightWink)
        {
            eyeAction = EyeAction.WinkRight;
            return;
        }
        if (EmoExpressiv.isLookingLeft)
        {
            eyeAction = EyeAction.LookLeft;
            return;
        }
        if (EmoExpressiv.isLookingRight)
        {
            eyeAction = EyeAction.LookRight;
            return;
        }

        eyeAction = EyeAction.Neutral;
        return;
    }

    public void CheckLowerFace()
    {
        if (EmoExpressiv.smileExtent >= 0.1F)
        {
            lowerFaceAction = LowerFaceAction.Smile;
            lowerFacePower = EmoExpressiv.smileExtent;
            return;
        }

        if (EmoExpressiv.clenchExtent >= 0.1F)
        {
            lowerFaceAction = LowerFaceAction.Clench;
            lowerFacePower = EmoExpressiv.clenchExtent;
            return;
        }

        lowerFacePower = 0.0F;
        return;
    }

    public void CheckUpperFace()
    {
        if (EmoExpressiv.eyebrowExtent >= 0.1F)
        {
            upperFaceAction = UpperFaceAction.RaiseBrow;
            upperFacePower = EmoExpressiv.eyebrowExtent;
            return;
        }

        upperFacePower = 0.0F;
        return;
    }

    public void OnGUI()
    {
        if (show)
        {
            GUI.DrawTexture(upperFaceRect, upperFaceTex);

            CheckUpperFace();
            upperFace.action = upperFaceAction;
            upperFace.power = upperFacePower;
            upperFace.OnGUI();

            if (upperFace.power < 0.1F)
            {
                CheckEye();
                eye.action = eyeAction;
                eye.OnGUI();
            }

            GUI.DrawTexture(midFaceRect, midFaceTex);

            CheckLowerFace();
            lowerFace.action = lowerFaceAction;
            lowerFace.power = lowerFacePower;
            lowerFace.OnGUI();
        }
    }
}
