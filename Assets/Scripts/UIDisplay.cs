using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Health playerHealth;

    [Header("Rocket")]
    [SerializeField] TextMeshProUGUI rocketCount;
    [SerializeField] Shooter playerShooter;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;
    Player player;
    //int score=0;
    void Awake(){
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
        player=FindObjectOfType<Player>();
    }
    void Start(){
        healthSlider.maxValue=playerHealth.GetHealth();
    }
    void Update(){
        healthSlider.value=playerHealth.GetHealth();
        rocketCount.text=playerShooter.GetRocketCount().ToString();
        scoreText.text=scoreKeeper.GetScore().ToString("000000000");
    }
}
