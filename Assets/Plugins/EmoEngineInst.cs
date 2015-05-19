using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class EmoEngineInst : MonoBehaviour 
{
    //----------------------------------------
    EmoEngine engine = EmoEngine.Instance;
    //ConsoleKeyInfo cki = new ConsoleKeyInfo();
    //----------------------------------------
    public static int[] Cq;
    public static int nChan;
    public static bool IsStarted = false;
    public static int numOfGoodContacts = 0;
    //----------------------------------------
   
    void Start()
    {
        if (!IsStarted)
        {
            Cq = new int[18];
            engine.EmoEngineConnected +=
                 new EmoEngine.EmoEngineConnectedEventHandler(engine_EmoEngineConnected);
            engine.EmoEngineDisconnected +=
                new EmoEngine.EmoEngineDisconnectedEventHandler(engine_EmoEngineDisconnected);
            engine.EmoEngineEmoStateUpdated +=
                new EmoEngine.EmoEngineEmoStateUpdatedEventHandler(engine_EmoEngineEmoStateUpdated);
            
			//engine.Connect(); 							//real headset
			//engine.RemoteConnect("127.0.0.1",1726);	//emocomposer
			engine.RemoteConnect("127.0.0.1",3008);	//control panel
            IsStarted = true;
        }  
        
    }
    void Stop()
    {
        engine.Disconnect();
    }
	static void keyHandler(ConsoleKey key)
	{}
	
    void Update()
    {
        try
        {
			/*
            if (Console.KeyAvailable)
            {
                cki = Console.ReadKey(true);
                keyHandler(cki.Key);
            }
			*/
            engine.ProcessEvents(1000);
        }
        catch (EmoEngineException e)
        {
            Console.WriteLine("{0}", e.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine("{0}", e.ToString());
        }
    }
	
    static void engine_EmoEngineEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState;
        Int32 numCqChan = es.GetNumContactQualityChannels();
        EdkDll.EE_EEG_ContactQuality_t[] cq = es.GetContactQualityFromAllChannels();
        nChan = numCqChan;
        int temp = 0;
        for (Int32 i = 0; i < numCqChan; ++i)
        {
            if (cq[i] != es.GetContactQuality(i))
            {
                throw new Exception();
            }
           
            switch (cq[i])
            {
                case EdkDll.EE_EEG_ContactQuality_t.EEG_CQ_NO_SIGNAL:
                    Cq[i] = 0;
                    break;
                case EdkDll.EE_EEG_ContactQuality_t.EEG_CQ_VERY_BAD:
                    Cq[i] = 1;
                    break;
                case EdkDll.EE_EEG_ContactQuality_t.EEG_CQ_POOR:
                    Cq[i] = 2;
                    break;
                case EdkDll.EE_EEG_ContactQuality_t.EEG_CQ_FAIR:
                    Cq[i] = 3;
                    break;
                case EdkDll.EE_EEG_ContactQuality_t.EEG_CQ_GOOD:
                    temp++;
                    Cq[i] = 4;
                    break;
            }

            //---------------------
        }
        numOfGoodContacts = temp;
        //EdkDll.EE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();
        Int32 chargeLevel = 0;
        Int32 maxChargeLevel = 0;
        es.GetBatteryChargeLevel(out chargeLevel, out maxChargeLevel);

        EdkDll.EE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();
        if (signalStrength == EdkDll.EE_SignalStrength_t.NO_SIGNAL)
        {
            for (Int32 i = 0; i < numCqChan; ++i)
            {
                Cq[i] = 0;
            }
        }

    }
    static void engine_EmoEngineConnected(object sender, EmoEngineEventArgs e)
    {
        
    }
    static void engine_EmoEngineDisconnected(object sender, EmoEngineEventArgs e)
    {
        
    }
}
