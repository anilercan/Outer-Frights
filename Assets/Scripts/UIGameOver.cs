using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScore;
    ScoreKeeper scoreKeeper;
    SavedPrefs savedPrefs;
    void Awake(){
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
        savedPrefs=FindObjectOfType<LevelManager>().GetComponent<SavedPrefs>();
        savedPrefs.LoadGame();
    }
    void Start()
    {
        scoreText.text="You Scored:\n"+scoreKeeper.GetScore().ToString();
        if (scoreKeeper.GetScore()>savedPrefs.GetLocalHighscore()){
            highScore.text="High Score:\n"+scoreKeeper.GetScore().ToString();
            savedPrefs.SetLocalHighscore(scoreKeeper.GetScore());
            savedPrefs.SaveGame();
        }
        else{
            highScore.text="High Score:\n"+savedPrefs.GetLocalHighscore().ToString();
        }
    }
}
