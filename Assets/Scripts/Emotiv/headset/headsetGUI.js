// Headset node position
// 0: AF3 3
// 1: AF4  16
// 2: F3    5
// 3: F4   14
// 4: F7    4
// 5: F8    15
// 6: FC5   6
// 7: FC6  13
// 8: T7    7
// 9: T8    12
// 10: CMS   0
// 11: DLR   1
// 12: P7     8
// 13: P8     11
// 14: O1     9
// 15: O2     10
//typedef enum EE_InputChannels_enum 
//{
//            EE_CHAN_CMS = 0, EE_CHAN_DRL 1, EE_CHAN_FP1 2 , EE_CHAN_AF3 3, EE_CHAN_F7 4,  
//			EE_CHAN_F3 5, EE_CHAN_FC5 6 , EE_CHAN_T7 7 , EE_CHAN_P7 8 , EE_CHAN_O1 9,
//			EE_CHAN_O2, EE_CHAN_P8, EE_CHAN_T8, EE_CHAN_FC6, EE_CHAN_F4,
//		    EE_CHAN_F8, EE_CHAN_AF4, EE_CHAN_FP2
//} EE_InputChannels_t;

//var EpocManager:GameObject; 


/// scale

var int_scale : float = 0.7;


var headArea : Rect;
private var head : Rect;
public static var node:int[];

var headset:Texture2D;
var redButt:Texture2D;
var blackButt:Texture2D;
var orangeButt:Texture2D;
var yellowButt:Texture2D;
var greenButt:Texture2D;
var isEnable = true;

// nodeStatus: 
// O: Black
// 1: Red
// 2: Orange
// 3: Yellow
// 4: Green
function nodeStatus(node:int): Texture2D
{
	var returnButt: Texture2D;
	switch (node)
	{ 
		case 0:
			returnButt = blackButt;
			break;
		case 1:
			returnButt = redButt; 
			break; 
		case 2:
			returnButt = orangeButt;
			break; 
		case 3:
			returnButt = yellowButt; 
			break; 
		case 4:
			returnButt = greenButt; 
			break; 
		default:
			returnButt = blackButt;
			break;			
	}
	return returnButt; 
}

function DisableInfo()
{
	isEnable = false;
}

function EnableInfo()
{
	isEnable = true;
}

function Start()
{
	var i:int;
	node = new int[18];
	for (i = 0 ; i<18 ;i++)
 	{
  	  node[i] = 0;
 	}  

	if(headArea == Rect(0,0,0,0))
	{
		headArea = Rect(600, 70, 225*int_scale, 200*int_scale);
	}
	if(head == Rect(0,0,0,0))
	{
		head = Rect(0, 0, 200*int_scale, 200*int_scale);
	}
}

function DrawGUI(){
	
	  GUI.BeginGroup (headArea);
      GUI.DrawTexture ( head , headset);
	  
	  GUI.DrawTexture ( Rect(47*int_scale, 26*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[3]));
	  GUI.DrawTexture ( Rect(130*int_scale, 26*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[16]));
	  
	  GUI.DrawTexture ( Rect(67*int_scale, 51*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[5]));
	  GUI.DrawTexture ( Rect(110*int_scale, 51*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[14]));
	  
	  GUI.DrawTexture ( Rect(18*int_scale, 53*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[4]));
	  GUI.DrawTexture ( Rect(159*int_scale, 53*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[15]));
	  
	  GUI.DrawTexture ( Rect(37*int_scale, 71*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[5]));
	  GUI.DrawTexture ( Rect(141*int_scale, 71*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[13]));
	  
	  // T7 T8
	  GUI.DrawTexture ( Rect(8*int_scale, 93*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[7]));
	  GUI.DrawTexture ( Rect(169*int_scale, 93*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[12]));
	  
	  //CMS
	  GUI.DrawTexture ( Rect(18*int_scale, 118*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[0]));
	  GUI.DrawTexture ( Rect(159*int_scale, 118*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[1]));
	  
	  GUI.DrawTexture ( Rect(37*int_scale, 148*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[8]));
	  GUI.DrawTexture ( Rect(140*int_scale, 148*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[11]));
	  
	  GUI.DrawTexture ( Rect(64*int_scale, 172*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[9]));
	  GUI.DrawTexture ( Rect(113*int_scale, 172*int_scale, 23*int_scale, 23*int_scale) , nodeStatus(node[10]));

    GUI.EndGroup(); 
}

function OnGUI()
{
	if (isEnable) DrawGUI();
}
function Update()
{
 
	 for (i = 0 ; i< EmoEngineInst.nChan ;i++)
	 {
		  node[i] = EmoEngineInst.Cq[i];
	 }  
}

