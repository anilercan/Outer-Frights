using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    WaveConfigSO currentWave;
    [SerializeField] List<WaveConfigSO> waveConfigs;
    [SerializeField] GameObject finalBoss;
    [SerializeField] float timeBetweenWaves=1f;
    [SerializeField] float timeBetweenWavesReducePerLoop=0.2f;
    [SerializeField] float timeBetweenLoops=3f;
    [SerializeField] int waveCount=10;
    [SerializeField] bool isLooping;
    [SerializeField] bool bossSpawned=false;
    [SerializeField] bool lastWaveIsReversed=true;
    [SerializeField] PickupSpawner pickupSpawner;
    Coroutine waveSpawning;
    /*
    void Start()
    {
        StartCoroutine(Spawner());
        
    }
    */
    void Update(){
        Spawner();
    }
    
    void Spawner(){
        if(bossSpawned==false&&waveSpawning==null){
            //yield return new WaitForSeconds(timeBetweenLoops);
            waveSpawning=StartCoroutine(SpawnWaves());
        }
        else if(bossSpawned==true&&waveSpawning!=null){
            StopCoroutine(waveSpawning);
            waveSpawning=null;
        }
        
    }
    IEnumerator SpawnWaves(){
        pickupSpawner.SetPowerupSpawnTime(10f);
        yield return new WaitForSeconds(timeBetweenLoops);
        do{
            /*
            foreach (WaveConfigSO wave in waveConfigs){
                currentWave=wave;
                for (int i=0;i<currentWave.GetEnemyCount();i++){
                    Instantiate(currentWave.GetEnemyPrefab(i),
                    currentWave.GetStartingWaypoint().position,
                    Quaternion.identity,
                    transform);
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            */
            for (int i=0;i<waveCount;i++){
                //currentWave=waveConfigs[Random.Range(0,7)];
                if (lastWaveIsReversed==true){
                    currentWave=waveConfigs[Random.Range(0,3)];
                    lastWaveIsReversed=false;   
                }
                else {
                    currentWave=waveConfigs[Random.Range(4,7)];
                    lastWaveIsReversed=true;
                }
                for (int j=0;j<currentWave.GetEnemyCount();j++){
                    Instantiate(currentWave.GetEnemyPrefab(j),
                    currentWave.GetStartingWaypoint().position,
                    Quaternion.identity,
                    transform);
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            //yield return new WaitForSeconds(timeBetweenLoops);
            Instantiate(finalBoss,new Vector3(0,15,0),Quaternion.identity);
            ChangeBossState();
            pickupSpawner.SetPowerupSpawnTime(20f);
            timeBetweenWaves-=timeBetweenWavesReducePerLoop;
        }
        while(isLooping==true&&bossSpawned==false);
            
    }
    public WaveConfigSO GetCurrentWave(){
        return currentWave;
    }
    public void ChangeBossState(){
        bossSpawned=!bossSpawned;
    }
}
