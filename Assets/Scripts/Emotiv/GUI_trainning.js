
/// Rect

// Rect for labels
var rct_label_EPOC : Rect;
var rct_label_profile : Rect;

// Rect for buttons EPOC, profile
var rct_bt_EPOC : Rect;
var rct_bt_profile : Rect;

// Rect for buttons
var rct_grid_button : Rect;
var rct_bt_accept : Rect;
var rct_bt_reject : Rect;

// Rect for image (result)
var rct_result : Rect;

var Result : int = 0;
/// Strings
var str_label_EPOC : String[];
var int_label_EPOC : int = 0;

var str_label_profile : Array = new Array();
var int_label_profile : int = 0;

/// Vector2 Scoll View
private var Vct2 : Vector2;
private var Vct2_profile : Vector2;

/// Private values
private var rct_popUp_EPOC : Rect;
private var rct_popUp_profile : Rect;

private var int_grid_button : int;

private var arr_str_temp : String[];

private var int_popUp : int = 0;

private var str_profile : String = "";

//haxoan
var EpocManager:GameObject;

function Start () {
	
	
	
	if(rct_label_EPOC == Rect(0,0,0,0))
	{
		rct_label_EPOC = Rect(100,100,100,20);
	}
	if(rct_bt_EPOC == Rect(0,0,0,0))
	{
		rct_bt_EPOC = Rect(200,100,100,20);
	}
	if(rct_label_profile == Rect(0,0,0,0))
	{
		rct_label_profile = Rect(350,100,100,20);
	}
	if(rct_bt_profile == Rect(0,0,0,0))
	{ 
		rct_bt_profile = Rect(450,100,100,20);
	}
	if(rct_grid_button == Rect(0,0,0,0))
	{
		rct_grid_button = Rect(100,400,400,20);
	}
	if(rct_bt_accept == Rect(0,0,0,0))
	{ 
		rct_bt_accept = Rect(100,430,100,20);
	}
	if(rct_bt_reject == Rect(0,0,0,0))
	{ 
		rct_bt_reject = Rect(300,430,100,20);
	}

	if(rct_result == Rect(0,0,0,0))
	{ 
		
	}
	
	rct_popUp_EPOC = rct_bt_EPOC;
	rct_popUp_EPOC.width += 20;
	rct_popUp_EPOC.y += rct_bt_EPOC.height;
	rct_popUp_EPOC.height = 4*rct_bt_EPOC.height;
	
	rct_popUp_profile = rct_bt_profile;
	rct_popUp_profile.width += 20;
	rct_popUp_profile.y += rct_bt_profile.height;
	rct_popUp_profile.height = rct_bt_profile.height*str_label_profile.length;
	
	
}

function OnGUI () {
	
	GUI.BeginGroup(Rect(20,20,Screen.width - 40,Screen.height - 40));
	
	// Labels
	GUI.Label(rct_label_EPOC, "EPOC");
	GUI.Label(rct_label_profile, "Profile");
	
	// Buttons
	if(GUI.Button(rct_bt_accept, "SaveProfilesToFile")) OnAccept();
	if(GUI.Button(rct_bt_reject, "LoadProfilesFromFile")) OnReject();
	
	// popUp
	GUI.SetNextControlName("EPOC");
	GUI.TextArea(rct_bt_EPOC, str_label_EPOC[int_label_EPOC]);
	if(GUI.GetNameOfFocusedControl() == "EPOC") int_popUp = 1;
	
	if(int_popUp == 1)
	{
		Vct2 = GUI.BeginScrollView(rct_popUp_EPOC, Vct2, Rect(0,0,rct_popUp_EPOC.width,rct_popUp_EPOC.height));
		for(var i : int = 0; i < str_label_EPOC.Length; i++)
		{
			if(GUI.Button(Rect(0,i*rct_bt_EPOC.height,rct_bt_EPOC.width, rct_bt_EPOC.height), str_label_EPOC[i]))
			{
				int_label_EPOC = i;
				int_popUp = 0;
				GUI.FocusControl("G_bt");
				break;
			}
		}
		GUI.EndScrollView();
	}
	
	GUI.SetNextControlName("Profile");
	str_profile = GUI.TextField(rct_bt_profile, str_profile);
	if(GUI.GetNameOfFocusedControl() == "Profile") int_popUp = 2;
	
	if(int_popUp == 2)
	{
				
		var count_bt : int = 0;
		var arr_temp = new String[50];
		for(var count : int = 0; count < str_label_profile.length ; count++)
		{
			if(str_profile.Length <= str_label_profile[count].Length)
			{
				if(str_profile == str_label_profile[count].Substring(0, str_profile.Length))
				{
					arr_temp[count_bt++] = str_label_profile[count];
				}
			}
		}

		Vct2_profile = GUI.BeginScrollView(Rect(rct_popUp_profile.x,rct_popUp_profile.y,rct_popUp_profile.width, 80), Vct2_profile, Rect(0,0, rct_popUp_profile.width, count_bt*20));
		for(var j : int = 0; j < count_bt; j++)
		{
			if(GUI.Button(Rect(0,j*20,rct_bt_profile.width,20), arr_temp[j]))
				{
					str_profile = str_label_profile[j];
					int_popUp = 0;
					GUI.FocusControl("G_bt");
					break;
				}
		}
		GUI.EndScrollView();
	}
	
	if(GUI.GetNameOfFocusedControl() == "G_bt") int_popUp = 0;
	
	GUI.EndGroup();
}

function DrawResult (int_input : int) 
{
			
}


function SetResult (int_input : int)
{ 
	// function set result
	Result = int_input;
}

function OnAccept () 
{	
	EpocManager.SendMessage("SaveProfilesToFile");
}

function OnReject () 
{
	EpocManager.SendMessage("LoadProfilesFromFile");	
	for(var i=0 ; i<EmoProfileManagement.GetProfilesArraySize();i++)
	{
		str_label_profile.Add(EmoProfileManagement.GetProfileName(i));
	}
	
}

function Update () 
{
	var bl_new_profile = true;
	for(var int_temp : int = 0; int_temp < str_label_profile.length; int_temp++)
	{
		if(str_profile == str_label_profile[int_temp])
			bl_new_profile = false;
	}
	
	
	if(bl_new_profile && (str_profile != "" ))
	{
		if(Input.GetKeyUp(KeyCode.Return)||Input.GetKeyUp(KeyCode.KeypadEnter))
		{
			str_label_profile.Add(str_profile);
			EpocManager.SendMessage("AddNewProfile",str_profile);
			Debug.Log("Them profile");// add into str_label_profile[]
		}
	}
	
	
}