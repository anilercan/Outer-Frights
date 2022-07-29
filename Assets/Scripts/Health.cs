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
    void Awake(){
        cameraShake=Camera.main.GetComponent<CameraShake>();
        audioPlayer=FindObjectOfType<AudioPlayer>();
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
        levelManager=FindObjectOfType<LevelManager>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        Damage damageDealer=other.GetComponent<Damage>();
        if (damageDealer!=null){
            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            audioPlayer.PlayDamageTaken();
            ShakeCamera();
            damageDealer.Hit();
        }
    }
    public void TakeDamage(int damage){
        health=health-damage;
        if (health<=0){
            if (gameObject.tag!="Player"){
                scoreKeeper.ModifyScore(scoreAdd);
            }
            else{
                levelManager.LoadGameOver();
            }
            Destroy(gameObject);
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
}
