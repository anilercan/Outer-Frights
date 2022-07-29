using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed=10f;
    [SerializeField] float projectileLifetime=5f;
    [SerializeField] float baseFireRate=0.2f;
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
            Rigidbody2D rb2d=instance.GetComponent<Rigidbody2D>();
            if(rb2d!=null&&useAI==false){
                rb2d.velocity=transform.up*projectileSpeed;
                audioPlayer.PlayPlayerShootingClip();
            }
            if(rb2d!=null&&useAI==true){
                rb2d.velocity=-transform.up*projectileSpeed;
                audioPlayer.PlayEnemyShootingClip();
            }
            Destroy(instance,projectileLifetime);
            float timeToNextProjectile=Random.Range(baseFireRate-fireRateVar,baseFireRate+fireRateVar);
            timeToNextProjectile=Mathf.Clamp(timeToNextProjectile,minFireRate,float.MaxValue);
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
