using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("General")]
    //[SerializeField] Player player;
    Player player;
    Shooter playerShooter;

    [Header("Projectiles")]
    [SerializeField] GameObject defaultProjectile;
    [SerializeField] GameObject powerupProjectile;
    [SerializeField] float powerupDuration=6f;
    [SerializeField] bool vShaped;
    [SerializeField] bool doubleLaser;
    [SerializeField] bool doubleDamage;
    [SerializeField] bool fasterLaser;
    [SerializeField] bool healthPickup;
    
    void Awake(){
        player=FindObjectOfType<Player>();
        if (player!=null){
            playerShooter=player.GetComponent<Shooter>();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag=="Player"){
            StartCoroutine(SetProjectile());
        }
    }
    IEnumerator SetProjectile(){
        //gameObject.SetActive(false);
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        if (vShaped==true){
            playerShooter.SetProjectile(powerupProjectile);
            SetAllFalse();
            playerShooter.vShaped=true;  
            yield return new WaitForSeconds(powerupDuration);
            playerShooter.SetProjectile(defaultProjectile);
            playerShooter.vShaped=false;
        }
        if (doubleLaser==true){ 
            playerShooter.SetProjectile(powerupProjectile);
            SetAllFalse();
            playerShooter.doubleLaser=true; 
            yield return new WaitForSeconds(powerupDuration);
            playerShooter.SetProjectile(defaultProjectile);
            playerShooter.doubleLaser=false;
        }
        if (doubleDamage==true){
            playerShooter.SetProjectile(powerupProjectile);
            SetAllFalse();
            playerShooter.doubleDamage=true;
            yield return new WaitForSeconds(powerupDuration);
            playerShooter.SetProjectile(defaultProjectile);
            playerShooter.doubleDamage=false;
        }
        if (fasterLaser==true){
            SetAllFalse();
            playerShooter.baseFireRate=0.1f;
            yield return new WaitForSeconds(powerupDuration);
            playerShooter.baseFireRate=0.2f;
        }
        if (healthPickup==true){
            if (0<player.GetComponent<Health>().GetHealth()&&player.GetComponent<Health>().GetHealth()<=70){
                player.GetComponent<Health>().TakeDamage(-30);
            }
            else if (70<player.GetComponent<Health>().GetHealth()&&player.GetComponent<Health>().GetHealth()<100){
                player.GetComponent<Health>().TakeDamage(-(100-(player.GetComponent<Health>().GetHealth())));
            }
        }
        Destroy(gameObject);
    }
    void SetAllFalse(){
        playerShooter.vShaped=false;
        playerShooter.doubleLaser=false;
        playerShooter.doubleDamage=false;
    }
    
}
