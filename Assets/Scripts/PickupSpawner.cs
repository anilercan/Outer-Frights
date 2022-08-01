using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    GameObject currentPickup;
    [SerializeField] List<GameObject> pickupList;
    [SerializeField] float powerupCooldown=10f;
    [SerializeField] bool isLooping;
    void Start()
    {  
        StartCoroutine(SpawnPickups());        
    }
    IEnumerator SpawnPickups(){
        do{
            int xLocation=Random.Range(-10,10);
            currentPickup=pickupList[Random.Range(0,pickupList.Count)];
            GameObject instance=Instantiate(currentPickup,new Vector2(xLocation,9f),Quaternion.identity); //transform.position
            instance.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-5);
            yield return new WaitForSeconds(powerupCooldown);
            Destroy(instance,2);
        }
        while(isLooping==true);
    }
}
