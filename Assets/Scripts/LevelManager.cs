using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay=2f;
    ScoreKeeper scoreKeeper;
    [SerializeField] Canvas mainMenuCanvas;
    [SerializeField] Canvas optionsCanvas;
    void Awake(){
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadGameLevel(){
        scoreKeeper.ResetScore();
        SceneManager.LoadScene("GameLevel");
    }
    public void LoadGameOver(){
        //SceneManager.LoadScene("GameOver");
        StartCoroutine(WaitAndLoad("GameOver",sceneLoadDelay));
    }
    public void LoadOptions(){
        //SceneManager.LoadScene("Options");
        mainMenuCanvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(true);

    }
    public void LoadMenuBack(){
        optionsCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void SetTimeScale(){
        Time.timeScale=1;
    }
    IEnumerator WaitAndLoad(string sceneName, float delay){
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

}
