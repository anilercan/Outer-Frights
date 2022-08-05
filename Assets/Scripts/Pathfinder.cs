using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    EnemySpawner enemySpawner;
    WaveConfigSO waveConfig;
    List<Transform> waypoints;
    int waypointIndex=0;
    bool currentlyFollowing=true;
    void Awake(){
        enemySpawner=FindObjectOfType<EnemySpawner>();
    }
    void Start()
    {
        waveConfig=enemySpawner.GetCurrentWave();
        waypoints=waveConfig.GetWaypoints();
        transform.position=waypoints[waypointIndex].position;
    }

    void Update()
    {
        FollowPath();
        
    }
    void FollowPath(){
        if (waypointIndex<waypoints.Count&&currentlyFollowing==true){
            Vector3 targetPosition=waypoints[waypointIndex].position;
            float delta=waveConfig.GetMoveSpeed()*Time.deltaTime;
            transform.position=Vector2.MoveTowards(transform.position,targetPosition,delta);
            if (transform.position==targetPosition){
                waypointIndex=waypointIndex+1;
            }
        }
        else if (waypointIndex>=waypoints.Count){
            Destroy(gameObject);
        }
    }
    public void SetFollowing(bool status){
        currentlyFollowing=status;
    }
}
