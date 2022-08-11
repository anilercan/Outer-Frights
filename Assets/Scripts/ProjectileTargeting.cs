using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTargeting : MonoBehaviour
{
    [SerializeField] GameObject projectileTarget=null;
    [SerializeField] float rotationSpeed=10.0f;
    bool locked=false;
    GameObject enemySpawner;
    void Awake(){
        enemySpawner=FindObjectOfType<EnemySpawner>().gameObject;
    }
    void Update(){
        if (projectileTarget!=null){
            locked=true;
            SetProjectilesVelocity();
        }
        if (projectileTarget==null&&locked==true){
            if (enemySpawner.transform.childCount!=0){
                projectileTarget=GetClosestEnemy();
            }
            else{
                Destroy(gameObject);
            }
        }
    }
    public void SetProjectileTarget(GameObject obj){
        projectileTarget=obj;
    }
    public GameObject GetProjectileTarget(){
        return projectileTarget;
    }
    
    void SetProjectilesVelocity(){
        Vector3 closestTargetPosition=projectileTarget.transform.position;
        float projectileSpeed=FindObjectOfType<Player>().GetComponent<Shooter>().GetProjectileSpeed();
        float delta=projectileSpeed*Time.deltaTime;
        foreach (Transform child in gameObject.transform){
            child.gameObject.GetComponent<Rigidbody2D>().velocity=new Vector2(0f,0f);
            child.position=Vector2.MoveTowards(child.position,closestTargetPosition,delta);
            //rotate
            Vector3 targetDirection=projectileTarget.transform.position-child.position; 
            float singleRotationStep=rotationSpeed*Time.deltaTime;
            Vector3 newDirection=Vector3.RotateTowards(child.forward, targetDirection, singleRotationStep, 0.0f);
            child.rotation=Quaternion.LookRotation(newDirection);
        }
    }
    
    public GameObject GetClosestEnemy(){
        List<GameObject> enemiesList=new List<GameObject>();
        for (int i=0;i<enemySpawner.transform.childCount;i++){
            GameObject currentChild=enemySpawner.transform.GetChild(i).gameObject;
            if (currentChild.tag=="Enemy"){
                enemiesList.Add(currentChild);
            }
        }
        GameObject closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = gameObject.transform.position;
        foreach(GameObject potentialEnemy in enemiesList)
        {
            Transform potentialTarget=potentialEnemy.transform;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy=potentialEnemy;
            }
        }
        enemiesList.Clear();
        return closestEnemy;
    }
}
