using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


public enum Status
{
    Center,
    Left,
    Right,
    Up,
    Down,
    Deny
};

public class EmoGyroData : MonoBehaviour
{


   

    public static Status headPosition = Status.Center;
    public Status oldPosition = Status.Center;
    //----------------------------------------
    EmoEngine engine = EmoEngine.Instance;
    public static int GyroX = 0;
    public static int GyroY = 0;
    public int GTempX = 0;
    public int GTempY = 0;
    
    public float delayUpdate = 0.1f;
    public float timeNoMove = 0;

    public bool isMoveBack = false;

    public int oldPositionX = 0;
    public int oldPositionY = 0;
    public float timeMoveBack = 0.0f;

    public float timeCheckStatus = 0;
    float dis;

    //----------------------------------------
    
   
    void Update()
    {
        delayUpdate -= Time.deltaTime;
        timeCheckStatus += Time.deltaTime;
        
        if(delayUpdate <= 0)
        {
            delayUpdate = 0.1f;
            engine.HeadsetGetGyroDelta(0, out GTempX, out GTempY);
            //Debug.Log("x:" + GyroX + "y:" + GyroY);
            if ((Math.Abs(GTempX) < 15) && (Math.Abs(GTempY) < 15))
            {
                timeNoMove += Time.deltaTime;
                if(timeNoMove > 0.9)
                {
                    oldPositionX = GyroX;
                    oldPositionY = GyroY;
                    timeMoveBack = 0.0f;
                    isMoveBack = true;
                }
            }
            else
            {
                isMoveBack = false;
                float tmp1 = (GyroX + GTempX / 5);
                float tmp2 = (GyroY - GTempY / 4);
                dis = Mathf.Sqrt(tmp1 * tmp1 + tmp2 * tmp2);

                if (dis < 175)
                {
                    GyroX = (int)tmp1;
                    GyroY = (int)tmp2;
                }
                else
                {
                    GyroX = (int)(180 * GyroX / dis);
                    GyroY = (int)(180 * GyroY / dis);
                    
                }

                
            }
            dis = Mathf.Sqrt(GyroX * GyroX + GyroY * GyroY);

            if (dis < 90)
            {
                headPosition = Status.Center;

            }
            else
            {
                if ((GyroX > 100) && (headPosition != Status.Deny))
                {
                    headPosition = Status.Right;
                }
                else if ((GyroX < -100) && (headPosition != Status.Deny))
                {
                    headPosition = Status.Left;
                }

                if (GyroY > 100)
                {
                    headPosition = Status.Down;
                }
                else if (GyroY < -100)
                {
                    headPosition = Status.Up;
                }
            }

            if(headPosition != oldPosition)
            {
                
                if((oldPosition == Status.Left)&&(headPosition==Status.Right)&&timeCheckStatus < 1.5)
                {
                    headPosition = Status.Deny;

                }


                if ((oldPosition == Status.Right) && (headPosition == Status.Left) && timeCheckStatus < 1.5)
                {
                    headPosition = Status.Deny;

                }
                if (headPosition != Status.Center)
                {
                    oldPosition = headPosition;
                    timeCheckStatus = 0.0f;
                }
                
            }




        }

        if(isMoveBack)
        {
            timeMoveBack += Time.deltaTime*1.5f;
            Vector3 tmpVector = Vector3.Slerp(new Vector3(oldPositionX, oldPositionY, 0), Vector3.zero, timeMoveBack);
            GyroX = (int)tmpVector.x;
            GyroY = (int)tmpVector.y;
        }

        
        
        
    }

    

   
}