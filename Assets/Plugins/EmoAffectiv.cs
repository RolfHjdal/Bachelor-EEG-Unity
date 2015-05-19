using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class EmoAffectiv : MonoBehaviour
{
    //----------------------------------------
    EmoEngine engine = EmoEngine.Instance;
    public static float longTermExcitementScore = 0.0f;
    public static float shortTermExcitementScore = 0.0f;
    public static float meditationScore = 0.0f;
    public static float frustrationScore = 0.0f;
    public static float boredomScore = 0.0f;
    public static EdkDll.EE_AffectivAlgo_t[] affAlgoList = { 
												  EdkDll.EE_AffectivAlgo_t.AFF_ENGAGEMENT_BOREDOM,
												  EdkDll.EE_AffectivAlgo_t.AFF_EXCITEMENT,
												  EdkDll.EE_AffectivAlgo_t.AFF_FRUSTRATION,
												  EdkDll.EE_AffectivAlgo_t.AFF_MEDITATION                                                     
												  };

    public static Boolean[] isAffActiveList = new Boolean[affAlgoList.Length];
    public static bool IsStarted = false;
    //----------------------------------------

    void Update()
    {

    }
    void Start()
    {
        if (!IsStarted)
        {
            engine.AffectivEmoStateUpdated +=
              new EmoEngine.AffectivEmoStateUpdatedEventHandler(engine_AffectivEmoStateUpdated);
            IsStarted = true;
        }
       
    }
    static void engine_AffectivEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState;
        longTermExcitementScore =(float) es.AffectivGetExcitementLongTermScore();
        shortTermExcitementScore = (float)es.AffectivGetExcitementShortTermScore();
        for (int i = 0; i < affAlgoList.Length; ++i)
        {
            isAffActiveList[i] = es.AffectivIsActive(affAlgoList[i]);
        }
        meditationScore = (float)es.AffectivGetMeditationScore();
        frustrationScore = (float)es.AffectivGetFrustrationScore();
        boredomScore = (float)es.AffectivGetEngagementBoredomScore();
    }
}