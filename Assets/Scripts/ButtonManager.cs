using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void btn_StartTheGame()
    {
        TitleBGM.S.goToGame();
        StartCoroutine(waitStart("Level01"));
    }

    public void btn_goToInstr()
    {
        Debug.Log("Going to Instr");
        StartCoroutine(waitStart("Instructions"));
    }

    public void btn_goToCredit()
    {
        Debug.Log("Going to Credits");
        StartCoroutine(waitStart("Credits"));
    }

    public void btn_quitGame()
    {
        Debug.Log("Game was quit");
        Application.Quit();
    }

    public void btn_goToTitle()
    {
        StartCoroutine(waitStart("TitleMenu"));
    }

    public IEnumerator waitStart(string scene)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(scene);
    }
}
