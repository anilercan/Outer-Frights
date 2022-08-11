using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed=10f;
    [SerializeField] float projectileLifetime=5f;
    [SerializeField] public float baseFireRate=0.2f;
    [SerializeField] public bool vShaped;
    [SerializeField] public bool doubleLaser;
    [SerializeField] public bool doubleDamage;
    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float fireRateVar=0f;
    [SerializeField] float minFireRate=0.1f;
    public bool isFiring;
    float bulletSway=0f;
    bool swayIncreasing=true;

    [Header("Player Rocket Launcher")]
    [SerializeField] int rocketCount=1;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] float rocketCooldown=2f;
    bool rocketOnCooldown=false;
    bool homingProjectiles=false;

    Coroutine firingCo;
    AudioPlayer audioPlayer;
    void Awake(){
        audioPlayer=FindObjectOfType<AudioPlayer>();
    }
    void Start()
    {
        if (useAI&&gameObject.tag!="BossBody"){
            isFiring=true;
        }
        
    }
    void Update()
    {
        Fire();
    }
    void Fire(){
        if(isFiring==true&&firingCo==null){
            firingCo=StartCoroutine(FireContinuously());
        }
        else if(isFiring==false&&firingCo!=null){
            StopCoroutine(firingCo);
            firingCo=null;
        }
    }
    IEnumerator FireContinuously(){
        while(true){
            GameObject instance=Instantiate(projectilePrefab,transform.position,Quaternion.identity);
            GameObject firstBullet = instance.transform.GetChild(0).gameObject;
            if(useAI==false){
                firstBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,projectileSpeed);
                if (vShaped==true){
                    GameObject leftBullet = instance.transform.GetChild(1).gameObject;
                    GameObject rightBullet = instance.transform.GetChild(2).gameObject;
                    leftBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(-projectileSpeed/8,projectileSpeed);
                    rightBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(projectileSpeed/8,projectileSpeed);
                }
                if (doubleLaser==true){
                    GameObject rightBullet = instance.transform.GetChild(1).gameObject;
                    rightBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,projectileSpeed);
                }
                if (homingProjectiles==true){
                    instance.GetComponent<ProjectileTargeting>().SetProjectileTarget(
                        instance.GetComponent<ProjectileTargeting>().GetClosestEnemy());
                }
                audioPlayer.PlayPlayerShootingClip();
            }
            if(useAI==true){ 
                if (gameObject.tag=="BossBody"){
                    GameObject rightBullet2 = instance.transform.GetChild(1).gameObject;
                    GameObject leftBullet1 = instance.transform.GetChild(2).gameObject;
                    GameObject leftBullet2 = instance.transform.GetChild(3).gameObject;
                    if (bulletSway<1.2f&&swayIncreasing==true){
                        bulletSway+=0.3f;
                        if (bulletSway==1.2f){
                            swayIncreasing=false;
                        }
                    }
                    else if (bulletSway>-1.2f&&swayIncreasing==false){
                        bulletSway-=0.3f;
                        if (bulletSway==-1.2f){
                            swayIncreasing=true;
                        }
                    }
                    //bulletSway=0;
                    firstBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(-projectileSpeed/5+bulletSway,-projectileSpeed);
                    rightBullet2.GetComponent<Rigidbody2D>().velocity=new Vector2(projectileSpeed/5+bulletSway,-projectileSpeed);
                    leftBullet1.GetComponent<Rigidbody2D>().velocity=new Vector2(-projectileSpeed/5+bulletSway,-projectileSpeed);
                    leftBullet2.GetComponent<Rigidbody2D>().velocity=new Vector2(projectileSpeed/5+bulletSway,-projectileSpeed);

                }
                if (gameObject.tag=="Turret"){
                    if (gameObject.name=="leftTurret"){
                        firstBullet.transform.position+=new Vector3(0.075f,0f,0f);
                    }
                    if (gameObject.name=="rightTurret"){
                        firstBullet.transform.position+=new Vector3(-0.075f,0f,0f);
                    }
                }
                if (gameObject.tag!="BossBody"){
                    firstBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-projectileSpeed);
                }
                if (gameObject.name=="Enemy 4(Clone)"){
                    GameObject rightBullet = instance.transform.GetChild(1).gameObject;
                    rightBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-projectileSpeed);
                }
                audioPlayer.PlayEnemyShootingClip();
            }
            Destroy(instance,projectileLifetime);
            float timeToNextProjectile=Random.Range(baseFireRate-fireRateVar,baseFireRate+fireRateVar);
            timeToNextProjectile=Mathf.Clamp(timeToNextProjectile,minFireRate,float.MaxValue);
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
    public float GetProjectileSpeed(){
        return projectileSpeed;
    }

    public void SetHoming(bool value){
        homingProjectiles=value;
    }
    public void FireRocket(){
        StartCoroutine(RocketLauncher());
    }
    IEnumerator RocketLauncher(){
        if (rocketCount>0&&rocketOnCooldown==false){
            rocketOnCooldown=true;
            rocketCount=rocketCount-1;
            GameObject instance=Instantiate(rocketPrefab,transform.position,Quaternion.identity);
            GameObject rocketBody = instance.transform.GetChild(0).gameObject;
            rocketBody.GetComponent<Rigidbody2D>().velocity=new Vector2(0,projectileSpeed*2/3);
            audioPlayer.PlayRocketClip();
            yield return new WaitForSeconds(rocketCooldown);
            rocketOnCooldown=false;
            Destroy(instance,projectileLifetime);
        }
    }
    public int GetRocketCount(){
        return rocketCount;
    }
    public void SetRocketCount(int value){
        rocketCount+=value;
    }
    public void SetProjectile(GameObject projectile){
        projectilePrefab=projectile;
    }
    public bool FiringStatus(){
        return isFiring;
    }
    public void ChangeFiringStatus(bool status){
        isFiring=status;
    }
    
}
