using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedPrefs : MonoBehaviour
{
    
    int localHighscore=0;
    float localVolumeLevel;
    public void SaveGame(){
        PlayerPrefs.SetInt("SavedInteger",localHighscore);
        PlayerPrefs.SetFloat("SavedFloat",localVolumeLevel);
        PlayerPrefs.Save();
    }
    public void LoadGame(){
        if (PlayerPrefs.HasKey("SavedInteger")&&PlayerPrefs.HasKey("SavedFloat"))
	    {
            localHighscore = PlayerPrefs.GetInt("SavedInteger");
		    localVolumeLevel = PlayerPrefs.GetFloat("SavedFloat");
        }
    }
    
    public void SetLocalHighscore(int value){
        localHighscore=value;
    }
    public void SetLocalVolumeLevel(float value){
        localVolumeLevel=value;
    }
    public int GetLocalHighscore(){
        return localHighscore;
    }
    public float GetLocalVolumeLevel(){
        return localVolumeLevel;
    }
}
