using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    
    void Update()
    {
        CheckTimeScale();
    }
    void CheckTimeScale(){
        if (Time.timeScale==0){
            Time.timeScale=1;
        }
    }
}
