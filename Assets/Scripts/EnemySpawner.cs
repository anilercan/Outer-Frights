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
    int randomWave=0;
    List<int> wavesSeen=new List<int>();
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
                int waveIndex=0;
                if (lastWaveIsReversed==true){
                    StartCoroutine(GetRandomWaveIndex(true));
                    waveIndex=randomWave;
                    //waveIndex=GetRandomWaveIndex(true);
                    currentWave=waveConfigs[waveIndex];
                    lastWaveIsReversed=false;
                }
                else {
                    StartCoroutine(GetRandomWaveIndex(false));
                    waveIndex=randomWave;
                    //waveIndex=GetRandomWaveIndex(false);
                    currentWave=waveConfigs[waveIndex];
                    lastWaveIsReversed=true;
                }
                //wavesSeen.Add(waveIndex);
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
            wavesSeen.Clear();
            Instantiate(finalBoss,new Vector3(0,15,0),Quaternion.identity);
            ChangeBossState();
            pickupSpawner.SetPowerupSpawnTime(20f);
            timeBetweenWaves-=timeBetweenWavesReducePerLoop;
        }
        while(isLooping==true&&bossSpawned==false);
            
    }
    /*
    int GetRandomWaveIndex(bool value){
        int returnIndex=0;
        if (value==true){
            returnIndex=Random.Range(0,5);
            if (wavesSeen.Contains(returnIndex)==true){
                returnIndex=Random.Range(0,5);
                if (wavesSeen.Contains(returnIndex)==true){
                    returnIndex=Random.Range(0,5);
                    if (wavesSeen.Contains(returnIndex)==true){
                        returnIndex=Random.Range(0,5);
                        if (wavesSeen.Contains(returnIndex)==true){
                            returnIndex=Random.Range(0,5);
                        }
                    }
                }
            }
        }
        else if (value==false){
            returnIndex=Random.Range(5,10);
            if (wavesSeen.Contains(returnIndex)==true){
                returnIndex=Random.Range(5,10);
                if (wavesSeen.Contains(returnIndex)==true){
                    returnIndex=Random.Range(5,10);
                    if (wavesSeen.Contains(returnIndex)==true){
                        returnIndex=Random.Range(5,10);
                        if (wavesSeen.Contains(returnIndex)==true){
                            returnIndex=Random.Range(5,10);
                        }
                    }
                }
            }
        }
        wavesSeen.Add(returnIndex);
        return returnIndex;
    }
    */
    IEnumerator GetRandomWaveIndex(bool value){
        if (value==true){
            randomWave=Random.Range(0,5);
            while(wavesSeen.Contains(randomWave)==true){
                randomWave=Random.Range(0,5);
            }
        }
        if (value==false){
            randomWave=Random.Range(5,10);
            while(wavesSeen.Contains(randomWave)==true){
                randomWave=Random.Range(5,10);
            }
        }
        wavesSeen.Add(randomWave);
        yield return null;
    }
    public WaveConfigSO GetCurrentWave(){
        return currentWave;
    }
    public void ChangeBossState(){
        bossSpawned=!bossSpawned;
    }
}
