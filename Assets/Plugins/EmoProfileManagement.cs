using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;


/// <summary>
/// User Profile Class
/// </summary>
public class UserProfile
{
    public Profile profile;
    public string profileName;
    public UserProfile()
    {
        profile = new Profile();
        profileName = "";
    }
}


public class EmoProfileManagement : MonoBehaviour
{
    //----------------------------------------
    EmoEngine engine = EmoEngine.Instance;
    public static int currentIndex = 0;
    static ArrayList userProfiles = new ArrayList();      
    //----------------------------------------
    
/// <summary>
/// Function to save byte array to a file
/// </summary>
/// <param name="_FileName">File name to save byte array</param>
/// <param name="_ByteArray">Byte array to save to external file</param>
/// <returns>Return true if byte array save successfully, if not return false</returns>
    public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
    {
        try
        {        
        System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        _FileStream.Write(_ByteArray, 0, _ByteArray.Length);
        _FileStream.Close();
        return true;
        }catch (Exception _Exception){
        Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());}return false;
    }
    /// <summary>
    /// Save all profiles in arraylist to file
    /// </summary>
    public void SaveProfilesToFile()
    {
        Debug.Log("Save file");
        //---------------------
        string mStr = System.IO.Directory.GetCurrentDirectory();
        if (!System.IO.Directory.Exists(mStr + @"/EmotivUserProfile"))
        {
            System.IO.Directory.CreateDirectory(mStr + @"/EmotivUserProfile");
        }
        System.IO.Directory.SetCurrentDirectory(mStr + @"/EmotivUserProfile");
        for (int i = 0; i < userProfiles.Count; i++)
            {
                UserProfile tmp = (UserProfile)userProfiles[i];
                ByteArrayToFile(tmp.profileName + ".up", tmp.profile.GetBytes());
            }
       System.IO.Directory.SetCurrentDirectory(mStr);
    }
    /// <summary>
    /// Get Profile List
    /// </summary>
    /// <returns></returns>
    public static string[] GetProfileList()
    {
        string mStr = System.IO.Directory.GetCurrentDirectory();
        if (!System.IO.Directory.Exists(mStr + @"/EmotivUserProfile"))
        {
            System.IO.Directory.CreateDirectory(mStr + @"/EmotivUserProfile");
        }
        mStr += @"/EmotivUserProfile";
        string[] strArrFiles = System.IO.Directory.GetFiles(mStr, "*.up");
        for (int i = 0; i < strArrFiles.Length; i++)
        {
            strArrFiles[i] = strArrFiles[i].Substring(0, strArrFiles[i].Length - 3);
            strArrFiles[i] = Path.GetFileName(strArrFiles[i]);            
        }
        return strArrFiles;
    }
    /// <summary>
    /// Load all profile file in EmotivUserProfile folder into arraylist
    /// </summary>
    public void LoadProfilesFromFile()
    {
        Debug.Log("Load file");
        //---------------------
        string mStr = System.IO.Directory.GetCurrentDirectory();
        if (!System.IO.Directory.Exists(mStr + @"/EmotivUserProfile"))
        {
            System.IO.Directory.CreateDirectory(mStr + @"/EmotivUserProfile");
        }
        mStr += @"/EmotivUserProfile";
        string[] strArrFiles = System.IO.Directory.GetFiles(mStr, "*.up");
        for (int i = 0; i < strArrFiles.Length; i++)
        {
            System.IO.FileStream _FileStream = new System.IO.FileStream(strArrFiles[i], System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Byte[] buffer = new Byte[_FileStream.Length];
            _FileStream.Read(buffer, 0,(int)_FileStream.Length);
            _FileStream.Close();
            engine.SetUserProfile((uint)EmoUserManagement.currentUser, buffer);
            UserProfile tmp = new UserProfile();
            tmp.profileName = strArrFiles[i].Substring(0, strArrFiles[i].Length -3);
            tmp.profileName = Path.GetFileName(tmp.profileName);
            tmp.profile = engine.GetUserProfile((uint)EmoUserManagement.currentUser);
            userProfiles.Add(tmp);
        }     
    }
    /// <summary>
    /// Set user profile , remember to save current profile before or lose it
    /// </summary>
    /// <param name="prName">Profile Name</param>
    /// <returns>Return false if no profile with this name</returns>
    public Boolean SetUserProfile(string prName)
    {
        int i = FindProfileName(prName);
        if (i != userProfiles.Count)
        {
            UserProfile tmp = (UserProfile)userProfiles[i];
            engine.SetUserProfile((uint)EmoUserManagement.currentUser, tmp.profile);
            currentIndex = i;
            Debug.Log(prName);
            EmoCognitiv.cognitivActionsEnabled = CheckCurrentProfile();
            return true;
        }
        else 
        {
            Debug.Log("Set user profile failed");
            return false; 
        }// have no profile with this name
    }
    /// <summary>
    /// Find index of profile in arraylist 
    /// </summary>
    /// <param name="prName">Profile Name</param>
    /// <returns></returns>
    int FindProfileName(string prName)
    {
        int i = 0;
        Boolean IsFound = false;
        while ((!IsFound)&&(i<userProfiles.Count))
        {
            UserProfile tmp = (UserProfile) userProfiles[i];
            if (prName == tmp.profileName)
            {
                IsFound = true;            
            }else i++;
        }
		return i;
    }
    /// <summary>
    /// Add new profile into arraylist , set this profile to current user
    /// </summary>
    /// <param name="prName"></param>
    /// <returns></returns>
    public Boolean AddNewProfile(string prName)
    {
        Debug.Log("add new profile");
        if (FindProfileName(prName) == userProfiles.Count )
        {
            if(userProfiles.Count > 0 ) SaveCurrentProfile();
            UserProfile tmp = new UserProfile();
            tmp.profileName = prName;
            currentIndex = 0;
            userProfiles.Insert(currentIndex, tmp);
            engine.SetUserProfile((uint)EmoUserManagement.currentUser, tmp.profile);
            SaveCurrentProfile();
            //check current profile
            EmoCognitiv.cognitivActionsEnabled = CheckCurrentProfile();
            return true;
        }
        else
        {
            Debug.Log("Already have this profile name");
            return false;//Already have this profile name
        }
    }
    /// <summary>
    /// Back up current profile of current user into arraylist
    /// </summary>
    public void SaveCurrentProfile()
    {
        UserProfile tmp = (UserProfile)userProfiles[currentIndex];
        tmp.profile = engine.GetUserProfile((uint)EmoUserManagement.currentUser);
        userProfiles.RemoveAt(currentIndex);
        userProfiles.Insert(currentIndex, tmp);
    }
    /// <summary>
    /// Delete a profile in arraylist
    /// </summary>
    /// <param name="prName"></param>
    /// <returns></returns>
    public Boolean DeleteProfile(string prName)
    {
        int i = FindProfileName(prName);
        if (i != userProfiles.Count + 1)
        {
            userProfiles.RemoveAt(i);
            if ( i== currentIndex)
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                }
            }
            return true;
        }
        else return false;// have no profile with this name
    }
    /// <summary>
    /// Get a profile name in arraylist
    /// </summary>
    /// <param name="profileIndex">Index of a profile</param>
    /// <returns></returns>
    public static string GetProfileName(int profileIndex)
    {
        if (profileIndex < userProfiles.Count)
        {
            UserProfile tmp = (UserProfile)userProfiles[profileIndex];
            return tmp.profileName;
        }
        else return null;
        
    }
    /// <summary>
    /// Get number of profile in arraylist
    /// </summary>
    /// <returns>Arraylist size</returns>
    public static int GetProfilesArraySize()
    {
        return userProfiles.Count;
    }
    public static void ClearProfileList()
    {
        userProfiles.Clear();
        currentIndex = 0;
    }
    void Start()
    {
    }
    //test
    //public int numOfProfile;
    //public string currentDir;
    //void Update()
    //{
    //    numOfProfile = GetProfilesArraySize();
    //    currentDir = System.IO.Directory.GetCurrentDirectory();
    //}
	public static bool[] CheckCurrentProfile()
    {
        uint temp;
        EdkDll.EE_CognitivGetActiveActions((uint)EmoUserManagement.currentUser,out temp);
        string test = Convert.ToString(temp, 2);
        Debug.Log(test);
        bool[] actionLever = new bool[EmoCognitiv.cognitivActionList.Length];
        for (int i = 0; i < EmoCognitiv.cognitivActionList.Length; i++ )
        {
            actionLever[i] = false;
        }
        for (int i = test.Length -1; i >= 0 ;i-- )
        {
            //if (test[i] == '0')
            //{
            //    actionLever[test.Length - i -1] = false;
            //}
            if (test[i] == '1')
            {
                actionLever[test.Length - i -1] = true;
            }
        }
        return actionLever;
    }
}