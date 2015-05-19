//using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

public class EdkDll
{
	public const Int32 EDK_OK                           = 0x0000;
	public const Int32 EDK_UNKNOWN_ERROR                = 0x0001;

	public const Int32 EDK_INVALID_PROFILE_ARCHIVE      = 0x0101;
	public const Int32 EDK_NO_USER_FOR_BASEPROFILE      = 0x0102;

	public const Int32 EDK_CANNOT_ACQUIRE_DATA          = 0x0200;

	public const Int32 EDK_BUFFER_TOO_SMALL             = 0x0300;
	public const Int32 EDK_OUT_OF_RANGE                 = 0x0301;
	public const Int32 EDK_INVALID_PARAMETER            = 0x0302;
	public const Int32 EDK_PARAMETER_LOCKED             = 0x0303;
	public const Int32 EDK_COG_INVALID_TRAINING_ACTION  = 0x0304;
	public const Int32 EDK_COG_INVALID_TRAINING_CONTROL = 0x0305;
	public const Int32 EDK_COG_INVALID_ACTIVE_ACTION    = 0x0306;
	public const Int32 EDK_COG_EXCESS_MAX_ACTIONS       = 0x0307;
	public const Int32 EDK_EXP_NO_SIG_AVAILABLE         = 0x0308;
	public const Int32 EDK_FILESYSTEM_ERROR             = 0x0309;

	public const Int32 EDK_INVALID_USER_ID              = 0x0400;

	public const Int32 EDK_EMOENGINE_UNINITIALIZED      = 0x0500;
	public const Int32 EDK_EMOENGINE_DISCONNECTED       = 0x0501;
	public const Int32 EDK_EMOENGINE_PROXY_ERROR        = 0x0502;

	public const Int32 EDK_NO_EVENT                     = 0x0600;

	public const Int32 EDK_GYRO_NOT_CALIBRATED          = 0x0700;

	public const Int32 EDK_OPTIMIZATION_IS_ON           = 0x0800;

	public const Int32 EDK_RESERVED1                    = 0x0900;

	public enum EE_ExpressivThreshold_t
	{
		EXP_SENSITIVITY
	} ;

	public enum EE_ExpressivTrainingControl_t
	{
		EXP_NONE = 0, EXP_START, EXP_ACCEPT, EXP_REJECT, EXP_ERASE, EXP_RESET
	} ;

	public enum EE_ExpressivSignature_t
	{
		EXP_SIG_UNIVERSAL = 0, EXP_SIG_TRAINED
	} ;

	public enum EE_CognitivTrainingControl_t
	{
		COG_NONE = 0, COG_START, COG_ACCEPT, COG_REJECT, COG_ERASE, COG_RESET
	} ;

	public enum EE_Event_t
	{
		EE_UnknownEvent          = 0x0000,
		EE_EmulatorError         = 0x0001,
		EE_ReservedEvent         = 0x0002,
		EE_UserAdded             = 0x0010,
		EE_UserRemoved           = 0x0020,
		EE_EmoStateUpdated       = 0x0040,
		EE_ProfileEvent          = 0x0080,
		EE_CognitivEvent         = 0x0100,
		EE_ExpressivEvent        = 0x0200,
		EE_InternalStateChanged  = 0x0400,
		EE_AllEvent              = EE_UserAdded | EE_UserRemoved | EE_EmoStateUpdated | EE_ProfileEvent |
								   EE_CognitivEvent | EE_ExpressivEvent | EE_InternalStateChanged
	} ;

	public enum EE_ExpressivEvent_t
	{
		EE_ExpressivNoEvent = 0,
		EE_ExpressivTrainingStarted,
		EE_ExpressivTrainingSucceeded,
		EE_ExpressivTrainingFailed,
		EE_ExpressivTrainingCompleted,
		EE_ExpressivTrainingDataErased,
		EE_ExpressivTrainingRejected,
		EE_ExpressivTrainingReset
	} ;

	public enum EE_CognitivEvent_t
	{
		EE_CognitivNoEvent = 0,
		EE_CognitivTrainingStarted,
		EE_CognitivTrainingSucceeded,
		EE_CognitivTrainingFailed,
		EE_CognitivTrainingCompleted,
		EE_CognitivTrainingDataErased,
		EE_CognitivTrainingRejected,
		EE_CognitivTrainingReset,
		EE_CognitivAutoSamplingNeutralCompleted,
		EE_CognitivSignatureUpdated
	} ;

	public enum EE_DataChannel_t
	{
		COUNTER = 0, INTERPOLATED, RAW_CQ,
		AF3, F7, F3, FC5, T7,
		P7, O1, O2, P8, T8,
		FC6, F4, F8, AF4, GYROX,
		GYROY, 
		TIMESTAMP, ES_TIMESTAMP, 
		FUNC_ID, FUNC_VALUE, MARKER,
		SYNC_SIGNAL
	} ;

	[StructLayout(LayoutKind.Sequential)]
	public class InputSensorDescriptor_t
	{
		public EE_InputChannels_t channelId; // logical channel id
		public Int32 fExists;                // does this sensor exist on this headset model
		public String pszLabel;              // text label identifying this sensor
		public Double xLoc;                  // x coordinate from center of head towards nose
		public Double yLoc;                  // y coordinate from center of head towards ears
		public Double zLoc;                  // z coordinate from center of head toward top of skull
	}

	public enum EE_EmotivSuite_t
	{
		EE_EXPRESSIV = 0, EE_AFFECTIV, EE_COGNITIV
	} ;

	public enum EE_ExpressivAlgo_t
	{
		EXP_NEUTRAL     = 0x0001,
		EXP_BLINK       = 0x0002,
		EXP_WINK_LEFT   = 0x0004,
		EXP_WINK_RIGHT  = 0x0008,
		EXP_HORIEYE     = 0x0010,
		EXP_EYEBROW     = 0x0020,
		EXP_FURROW      = 0x0040,
		EXP_SMILE       = 0x0080,
		EXP_CLENCH      = 0x0100,
		EXP_LAUGH       = 0x0200,
		EXP_SMIRK_LEFT  = 0x0400,
		EXP_SMIRK_RIGHT = 0x0800
	} ;

	public enum EE_AffectivAlgo_t
	{
		AFF_EXCITEMENT         = 0x0001,
		AFF_MEDITATION         = 0x0002,
		AFF_FRUSTRATION        = 0x0004,
		AFF_ENGAGEMENT_BOREDOM = 0x0008
	} ;

	public enum EE_CognitivAction_t
	{
		COG_NEUTRAL                  = 0x0001,
		COG_PUSH                     = 0x0002,
		COG_PULL                     = 0x0004,
		COG_LIFT                     = 0x0008,
		COG_DROP                     = 0x0010,
		COG_LEFT                     = 0x0020,
		COG_RIGHT                    = 0x0040,
		COG_ROTATE_LEFT              = 0x0080,
		COG_ROTATE_RIGHT             = 0x0100,
		COG_ROTATE_CLOCKWISE         = 0x0200,
		COG_ROTATE_COUNTER_CLOCKWISE = 0x0400,
		COG_ROTATE_FORWARDS          = 0x0800,
		COG_ROTATE_REVERSE           = 0x1000,
		COG_DISAPPEAR                = 0x2000

       
	} ;
	//TienAdd4test
	public enum EE_CognitivLevel_t
	{
		COG_LEVEL1 = 1, COG_LEVEL2, COG_LEVEL3, COG_LEVEL4

	} ;
	public enum EE_SignalStrength_t
	{
		NO_SIGNAL = 0, BAD_SIGNAL, GOOD_SIGNAL
	} ;
	public enum EE_InputChannels_t
	{
		EE_CHAN_CMS = 0, EE_CHAN_DRL, EE_CHAN_FP1, EE_CHAN_AF3, EE_CHAN_F7,
		EE_CHAN_F3, EE_CHAN_FC5, EE_CHAN_T7, EE_CHAN_P7, EE_CHAN_O1,
		EE_CHAN_O2, EE_CHAN_P8, EE_CHAN_T8, EE_CHAN_FC6, EE_CHAN_F4,
		EE_CHAN_F8, EE_CHAN_AF4, EE_CHAN_FP2
	} ;
	public enum EE_EEG_ContactQuality_t
	{
		EEG_CQ_NO_SIGNAL, EEG_CQ_VERY_BAD, EEG_CQ_POOR,
		EEG_CQ_FAIR, EEG_CQ_GOOD
	} ;

	[DllImport("edk.dll", EntryPoint = "EE_EngineConnect")]
    static extern Int32 Unmanged_EE_EngineConnect(String security);
	[DllImport("edk.dll", EntryPoint = "EE_EngineRemoteConnect")]
	static extern Int32 Unmanged_EE_EngineRemoteConnect(String szHost, UInt16 port);
	[DllImport("edk.dll", EntryPoint = "EE_EngineDisconnect")]
	static extern Int32 Unmanged_EE_EngineDisconnect();
	[DllImport("edk.dll", EntryPoint = "EE_EnableDiagnostics")]
	static extern Int32 Unmanged_EE_EnableDiagnostics(String szFilename, Int32 fEnable, Int32 nReserved);
	[DllImport("edk.dll", EntryPoint = "EE_EmoEngineEventCreate")]
	static extern IntPtr Unmanged_EE_EmoEngineEventCreate();
	[DllImport("edk.dll", EntryPoint = "EE_ProfileEventCreate")]
	static extern IntPtr Unmanged_EE_ProfileEventCreate();
	[DllImport("edk.dll", EntryPoint = "EE_EmoEngineEventFree")]
	static extern void Unmanged_EE_EmoEngineEventFree(IntPtr hEvent);
	[DllImport("edk.dll", EntryPoint = "EE_EmoStateCreate")]
	static extern IntPtr Unmanged_EE_EmoStateCreate();
	[DllImport("edk.dll", EntryPoint = "EE_EmoStateFree")]
	static extern void Unmanged_EE_EmoStateFree(IntPtr hState);
	[DllImport("edk.dll", EntryPoint = "EE_EmoEngineEventGetType")]
	static extern EE_Event_t Unmanged_EE_EmoEngineEventGetType(IntPtr hEvent);
	[DllImport("edk.dll", EntryPoint = "EE_CognitivEventGetType")]
	static extern EE_CognitivEvent_t Unmanged_EE_CognitivEventGetType(IntPtr hEvent);
	[DllImport("edk.dll", EntryPoint = "EE_ExpressivEventGetType")]
	static extern EE_ExpressivEvent_t Unmanged_EE_ExpressivEventGetType(IntPtr hEvent);

	[DllImport("edk.dll", EntryPoint = "EE_EmoEngineEventGetUserId")]
	static extern Int32 Unmanged_EE_EmoEngineEventGetUserId(IntPtr hEvent, out UInt32 pUserIdOut);

	[DllImport("edk.dll", EntryPoint = "EE_EmoEngineEventGetEmoState")]
	static extern Int32 Unmanged_EE_EmoEngineEventGetEmoState(IntPtr hEvent, IntPtr hEmoState);

	[DllImport("edk.dll", EntryPoint = "EE_EngineGetNextEvent")]
	static extern Int32 Unmanged_EE_EngineGetNextEvent(IntPtr hEvent);

	[DllImport("edk.dll", EntryPoint = "EE_EngineClearEventQueue")]
	static extern Int32 Unmanged_EE_EngineClearEventQueue(Int32 eventTypes);

	[DllImport("edk.dll", EntryPoint = "EE_EngineGetNumUser")]
	static extern Int32 Unmanged_EE_EngineGetNumUser(out UInt32 pNumUserOut);

	[DllImport("edk.dll", EntryPoint = "EE_SetHardwarePlayerDisplay")]
	static extern Int32 Unmanged_EE_SetHardwarePlayerDisplay(UInt32 userId, UInt32 playerNum);

	[DllImport("edk.dll", EntryPoint = "EE_SetUserProfile")]
	static extern Int32 Unmanged_EE_SetUserProfile(UInt32 userId, Byte[] profileBuffer, UInt32 length);

	[DllImport("edk.dll", EntryPoint = "EE_GetUserProfile")]
	static extern Int32 Unmanged_EE_GetUserProfile(UInt32 userId, IntPtr hEvent);

	[DllImport("edk.dll", EntryPoint = "EE_GetBaseProfile")]
	static extern Int32 Unmanged_EE_GetBaseProfile(IntPtr hEvent);

	[DllImport("edk.dll", EntryPoint = "EE_GetUserProfileSize")]
	static extern Int32 Unmanged_EE_GetUserProfileSize(IntPtr hEvt, out UInt32 pProfileSizeOut);


	[DllImport("edk.dll", EntryPoint = "EE_GetUserProfileBytes")]
	static extern Int32 Unmanged_EE_GetUserProfileBytes(IntPtr hEvt, Byte[] destBuffer, UInt32 length);

	[DllImport("edk.dll", EntryPoint = "EE_LoadUserProfile")]
	static extern Int32 Unmanged_EE_LoadUserProfile(UInt32 userID, String szInputFilename);

	[DllImport("edk.dll", EntryPoint = "EE_SaveUserProfile")]
	static extern Int32 Unmanged_EE_SaveUserProfile(UInt32 userID, String szOutputFilename);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivSetThreshold")]
	static extern Int32 Unmanged_EE_ExpressivSetThreshold(UInt32 userId, EE_ExpressivAlgo_t algoName, EE_ExpressivThreshold_t thresholdName, Int32 value);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivGetThreshold")]
	static extern Int32 Unmanged_EE_ExpressivGetThreshold(UInt32 userId, EE_ExpressivAlgo_t algoName, EE_ExpressivThreshold_t thresholdName, out Int32 pValueOut);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivSetTrainingAction")]
	static extern Int32 Unmanged_EE_ExpressivSetTrainingAction(UInt32 userId, EE_ExpressivAlgo_t action);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivSetTrainingControl")]
	static extern Int32 Unmanged_EE_ExpressivSetTrainingControl(UInt32 userId, EE_ExpressivTrainingControl_t control);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivGetTrainingAction")]
	static extern Int32 Unmanged_EE_ExpressivGetTrainingAction(UInt32 userId, out EE_ExpressivAlgo_t pActionOut);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivGetTrainingTime")]
	static extern Int32 Unmanged_EE_ExpressivGetTrainingTime(UInt32 userId, out UInt32 pTrainingTimeOut);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivGetTrainedSignatureActions")]
	static extern Int32 Unmanged_EE_ExpressivGetTrainedSignatureActions(UInt32 userId, out UInt32 pTrainedActionsOut);


	[DllImport("edk.dll", EntryPoint = "EE_ExpressivGetTrainedSignatureAvailable")]
	static extern Int32 Unmanged_EE_ExpressivGetTrainedSignatureAvailable(UInt32 userId, out Int32 pfAvailableOut);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivSetSignatureType")]
	static extern Int32 Unmanged_EE_ExpressivSetSignatureType(UInt32 userId, EE_ExpressivSignature_t sigType);

	[DllImport("edk.dll", EntryPoint = "EE_ExpressivGetSignatureType")]
	static extern Int32 Unmanged_EE_ExpressivGetSignatureType(UInt32 userId, out EE_ExpressivSignature_t pSigTypeOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetActiveActions")]
	static extern Int32 Unmanged_EE_CognitivSetActiveActions(UInt32 userId, UInt32 activeActions);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetActiveActions")]
	static extern Int32 Unmanged_EE_CognitivGetActiveActions(UInt32 userId, out UInt32 pActiveActionsOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetTrainingTime")]
	static extern Int32 Unmanged_EE_CognitivGetTrainingTime(UInt32 userId, out UInt32 pTrainingTimeOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetTrainingControl")]
	static extern Int32 Unmanged_EE_CognitivSetTrainingControl(UInt32 userId, EE_CognitivTrainingControl_t control);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetTrainingAction")]
	static extern Int32 Unmanged_EE_CognitivSetTrainingAction(UInt32 userId, EE_CognitivAction_t action);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetTrainingAction")]
	static extern Int32 Unmanged_EE_CognitivGetTrainingAction(UInt32 userId, out EE_CognitivAction_t pActionOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetTrainedSignatureActions")]
	static extern Int32 Unmanged_EE_CognitivGetTrainedSignatureActions(UInt32 userId, out UInt32 pTrainedActionsOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetOverallSkillRating")]
	static extern Int32 Unmanged_EE_CognitivGetOverallSkillRating(UInt32 userId, out Single pOverallSkillRatingOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetActionSkillRating")]
	static extern Int32 Unmanged_EE_CognitivGetActionSkillRating(UInt32 userId, EE_CognitivAction_t action, out Single pActionSkillRatingOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetActivationLevel")]
	static extern Int32 Unmanged_EE_CognitivSetActivationLevel(UInt32 userId, Int32 level);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetActionSensitivity")]
	static extern Int32 Unmanged_EE_CognitivSetActionSensitivity(UInt32 userId,
										Int32 action1Sensitivity, Int32 action2Sensitivity,
										Int32 action3Sensitivity, Int32 action4Sensitivity);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetActivationLevel")]
	static extern Int32 Unmanged_EE_CognitivGetActivationLevel(UInt32 userId, out Int32 pLevelOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetActionSensitivity")]
	static extern Int32 Unmanged_EE_CognitivGetActionSensitivity(UInt32 userId,
										out Int32 pAction1SensitivityOut, out Int32 pAction2SensitivityOut,
										out Int32 pAction3SensitivityOut, out Int32 pAction4SensitivityOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivStartSamplingNeutral")]
	static extern Int32 Unmanged_EE_CognitivStartSamplingNeutral(UInt32 userId);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivStopSamplingNeutral")]
	static extern Int32 Unmanged_EE_CognitivStopSamplingNeutral(UInt32 userId);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetSignatureCaching")]
	static extern Int32 Unmanged_EE_CognitivSetSignatureCaching(UInt32 userId, UInt32 enabled);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetSignatureCaching")]
	static extern Int32 Unmanged_EE_CognitivGetSignatureCaching(UInt32 userId, out UInt32 pEnabledOut);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetSignatureCacheSize")]
	static extern Int32 Unmanged_EE_CognitivSetSignatureCacheSize(UInt32 userId, UInt32 size);

	[DllImport("edk.dll", EntryPoint = "EE_CognitivGetSignatureCacheSize")]
	static extern Int32 Unmanged_EE_CognitivGetSignatureCacheSize(UInt32 userId, out UInt32 pSizeOut);

	[DllImport("edk.dll", EntryPoint = "EE_HeadsetGetSensorDetails")]
	static extern Int32 Unmanged_EE_HeadsetGetSensorDetails(EE_InputChannels_t channelId, out InputSensorDescriptor_t pDescriptorOut);

	[DllImport("edk.dll", EntryPoint = "EE_HardwareGetVersion")]
	static extern Int32 Unmanged_EE_HardwareGetVersion(UInt32 userId, out UInt32 pHwVersionOut);

	[DllImport("edk.dll", EntryPoint = "EE_SoftwareGetVersion")]
	static extern Int32 Unmanged_EE_SoftwareGetVersion(StringBuilder pszVersionOut, UInt32 nVersionChars, out UInt32 pBuildNumOut);
//GyroDelta.--------------------------------------------------------------------------------------------------------
	[DllImport("edk.dll", EntryPoint = "EE_HeadsetGetGyroDelta")]
	static extern Int32 Unmanged_EE_HeadsetGetGyroDelta(UInt32 userId, out Int32 pXOut, out Int32 pYOut);

	[DllImport("edk.dll", EntryPoint = "EE_HeadsetGyroRezero")]
	static extern Int32 Unmanged_EE_HeadsetGyroRezero(UInt32 userId);
//--------------------------------------------------------------------------------------------------------------------

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationParamCreate")]
	static extern IntPtr Unmanged_EE_OptimizationParamCreate();

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationParamFree")]
	static extern void Unmanged_EE_OptimizationParamFree(IntPtr hParam);

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationEnable")]
	static extern Int32 Unmanged_EE_OptimizationEnable(IntPtr hParam);

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationIsEnabled")]
	static extern Int32 Unmanged_EE_OptimizationIsEnabled(out Boolean pEnabledOut);

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationDisable")]
	static extern Int32 Unmanged_EE_OptimizationDisable();

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationGetParam")]
	static extern Int32 Unmanged_EE_OptimizationGetParam(IntPtr hParam);

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationGetVitalAlgorithm")]
	static extern Int32 Unmanged_EE_OptimizationGetVitalAlgorithm(IntPtr hParam, EE_EmotivSuite_t suite, out UInt32 pVitalAlgorithmBitVectorOut);

	[DllImport("edk.dll", EntryPoint = "EE_OptimizationSetVitalAlgorithm")]
	static extern Int32 Unmanged_EE_OptimizationSetVitalAlgorithm(IntPtr hParam, EE_EmotivSuite_t suite, UInt32 vitalAlgorithmBitVector);

	[DllImport("edk.dll", EntryPoint = "EE_ResetDetection")]
	static extern Int32 Unmanged_EE_ResetDetection(UInt32 userId, EE_EmotivSuite_t suite, UInt32 detectionBitVector);

	[DllImport("edk.dll", EntryPoint = "EE_DataCreate")]
	static extern IntPtr Unmanaged_EE_DataCreate();
	[DllImport("edk.dll", EntryPoint = "EE_DataFree")]
	static extern void Unmanaged_EE_DataFree(IntPtr hData);
	[DllImport("edk.dll", EntryPoint = "EE_DataUpdateHandle")]
	static extern Int32 Unmanaged_EE_DataUpdateHandle(UInt32 userId, IntPtr hData);
	[DllImport("edk.dll", EntryPoint = "EE_DataGet")]
	static extern Int32 Unmanaged_EE_DataGet(IntPtr hData, EE_DataChannel_t channel, Double[] buffer, UInt32 bufferSizeInSample);
	[DllImport("edk.dll", EntryPoint = "EE_DataGetNumberOfSample")]
	static extern Int32 Unmanaged_EE_DataGetNumberOfSample(IntPtr hData, out UInt32 nSampleOut);
	[DllImport("edk.dll", EntryPoint = "EE_DataSetBufferSizeInSec")]
	static extern Int32 Unmanaged_EE_DataSetBufferSizeInSec(Single bufferSizeInSec);
	[DllImport("edk.dll", EntryPoint = "EE_DataGetBufferSizeInSec")]
	static extern Int32 Unmanaged_EE_DataGetBufferSizeInSec(out Single pBufferSizeInSecOut);
	[DllImport("edk.dll", EntryPoint = "EE_DataAcquisitionEnable")]
	static extern Int32 Unmanaged_EE_DataAcquisitionEnable(UInt32 userId, Boolean enable);
	[DllImport("edk.dll", EntryPoint = "EE_DataAcquisitionIsEnabled")]
	static extern Int32 Unmanaged_EE_DataAcquisitionIsEnabled(UInt32 userId, out Boolean pEnableOut);
	[DllImport("edk.dll", EntryPoint = "EE_DataSetSychronizationSignal")]
	static extern Int32 Unmanaged_EE_DataSetSychronizationSignal(UInt32 userId, Int32 signal);
	[DllImport("edk.dll", EntryPoint = "EE_DataSetMarker")]
	static extern Int32 Unmanaged_EE_DataSetMarker(UInt32 userId, Int32 marker);
	[DllImport("edk.dll", EntryPoint = "EE_DataGetSamplingRate")]
	static extern Int32 Unmanaged_EE_DataGetSamplingRate(UInt32 userId, out UInt32 pSamplingRate);

	[DllImport("edk.dll", EntryPoint = "ES_Create")]
	static extern IntPtr Unmanaged_ES_Create();

	[DllImport("edk.dll", EntryPoint = "ES_Free")]
	static extern void Unmanaged_ES_Free(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_Init")]
	static extern void Unmanaged_ES_Init(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_GetTimeFromStart")]
	static extern Single Unmanaged_ES_GetTimeFromStart(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_GetHeadsetOn")]
	static extern Int32 Unmanaged_ES_GetHeadsetOn(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_GetNumContactQualityChannels")]
	static extern Int32 Unmanaged_ES_GetNumContactQualityChannels(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_GetContactQuality")]
	static extern EE_EEG_ContactQuality_t Unmanaged_ES_GetContactQuality(IntPtr state, Int32 electroIdx);
	[DllImport("edk.dll", EntryPoint = "ES_GetContactQualityFromAllChannels")]
	static extern Int32 Unmanaged_ES_GetContactQualityFromAllChannels(IntPtr state, EE_EEG_ContactQuality_t[] contactQuality, UInt32 numChannels);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsBlink")]
	static extern Boolean Unmanaged_ES_ExpressivIsBlink(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsLeftWink")]
	static extern Boolean Unmanaged_ES_ExpressivIsLeftWink(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsRightWink")]
	static extern Boolean Unmanaged_ES_ExpressivIsRightWink(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsEyesOpen")]
	static extern Boolean Unmanaged_ES_ExpressivIsEyesOpen(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsLookingUp")]
	static extern Boolean Unmanaged_ES_ExpressivIsLookingUp(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsLookingDown")]
	static extern Boolean Unmanaged_ES_ExpressivIsLookingDown(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsLookingLeft")]
	static extern Boolean Unmanaged_ES_ExpressivIsLookingLeft(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsLookingRight")]
	static extern Boolean Unmanaged_ES_ExpressivIsLookingRight(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetEyelidState")]
	static extern void Unmanaged_ES_ExpressivGetEyelidState(IntPtr state, out Single leftEye, out Single rightEye);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetEyeLocation")]
	static extern void Unmanaged_ES_ExpressivGetEyeLocation(IntPtr state, out Single x, out Single y);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetEyebrowExtent")]
	static extern Single Unmanaged_ES_ExpressivGetEyebrowExtent(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetSmileExtent")]
	static extern Single Unmanaged_ES_ExpressivGetSmileExtent(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetClenchExtent")]
	static extern Single Unmanaged_ES_ExpressivGetClenchExtent(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetUpperFaceAction")]
	static extern EE_ExpressivAlgo_t Unmanaged_ES_ExpressivGetUpperFaceAction(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetUpperFaceActionPower")]
	static extern Single Unmanaged_ES_ExpressivGetUpperFaceActionPower(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetLowerFaceAction")]
	static extern EE_ExpressivAlgo_t Unmanaged_ES_ExpressivGetLowerFaceAction(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_ExpressivGetLowerFaceActionPower")]
	static extern Single Unmanaged_ES_ExpressivGetLowerFaceActionPower(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivIsActive")]
	static extern Boolean Unmanaged_ES_ExpressivIsActive(IntPtr state, EE_ExpressivAlgo_t type);

	[DllImport("edk.dll", EntryPoint = "ES_AffectivGetExcitementLongTermScore")]
	static extern Single Unmanaged_ES_AffectivGetExcitementLongTermScore(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_AffectivGetExcitementShortTermScore")]
	static extern Single Unmanaged_ES_AffectivGetExcitementShortTermScore(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_AffectivIsActive")]
	static extern Boolean Unmanaged_ES_AffectivIsActive(IntPtr state, EE_AffectivAlgo_t type);

	[DllImport("edk.dll", EntryPoint = "ES_AffectivGetMeditationScore")]
	static extern Single Unmanaged_ES_AffectivGetMeditationScore(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_AffectivGetFrustrationScore")]
	static extern Single Unmanaged_ES_AffectivGetFrustrationScore(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_AffectivGetEngagementBoredomScore")]
	static extern Single Unmanaged_ES_AffectivGetEngagementBoredomScore(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_CognitivGetCurrentAction")]
	static extern EE_CognitivAction_t Unmanaged_ES_CognitivGetCurrentAction(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_CognitivGetCurrentActionPower")]
	static extern Single Unmanaged_ES_CognitivGetCurrentActionPower(IntPtr state);

	[DllImport("edk.dll", EntryPoint = "ES_CognitivIsActive")]
	static extern Boolean Unmanaged_ES_CognitivIsActive(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_CognitivGetCurrentLevelRating")]
	static extern Single Unmanaged_ES_CognitivGetCurrentLevelRating(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_GetWirelessSignalStatus")]
	static extern EE_SignalStrength_t Unmanaged_ES_GetWirelessSignalStatus(IntPtr state);
	[DllImport("edk.dll", EntryPoint = "ES_Copy")]
	static extern void Unmanaged_ES_Copy(IntPtr a, IntPtr b);
	[DllImport("edk.dll", EntryPoint = "ES_AffectivEqual")]
	static extern Boolean Unmanaged_ES_AffectivEqual(IntPtr a, IntPtr b);
	[DllImport("edk.dll", EntryPoint = "ES_ExpressivEqual")]
	static extern Boolean Unmanaged_ES_ExpressivEqual(IntPtr a, IntPtr b);
	[DllImport("edk.dll", EntryPoint = "ES_CognitivEqual")]
	static extern Boolean Unmanaged_ES_CognitivEqual(IntPtr a, IntPtr b);
	[DllImport("edk.dll", EntryPoint = "ES_EmoEngineEqual")]
	static extern Boolean Unmanaged_ES_EmoEngineEqual(IntPtr a, IntPtr b);
	[DllImport("edk.dll", EntryPoint = "ES_Equal")]
	static extern Boolean Unmanaged_ES_Equal(IntPtr a, IntPtr b);
	[DllImport("edk.dll", EntryPoint = "ES_GetBatteryChargeLevel")]
	static extern void Unmanaged_ES_GetBatteryChargeLevel(IntPtr state, out Int32 chargeLevel, out Int32 maxChargeLevel);       

	//Tien031109 test
	[DllImport("edk.dll", EntryPoint = "EE_CognitivSetCurrentLevel")]
	static extern Int32 Unmanaged_EE_CognitivSetCurrentLevel(Int16  userId,
												  EE_CognitivLevel_t  level,  
												  EE_CognitivAction_t  level1Action,  
												  EE_CognitivAction_t  level2Action,  
												  EE_CognitivAction_t  level3Action,  
												  EE_CognitivAction_t  level4Action);
	
	public static Int32 EE_CognitivSetCurrentLevel(Int16  userId,
												  EE_CognitivLevel_t  level,  
												  EE_CognitivAction_t  level1Action,  
												  EE_CognitivAction_t  level2Action,  
												  EE_CognitivAction_t  level3Action,  
												  EE_CognitivAction_t  level4Action)
	{
		return Unmanaged_EE_CognitivSetCurrentLevel(userId,level,level1Action,level2Action,level3Action,level4Action);
	}
    //Tien280110 test
    /// <summary>
    /// Enable multi-cognitiv actions at a time (Tested working in Unity)
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="activeActions"></param>
    /// <param name="numOfActions"></param>
    [DllImport("cognitivEX.dll", EntryPoint = "SetMultiActiveActions")]
    static extern void Unmanaged_SetMultiActiveActions(int UserID ,EE_CognitivAction_t[] activeActions,int numOfActions);

    public static void SetMultiActiveActions(int UserID, EE_CognitivAction_t[] activeActions, int numOfActions)
    {
        Unmanaged_SetMultiActiveActions(UserID,activeActions,numOfActions);
    }
	
	public static Int32 EE_EngineConnect(String security)
	{
        return Unmanged_EE_EngineConnect(security);
	}
	public static Int32 EE_EngineRemoteConnect(String szHost, UInt16 port)
	{
		return Unmanged_EE_EngineRemoteConnect(szHost, port);
	}
	public static Int32 EE_EngineDisconnect()
	{
		return Unmanged_EE_EngineDisconnect();
	}
	public static Int32 EE_EnableDiagnostics(String szFilename, Int32 fEnable, Int32 nReserved)
	{
		return Unmanged_EE_EnableDiagnostics(szFilename, fEnable, nReserved);
	}
	public static IntPtr EE_EmoEngineEventCreate()
	{
		return Unmanged_EE_EmoEngineEventCreate();
	}
	public static IntPtr EE_ProfileEventCreate()
	{
		return Unmanged_EE_ProfileEventCreate();
	}
	public static void EE_EmoEngineEventFree(IntPtr hEvent)
	{
		Unmanged_EE_EmoEngineEventFree(hEvent);
	}
	public static IntPtr EE_EmoStateCreate()
	{
		return Unmanged_EE_EmoStateCreate();
	}
	public static void EE_EmoStateFree(IntPtr hState)
	{
		Unmanged_EE_EmoStateFree(hState);
	}
	public static EE_Event_t EE_EmoEngineEventGetType(IntPtr hEvent)
	{
		return Unmanged_EE_EmoEngineEventGetType(hEvent);
	}
	public static EE_CognitivEvent_t EE_CognitivEventGetType(IntPtr hEvent)
	{
		return Unmanged_EE_CognitivEventGetType(hEvent);
	}
	public static EE_ExpressivEvent_t EE_ExpressivEventGetType(IntPtr hEvent)
	{
		return Unmanged_EE_ExpressivEventGetType(hEvent);
	}

	public static Int32 EE_EmoEngineEventGetUserId(IntPtr hEvent, out UInt32 pUserIdOut)
	{
		return Unmanged_EE_EmoEngineEventGetUserId(hEvent, out pUserIdOut);
	}

	public static Int32 EE_EmoEngineEventGetEmoState(IntPtr hEvent, IntPtr hEmoState)
	{
		return Unmanged_EE_EmoEngineEventGetEmoState(hEvent, hEmoState);
	}

	public static Int32 EE_EngineGetNextEvent(IntPtr hEvent)
	{
		return Unmanged_EE_EngineGetNextEvent(hEvent);
	}

	public static Int32 EE_EngineClearEventQueue(Int32 eventTypes)
	{
		return Unmanged_EE_EngineClearEventQueue(eventTypes);
	}

	public static Int32 EE_EngineGetNumUser(out UInt32 pNumUserOut)
	{
		return Unmanged_EE_EngineGetNumUser(out pNumUserOut);
	}

	public static Int32 EE_SetHardwarePlayerDisplay(UInt32 userId, UInt32 playerNum)
	{
		return Unmanged_EE_SetHardwarePlayerDisplay(userId, playerNum);
	}

	public static Int32 EE_SetUserProfile(UInt32 userId, byte[] profileBuffer, UInt32 length)
	{
		return Unmanged_EE_SetUserProfile(userId, profileBuffer, length);
	}

	public static Int32 EE_GetUserProfile(UInt32 userId, IntPtr hEvent)
	{
		return Unmanged_EE_GetUserProfile(userId, hEvent);
	}

	public static Int32 EE_GetBaseProfile(IntPtr hEvent)
	{
		return Unmanged_EE_GetBaseProfile(hEvent);
	}

	public static Int32 EE_GetUserProfileSize(IntPtr hEvt, out UInt32 pProfileSizeOut)
	{
		return Unmanged_EE_GetUserProfileSize(hEvt, out pProfileSizeOut);
	}


	public static Int32 EE_GetUserProfileBytes(IntPtr hEvt, Byte[] destBuffer, UInt32 length)
	{
		return Unmanged_EE_GetUserProfileBytes(hEvt, destBuffer, length);
	}

	public static Int32 EE_LoadUserProfile(UInt32 userID, String szInputFilename)
	{
		return Unmanged_EE_LoadUserProfile(userID, szInputFilename);
	}

	public static Int32 EE_SaveUserProfile(UInt32 userID, String szOutputFilename)
	{
		return Unmanged_EE_SaveUserProfile(userID, szOutputFilename);
	}

	public static Int32 EE_ExpressivSetThreshold(UInt32 userId, EE_ExpressivAlgo_t algoName, EE_ExpressivThreshold_t thresholdName, Int32 value)
	{
		return Unmanged_EE_ExpressivSetThreshold(userId, algoName, thresholdName, value);
	}

	public static Int32 EE_ExpressivGetThreshold(UInt32 userId, EE_ExpressivAlgo_t algoName, EE_ExpressivThreshold_t thresholdName, out Int32 pValueOut)
	{
		return Unmanged_EE_ExpressivGetThreshold(userId, algoName, thresholdName, out pValueOut);
	}

	public static Int32 EE_ExpressivSetTrainingAction(UInt32 userId, EE_ExpressivAlgo_t action)
	{
		return Unmanged_EE_ExpressivSetTrainingAction(userId, action);
	}

	public static Int32 EE_ExpressivSetTrainingControl(UInt32 userId, EE_ExpressivTrainingControl_t control)
	{
		return Unmanged_EE_ExpressivSetTrainingControl(userId, control);
	}

	public static Int32 EE_ExpressivGetTrainingAction(UInt32 userId, out EE_ExpressivAlgo_t pActionOut)
	{
		return Unmanged_EE_ExpressivGetTrainingAction(userId, out pActionOut);
	}

	public static Int32 EE_ExpressivGetTrainingTime(UInt32 userId, out UInt32 pTrainingTimeOut)
	{
		return Unmanged_EE_ExpressivGetTrainingTime(userId, out pTrainingTimeOut);
	}

	public static Int32 EE_ExpressivGetTrainedSignatureActions(UInt32 userId, out UInt32 pTrainedActionsOut)
	{
		return Unmanged_EE_ExpressivGetTrainedSignatureActions(userId, out pTrainedActionsOut);
	}


	public static Int32 EE_ExpressivGetTrainedSignatureAvailable(UInt32 userId, out Int32 pfAvailableOut)
	{
		return Unmanged_EE_ExpressivGetTrainedSignatureAvailable(userId, out pfAvailableOut);
	}

	public static Int32 EE_ExpressivSetSignatureType(UInt32 userId, EE_ExpressivSignature_t sigType)
	{
		return Unmanged_EE_ExpressivSetSignatureType(userId, sigType);
	}

	public static Int32 EE_ExpressivGetSignatureType(UInt32 userId, out EE_ExpressivSignature_t pSigTypeOut)
	{
		return Unmanged_EE_ExpressivGetSignatureType(userId, out pSigTypeOut);
	}

	public static Int32 EE_CognitivSetActiveActions(UInt32 userId, UInt32 activeActions)
	{
		return Unmanged_EE_CognitivSetActiveActions(userId, activeActions);
	}

	public static Int32 EE_CognitivGetActiveActions(UInt32 userId, out UInt32 pActiveActionsOut)
	{
		return Unmanged_EE_CognitivGetActiveActions(userId, out pActiveActionsOut);
	}

	public static Int32 EE_CognitivGetTrainingTime(UInt32 userId, out UInt32 pTrainingTimeOut)
	{
		return Unmanged_EE_CognitivGetTrainingTime(userId, out pTrainingTimeOut);
	}

	public static Int32 EE_CognitivSetTrainingControl(UInt32 userId, EE_CognitivTrainingControl_t control)
	{
		return Unmanged_EE_CognitivSetTrainingControl(userId, control);
	}

	public static Int32 EE_CognitivSetTrainingAction(UInt32 userId, EE_CognitivAction_t action)
	{
		return Unmanged_EE_CognitivSetTrainingAction(userId, action);
	}

	public static Int32 EE_CognitivGetTrainingAction(UInt32 userId, out EE_CognitivAction_t pActionOut)
	{
		return Unmanged_EE_CognitivGetTrainingAction(userId, out pActionOut);
	}

	public static Int32 EE_CognitivGetTrainedSignatureActions(UInt32 userId, out UInt32 pTrainedActionsOut)
	{
		return Unmanged_EE_CognitivGetTrainedSignatureActions(userId, out pTrainedActionsOut);
	}

	public static Int32 EE_CognitivGetOverallSkillRating(UInt32 userId, out Single pOverallSkillRatingOut)
	{
		return Unmanged_EE_CognitivGetOverallSkillRating(userId, out pOverallSkillRatingOut);
	}

	public static Int32 EE_CognitivGetActionSkillRating(UInt32 userId, EE_CognitivAction_t action, out Single pActionSkillRatingOut)
	{
		return Unmanged_EE_CognitivGetActionSkillRating(userId, action, out pActionSkillRatingOut);
	}

	public static Int32 EE_CognitivSetActivationLevel(UInt32 userId, Int32 level)
	{
		return Unmanged_EE_CognitivSetActivationLevel(userId, level);
	}

	public static Int32 EE_CognitivSetActionSensitivity(UInt32 userId,
										Int32 action1Sensitivity, Int32 action2Sensitivity,
										Int32 action3Sensitivity, Int32 action4Sensitivity)
	{
		return Unmanged_EE_CognitivSetActionSensitivity(userId, action1Sensitivity, action2Sensitivity, action3Sensitivity, action4Sensitivity);
	}

	public static Int32 EE_CognitivGetActivationLevel(UInt32 userId, out Int32 pLevelOut)
	{
		return Unmanged_EE_CognitivGetActivationLevel(userId, out pLevelOut);
	}

	public static Int32 EE_CognitivGetActionSensitivity(UInt32 userId,
										out Int32 pAction1SensitivityOut, out Int32 pAction2SensitivityOut,
										out Int32 pAction3SensitivityOut, out Int32 pAction4SensitivityOut)
	{
		return Unmanged_EE_CognitivGetActionSensitivity(userId, out pAction1SensitivityOut, out pAction2SensitivityOut, 
			out pAction3SensitivityOut, out pAction4SensitivityOut);
	}

	public static Int32 EE_CognitivStartSamplingNeutral(UInt32 userId)
	{
		return Unmanged_EE_CognitivStartSamplingNeutral(userId);
	}

	public static Int32 EE_CognitivStopSamplingNeutral(UInt32 userId)
	{
		return Unmanged_EE_CognitivStopSamplingNeutral(userId);
	}

	public static Int32 EE_CognitivSetSignatureCaching(UInt32 userId, UInt32 enabled)
	{
		return Unmanged_EE_CognitivSetSignatureCaching(userId, enabled);
	}

	public static Int32 EE_CognitivGetSignatureCaching(UInt32 userId, out UInt32 pEnabledOut)
	{
		return Unmanged_EE_CognitivGetSignatureCaching(userId, out pEnabledOut);
	}

	public static Int32 EE_CognitivSetSignatureCacheSize(UInt32 userId, UInt32 size)
	{
		return Unmanged_EE_CognitivSetSignatureCacheSize(userId, size);
	}

	public static Int32 EE_CognitivGetSignatureCacheSize(UInt32 userId, out UInt32 pSizeOut)
	{
		return Unmanged_EE_CognitivGetSignatureCacheSize(userId, out pSizeOut);
	}

	public static Int32 EE_HeadsetGetSensorDetails(EE_InputChannels_t channelId, out InputSensorDescriptor_t pDescriptorOut)
	{
		return Unmanged_EE_HeadsetGetSensorDetails(channelId, out pDescriptorOut);
	}

	public static Int32 EE_HardwareGetVersion(UInt32 userId, out UInt32 pHwVersionOut)
	{
		return Unmanged_EE_HardwareGetVersion(userId, out pHwVersionOut);
	}

	public static Int32 EE_SoftwareGetVersion(StringBuilder pszVersionOut, UInt32 nVersionChars, out UInt32 pBuildNumOut)
	{
		return Unmanged_EE_SoftwareGetVersion(pszVersionOut, nVersionChars, out pBuildNumOut);
	}

	public static Int32 EE_HeadsetGetGyroDelta(UInt32 userId, out Int32 pXOut, out Int32 pYOut)
	{
		return Unmanged_EE_HeadsetGetGyroDelta(userId, out pXOut, out pYOut);
	}

	public static Int32 EE_HeadsetGyroRezero(UInt32 userId)
	{
		return Unmanged_EE_HeadsetGyroRezero(userId);
	}

	public static IntPtr EE_OptimizationParamCreate()
	{
		return Unmanged_EE_OptimizationParamCreate();
	}

	public static void EE_OptimizationParamFree(IntPtr hParam)
	{
		Unmanged_EE_OptimizationParamFree(hParam);
	}

	public static Int32 EE_OptimizationEnable(IntPtr hParam)
	{
		return Unmanged_EE_OptimizationEnable(hParam);
	}

	public static Int32 EE_OptimizationIsEnabled(out Boolean pEnabledOut)
	{
		return Unmanged_EE_OptimizationIsEnabled(out pEnabledOut);
	}

	public static Int32 EE_OptimizationDisable()
	{
		return Unmanged_EE_OptimizationDisable();
	}

	public static Int32 EE_OptimizationGetParam(IntPtr hParam)
	{
		return Unmanged_EE_OptimizationGetParam(hParam);
	}

	public static Int32 EE_OptimizationGetVitalAlgorithm(IntPtr hParam, EE_EmotivSuite_t suite, out UInt32 pVitalAlgorithmBitVectorOut)
	{
		return Unmanged_EE_OptimizationGetVitalAlgorithm(hParam, suite, out pVitalAlgorithmBitVectorOut);
	}

	
	public static Int32 EE_OptimizationSetVitalAlgorithm(IntPtr hParam, EE_EmotivSuite_t suite, UInt32 vitalAlgorithmBitVector)
	{
		return Unmanged_EE_OptimizationSetVitalAlgorithm(hParam, suite, vitalAlgorithmBitVector);
	}

	
	public static Int32 EE_ResetDetection(UInt32 userId, EE_EmotivSuite_t suite, UInt32 detectionBitVector)
	{
		return Unmanged_EE_ResetDetection(userId, suite, detectionBitVector);
	}

//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_START
	//public static IntPtr EE_DataCreate()
	//{
		//return Unmanaged_EE_DataCreate();
	//}
	//
	//public static void EE_DataFree(IntPtr hData)
	//{
		//Unmanaged_EE_DataFree(hData);
	//}
	//
	//public static Int32 EE_DataUpdateHandle(UInt32 userId, IntPtr hData)
	//{
		//return Unmanaged_EE_DataUpdateHandle(userId, hData);
	//}
	//
	//public static Int32 EE_DataGet(IntPtr hData, EE_DataChannel_t channel, Double[] buffer, UInt32 bufferSizeInSample)
	//{
		//return Unmanaged_EE_DataGet(hData, channel, buffer, bufferSizeInSample);
	//}
	//
	//public static Int32 EE_DataGetNumberOfSample(IntPtr hData, out UInt32 nSampleOut)
	//{
		//return Unmanaged_EE_DataGetNumberOfSample(hData, out nSampleOut);
	//}
	//
	//public static Int32 EE_DataSetBufferSizeInSec(Single bufferSizeInSec)
	//{
		//return Unmanaged_EE_DataSetBufferSizeInSec(bufferSizeInSec);
	//}
	//
	//public static Int32 EE_DataGetBufferSizeInSec(out Single pBufferSizeInSecOut)
	//{
		//return Unmanaged_EE_DataGetBufferSizeInSec(out pBufferSizeInSecOut);
	//}
	//
	//public static Int32 EE_DataAcquisitionEnable(UInt32 userId, Boolean enable)
	//{
		//return Unmanaged_EE_DataAcquisitionEnable(userId, enable);
	//}
//
	//public static Int32 EE_DataAcquisitionIsEnabled(UInt32 userId, out Boolean pEnableOut)
	//{
		//return Unmanaged_EE_DataAcquisitionIsEnabled(userId, out pEnableOut);
	//}
//
	//public static Int32 EE_DataSetSychronizationSignal(UInt32 userId, Int32 signal)
	//{
		//return Unmanaged_EE_DataSetSychronizationSignal(userId, signal);
	//}
//
	//public static Int32 EE_DataSetMarker(UInt32 userId, Int32 marker)
	//{
		//return Unmanaged_EE_DataSetMarker(userId, marker);
	//}
//
	//public static Int32 EE_DataGetSamplingRate(UInt32 userId, out UInt32 pSamplingRateOut)
	//{
		//return Unmanaged_EE_DataGetSamplingRate(userId, out pSamplingRateOut);
	//}
//DEPLOYMENT::NON_PREMIUM_RELEASE::REMOVE_END

	public static IntPtr ES_Create()
	{
		return Unmanaged_ES_Create();
	}
	public static void ES_Free(IntPtr state)
	{
		Unmanaged_ES_Free(state);
	}
	public static void ES_Init(IntPtr state)
	{
		Unmanaged_ES_Init(state);
	}
	public static Single ES_GetTimeFromStart(IntPtr state)
	{
		return Unmanaged_ES_GetTimeFromStart(state);
	}
	public static Int32 ES_GetHeadsetOn(IntPtr state)
	{
		return Unmanaged_ES_GetHeadsetOn(state);
	}
	public static Int32 ES_GetNumContactQualityChannels(IntPtr state)
	{
		return Unmanaged_ES_GetNumContactQualityChannels(state);
	}
	public static EE_EEG_ContactQuality_t ES_GetContactQuality(IntPtr state, Int32 electroIdx)
	{
		return Unmanaged_ES_GetContactQuality(state, electroIdx);
	}
	public static Int32 ES_GetContactQualityFromAllChannels(IntPtr state, out EE_EEG_ContactQuality_t[] contactQuality)
	{
		Int32 numChannels = EdkDll.ES_GetNumContactQualityChannels(state);
		contactQuality = new EE_EEG_ContactQuality_t[numChannels];
		return Unmanaged_ES_GetContactQualityFromAllChannels(state, contactQuality, (UInt32)contactQuality.Length);
	}
	public static Boolean ES_ExpressivIsBlink(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsBlink(state);
	}

	public static Boolean ES_ExpressivIsLeftWink(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsLeftWink(state);
	}
	public static Boolean ES_ExpressivIsRightWink(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsRightWink(state);
	}
	public static Boolean ES_ExpressivIsEyesOpen(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsEyesOpen(state);
	}
	public static Boolean ES_ExpressivIsLookingUp(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsLookingUp(state);
	}
	public static Boolean ES_ExpressivIsLookingDown(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsLookingDown(state);
	}

	public static Boolean ES_ExpressivIsLookingLeft(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsLookingLeft(state);
	}
	public static Boolean ES_ExpressivIsLookingRight(IntPtr state)
	{
		return Unmanaged_ES_ExpressivIsLookingRight(state);
	}

	public static void ES_ExpressivGetEyelidState(IntPtr state, out Single leftEye, out Single rightEye)
	{
		Unmanaged_ES_ExpressivGetEyelidState(state, out leftEye, out rightEye);
	}

	public static void ES_ExpressivGetEyeLocation(IntPtr state, out Single x, out Single y)
	{
		Unmanaged_ES_ExpressivGetEyeLocation(state, out x, out y);
	}

	public static Single ES_ExpressivGetEyebrowExtent(IntPtr state)
	{
		return Unmanaged_ES_ExpressivGetEyebrowExtent(state);
	}

	public static Single ES_ExpressivGetSmileExtent(IntPtr state)
	{
		return Unmanaged_ES_ExpressivGetSmileExtent(state);
	}

	public static Single ES_ExpressivGetClenchExtent(IntPtr state)
	{
		return Unmanaged_ES_ExpressivGetClenchExtent(state);
	}

	public static EE_ExpressivAlgo_t ES_ExpressivGetUpperFaceAction(IntPtr state)
	{
		return Unmanaged_ES_ExpressivGetUpperFaceAction(state);
	}

	public static Single ES_ExpressivGetUpperFaceActionPower(IntPtr state)
	{
		return Unmanaged_ES_ExpressivGetUpperFaceActionPower(state);
	}

	public static EE_ExpressivAlgo_t ES_ExpressivGetLowerFaceAction(IntPtr state)
	{
		return Unmanaged_ES_ExpressivGetLowerFaceAction(state);
	}

	public static Single ES_ExpressivGetLowerFaceActionPower(IntPtr state)
	{
		return Unmanaged_ES_ExpressivGetLowerFaceActionPower(state);
	}
	public static Boolean ES_ExpressivIsActive(IntPtr state, EE_ExpressivAlgo_t type)
	{
		return Unmanaged_ES_ExpressivIsActive(state, type);
	}

	public static Single ES_AffectivGetExcitementLongTermScore(IntPtr state)
	{
		return Unmanaged_ES_AffectivGetExcitementLongTermScore(state);
	}

	public static Single ES_AffectivGetExcitementShortTermScore(IntPtr state)
	{
		return Unmanaged_ES_AffectivGetExcitementShortTermScore(state);
	}

	public static Boolean ES_AffectivIsActive(IntPtr state, EE_AffectivAlgo_t type)
	{
		return Unmanaged_ES_AffectivIsActive(state, type);
	}

	public static Single ES_AffectivGetMeditationScore(IntPtr state)
	{
		return Unmanaged_ES_AffectivGetMeditationScore(state);
	}

	public static Single ES_AffectivGetFrustrationScore(IntPtr state)
	{
		return Unmanaged_ES_AffectivGetFrustrationScore(state);
	}
	public static Single ES_AffectivGetEngagementBoredomScore(IntPtr state)
	{
		return Unmanaged_ES_AffectivGetEngagementBoredomScore(state);
	}

	public static EE_CognitivAction_t ES_CognitivGetCurrentAction(IntPtr state)
	{
		return Unmanaged_ES_CognitivGetCurrentAction(state);
	}

	public static Single ES_CognitivGetCurrentActionPower(IntPtr state)
	{
		return Unmanaged_ES_CognitivGetCurrentActionPower(state);
	}

	public static Boolean ES_CognitivIsActive(IntPtr state)
	{
		return Unmanaged_ES_CognitivIsActive(state);
	}
	public static Single ES_CognitivGetCurrentLevelRating(IntPtr state)
	{
		return Unmanaged_ES_CognitivGetCurrentLevelRating(state);
	}
	public static EE_SignalStrength_t ES_GetWirelessSignalStatus(IntPtr state)
	{
		return Unmanaged_ES_GetWirelessSignalStatus(state);
	}
	public static void ES_Copy(IntPtr a, IntPtr b)
	{
		Unmanaged_ES_Copy(a, b);
	}
	public static Boolean ES_AffectivEqual(IntPtr a, IntPtr b)
	{
		return Unmanaged_ES_AffectivEqual(a, b);
	}
	public static Boolean ES_ExpressivEqual(IntPtr a, IntPtr b)
	{
		return Unmanaged_ES_ExpressivEqual(a, b);
	}
	public static Boolean ES_CognitivEqual(IntPtr a, IntPtr b)
	{
		return Unmanaged_ES_CognitivEqual(a, b);
	}
	public static Boolean ES_EmoEngineEqual(IntPtr a, IntPtr b)
	{
		return Unmanaged_ES_EmoEngineEqual(a, b);
	}
	public static Boolean ES_Equal(IntPtr a, IntPtr b)
	{
		return Unmanaged_ES_Equal(a, b);
	}
	public static void ES_GetBatteryChargeLevel(IntPtr state, out Int32 chargeLevel, out Int32 maxChargeLevel)
	{
		Unmanaged_ES_GetBatteryChargeLevel(state, out chargeLevel, out maxChargeLevel);
	}
}
