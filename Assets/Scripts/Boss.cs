using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{   
    GameObject bossMain;
    GameObject leftTurret;
    GameObject rightTurret;
    GameObject bossShield;
    Shooter mainWeapons;
    bool turretsDown=false;
    [SerializeField] float shieldDownTimer=5f;
    [SerializeField] float moveSpeed=3f;
    //EnemySpawner enemySpawner;
    void Awake(){
        bossMain = gameObject.transform.GetChild(0).gameObject;
        leftTurret = gameObject.transform.GetChild(1).gameObject;
        rightTurret = gameObject.transform.GetChild(2).gameObject;
        bossShield = gameObject.transform.GetChild(3).gameObject;
        mainWeapons=bossMain.GetComponent<Shooter>();
        //mainWeapons.isFiring=false;
        //enemySpawner=FindObjectOfType<EnemySpawner>();
        //enemySpawner.BossSpawn();
    }
    /*
    void Start(){
        mainWeapons.isFiring=false;
    }
    */

    void Update(){
        MoveBoss();
        CheckTurrets();
        CheckBossHealth();
    }

    void CheckBossHealth(){
        if (gameObject.transform.childCount<4){
            FindObjectOfType<EnemySpawner>().ChangeBossState();
            Destroy(gameObject);
        }
    }
    void CheckTurrets(){
        if (leftTurret.GetComponent<Shooter>().FiringStatus()==false&&rightTurret.GetComponent<Shooter>().FiringStatus()==false&&turretsDown==false){
            turretsDown=true;
            StartCoroutine(TurretsDown());
        }
        if (leftTurret.GetComponent<Shooter>().FiringStatus()==true||rightTurret.GetComponent<Shooter>().FiringStatus()==true||turretsDown==false){
            mainWeapons.ChangeFiringStatus(false);
        }
    }
    
    IEnumerator TurretsDown(){
        if (turretsDown==true){
            mainWeapons.ChangeFiringStatus(true);
            bossShield.SetActive(false);
            yield return new WaitForSeconds(shieldDownTimer);
            bossShield.SetActive(true);
            SetTurret(leftTurret,true);
            SetTurret(rightTurret,true);
            leftTurret.GetComponent<Health>().SetHealth(90);
            rightTurret.GetComponent<Health>().SetHealth(90);
            turretsDown=false;
        }
    }
    public void SetTurret(GameObject gameObject, bool value){
        gameObject.GetComponent<PolygonCollider2D>().enabled=value;
        gameObject.GetComponent<SpriteRenderer>().enabled=value;
        gameObject.GetComponent<Shooter>().ChangeFiringStatus(value);
        //leftTurret.GetComponent<Health>().SetHealth(90);
        //leftTurret.GetComponent<Rigidbody2D>().SetActive(value);
        //rightTurret.GetComponent<PolygonCollider2D>().enabled=value;
        //rightTurret.GetComponent<SpriteRenderer>().enabled=value;
        //rightTurret.GetComponent<Shooter>().ChangeFiringStatus();
        //rightTurret.GetComponent<Health>().SetHealth(90);
    }
    void MoveBoss(){
        Vector3 targetPosition=new Vector3 (0,5,0);
        float delta=moveSpeed*Time.deltaTime;
        transform.position=Vector2.MoveTowards(transform.position,targetPosition,delta);
        if (transform.position==targetPosition){
                moveSpeed=0f;
        }
    }
}
