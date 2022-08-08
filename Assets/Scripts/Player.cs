using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector2 rawInput;
    Vector2 minBounds;
    Vector2 maxBounds;
    Shooter shooter;
    [SerializeField] float moveSpeed=5f;
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingUp;
    [SerializeField] float paddingDown;
    [SerializeField] bool gamePaused=false;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject optionsScreen;
    void Awake(){
        shooter=GetComponent<Shooter>();
    }
    void Start(){
        InitBounds();
    }
    void Update(){
        Move();
        CheckPauseState();
    }
    void Move(){
        Vector2 delta=rawInput*moveSpeed*Time.deltaTime;
        Vector2 newPos=new Vector2();
        newPos.x=Mathf.Clamp(transform.position.x+delta.x,minBounds.x+paddingLeft,maxBounds.x-paddingRight);
        newPos.y=Mathf.Clamp(transform.position.y+delta.y,minBounds.y+paddingDown,maxBounds.y-paddingUp);
        transform.position=newPos;
    }
    
    void OnMove(InputValue value){
        if (gamePaused==false){
            rawInput=value.Get<Vector2>();
        }
    }
    void OnFire(InputValue value){
        if (shooter!=null&&gamePaused==false){
            shooter.isFiring=value.isPressed;
        }
    }
    void OnRocket(){ //InputValue value
        if (gamePaused==false){
            shooter.FireRocket();
        }
    }
    void OnPause(){
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
    void InitBounds(){
        Camera mainCamera=Camera.main;
        minBounds=mainCamera.ViewportToWorldPoint(new Vector2(0,0));
        maxBounds=mainCamera.ViewportToWorldPoint(new Vector2(1,1));
    }
}
