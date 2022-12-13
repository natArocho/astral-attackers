using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeColors : MonoBehaviour
{
    // Start is called before the first frame update

    public bool stage;
    private bool started;
    void Start()
    {
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        Color blueTrans = Color.blue;
        blueTrans.a = 0f;
        //weezer009ccf
        Color weezerBlue = new Color();
        weezerBlue.r = 0f;
        weezerBlue.g = 156f/255f;
        weezerBlue.b = 207f/255f;

        if (GameManager.S.gameState == GameState.gameWon && LevelManager.S.finalScene && !started)
        {
            started=true;
            if (stage) StartCoroutine(TransitionColorChildren(Color.blue, blueTrans, 12f));
            else StartCoroutine(TransitionColor(this.gameObject.GetComponent<Camera>().backgroundColor, weezerBlue, 17f));
        }
    }
    public void SetAllChildColors(Color colorValue)
    {
        Renderer[] cubelist = GetComponentsInChildren<Renderer>();

        foreach (Renderer cube in cubelist)
        {
            Color newCol = new Color();
            newCol.r = cube.material.color.r;
            newCol.g = cube.material.color.g;
            newCol.b = cube.material.color.b;
            newCol.a = colorValue.a;

            //Debug.Log("doing thing "+newCol);
            cube.material.SetColor("_Color", newCol);
        }
    }

    public IEnumerator TransitionColorChildren(Color startColor, Color newColor, float fadeTime)
    {
        Color lerpedColor;
        float timer = 0.0f;

        while (timer < fadeTime)
        {
            float timeElapsed = timer / fadeTime;

            lerpedColor = Color.Lerp(startColor, newColor, timeElapsed);

            SetAllChildColors(lerpedColor);

            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }
    }

    public IEnumerator TransitionColor(Color startColor, Color newColor, float fadeTime)
    {
        Color lerpedColor;
        float timer = 0.0f;

        while (timer < fadeTime)
        {
            float timeElapsed = timer / fadeTime;

            lerpedColor = Color.Lerp(startColor, newColor, timeElapsed);

            this.gameObject.GetComponent<Camera>().backgroundColor = lerpedColor;

            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }
        yield return new WaitForSeconds(0.1f);
        GameManager.S.weezer.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            GameManager.S.weezer.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i=0; i<3; i++)
        {
            GameManager.S.weezer.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(5.0f);
        }
        GameManager.S.restartBtn.SetActive(true);
        Vector3 buttonPos = GameManager.S.restartBtn.transform.position;

        GameManager.S.restartBtn.transform.position = new Vector3(buttonPos.x, 630+450+450, buttonPos.z); //no clue why i gotta add 450 twice

    }
}
