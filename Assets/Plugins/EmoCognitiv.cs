using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class EmoCognitiv : MonoBehaviour
{
    //----------------------------------------
    EmoEngine engine = EmoEngine.Instance;
    public static EdkDll.EE_CognitivAction_t[] cognitivActionList = {EdkDll.EE_CognitivAction_t.COG_NEUTRAL,
                                                                    EdkDll.EE_CognitivAction_t.COG_PUSH,
                                                                    EdkDll.EE_CognitivAction_t.COG_PULL,
                                                                    EdkDll.EE_CognitivAction_t.COG_LIFT,
                                                                    EdkDll.EE_CognitivAction_t.COG_DROP,
                                                                    EdkDll.EE_CognitivAction_t.COG_LEFT,
                                                                    EdkDll.EE_CognitivAction_t.COG_RIGHT,
                                                                    EdkDll.EE_CognitivAction_t.COG_ROTATE_LEFT,
                                                                    EdkDll.EE_CognitivAction_t.COG_ROTATE_RIGHT,
                                                                    EdkDll.EE_CognitivAction_t.COG_ROTATE_CLOCKWISE,
                                                                    EdkDll.EE_CognitivAction_t.COG_ROTATE_COUNTER_CLOCKWISE,
                                                                    EdkDll.EE_CognitivAction_t.COG_ROTATE_FORWARDS,
                                                                    EdkDll.EE_CognitivAction_t.COG_ROTATE_REVERSE,
                                                                    EdkDll.EE_CognitivAction_t.COG_DISAPPEAR
                                                                    };
    public static Boolean[] cognitivActionsEnabled = new Boolean[cognitivActionList.Length];
    public static float[] CognitivActionPower = new float[cognitivActionList.Length];
    public static int cognitivActionLever = 0; //Number Of Active Action   
    public static bool IsStarted = false;
	public static string currentCognitivAction;
    //----------------------------------------  
    void Start() 
    {
        if (!IsStarted)
        {
            cognitivActionsEnabled[0] = true;
            for (int i = 1; i < cognitivActionList.Length; i++)
            {
                cognitivActionsEnabled[i] = false;
            }
            engine.CognitivEmoStateUpdated +=
                new EmoEngine.CognitivEmoStateUpdatedEventHandler(engine_CognitivEmoStateUpdated);
            engine.CognitivTrainingStarted +=
                new EmoEngine.CognitivTrainingStartedEventEventHandler(engine_CognitivTrainingStarted);
            engine.CognitivTrainingSucceeded +=
                new EmoEngine.CognitivTrainingSucceededEventHandler(engine_CognitivTrainingSucceeded);
            engine.CognitivTrainingCompleted +=
                new EmoEngine.CognitivTrainingCompletedEventHandler(engine_CognitivTrainingCompleted);
            engine.CognitivTrainingRejected +=
                new EmoEngine.CognitivTrainingRejectedEventHandler(engine_CognitivTrainingRejected);
            IsStarted = true;
        }
    }
    float timeVariable;
    void Update()
    {
        //if in 2s , there's no updata of cognitivStatei
		//set the state down to 0
		timeVariable += Time.deltaTime;
		if (timeVariable <0.3)
		{
			isNotResponding = true;
		}
		
		if (timeVariable >1.0f) 
		{
			timeVariable = 0.0f;
			if (isNotResponding)
			{
					//call smooth state
					ResetCognitivPower(2);
                    ResetCognitivPower(3);
			}
		}
    }
	public static string getCurrentCognitivAction(){

		return currentCognitivAction;
	}
    public  static bool isNotResponding = true;
    static void engine_CognitivEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        //Debug.LogError("CognitivEmoStateUpdated");
        isNotResponding = false;
        EmoState es = e.emoState;
        EdkDll.EE_CognitivAction_t cogAction = es.CognitivGetCurrentAction();
		currentCognitivAction = cogAction.ToString();
        //Debug.LogError(cogAction);
        float power = (float)es.CognitivGetCurrentActionPower();
        //Debug.LogError(power + ":" +(uint)cogAction);
        //CognitivActionPower[(uint)cogAction] = power;
        for (int i = 1; i < cognitivActionList.Length; i++)
        {
            if (cogAction == cognitivActionList[i])
            {
                CognitivActionPower[i] = power;
                //Debug.LogError(CognitivActionPower[i] + "----------------------");
            }
            else CognitivActionPower[i] = 0;
        }
    }
    static void engine_CognitivTrainingStarted(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Cognitiv Training Started");
        
    }

    static void engine_CognitivTrainingSucceeded(object sender, EmoEngineEventArgs e)
    {
        EmoEngine.Instance.CognitivSetTrainingControl(0, EdkDll.EE_CognitivTrainingControl_t.COG_ACCEPT);
        Debug.Log("Cognitiv Training Succeeded");
    }

    static void engine_CognitivTrainingCompleted(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Cognitiv Training Completed");

    }

    static void engine_CognitivTrainingRejected(object sender, EmoEngineEventArgs e)
    {
        Debug.Log("Cognitiv Training Rejected");

    }
    /// <summary>
    /// Start traning cognitiv action
    /// </summary>
    /// <param name="cognitivAction">Cognitiv Action</param>
    public static void StartTrainingCognitiv(EdkDll.EE_CognitivAction_t cognitivAction)
    {
        if (cognitivAction == EdkDll.EE_CognitivAction_t.COG_NEUTRAL)
        {
            EmoEngine.Instance.CognitivSetTrainingAction((uint)EmoUserManagement.currentUser, cognitivAction);
            EmoEngine.Instance.CognitivSetTrainingControl((uint)EmoUserManagement.currentUser, EdkDll.EE_CognitivTrainingControl_t.COG_START);
        }
        else
            for (int i = 1; i < cognitivActionList.Length; i++)
            {
                if (cognitivAction == cognitivActionList[i])
                {
                    Debug.Log("Action compare");
                    if (cognitivActionsEnabled[i])
                    {
                        Debug.Log("Action is enabled");
                        EmoEngine.Instance.CognitivSetTrainingAction((uint)EmoUserManagement.currentUser, cognitivAction);
                        EmoEngine.Instance.CognitivSetTrainingControl((uint)EmoUserManagement.currentUser, EdkDll.EE_CognitivTrainingControl_t.COG_START);
                    }
                    else Debug.Log("Action is not enabled");
                }
            }

    }
    /// <summary>
    /// Enable cognitiv action in arraylist
    /// </summary>
    /// <param name="cognitivAction">Cognitiv Action</param>
    /// <param name="iBool">True = Enable/False = Disable</param>
    public static void EnableCognitivAction(EdkDll.EE_CognitivAction_t cognitivAction, Boolean iBool)
    {
        for (int i = 1; i < cognitivActionList.Length; i++)
        {
            if (cognitivAction == cognitivActionList[i])
            {
                cognitivActionsEnabled[i] = iBool;
                Debug.Log("CognitivEnabledList has changed");
            }
        }

    }
    /// <summary>
    /// Enable actions in arraylist (Working in both Unity 2.5 vs 2.6)
    /// </summary>
    public static void EnableCognitivActionsListEx()
    {
        Debug.Log("Set CognitivList Enable");
        cognitivActionLever = 0;
        for (int i = 1; i < cognitivActionList.Length; i++)
        {
            if (cognitivActionsEnabled[i])
            {
                cognitivActionLever++;
            }
        }
        EdkDll.EE_CognitivAction_t[] activeActions = new EdkDll.EE_CognitivAction_t[cognitivActionLever];
        int tmp = 0;
        for (int i = 1; i < cognitivActionList.Length; i++)
        {
            if (cognitivActionsEnabled[i])
            {
                activeActions[tmp] = cognitivActionList[i];
                tmp++;
            }
        }
        EdkDll.SetMultiActiveActions(EmoUserManagement.currentUser, activeActions, cognitivActionLever);
    }
    /// <summary>
    /// Enable actions in arraylist , Work only in 2.6 or upper
    /// </summary>
    public static void EnableCognitivActionsList()
    {
        Debug.Log("Set CognitivList Enable");
        cognitivActionLever = 0;
        uint cognitivActions = 0x0000;
        for (int i = 1; i < cognitivActionList.Length; i++)
        {
            if (cognitivActionsEnabled[i])
            {
                cognitivActions = cognitivActions | ((uint)cognitivActionList[i]);
                cognitivActionLever++;
            }
        }
        EdkDll.EE_CognitivSetActiveActions((uint)EmoUserManagement.currentUser, cognitivActions);
        //EmoEngine.Instance.CognitivSetActiveActions((uint)EmoUserManagement.currentUser,(uint) cognitivActions);
    }
    /// <summary>
    /// Get cognitiv action power in an array of float
    /// </summary>
    /// <returns></returns>
    public static float[] GetCognitivActionPower()
    {
        return CognitivActionPower;
    }
    public static void ResetAllCognitivPower()
    {
        for (int i = 0; i < cognitivActionList.Length;i++ )
        {
            CognitivActionPower[i] = 0;
        }
    }
    public static void ResetCognitivList()
    {
        for (int i = 1; i < cognitivActionsEnabled.Length; i++)
        {
            cognitivActionsEnabled[i] = false;
        }
    }
    public static void ResetCognitivPower(int cognitivAction)
    {
        CognitivActionPower[cognitivAction] = 0;
    }
}