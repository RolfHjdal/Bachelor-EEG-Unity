using UnityEngine;
using System.Collections;

public class EmoEngineTest : MonoBehaviour
{
	// Use this for initialization
	void Start () 
    {
        //enable cognitiv action
        EmoCognitiv.EnableCognitivAction(EdkDll.EE_CognitivAction_t.COG_LIFT, true);
        EmoCognitiv.EnableCognitivAction(EdkDll.EE_CognitivAction_t.COG_PUSH, true);
        EmoCognitiv.EnableCognitivAction(EdkDll.EE_CognitivAction_t.COG_PULL, true);
        EmoCognitiv.EnableCognitivActionsList();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyUp(KeyCode.P)) { EmoCognitiv.StartTrainingCognitiv(EdkDll.EE_CognitivAction_t.COG_NEUTRAL); }
        if (Input.GetKeyUp(KeyCode.O)) { EmoCognitiv.StartTrainingCognitiv(EdkDll.EE_CognitivAction_t.COG_LIFT); }
        if (Input.GetKeyUp(KeyCode.I)) { EmoCognitiv.StartTrainingCognitiv(EdkDll.EE_CognitivAction_t.COG_PUSH); }
        if (Input.GetKeyUp(KeyCode.U)) { EmoCognitiv.StartTrainingCognitiv(EdkDll.EE_CognitivAction_t.COG_PULL); }
             
	}  
	
}

