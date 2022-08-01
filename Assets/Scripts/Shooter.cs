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
    [HideInInspector] public bool isFiring;

    Coroutine firingCo;
    AudioPlayer audioPlayer;
    void Awake(){
        audioPlayer=FindObjectOfType<AudioPlayer>();
    }
    void Start()
    {
        if (useAI){
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
            //Rigidbody2D rb2d=instance.GetComponent<Rigidbody2D>();
            GameObject middleBullet = instance.transform.GetChild(0).gameObject;
            if(useAI==false){ //rb2d!=null&&
                middleBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,projectileSpeed);
                //rb2d.velocity=transform.up*projectileSpeed;
                //rb2d.velocity=new Vector2(0,projectileSpeed);
                
                if (vShaped==true){
                    GameObject leftBullet = instance.transform.GetChild(1).gameObject;
                    //GameObject middleBullet = instance.transform.GetChild(1).gameObject;
                    GameObject rightBullet = instance.transform.GetChild(2).gameObject;
                    leftBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(-projectileSpeed/8,projectileSpeed);
                    //middleBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,projectileSpeed);
                    rightBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(projectileSpeed/8,projectileSpeed);
                    //Debug.Log("vshaped");
                }
                if (doubleLaser==true){
                    GameObject rightBullet = instance.transform.GetChild(1).gameObject;
                    rightBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,projectileSpeed);
                }
                audioPlayer.PlayPlayerShootingClip();
            }
            if(useAI==true){ //rb2d!=null&&
                //rb2d.velocity=-transform.up*projectileSpeed;
                middleBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-projectileSpeed);
                audioPlayer.PlayEnemyShootingClip();
            }
            Destroy(instance,projectileLifetime);
            float timeToNextProjectile=Random.Range(baseFireRate-fireRateVar,baseFireRate+fireRateVar);
            timeToNextProjectile=Mathf.Clamp(timeToNextProjectile,minFireRate,float.MaxValue);
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
    public void SetProjectile(GameObject projectile){
        projectilePrefab=projectile;
    }
}
