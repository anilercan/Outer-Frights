using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health=50;
    [SerializeField] int scoreAdd=50;
    [SerializeField] ParticleSystem hitEffect;
    CameraShake cameraShake;
    [SerializeField] bool applyCameraShake;
    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;
    Boss enemyBoss;
    void Awake(){
        cameraShake=Camera.main.GetComponent<CameraShake>();
        audioPlayer=FindObjectOfType<AudioPlayer>();
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
        levelManager=FindObjectOfType<LevelManager>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        Damage damageDealer=other.GetComponent<Damage>();
        if (damageDealer!=null&&gameObject.tag!="Shield"){
            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            if (other.tag!="Rocket"&&health>0){
                audioPlayer.PlayDamageTaken();
            }
            else if (other.tag=="Rocket"&&health>0){
                audioPlayer.PlayExplosion();
            }
            ShakeCamera();
            damageDealer.Hit();
        }
        if (damageDealer!=null&&gameObject.tag=="Shield"){
            audioPlayer.PlayShieldHit();
            damageDealer.Hit();
        }
    }
    public void TakeDamage(int damage){
        health=health-damage;
        if (health<=0){
            if (gameObject.tag!="Player"){
                PlayHitEffect();
                audioPlayer.PlayExplosion();
                scoreKeeper.ModifyScore(scoreAdd);
            }
            else{
                levelManager.LoadGameOver();
            }
            if (gameObject.tag!="Turret"){
                Destroy(gameObject);
            }
            else{
                enemyBoss=FindObjectOfType<Boss>();
                enemyBoss.SetTurret(gameObject,false);
            }
        }
    }
    void PlayHitEffect(){
        if (hitEffect!=null){
            ParticleSystem instance=Instantiate(hitEffect,transform.position,Quaternion.identity);
            Destroy(instance.gameObject,instance.main.duration+instance.main.startLifetime.constantMax);
        }
    }
    void ShakeCamera(){
        if (cameraShake!=null&&applyCameraShake==true){
            cameraShake.Play();
        }
    }
    public int GetHealth(){
        return health;
    }
    public void SetHealth(int setHealth){
        health=setHealth;
    }
}
