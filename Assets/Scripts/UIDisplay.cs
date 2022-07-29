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

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;
    //int score=0;
    void Awake(){
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
    }
    void Start(){
        healthSlider.maxValue=playerHealth.GetHealth();
    }
    void Update(){
        healthSlider.value=playerHealth.GetHealth();
        scoreText.text=scoreKeeper.GetScore().ToString("000000000");
    }
}