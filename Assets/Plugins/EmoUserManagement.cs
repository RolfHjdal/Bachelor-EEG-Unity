using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class EmoUserManagement : MonoBehaviour
{
    //----------------------------------------
    EmoEngine engine = EmoEngine.Instance;
    public static int numUser = 0;
    public static int currentUser = 0;
    public int NumberOfUser = 0;
    public int IndexOfCurrentUser = 0;
    public static bool IsStarted = false;
    //----------------------------------------
    void Start()
    {
        if (!IsStarted)
        {
            engine.UserAdded +=
            new EmoEngine.UserAddedEventHandler(engine_UserAdded);
            engine.UserRemoved +=
            new EmoEngine.UserRemovedEventHandler(engine_UserRemoved);
            IsStarted = true;
        }        
    }
    
    void Update()
    {
        NumberOfUser = numUser;
        IndexOfCurrentUser = currentUser;
    }
    static void engine_UserAdded(object sender, EmoEngineEventArgs e)
    {
        numUser ++;
        currentUser = numUser -1;        
    }

    static void engine_UserRemoved(object sender, EmoEngineEventArgs e)
    {
        numUser --;
        currentUser = numUser -1;
    }
    
}