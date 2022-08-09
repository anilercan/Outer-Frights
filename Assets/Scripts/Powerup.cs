using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("General")]
    //[SerializeField] Player player;
    Player player;
    Shooter playerShooter;
    EnemySpawner enemySpawner;
    AudioPlayer audioPlayer;
    [Header("General")]
    [SerializeField] float powerupDuration=6f;

    [Header("Projectiles")]
    [SerializeField] GameObject defaultProjectile;
    [SerializeField] GameObject powerupProjectile;
    [Header("Powerup Type")]
    [SerializeField] bool vShaped;
    [SerializeField] bool doubleLaser;
    [SerializeField] bool doubleDamage;
    [SerializeField] bool fasterLaser;
    [SerializeField] bool healthPickup;
    [SerializeField] bool rocketPickup;
    [SerializeField] bool freezePickup;
    [SerializeField] bool explosionPickup;

    //variables used in freeze
    //int enemyCount=0;
    List<Vector2> currentVelocities=new List<Vector2>();
    List<GameObject> frozenEnemies=new List<GameObject>();
    
    void Awake(){
        audioPlayer=FindObjectOfType<AudioPlayer>();
        player=FindObjectOfType<Player>();
        if (player!=null){
            playerShooter=player.GetComponent<Shooter>();
        }
        enemySpawner=FindObjectOfType<EnemySpawner>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag=="Player"){
            audioPlayer.PlayPickupClip();
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
        if (rocketPickup==true){
            playerShooter.SetRocketCount(3);
        }
        if (freezePickup==true){
            //List<GameObject> enemiesOnScreen=new List<GameObject>();
            FreezeEnemies();
            yield return new WaitForSeconds(powerupDuration);
            UnfreezeEnemies();
        }
        if (explosionPickup==true){
            //List<GameObject> enemiesOnScreen=new List<GameObject>();
            DestroyEnemies();
        }
        Destroy(gameObject);
    }
    void DestroyEnemies(){
        for (int i=0;i<enemySpawner.transform.childCount;i++){
            GameObject currentChild=enemySpawner.transform.GetChild(i).gameObject;
            if (currentChild.tag=="Enemy"){
                currentChild.GetComponent<Health>().TakeDamage(currentChild.GetComponent<Health>().GetHealth());
            }
        }
    }
    void FreezeEnemies(){
        //int enemyCount=enemySpawner.transform.childCount;
        //Debug.Log("Child count: "+enemyCount.ToString());
        for (int i=0;i<enemySpawner.transform.childCount;i++){
            GameObject currentChild=enemySpawner.transform.GetChild(i).gameObject;
            if (currentChild.tag=="Enemy"){
                currentChild.GetComponent<Pathfinder>().SetFollowing(false);
                currentChild.GetComponent<Shooter>().ChangeFiringStatus(false);
                Rigidbody2D currentRb2d=currentChild.GetComponent<Rigidbody2D>();
                frozenEnemies.Add(currentChild);
                currentVelocities.Add(currentRb2d.velocity);
                currentRb2d.velocity=new Vector2(0f,0f);
            }
            else if (currentChild.tag=="Boss"){
                currentVelocities.Add(new Vector2(0f,0f));
            }
        }
    }
    void UnfreezeEnemies(){
        /*
        for (int i=0;i<enemyCount;i++){
            GameObject currentChild=enemySpawner.transform.GetChild(i).gameObject;
            if (currentChild.tag=="Enemy"){
                currentChild.GetComponent<Pathfinder>().SetFollowing(true);
                currentChild.GetComponent<Shooter>().ChangeFiringStatus(true);
                Rigidbody2D currentRb2d=currentChild.GetComponent<Rigidbody2D>();
                //currentVelocities.Add(currentRb2d.velocity);
                currentRb2d.velocity=currentVelocities[i];
            }
        }
        */
        for (int i=0;i<frozenEnemies.Count;i++){
            if (frozenEnemies[i]!=null){
                GameObject currentEnemy=frozenEnemies[i];
                currentEnemy.GetComponent<Pathfinder>().SetFollowing(true);
                currentEnemy.GetComponent<Shooter>().ChangeFiringStatus(true);
                currentEnemy.GetComponent<Rigidbody2D>().velocity=currentVelocities[i];
                //Rigidbody2D currentRb2d=currentEnemy.GetComponent<Rigidbody2D>();
            }
        }
        //enemyCount=0;
        currentVelocities.Clear();
    }
    void SetAllFalse(){
        playerShooter.vShaped=false;
        playerShooter.doubleLaser=false;
        playerShooter.doubleDamage=false;
    }
    
}
