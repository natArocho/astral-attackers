using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    public static Shields S; //make shield a singleton so we can preserve it when reloading level
    private bool started;
    private void Awake()
    {
        started = false;
        if (Shields.S)
        {
            //singleton exists, delete
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    void Update()
    {
        Color blueTrans = Color.blue;
        blueTrans.a = 0f;

        if (GameManager.S.gameState == GameState.gameWon && LevelManager.S.finalScene && !started)
        {
            started = true;
            StartCoroutine(TransitionColorChildren(Color.blue, blueTrans, 12f));
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

            //Debug.Log("doing thing " + newCol);
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
}
