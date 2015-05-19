using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class EmoExpressiv : MonoBehaviour
{
    //----------------------------------------
    EmoEngine engine = EmoEngine.Instance;
    public static Boolean isBlink = false;
    public static Boolean isLeftWink = false;
    public static Boolean isRightWink = false;
    public static Boolean isEyesOpen = false;
    public static Boolean isLookingUp = false;
    public static Boolean isLookingDown = false;
    public static Boolean isLookingLeft = false;
    public static Boolean isLookingRight = false;
    public static float eyelidStateLeft = 0.0f;
    public static float eyelidStateRight = 0.0f;
    public static float eyeLocationX = 0.0f;
    public static float eyeLocationY = 0.0f;
    public static float eyebrowExtent = 0.0f;
    public static float smileExtent = 0.0f;
    public static float clenchExtent = 0.0f;
    public static EdkDll.EE_ExpressivAlgo_t upperFaceAction;
    public static EdkDll.EE_ExpressivAlgo_t lowerFaceAction;
    public static float upperFacePower = 0.0f;
    public static float lowerFacePower = 0.0f;
    public static EdkDll.EE_ExpressivAlgo_t[] expAlgoList = { 
												  EdkDll.EE_ExpressivAlgo_t.EXP_BLINK, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_CLENCH, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_EYEBROW, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_FURROW, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_HORIEYE, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_LAUGH, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_NEUTRAL, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_SMILE, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_SMIRK_LEFT, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_SMIRK_RIGHT, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_WINK_LEFT, 
												  EdkDll.EE_ExpressivAlgo_t.EXP_WINK_RIGHT
												  };
    public static Boolean[] isExpActiveList = new Boolean[expAlgoList.Length];
    public static bool IsStarted = false;
    //----------------------------------------

    void Update()
    {

    }
    void Start()
    {
        if (!IsStarted)
        {
            engine.ExpressivEmoStateUpdated +=
            new EmoEngine.ExpressivEmoStateUpdatedEventHandler(engine_ExpressivEmoStateUpdated);
            engine.ExpressivTrainingStarted +=
                new EmoEngine.ExpressivTrainingStartedEventEventHandler(engine_ExpressivTrainingStarted);
            engine.ExpressivTrainingSucceeded +=
                new EmoEngine.ExpressivTrainingSucceededEventHandler(engine_ExpressivTrainingSucceeded);
            engine.ExpressivTrainingCompleted +=
                new EmoEngine.ExpressivTrainingCompletedEventHandler(engine_ExpressivTrainingCompleted);
            engine.ExpressivTrainingRejected +=
                new EmoEngine.ExpressivTrainingRejectedEventHandler(engine_ExpressivTrainingRejected); 
            IsStarted = true;
        }
        
    }
    static void engine_ExpressivEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState;
        isBlink = es.ExpressivIsBlink();
        isLeftWink = es.ExpressivIsLeftWink();
        isRightWink = es.ExpressivIsRightWink();
        isEyesOpen = es.ExpressivIsEyesOpen();
        isLookingUp = es.ExpressivIsLookingUp();
        isLookingDown = es.ExpressivIsLookingDown();
        isLookingLeft = es.ExpressivIsLookingLeft();
        isLookingRight = es.ExpressivIsLookingRight();
        es.ExpressivGetEyelidState(out eyelidStateLeft, out eyelidStateRight);
        es.ExpressivGetEyeLocation(out eyeLocationX, out eyeLocationY);
        eyebrowExtent = es.ExpressivGetEyebrowExtent();
        smileExtent = es.ExpressivGetSmileExtent();
        clenchExtent = es.ExpressivGetClenchExtent();
        upperFaceAction = es.ExpressivGetUpperFaceAction();
        upperFacePower = es.ExpressivGetUpperFaceActionPower();
        lowerFaceAction = es.ExpressivGetLowerFaceAction();
        lowerFacePower = es.ExpressivGetLowerFaceActionPower();
        for (int i = 0; i < expAlgoList.Length; ++i)
        {
            isExpActiveList[i] = es.ExpressivIsActive(expAlgoList[i]);
        }
    }
    static void engine_ExpressivTrainingStarted(object sender, EmoEngineEventArgs e)
    {
       
    }

    static void engine_ExpressivTrainingSucceeded(object sender, EmoEngineEventArgs e)
    {
        EmoEngine.Instance.ExpressivSetTrainingControl(0, EdkDll.EE_ExpressivTrainingControl_t.EXP_ACCEPT);
    }

    static void engine_ExpressivTrainingCompleted(object sender, EmoEngineEventArgs e)
    {
        
    }

    static void engine_ExpressivTrainingRejected(object sender, EmoEngineEventArgs e)
    {
        
    }

    public void StartTrainExpressiv(EdkDll.EE_ExpressivAlgo_t expressivAlg)
    {
        engine.ExpressivSetTrainingAction((uint)EmoUserManagement.currentUser, expressivAlg);
        engine.ExpressivSetTrainingControl((uint)EmoUserManagement.currentUser, EdkDll.EE_ExpressivTrainingControl_t.EXP_START);
    } 
}