using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    
    [SerializeField] public bool gamePaused=false;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject optionsScreen;
    
    void Update(){
        CheckPauseState();
    }


    public void PauseGame(){
        if (optionsScreen.activeSelf==false){
            gamePaused=!gamePaused;
        }
    }
    
    void CheckPauseState(){
        if (gamePaused==true&&Time.timeScale==1){
            Time.timeScale=0;
            pauseScreen.SetActive(true);
        }
        else if (gamePaused==false&&Time.timeScale==0){
            Time.timeScale=1;
            pauseScreen.SetActive(false);
        }
    }
    
}
