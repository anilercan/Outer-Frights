using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] [Range(0f,1f)] public float masterVolume=0.05f;
    [Header("Shooting")]
    [SerializeField] AudioClip playerShootingClip;
    [SerializeField] AudioClip enemyShootingClip;
    [SerializeField] AudioClip rocketClip;
    [Header("Damage")]
    [SerializeField] AudioClip damageTakenClip;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip shieldHitClip; 
    [SerializeField] AudioClip pickupClip;
    AudioPlayer audioPlayer;
    bool muteAudio;
    bool bgmPlaying=true;
    bool volUpPressed=false;
    bool volDownPressed=false;
    float lastVolumeLevel=0f;
    SavedPrefs savedPrefs;
    void Awake(){
        audioPlayer=GetComponent<AudioPlayer>();
        savedPrefs=FindObjectOfType<LevelManager>().GetComponent<SavedPrefs>();
        savedPrefs.LoadGame();
        masterVolume=savedPrefs.GetLocalVolumeLevel();
        audioPlayer.GetComponent<AudioSource>().volume=masterVolume;
        if (masterVolume<=0f){
            bgmPlaying=false;
        }
        ManageSingleton();
    }
    void Update(){
        VolumeControl();
        MuteGame();
    }
    void ManageSingleton(){
        int instanceCount=FindObjectsOfType(GetType()).Length;
        if(instanceCount>1){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }
    

    public void PlayPlayerShootingClip(){
        PlayClip(playerShootingClip,masterVolume);
    }
    public void PlayRocketClip(){
        PlayClip(rocketClip,masterVolume);
    }
    public void PlayEnemyShootingClip(){
        PlayClip(enemyShootingClip,masterVolume);
    }
    public void PlayDamageTaken(){
        PlayClip(damageTakenClip,masterVolume);
    }
    public void PlayExplosion(){
        PlayClip(explosionClip,masterVolume/2);
    }
    public void PlayShieldHit(){
        PlayClip(shieldHitClip,masterVolume);
    }
    public void PlayPickupClip(){
        PlayClip(pickupClip,masterVolume);
    }
    void PlayClip(AudioClip clip, float volume){
        if (clip!=null){
            AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position,volume);
        }
    }



    void MuteGame(){
        if (muteAudio==true&&bgmPlaying==true){
            lastVolumeLevel=audioPlayer.GetComponent<AudioSource>().volume;
            audioPlayer.GetComponent<AudioSource>().volume=0f;
            masterVolume=0f;
            bgmPlaying=false;
        }
        else if (muteAudio==false&&bgmPlaying==false){
            audioPlayer.GetComponent<AudioSource>().volume=lastVolumeLevel;
            masterVolume=lastVolumeLevel;
            bgmPlaying=true;
        }
    }
    void VolumeControl(){
        if (volUpPressed==true){
            if (audioPlayer.GetComponent<AudioSource>().volume<1&&bgmPlaying==true){
                audioPlayer.GetComponent<AudioSource>().volume+=0.05f;
                masterVolume+=0.05f;
                savedPrefs.SetLocalVolumeLevel(masterVolume);
                savedPrefs.SaveGame();
            }
            if (audioPlayer.GetComponent<AudioSource>().volume>=0.05f&&bgmPlaying==false){
                bgmPlaying=true;
            }
            volUpPressed=false;
        }
        if (volDownPressed==true){
            if (audioPlayer.GetComponent<AudioSource>().volume>0&&bgmPlaying==true){
                audioPlayer.GetComponent<AudioSource>().volume-=0.05f;
                masterVolume-=0.05f;
                savedPrefs.SetLocalVolumeLevel(masterVolume);
                savedPrefs.SaveGame();
            }
            if (audioPlayer.GetComponent<AudioSource>().volume<=0&&bgmPlaying==false){
                bgmPlaying=false;
            }
            volDownPressed=false;
        }
    }
    void OnMute(){
        muteAudio=!muteAudio;
    }
    void OnVolumeUp(){
        volUpPressed=true;
    }
    void OnVolumeDown(){
        volDownPressed=true;
    }
}
