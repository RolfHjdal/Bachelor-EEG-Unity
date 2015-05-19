using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;


/// <summary>
/// Exception class for EmoEngine
/// </summary>
public class EmoEngineException : System.ApplicationException
{
	/// <summary>
	/// Constructor
	/// </summary>
	public EmoEngineException() : base() { }
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="message">Error message</param>
	public EmoEngineException(string message) : base(message) { }
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="message">Error message</param>
	/// <param name="inner"></param>
	public EmoEngineException(string message, System.Exception inner) : base(message, inner) { }
	/// <summary>
	/// Constructor needed for serialization when exception propagates from a remoting server to the client.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected EmoEngineException(System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

	private Int32 errorCode = 0;

	/// <summary>
	/// Error code defined in edk.h returned directly from the unmanaged APIs in edk.dll
	/// </summary> 
	public Int32 ErrorCode
	{
		get
		{
			return errorCode;
		}
		set
		{
			errorCode = value;
		}
	}
}

/// <summary>
/// Class to hold metadata of EmoEngine event 
/// </summary> 
public class EmoEngineEventArgs : EventArgs
{
	/// <summary>
	/// User ID
	/// </summary>
	public UInt32 userId;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="userId">User ID</param>
	public EmoEngineEventArgs(UInt32 userId)
	{
		this.userId= userId;
	}
}

/// <summary>
/// Class to hold metadata of EmoStateUpdated event
/// </summary> 
public class EmoStateUpdatedEventArgs : EmoEngineEventArgs
{
	/// <summary>
	/// EmoState
	/// </summary>
	public EmoState emoState;
	
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="emoState">EmoState</param>
	public EmoStateUpdatedEventArgs(UInt32 userId, EmoState emoState) : base(userId)
	{
		this.emoState = emoState;
	}
}

/// <summary>
/// Optimization parameter
/// </summary> 
public class OptimizationParam
{
	IntPtr hOptimizationParam;
	/// <summary>
	/// Constructor
	/// </summary>
	public OptimizationParam()
	{
		this.hOptimizationParam = EdkDll.EE_OptimizationParamCreate();
	}
	/// <summary>
	/// Destructor
	/// </summary>
	~OptimizationParam()
	{
		//if (this.hOptimizationParam != null)
		{
			EdkDll.EE_OptimizationParamFree(this.hOptimizationParam);
		}
	}
	/// <summary>
	/// Get a list of vital algorithms of specific suite from optimization parameter
	/// </summary>
	/// <param name="suite">suite that you are interested in</param>
	/// <returns>returns a list of vital algorithm composed of EE_ExpressivAlgo_t, EE_AffectivAlgo_t or EE_CognitivAction_t depending on the suite parameter</returns>
	public UInt32 GetVitalAlgorithm(EdkDll.EE_EmotivSuite_t suite)
	{
		UInt32 vitalAlgorithmBitVectorOut = 0;
		EmoEngine.errorHandler(EdkDll.EE_OptimizationGetVitalAlgorithm(hOptimizationParam, suite, out vitalAlgorithmBitVectorOut));
		return vitalAlgorithmBitVectorOut;
	}

	/// <summary>
	/// Set a list of vital algorithms of specific suite to optimization parameter
	/// </summary>
	/// <param name="suite">suite that you are interested in</param>
	/// <param name="vitalAlgorithmBitVector">a list of vital algorithm composed of EE_ExpressivAlgo_t, EE_AffectivAlgo_t or EE_CognitivAction_t depended on the suite parameter passed in</param>
	public void SetVitalAlgorithm(EdkDll.EE_EmotivSuite_t suite, UInt32 vitalAlgorithmBitVector)
	{
		EmoEngine.errorHandler(EdkDll.EE_OptimizationSetVitalAlgorithm(hOptimizationParam, suite, vitalAlgorithmBitVector));
	}

	/// <summary>
	/// Get internal handle of the optimization parameter
	/// </summary>
	/// <returns>Pointer which points to memory address representing the internal structure of optimization parameter</returns>
	public IntPtr GetHandle()
	{
		return hOptimizationParam;
	}
}

/// <summary>
/// User profile
/// </summary>
public class Profile
{
	private IntPtr hProfile;

	/// <summary>
	/// Constructor
	/// </summary>
	public Profile()
	{
		hProfile = EdkDll.EE_ProfileEventCreate();
		EdkDll.EE_GetBaseProfile(hProfile);        
	}
	/// <summary>
	/// Destructor
	/// </summary>
	~Profile()
	{
		//if (hProfile != null)
		{
			EdkDll.EE_EmoEngineEventFree(hProfile);
		}
	}

	/// <summary>
	/// Serializes the profile
	/// </summary>
	/// <returns>Serialized profile</returns>
	public Byte[] GetBytes()
	{
		UInt32 profileSizeOut = 0;
		EmoEngine.errorHandler(EdkDll.EE_GetUserProfileSize(hProfile, out profileSizeOut));
		Byte[] profileBytes = new Byte[profileSizeOut];
		EmoEngine.errorHandler(EdkDll.EE_GetUserProfileBytes(hProfile, profileBytes, profileSizeOut));
		return profileBytes;
	}

	/// <summary>
	/// Returns internal handle of the profile
	/// </summary>
	/// <returns>Pointer which points to memory address representing the internal structure of the profile</returns>
	public IntPtr GetHandle()
	{
		return hProfile;
	}
}

/// <summary>
/// Provide APIs to communicate with EmoEngine 
/// </summary>
public class EmoEngine
{
	private static EmoEngine instance;
	private Dictionary<UInt32, EmoState> lastEmoState = new Dictionary<UInt32, EmoState>();
	private IntPtr hEvent;

//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_START
	private IntPtr hData;
//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_END
	
	/// <summary>
	/// Function pointer of callback functions which will be called when EmoEngineConnectedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void EmoEngineConnectedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when EmoEngineDisconnectedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void EmoEngineDisconnectedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when UserAddedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void UserAddedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when UserRemovedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void UserRemovedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ProfileEventEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ProfileEventEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivTrainingStartedEventEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivTrainingStartedEventEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivTrainingSucceededEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivTrainingSucceededEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivTrainingFailedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivTrainingFailedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivTrainingCompletedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivTrainingCompletedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivTrainingDataErasedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivTrainingDataErasedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivTrainingRejectedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivTrainingRejectedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivTrainingResetEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivTrainingResetEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivAutoSamplingNeutralCompletedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivAutoSamplingNeutralCompletedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivSignatureUpdatedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivSignatureUpdatedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivTrainingStartedEventEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>

	public delegate void ExpressivTrainingStartedEventEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivTrainingSucceededEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ExpressivTrainingSucceededEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivTrainingFailedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ExpressivTrainingFailedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivTrainingCompletedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ExpressivTrainingCompletedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivTrainingDataErasedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ExpressivTrainingDataErasedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivTrainingRejectedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ExpressivTrainingRejectedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivTrainingResetEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ExpressivTrainingResetEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when InternalStateChangedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void InternalStateChangedEventHandler(object sender, EmoEngineEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when EmoStateUpdatedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void EmoStateUpdatedEventHandler(object sender, EmoStateUpdatedEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when EmoEngineEmoStateUpdatedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void EmoEngineEmoStateUpdatedEventHandler(object sender, EmoStateUpdatedEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when AffectivEmoStateUpdatedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void AffectivEmoStateUpdatedEventHandler(object sender, EmoStateUpdatedEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when ExpressivEmoStateUpdatedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void ExpressivEmoStateUpdatedEventHandler(object sender, EmoStateUpdatedEventArgs e);
	/// <summary>
	/// Function pointer of callback functions which will be called when CognitivEmoStateUpdatedEvent occurs
	/// </summary>
	/// <param name="sender">Object which triggers the event</param>
	/// <param name="e">Contains metadata of the event</param>
	public delegate void CognitivEmoStateUpdatedEventHandler(object sender, EmoStateUpdatedEventArgs e);
	
	/// <summary>
	/// Raise when EmoEngine is successfully connected
	/// </summary>
	public event EmoEngineConnectedEventHandler EmoEngineConnected;
	
	/// <summary>
	/// Raise when EmoEngine is disconnected
	/// </summary>
	public event EmoEngineDisconnectedEventHandler EmoEngineDisconnected;

	/// <summary>
	/// Raise when a new user is added or when the dongle is plugged in
	/// </summary>
	public event UserAddedEventHandler UserAdded;
	/// <summary>
	/// Raise when a user is removed or when the dongle is removed
	/// </summary>
	public event UserRemovedEventHandler UserRemoved;        

	/// <summary>
	/// Raise when cognitv training is stareted
	/// </summary>
	public event CognitivTrainingStartedEventEventHandler CognitivTrainingStarted;
	/// <summary>
	/// Raise when cognitiv training is completed and the training data is in good quality but the signature has not been updated yet.
	/// EmoEngine awaits the accept or reject training control signal after the event is raised.
	/// Once the control signal is received, EmoEngine will update signature for cognitiv correspondingly.
	/// </summary>
	public event CognitivTrainingSucceededEventHandler CognitivTrainingSucceeded;
	/// <summary>
	/// Raise when cognitiv training is completed but the signal during training is too noisy to be used for building cognitiv signature.
	/// </summary>
	public event CognitivTrainingFailedEventHandler CognitivTrainingFailed;
	/// <summary>
	/// Raise when the signature has successfully been updated after the accept training control is received.
	/// </summary>
	public event CognitivTrainingCompletedEventHandler CognitivTrainingCompleted;
	/// <summary>
	/// Raise when cognitiv training data is erased
	/// </summary>
	public event CognitivTrainingDataErasedEventHandler CognitivTrainingDataErased;
	/// <summary>
	/// Raise when the reject training control is received
	/// </summary>
	public event CognitivTrainingRejectedEventHandler CognitivTrainingRejected;
	/// <summary>
	/// Raise when the cognitiv algorithm is reset.
	/// </summary>
	public event CognitivTrainingResetEventHandler CognitivTrainingReset;
	/// <summary>
	/// Raise when auto sampling neutral is completed
	/// </summary>
	public event CognitivAutoSamplingNeutralCompletedEventHandler CognitivAutoSamplingNeutralCompleted;
	/// <summary>
	/// Raise when signature is updated after active actions are updated
	/// </summary>
	public event CognitivSignatureUpdatedEventHandler CognitivSignatureUpdated;
	/// <summary>
	/// Raise when expressiv training is started
	/// </summary>
	public event ExpressivTrainingStartedEventEventHandler ExpressivTrainingStarted;
	/// <summary>
	/// Raise when expressiv training is completed and the training data is in good quality but the signature has not been updated yet.
	/// EmoEngine awaits the accept or reject training control signal after the event is raised.
	/// Once the control signal is received, EmoEngine will update signature for expressiv correspondingly.
	/// </summary>
	public event ExpressivTrainingSucceededEventHandler ExpressivTrainingSucceeded;
	/// <summary>
	/// Raise when cognitiv training is completed but the signal during training is too noisy to be used for building cognitiv signature.
	/// </summary>
	public event ExpressivTrainingFailedEventHandler ExpressivTrainingFailed;
	/// <summary>
	/// Raise when the signature has successfully been updated after the accept training control is received.
	/// </summary>
	public event ExpressivTrainingCompletedEventHandler ExpressivTrainingCompleted;
	/// <summary>
	/// Raise when expressiv training data is erased
	/// </summary>
	public event ExpressivTrainingDataErasedEventHandler ExpressivTrainingDataErased;
	/// <summary>
	/// Raise when the reject training control is received
	/// </summary>
	public event ExpressivTrainingRejectedEventHandler ExpressivTrainingRejected;
	/// <summary>
	/// Raise when the expressiv algorithm is reset.
	/// </summary>
	public event ExpressivTrainingResetEventHandler ExpressivTrainingReset;
	/// <summary>
	/// Raise when EmoEngine is connected to Control Panel in proxy mode and 
	/// user has updated the internal state of EmoEngine with the Control Panel UI,
	/// such as changing the sensitivity of an expression or updating the optimization setting
	/// </summary>
	public event InternalStateChangedEventHandler InternalStateChanged;
	/// <summary>
	/// Raise when EmoState is updated
	/// </summary>
	public event EmoStateUpdatedEventHandler EmoStateUpdated;
	/// <summary>
	/// Raise when EmoEngine related EmoState is updated
	/// </summary>
	public event EmoEngineEmoStateUpdatedEventHandler EmoEngineEmoStateUpdated;
	/// <summary>
	/// Raise when affectiv related EmoState is updated
	/// </summary>
	public event AffectivEmoStateUpdatedEventHandler AffectivEmoStateUpdated;
	/// <summary>
	/// Raise when expressiv related EmoState is updated
	/// </summary>
	public event ExpressivEmoStateUpdatedEventHandler ExpressivEmoStateUpdated;
	/// <summary>
	/// Raise when cognitiv related EmoState is updated
	/// </summary>
	public event CognitivEmoStateUpdatedEventHandler CognitivEmoStateUpdated;

	private EmoEngine() 
	{ 

		hEvent = EdkDll.EE_EmoEngineEventCreate();
//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_START
		//hData = EdkDll.EE_DataCreate();
//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_END
	}

	/// <summary>
	/// Destructor of EmoEngine
	/// </summary>
	~EmoEngine()
	{
		if (hEvent != IntPtr.Zero) EdkDll.EE_EmoEngineEventFree(hEvent);
//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_START
		//if (hData != IntPtr.Zero) EdkDll.EE_DataFree(hData);
//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_END
	}

	/// <summary>
	/// Global instance of EmoEngine
	/// </summary>
	public static EmoEngine Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new EmoEngine();
			}
			return instance;
		}
	}
	/// <summary>
	/// Processes EmoEngine events until there is no more events
	/// </summary>
	public void ProcessEvents()
	{
		ProcessEvents(0);
	}
	/// <summary>
	/// Processes EmoEngine events until there is no more events or maximum processing time has elapsed
	/// </summary>
	/// <param name="maxTimeMilliseconds">maximum processing time in milliseconds</param>
	public void ProcessEvents(Int32 maxTimeMilliseconds)
	{
		Stopwatch st = new Stopwatch();

		st.Start();
		while (EdkDll.EE_EngineGetNextEvent(hEvent) == EdkDll.EDK_OK)
		{
			if (maxTimeMilliseconds != 0)
			{
				if (st.ElapsedMilliseconds >= maxTimeMilliseconds)
					break;
			}
			UInt32 userId = 0;
			EdkDll.EE_EmoEngineEventGetUserId(hEvent, out userId);
			EmoEngineEventArgs args = new EmoEngineEventArgs(userId);
			EdkDll.EE_Event_t eventType = EdkDll.EE_EmoEngineEventGetType(hEvent);
			switch (eventType)
			{
				case EdkDll.EE_Event_t.EE_UserAdded:
					OnUserAdded(args);
					break;
				case EdkDll.EE_Event_t.EE_UserRemoved:
					OnUserRemoved(args);
					break;
				case EdkDll.EE_Event_t.EE_EmoStateUpdated:
					EmoState curEmoState = new EmoState();
					errorHandler(EdkDll.EE_EmoEngineEventGetEmoState(hEvent, curEmoState.GetHandle()));
					EmoStateUpdatedEventArgs emoStateUpdatedEventArgs = new EmoStateUpdatedEventArgs(userId, curEmoState);
					OnEmoStateUpdated(emoStateUpdatedEventArgs);
					if (!curEmoState.EmoEngineEqual(lastEmoState[userId]))
					{
						emoStateUpdatedEventArgs = new EmoStateUpdatedEventArgs(userId, new EmoState(curEmoState));
						OnEmoEngineEmoStateUpdated(emoStateUpdatedEventArgs);  
					}
					if (!curEmoState.AffectivEqual(lastEmoState[userId]))
					{
						emoStateUpdatedEventArgs = new EmoStateUpdatedEventArgs(userId, new EmoState(curEmoState));
						OnAffectivEmoStateUpdated(emoStateUpdatedEventArgs);
					}
					if (!curEmoState.CognitivEqual(lastEmoState[userId]))
					{
						emoStateUpdatedEventArgs = new EmoStateUpdatedEventArgs(userId, new EmoState(curEmoState));
						OnCognitivEmoStateUpdated(emoStateUpdatedEventArgs);
					}
					if (!curEmoState.ExpressivEqual(lastEmoState[userId]))
					{
						emoStateUpdatedEventArgs = new EmoStateUpdatedEventArgs(userId, new EmoState(curEmoState));
						OnExpressivEmoStateUpdated(emoStateUpdatedEventArgs);
					}
					lastEmoState[userId] = (EmoState)curEmoState.Clone();
					break;     
				case EdkDll.EE_Event_t.EE_CognitivEvent: 
					EdkDll.EE_CognitivEvent_t cogType = EdkDll.EE_CognitivEventGetType(hEvent);
					switch(cogType){
						case EdkDll.EE_CognitivEvent_t.EE_CognitivTrainingStarted:
							OnCognitivTrainingStarted(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivTrainingSucceeded:
							OnCognitivTrainingSucceeded(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivTrainingFailed:
							OnCognitivTrainingFailed(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivTrainingCompleted:
							OnCognitivTrainingCompleted(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivTrainingDataErased:
							OnCognitivTrainingDataErased(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivTrainingRejected:
							OnCognitivTrainingRejected(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivTrainingReset:
							OnCognitivTrainingReset(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivAutoSamplingNeutralCompleted:
							OnCognitivAutoSamplingNeutralCompleted(args);
							break;
						case EdkDll.EE_CognitivEvent_t.EE_CognitivSignatureUpdated:
							OnCognitivSignatureUpdated(args);
							break;
						default:
							break;
					}
					break;
				case EdkDll.EE_Event_t.EE_ExpressivEvent:
					EdkDll.EE_ExpressivEvent_t expEvent = EdkDll.EE_ExpressivEventGetType(hEvent);
					switch (expEvent)
					{
						case EdkDll.EE_ExpressivEvent_t.EE_ExpressivTrainingStarted:
							OnExpressivTrainingStarted(args);
							break;
						case EdkDll.EE_ExpressivEvent_t.EE_ExpressivTrainingSucceeded:
							OnExpressivTrainingSucceeded(args);
							break;
						case EdkDll.EE_ExpressivEvent_t.EE_ExpressivTrainingFailed:
							OnExpressivTrainingFailed(args);
							break;
						case EdkDll.EE_ExpressivEvent_t.EE_ExpressivTrainingCompleted:
							OnExpressivTrainingCompleted(args);
							break;
						case EdkDll.EE_ExpressivEvent_t.EE_ExpressivTrainingDataErased:
							OnExpressivTrainingDataErased(args);
							break;
						case EdkDll.EE_ExpressivEvent_t.EE_ExpressivTrainingRejected:
							OnExpressivTrainingRejected(args);
							break;
						case EdkDll.EE_ExpressivEvent_t.EE_ExpressivTrainingReset:
							OnExpressivTrainingReset(args);
							break;                            
						default:
							break;
					}
					break;
				case EdkDll.EE_Event_t.EE_InternalStateChanged:
					OnInternalStateChanged(args);
					break;
				default:
					break;
			}
		}
	}
	
	/// <summary>
	/// Handler for EmoEngineConnected event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnEmoEngineConnected(EmoEngineEventArgs e)
	{
		lastEmoState.Clear();
		if (EmoEngineConnected != null)
			EmoEngineConnected(this, e);
	}

	/// <summary>
	/// Handler for EmoEngineDisconnected event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnEmoEngineDisconnected(EmoEngineEventArgs e)
	{            
		if (EmoEngineDisconnected != null)
			EmoEngineDisconnected(this, e);
	}

	/// <summary>
	/// Handler for UserAdded event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnUserAdded(EmoEngineEventArgs e)
	{
		lastEmoState.Add(e.userId, new EmoState());
		if (UserAdded != null)
			UserAdded(this, e);
	}

	/// <summary>
	/// Handler for UserRemoved event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnUserRemoved(EmoEngineEventArgs e)
	{
		lastEmoState.Remove(e.userId);
		if (UserRemoved != null)
			UserRemoved(this, e);
	}

	/// <summary>
	/// Handler for CognitivTrainingStarted event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivTrainingStarted(EmoEngineEventArgs e)
	{
		if (CognitivTrainingStarted != null)
			CognitivTrainingStarted(this, e);
	}

	/// <summary>
	/// Handler for CognitivTrainingSucceeded event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivTrainingSucceeded(EmoEngineEventArgs e)
	{
		if (CognitivTrainingSucceeded != null)
			CognitivTrainingSucceeded(this, e);
	}

	/// <summary>
	/// Handler for CognitivTrainingFailed event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivTrainingFailed(EmoEngineEventArgs e)
	{
		if (CognitivTrainingFailed != null)
			CognitivTrainingFailed(this, e);
	}

	/// <summary>
	/// Handler for CognitivTrainingCompleted event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivTrainingCompleted(EmoEngineEventArgs e)
	{
		if (CognitivTrainingCompleted != null)
			CognitivTrainingCompleted(this, e);
	}

	/// <summary>
	/// Handler for CognitivTrainingDataErased event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivTrainingDataErased(EmoEngineEventArgs e)
	{
		if (CognitivTrainingDataErased != null)
			CognitivTrainingDataErased(this, e);
	}

	/// <summary>
	/// Handler for CognitivTrainingRejected event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivTrainingRejected(EmoEngineEventArgs e)
	{
		if (CognitivTrainingRejected != null)
			CognitivTrainingRejected(this, e);
	}

	/// <summary>
	/// Handler for CognitivTrainingReset event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivTrainingReset(EmoEngineEventArgs e)
	{
		if (CognitivTrainingReset != null)
			CognitivTrainingReset(this, e);
	}

	/// <summary>
	/// Handler for CognitivAutoSamplingNeutralCompleted event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivAutoSamplingNeutralCompleted(EmoEngineEventArgs e)
	{
		if (CognitivAutoSamplingNeutralCompleted != null)
			CognitivAutoSamplingNeutralCompleted(this, e);
	}

	/// <summary>
	/// Handler for CognitivSignatureUpdated event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivSignatureUpdated(EmoEngineEventArgs e)
	{
		if (CognitivSignatureUpdated != null)
			CognitivSignatureUpdated(this, e);
	}

	/// <summary>
	/// Handler for ExpressivTrainingStarted event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivTrainingStarted(EmoEngineEventArgs e)
	{
		if (ExpressivTrainingStarted != null)
			ExpressivTrainingStarted(this, e);
	}

	/// <summary>
	/// Handler for ExpressivTrainingSucceeded event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivTrainingSucceeded(EmoEngineEventArgs e)
	{
		if (ExpressivTrainingSucceeded != null)
			ExpressivTrainingSucceeded(this, e);
	}

	/// <summary>
	/// Handler for ExpressivTrainingFailed event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivTrainingFailed(EmoEngineEventArgs e)
	{
		if (ExpressivTrainingFailed != null)
			ExpressivTrainingFailed(this, e);
	}

	/// <summary>
	/// Handler for ExpressivTrainingCompleted event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivTrainingCompleted(EmoEngineEventArgs e)
	{
		if (ExpressivTrainingCompleted != null)
			ExpressivTrainingCompleted(this, e);
	}

	/// <summary>
	/// Handler for ExpressivTrainingDataErased event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivTrainingDataErased(EmoEngineEventArgs e)
	{
		if (ExpressivTrainingDataErased != null)
			ExpressivTrainingDataErased(this, e);
	}

	/// <summary>
	/// Handler for ExpressivTrainingRejected event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivTrainingRejected(EmoEngineEventArgs e)
	{
		if (ExpressivTrainingRejected != null)
			ExpressivTrainingRejected(this, e);
	}

	/// <summary>
	/// Handler for ExpressivTrainingReset event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivTrainingReset(EmoEngineEventArgs e)
	{
		if (ExpressivTrainingReset != null)
			ExpressivTrainingReset(this, e);
	}

	/// <summary>
	/// Handler for InternalStateChanged event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnInternalStateChanged(EmoEngineEventArgs e)
	{
		if (InternalStateChanged != null)
			InternalStateChanged(this, e);
	}

	/// <summary>
	/// Handler for EmoStateUpdated event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnEmoStateUpdated(EmoStateUpdatedEventArgs e)
	{
		if (EmoStateUpdated != null)
			EmoStateUpdated(this, e);
	}

	/// <summary>
	/// Handler for EmoEngineEmoStateUpdated event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnEmoEngineEmoStateUpdated(EmoStateUpdatedEventArgs e)
	{
		if (EmoEngineEmoStateUpdated != null)
			EmoEngineEmoStateUpdated(this, e);
	}

	/// <summary>
	/// Handler for AffectivEmoStateUpated event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnAffectivEmoStateUpdated(EmoStateUpdatedEventArgs e)
	{
		if (AffectivEmoStateUpdated != null)
			AffectivEmoStateUpdated(this, e);
	}

	/// <summary>
	/// Handler for ExpressivEmoStateUpdated event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnExpressivEmoStateUpdated(EmoStateUpdatedEventArgs e)
	{
		if (ExpressivEmoStateUpdated != null)
			ExpressivEmoStateUpdated(this, e);
	}

	/// <summary>
	/// Handler for CognitivEmoStateUpdated event
	/// </summary>
	/// <param name="e">Contains metadata of the event, like userID</param>
	protected virtual void OnCognitivEmoStateUpdated(EmoStateUpdatedEventArgs e)
	{
		if (CognitivEmoStateUpdated != null)
			CognitivEmoStateUpdated(this, e);
	}

	/// <summary>
	/// Generates EmoEngineException
	/// </summary>
	/// <param name="errorCode">error code returned from APIs from edk.dll</param>
	public static void errorHandler(Int32 errorCode)
	{
		if (errorCode == EdkDll.EDK_OK)
			return;

		string errorStr = "";
		switch (errorCode)
		{
			case EdkDll.EDK_INVALID_PROFILE_ARCHIVE:
				errorStr = "Invalid profile archive";
				break;
			case EdkDll.EDK_NO_USER_FOR_BASEPROFILE:
				errorStr = "The base profile does not have a user ID";
				break;
			case EdkDll.EDK_CANNOT_ACQUIRE_DATA:
				errorStr = "EmoEngine is unable to acquire EEG input data";
				break;
			case EdkDll.EDK_BUFFER_TOO_SMALL:
				errorStr = "A supplied buffer is not large enough";
				break;
			case EdkDll.EDK_OUT_OF_RANGE:
				errorStr = "Parameter is out of range";
				break;
			case EdkDll.EDK_INVALID_PARAMETER:
				errorStr = "Parameter is invalid";
				break;
			case EdkDll.EDK_PARAMETER_LOCKED:
				errorStr = "Parameter is locked";
				break;
			case EdkDll.EDK_INVALID_USER_ID:
				errorStr = "User ID supplied to the function is invalid";
				break;
			case EdkDll.EDK_EMOENGINE_UNINITIALIZED:
				errorStr = "EmoEngine has not been initialized";
				break;
			case EdkDll.EDK_EMOENGINE_DISCONNECTED:
				errorStr = "Connection with remote instance of EmoEngine has been lost";
				break;
			case EdkDll.EDK_EMOENGINE_PROXY_ERROR:
				errorStr = "Unable to establish connection with remote instance of EmoEngine.";
				break;
			case EdkDll.EDK_NO_EVENT:
				errorStr = "There are no new EmoEngine events at this time.";
				break;
			case EdkDll.EDK_GYRO_NOT_CALIBRATED:
				errorStr = "The gyro could not be calibrated.  The headset must remain still for at least 0.5 secs.";
				break;
			case EdkDll.EDK_OPTIMIZATION_IS_ON:
				errorStr = "The requested operation failed due to optimization settings.";
				break;
			case EdkDll.EDK_UNKNOWN_ERROR:
				errorStr = "Unknown error";
				break;
			default:
				errorStr = "Unknown error";
				break;
		}

		EmoEngineException exception = new EmoEngineException(errorStr);
		exception.ErrorCode = errorCode;
		//throw exception;
	}

	/// <summary>
	/// Initializes the connection to EmoEngine. This function should be called at the beginning of programs that make use of EmoEngine, most probably in initialization routine or constructor.
	/// </summary>       
	public void Connect()
	{
        string version;
        UInt32 builtNum;
        SoftwareGetVersion(out version, out builtNum);
        if(version == "1.0.0.0")
            errorHandler(EdkDll.EE_EngineConnect(""));
        else
        errorHandler(EdkDll.EE_EngineConnect("Emotiv Systems-5"));
		OnEmoEngineConnected(new EmoEngineEventArgs(UInt32.MaxValue));
	}

	/// <summary>
	/// Initializes the connection to a remote instance of EmoEngine.
	/// </summary>
	/// <param name="ip">A string identifying the hostname or IP address of the remote EmoEngine server</param>
	/// <param name="port">The port number of the remote EmoEngine server. If connecting to the Emotiv Control Panel, use port 3008. If connecting to the EmoComposer, use port 1726</param>
	public void RemoteConnect(String ip, UInt16 port)
	{
		errorHandler(EdkDll.EE_EngineRemoteConnect(ip, port));
		OnEmoEngineConnected(new EmoEngineEventArgs(UInt32.MaxValue));
	}

	/// <summary>
	/// Terminates the connection to EmoEngine. This function should be called at the end of programs which make use of EmoEngine, most probably in clean up routine or destructor.
	/// </summary>
	public void Disconnect()
	{
		errorHandler(EdkDll.EE_EngineDisconnect());
		OnEmoEngineDisconnected(new EmoEngineEventArgs(UInt32.MaxValue));
	}

	/// <summary>
	/// Retrieves number of active users connected to the EmoEngine.
	/// </summary>
	/// <returns></returns>
	public UInt32 EngineGetNumUser()
	{
		UInt32 numUser = 0;
		errorHandler(EdkDll.EE_EngineGetNumUser(out numUser));
		return numUser;
	}

	/// <summary>
	/// Sets the player number displayed on the physical input device (currently the USB Dongle) that corresponds to the specified user
	/// </summary>
	/// <param name="userId">EmoEngine user ID</param>
	/// <param name="playerNum">application assigned player number displayed on input device hardware (must be in the range 1-4)</param>
	public void SetHardwarePlayerDisplay(UInt32 userId, UInt32 playerNum)
	{
		errorHandler(EdkDll.EE_SetHardwarePlayerDisplay(userId, playerNum));
	}

	/// <summary>
	/// Loads an EmoEngine profile for the specified user.
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="profile">user profile previously returned from EmoEngine.</param>
	public void SetUserProfile(UInt32 userId, Profile profile)
	{
		if (profile == null)
		{
			throw new NullReferenceException();
		}
		byte[] profileBytes = profile.GetBytes();
		errorHandler(EdkDll.EE_SetUserProfile(userId, profileBytes, (UInt32)profileBytes.Length));
	}

    public void SetUserProfile(UInt32 userId, Byte[] profileBytes)
    {
        if (profileBytes == null)
        {
            throw new NullReferenceException();
        }
        errorHandler(EdkDll.EE_SetUserProfile(userId, profileBytes, (UInt32)profileBytes.Length));
    }

	/// <summary>
	/// Fills in the event referred to by hEvent with an EE_ProfileEvent event that contains the profile data for the specified user.
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>user profile</returns>
	public Profile GetUserProfile(UInt32 userId)
	{
		Profile tempProfile = new Profile();
		errorHandler(EdkDll.EE_GetUserProfile(userId, tempProfile.GetHandle()));
		return tempProfile;
	}

	/// <summary>
	/// Loads a user profile from disk and assigns it to the specified user
	/// </summary>
	/// <param name="userID">a valid user ID</param>
	/// <param name="szInputFilename">platform-dependent filesystem path of saved user profile</param>
	public void LoadUserProfile(UInt32 userID, String szInputFilename)
	{
		errorHandler(EdkDll.EE_LoadUserProfile(userID, szInputFilename));
	}

	/// <summary>
	/// Saves a user profile for specified user to disk
	/// </summary>
	/// <param name="userID">a valid user ID</param>
	/// <param name="szOutputFilename">platform-dependent filesystem path for output file</param>
	public void EE_SaveUserProfile(UInt32 userID, String szOutputFilename)
	{
		errorHandler(EdkDll.EE_SaveUserProfile(userID, szOutputFilename));
	}

	/// <summary>
	/// Set threshold for Expressiv algorithms
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="algoName">Expressiv algorithm type</param>
	/// <param name="thresholdName">Expressiv threshold type</param>
	/// <param name="value">threshold value (min: 0 max: 1000)</param>
	public void ExpressivSetThreshold(UInt32 userId, EdkDll.EE_ExpressivAlgo_t algoName, EdkDll.EE_ExpressivThreshold_t thresholdName, Int32 value)
	{
		errorHandler(EdkDll.EE_ExpressivSetThreshold(userId, algoName, thresholdName, value));
	}

	/// <summary>
	/// Get threshold from Expressiv algorithms
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="algoName">Expressiv algorithm type</param>
	/// <param name="thresholdName">Expressiv threshold type</param>
	/// <returns>receives threshold value</returns>
	public Int32 ExpressivGetThreshold(UInt32 userId, EdkDll.EE_ExpressivAlgo_t algoName, EdkDll.EE_ExpressivThreshold_t thresholdName)
	{
		Int32 valueOut = 0;
		errorHandler(EdkDll.EE_ExpressivGetThreshold(userId, algoName, thresholdName, out valueOut));
		return valueOut;
	}

	/// <summary>
	/// Set the current facial expression for Expressiv training
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="action">which facial expression would like to be trained</param>
	public void ExpressivSetTrainingAction(UInt32 userId, EdkDll.EE_ExpressivAlgo_t action)
	{
		errorHandler(EdkDll.EE_ExpressivSetTrainingAction(userId, action));
	}

	/// <summary>
	/// Set the control flag for Expressiv training
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="control">pre-defined control command</param>
	public void ExpressivSetTrainingControl(UInt32 userId, EdkDll.EE_ExpressivTrainingControl_t control)
	{
		errorHandler(EdkDll.EE_ExpressivSetTrainingControl(userId, control));
	}

	/// <summary>
	/// Gets the facial expression currently selected for Expressiv training
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receives facial expression currently selected for training</returns>
	public EdkDll.EE_ExpressivAlgo_t ExpressivGetTrainingAction(UInt32 userId)
	{
		EdkDll.EE_ExpressivAlgo_t actionOut;
		errorHandler(EdkDll.EE_ExpressivGetTrainingAction(userId, out actionOut));
		return actionOut;
	}

	/// <summary>
	/// Return the duration of a Expressiv training session
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receive the training time in ms</returns>
	public UInt32 ExpressivGetTrainingTime(UInt32 userId)
	{
		UInt32 trainingTimeOut = 0;
		errorHandler(EdkDll.EE_ExpressivGetTrainingTime(userId, out trainingTimeOut));
		return trainingTimeOut;
	}

	/// <summary>
	/// Gets a list of the actions that have been trained by the user
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receives a bit vector composed of EE_ExpressivAlgo_t contants</returns>
	public UInt32 ExpressivGetTrainedSignatureActions(UInt32 userId)
	{
		UInt32 trainedActionsOut = 0;
		errorHandler(EdkDll.EE_ExpressivGetTrainedSignatureActions(userId, out trainedActionsOut));
		return trainedActionsOut;
	}

	/// <summary>
	/// Gets a flag indicating if the user has trained sufficient actions to activate a trained signature
	/// </summary>        
	/// <param name="userId">user ID</param>
	/// <returns>1 if the user has trained EXP_NEUTRAL and at least one other Expressiv action. Otherwise, 0 is returned.</returns>
	public Int32 ExpressivGetTrainedSignatureAvailable(UInt32 userId)
	{
		Int32 availableOut = 0;
		errorHandler(EdkDll.EE_ExpressivGetTrainedSignatureAvailable(userId, out availableOut));
		return availableOut;
	}

	/// <summary>
	/// Configures the Expressiv suite to use either the built-in, universal signature or a personal, trained signature
	/// </summary>
	/// <remarks>
	/// Expressiv defaults to use its universal signature.  This function will fail if EE_ExpressivGetTrainedSignatureAvailable returns false.
	/// </remarks>
	/// <param name="userId">user ID</param>
	/// <param name="sigType">signature type to use</param>
	public void ExpressivSetSignatureType(UInt32 userId, EdkDll.EE_ExpressivSignature_t sigType)
	{
		errorHandler(EdkDll.EE_ExpressivSetSignatureType(userId, sigType));
	}

	/// <summary>
	/// Indicates whether the Expressiv suite is currently using either the built-in, universal signature or a trained signature
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receives the signature type currently in use</returns>
	public EdkDll.EE_ExpressivSignature_t ExpressivGetSignatureType(UInt32 userId)
	{
		EdkDll.EE_ExpressivSignature_t sigTypeOut;
		errorHandler(EdkDll.EE_ExpressivGetSignatureType(userId, out sigTypeOut));
		return sigTypeOut;
	}

	/// <summary>
	/// Set the current Cognitiv active action types
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="activeActions">a bit vector composed of EE_CognitivAction_t contants</param>
	public void CognitivSetActiveActions(UInt32 userId, UInt32 activeActions)
	{
		errorHandler(EdkDll.EE_CognitivSetActiveActions(userId, activeActions));
	}

	/// <summary>
	/// Get the current Cognitiv active action types
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receive a bit vector composed of EE_CognitivAction_t contants</returns>
	public UInt32 CognitivGetActiveActions(UInt32 userId)
	{
		UInt32 activeActionsOut = 0;
		errorHandler(EdkDll.EE_CognitivGetActiveActions(userId, out activeActionsOut));
		return activeActionsOut;
	}

	/// <summary>
	/// Return the duration of a Cognitiv training session
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receive the training time in ms</returns>
	public UInt32 CognitivGetTrainingTime(UInt32 userId)
	{
		UInt32 trainingTimeOut = 0;
		errorHandler(EdkDll.EE_CognitivGetTrainingTime(userId, out trainingTimeOut));
		return trainingTimeOut;
	}

	/// <summary>
	/// Set the training control flag for Cognitiv training
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="control">pre-defined Cognitiv training control</param>
	public void CognitivSetTrainingControl(UInt32 userId, EdkDll.EE_CognitivTrainingControl_t control)
	{
		errorHandler(EdkDll.EE_CognitivSetTrainingControl(userId, control));
	}

	/// <summary>
	/// Set the type of Cognitiv action to be trained
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="action">which action would like to be trained</param>
	public void CognitivSetTrainingAction(UInt32 userId, EdkDll.EE_CognitivAction_t action)
	{
		errorHandler(EdkDll.EE_CognitivSetTrainingAction(userId, action));
	}

	/// <summary>
	/// Get the type of Cognitiv action currently selected for training
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>action that is currently selected for training</returns>
	public EdkDll.EE_CognitivAction_t CognitivGetTrainingAction(UInt32 userId)
	{
		EdkDll.EE_CognitivAction_t actionOut;
		errorHandler(EdkDll.EE_CognitivGetTrainingAction(userId, out actionOut));
		return actionOut;
	}

	/// <summary>
	/// Gets a list of the Cognitiv actions that have been trained by the user
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receives a bit vector composed of EE_CognitivAction_t contants</returns>
	public UInt32 CognitivGetTrainedSignatureActions(UInt32 userId)
	{
		UInt32 trainedActionsOut = 0;
		errorHandler(EdkDll.EE_CognitivGetTrainedSignatureActions(userId, out trainedActionsOut));
		return trainedActionsOut;
	}

	/// <summary>
	/// Gets the current overall skill rating of the user in Cognitiv
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>receives the overall skill rating [from 0.0 to 1.0]</returns>
	public Single CognitivGetOverallSkillRating(UInt32 userId)
	{
		Single overallSkillRatingOut = 0.0F;
		errorHandler(EdkDll.EE_CognitivGetOverallSkillRating(userId, out overallSkillRatingOut));
		return overallSkillRatingOut;
	}

	/// <summary>
	/// Gets the current skill rating for particular Cognitiv actions of the user
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="action">a particular action of EE_CognitivAction_t contant</param>
	/// <returns>receives the action skill rating [from 0.0 to 1.0]</returns>
	public Single CognitivGetActionSkillRating(UInt32 userId, EdkDll.EE_CognitivAction_t action)
	{
		Single actionSkillRatingOut = 0.0F;
		errorHandler(EdkDll.EE_CognitivGetActionSkillRating(userId, action, out actionSkillRatingOut));
		return actionSkillRatingOut;
	}

	/// <summary>
	/// Set the overall sensitivity for all Cognitiv actions
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="level">sensitivity level of all actions (lowest: 1, highest: 7)</param>
	public void CognitivSetActivationLevel(UInt32 userId, Int32 level)
	{
		errorHandler(EdkDll.EE_CognitivSetActivationLevel(userId, level));
	}

	/// <summary>
	/// Set the sensitivity of Cognitiv actions
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="action1Sensitivity">sensitivity of action 1 (min: 1, max: 10)</param>
	/// <param name="action2Sensitivity">sensitivity of action 2 (min: 1, max: 10)</param>
	/// <param name="action3Sensitivity">sensitivity of action 3 (min: 1, max: 10)</param>
	/// <param name="action4Sensitivity">sensitivity of action 4 (min: 1, max: 10)</param>
	public void CognitivSetActionSensitivity(UInt32 userId,
										Int32 action1Sensitivity, Int32 action2Sensitivity,
										Int32 action3Sensitivity, Int32 action4Sensitivity)
	{
		errorHandler(EdkDll.EE_CognitivSetActionSensitivity(userId, action1Sensitivity, action2Sensitivity, action3Sensitivity, action4Sensitivity));
	}

	/// <summary>
	/// Get the overall sensitivity for all Cognitiv actions
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>sensitivity level of all actions (min: 1, max: 10)</returns>
	public Int32 CognitivGetActivationLevel(UInt32 userId)
	{
		Int32 levelOut = 0;
		errorHandler(EdkDll.EE_CognitivGetActivationLevel(userId, out levelOut));
		return levelOut;
	}

	/// <summary>
	/// Query the sensitivity of Cognitiv actions
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="pAction1SensitivityOut">sensitivity of action 1</param>
	/// <param name="pAction2SensitivityOut">sensitivity of action 2</param>
	/// <param name="pAction3SensitivityOut">sensitivity of action 3</param>
	/// <param name="pAction4SensitivityOut">sensitivity of action 4</param>
	public void CognitivGetActionSensitivity(UInt32 userId,
										out Int32 pAction1SensitivityOut, out Int32 pAction2SensitivityOut,
										out Int32 pAction3SensitivityOut, out Int32 pAction4SensitivityOut)
	{
		errorHandler(EdkDll.EE_CognitivGetActionSensitivity(userId, out pAction1SensitivityOut, out pAction2SensitivityOut,
			out pAction3SensitivityOut, out pAction4SensitivityOut));
	}

	/// <summary>
	/// Start the sampling of Neutral state in Cognitiv
	/// </summary>
	/// <param name="userId">user ID</param>
	public void CognitivStartSamplingNeutral(UInt32 userId)
	{
		errorHandler(EdkDll.EE_CognitivStartSamplingNeutral(userId));
	}

	/// <summary>
	/// Stop the sampling of Neutral state in Cognitiv
	/// </summary>
	/// <param name="userId">user ID</param>
	public void CognitivStopSamplingNeutral(UInt32 userId)
	{
		errorHandler(EdkDll.EE_CognitivStopSamplingNeutral(userId));
	}

	/// <summary>
	/// Enable or disable signature caching in Cognitiv
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="enabled">flag to set status of caching (1: enable, 0: disable)</param>
	public void CognitivSetSignatureCaching(UInt32 userId, UInt32 enabled)
	{
		errorHandler(EdkDll.EE_CognitivSetSignatureCaching(userId, enabled));
	}

	/// <summary>
	/// Enable or disable signature caching in Cognitiv
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>flag to get status of caching (1: enable, 0: disable)</returns>
	public UInt32 CognitivGetSignatureCaching(UInt32 userId)
	{
		UInt32 enabledOut = 0;
		errorHandler(EdkDll.EE_CognitivGetSignatureCaching(userId, out enabledOut));
		return enabledOut;
	}

	/// <summary>
	/// Set the cache size for the signature caching in Cognitiv
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="size">number of signatures to be kept in the cache (0: unlimited)</param>
	public void CognitivSetSignatureCacheSize(UInt32 userId, UInt32 size)
	{
		errorHandler(EdkDll.EE_CognitivSetSignatureCacheSize(userId, size));
	}

	/// <summary>
	/// Get the current cache size for the signature caching in Cognitiv
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>number of signatures to be kept in the cache (0: unlimited)</returns>
	public UInt32 CognitivGetSignatureCacheSize(UInt32 userId)
	{
		UInt32 sizeOut = 0;
		errorHandler(EdkDll.EE_CognitivGetSignatureCacheSize(userId, out sizeOut));
		return sizeOut;
	}

	/// <summary>
	/// Returns a struct containing details about the specified EEG channel's headset 
	/// </summary>
	/// <param name="channelId">channel identifier</param>
	/// <returns>provides detailed sensor location and other info</returns>
	public EdkDll.InputSensorDescriptor_t HeadsetGetSensorDetails(EdkDll.EE_InputChannels_t channelId)
	{
		EdkDll.InputSensorDescriptor_t descriptorOut;
		errorHandler(EdkDll.EE_HeadsetGetSensorDetails(channelId, out descriptorOut));
		return descriptorOut;
	}

	/// <summary>
	/// Returns the current hardware version of the headset and dongle for a particular user
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>hardware version for the user headset/dongle pair. hiword is headset version, loword is dongle version.</returns>
	public UInt32 HardwareGetVersion(UInt32 userId)
	{
		UInt32 hwVersionOut;
		errorHandler(EdkDll.EE_HardwareGetVersion(userId, out hwVersionOut));
		return hwVersionOut;
	}

	/// <summary>
	/// Returns the current version of the Emotiv SDK software
	/// </summary>
	/// <param name="pszVersionOut">SDK software version in X.X.X.X format. Note: current beta releases have a major version of 0.</param>        
	/// <param name="pBuildNumOut">Build number.  Unique for each release.</param>
	public void SoftwareGetVersion(out String pszVersionOut, out UInt32 pBuildNumOut)
	{
		StringBuilder version = new StringBuilder(128);
		errorHandler(EdkDll.EE_SoftwareGetVersion(version,(UInt32) version.Capacity, out pBuildNumOut));
		pszVersionOut = version.ToString();
	}

	/// <summary>
	/// Returns the delta of the movement of the gyro since the previous call for a particular user
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="pXOut">horizontal displacement</param>
	/// <param name="pYOut">vertical displacment</param>
	public void HeadsetGetGyroDelta(UInt32 userId, out Int32 pXOut, out Int32 pYOut)
	{
		errorHandler(EdkDll.EE_HeadsetGetGyroDelta(userId, out pXOut, out pYOut));
	}

	/// <summary>
	/// Re-zero the gyro for a particular user
	/// </summary>
	/// <param name="userId">user ID</param>
	public void HeadsetGyroRezero(UInt32 userId)
	{
		errorHandler(EdkDll.EE_HeadsetGyroRezero(userId));
	}
   
	/// <summary>
	/// Enables optimization. EmoEngine will try to optimize its performance according to the information passed in with optimization parameter. EmoEngine guarantees the correctness of the results of vital algorithms. For algorithms that are not vital, results are undefined.
	/// </summary>
	/// <param name="param">OptimizationParam instance which includes information about how to optimize the performance of EmoEngine.</param>
	public void OptimizationEnable(OptimizationParam param)
	{
		if (param == null)
		{
			throw new NullReferenceException();
		}
		errorHandler(EdkDll.EE_OptimizationEnable(param.GetHandle()));
	}

	/// <summary>
	/// Determines whether optimization is on
	/// </summary>
	/// <returns>
	/// receives information about whether optimization is on
	/// </returns>
	public Boolean OptimizationIsEnabled()
	{
		Boolean enabledOut = false;
		errorHandler(EdkDll.EE_OptimizationIsEnabled(out enabledOut));
		return enabledOut;
	}

	/// <summary>
	/// Disables optimization
	/// </summary>
	public void OptimizationDisable()
	{
		errorHandler(EdkDll.EE_OptimizationDisable());
	}

	/// <summary>
	/// Gets optimization parameter.  If optimization is not enabled (this can be checked with EE_OptimmizationIsEnabled) then the results attached to the returned parameter are undefined.
	/// </summary>
	/// <returns></returns>
	public OptimizationParam OptimizationGetParam()
	{
		OptimizationParam param = new OptimizationParam();
		errorHandler(EdkDll.EE_OptimizationGetParam(param.GetHandle()));
		return param;
	}

	/// <summary>
	/// Resets all settings and user-specific profile data for the specified detection suite
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="suite">detection suite (Expressiv, Affectiv, or Cognitiv)</param>
	/// <param name="detectionBitVector">identifies specific detections.  Set to zero for all detections.</param>
	public void ResetDetection(UInt32 userId, EdkDll.EE_EmotivSuite_t suite, UInt32 detectionBitVector)
	{
		errorHandler(EdkDll.EE_ResetDetection(userId, suite, detectionBitVector));
	}

//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_START
	/// <summary>
	/// Sets the size of the data buffer. The size of the buffer affects how frequent GetData() needs to be called to prevent data loss.
	/// </summary>
	/// <param name="bufferSizeInSec">buffer size in second</param>
	//public void EE_DataSetBufferSizeInSec(Single bufferSizeInSec)
	//{
		//errorHandler(EdkDll.EE_DataSetBufferSizeInSec(bufferSizeInSec));
	//}

	/// <summary>
	/// Returns the size of the data buffer
	/// </summary>        
	/// <returns>
	/// the size of the data buffer
	/// </returns>
	//public Single EE_DataGetBufferSizeInSec()
	//{
		//Single bufferSizeInSecOut = 0;
		//errorHandler(EdkDll.EE_DataGetBufferSizeInSec(out bufferSizeInSecOut));
		//return bufferSizeInSecOut;
	//}

	/// <summary>
	/// Controls acquisition of data from EmoEngine (which is off by default).
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <param name="enable">If true, then enables data acquisition. If false, then disables data acquisition.</param>
	//public void DataAcquisitionEnable(UInt32 userId, bool enable)
	//{
		//errorHandler(EdkDll.EE_DataAcquisitionEnable(userId, enable));
	//}

	/// <summary>
	/// Returns whether data acquisition is enabled
	/// </summary>
	/// <param name="userId">user ID</param>     
	/// <returns>
	/// receives whether data acquisition is enabled
	/// </returns>
	//public Boolean IsDataAcquisitionEnabled(UInt32 userId)
	//{
		//Boolean result = false;
//
		//errorHandler(EdkDll.EE_DataAcquisitionIsEnabled(userId, out result));
//
		//return result;
	//}

	/// <summary>
	/// Returns latest data since the last call
	/// </summary>
	/// <param name="userId">user ID</param>
	/// <returns>
	/// receives latest data since the last call
	/// </returns>
	//public Dictionary<EdkDll.EE_DataChannel_t, double[]> GetData(UInt32 userId)
	//{
		//Dictionary<EdkDll.EE_DataChannel_t, double[]> result = new Dictionary<EdkDll.EE_DataChannel_t, double[]>();
//
		//errorHandler(EdkDll.EE_DataUpdateHandle(userId, hData));
//
		//UInt32 nSample = 10;
		//errorHandler(EdkDll.EE_DataGetNumberOfSample(hData, out nSample));
//
		//if (nSample == 0)
		//{
			//return null;
		//}
//
		//foreach (EdkDll.EE_DataChannel_t channel in Enum.GetValues(typeof(EdkDll.EE_DataChannel_t)))
		//{
			//result.Add(channel, new double[nSample]);
			//errorHandler(EdkDll.EE_DataGet(hData, channel, result[channel], nSample));
		//}
//
		//return result;
	//}
//
	/// <summary>
	/// Sets marker
	/// </summary>
	/// <param name="userId">user ID</param>            
	/// <param name="marker">value of the marker</param>     
	//public void DataSetMarker(UInt32 userId, Int32 marker)
	//{
		//errorHandler(EdkDll.EE_DataSetMarker(userId, marker));
	//}
//
	/// <summary>
	/// Sets sychronization signal
	/// </summary>
	/// <param name="userId">user ID</param>            
	/// <param name="signal">value of the sychronization signal</param>     
	//public void DataSetSychronizationSignal(UInt32 userId, Int32 signal)
	//{
		//errorHandler(EdkDll.EE_DataSetSychronizationSignal(userId, signal));
	//}
//
	/// <summary>
	/// Gets sampling rate
	/// </summary>
	/// <param name="userId">user ID</param>            
	//public UInt32 DataGetSamplingRate(UInt32 userId)
	//{
		//UInt32 samplingRate = 0;
		//errorHandler(EdkDll.EE_DataGetSamplingRate(userId, out samplingRate));
		//return samplingRate;
	//}
	//
	//TienTest-----------------------
	//public void CognitivSetCurrentLevel(Int16  userId,
									     //EdkDll.EE_CognitivLevel_t  level,  
									     //EdkDll.EE_CognitivAction_t  level1Action,  
										 //EdkDll.EE_CognitivAction_t  level2Action,  
										 //EdkDll.EE_CognitivAction_t  level3Action,  
										 //EdkDll.EE_CognitivAction_t  level4Action)
	//{
		//errorHandler(EdkDll.EE_CognitivSetCurrentLevel(userId,level,level1Action,level2Action,level3Action,level4Action));
	//}
//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_END
}

