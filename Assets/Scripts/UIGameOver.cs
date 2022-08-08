using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScore;
    ScoreKeeper scoreKeeper;
    void Awake(){
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
    }
    void Start()
    {
        scoreText.text="You Scored:\n"+scoreKeeper.GetScore().ToString();
        if (scoreKeeper.GetScore()>scoreKeeper.GetHighScore()){
            highScore.text="High Score:\n"+scoreKeeper.GetScore().ToString();
            scoreKeeper.SetHighScore(scoreKeeper.GetScore());
        }
        else{
            highScore.text="High Score:\n"+scoreKeeper.GetHighScore().ToString();
        }
    }
}
