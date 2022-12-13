using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager S;
    
    [Header ("Level Info")]
    public string levelName;
    public GameObject motherShip;

    [Header ("Scene Info")]
    public string nextScene;
    public bool finalScene;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        if (GameManager.S)
        {
            GameManager.S.restartBtn.GetComponentInChildren<Button>().onClick.AddListener(ReturnToMainMenu);
            GameManager.S.StartANewGame();
        }
    }

    public void RoundWin()
    {
        Destroy(Shields.S.gameObject);
        Shields.S = null;
        SceneManager.LoadScene(nextScene);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(waitStart());
    }

    public IEnumerator waitStart()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(GameManager.S.gameObject);
        Destroy(Shields.S.gameObject);
        SceneManager.LoadScene("TitleMenu");
    }
}
