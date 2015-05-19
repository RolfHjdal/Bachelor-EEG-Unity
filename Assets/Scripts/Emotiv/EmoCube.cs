using UnityEngine;
using System.Collections;

public class EmoCube : MonoBehaviour
{
    public float maxDistance = 0.0f;    
    public float refDistance = 5.0f;
    public GameObject OrgPosition;
    public float mForce = 0.0f;
    public Vector3 directionVector = new Vector3(0.0f, 0.0f, 0.0f);
    //test
    public float test = 0.0f;
	// Use this for initialization
	void Start () 
    {
        ResetPosition();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetKeyUp(KeyCode.Q))
	    {
            SetAction(EdkDll.EE_CognitivAction_t.COG_LIFT, 0.5f);
	    }
        SetLimitDistance();
        UpdatePostion();
        //test
        test = Vector3.Distance(transform.position, OrgPosition.transform.position); 
	}

    public void ResetPosition()
    {
        transform.position = OrgPosition.transform.position;
    }

    public void SetAction(EdkDll.EE_CognitivAction_t actionType , float mForce)
    {
        this.mForce = 30* mForce; 
        if (actionType == EdkDll.EE_CognitivAction_t.COG_LIFT)
        {
            directionVector = Vector3.up;
        }
    }

    public void SetLimitDistance()
    {
        maxDistance = mForce * refDistance;
    }

    public void IsReachLimitDistance()
    {

    }

    public void UpdatePostion()
    {
        if (Vector3.Distance(transform.position, OrgPosition.transform.position) < maxDistance)
        {
            rigidbody.AddForce(directionVector * mForce);
        }
        else rigidbody.Sleep();
    }
}
