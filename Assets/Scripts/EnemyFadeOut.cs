using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFadeOut : MonoBehaviour
{
    public bool fadeOut; //want to fade out color when enemy destroyed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color redTrans = Color.red;
        redTrans.a = 0;
        if (fadeOut)
        {
            StartCoroutine(TransitionColorChildren(Color.red, redTrans, 5.0f));
            fadeOut = false;
        }
    }

    public void SetAllChildColors(Color colorValue)
    {
        Renderer[] cubelist = GetComponentsInChildren<Renderer>();

        foreach (Renderer cube in cubelist)
        {
            cube.material.SetColor("_Color", colorValue);
        }
    }

    public IEnumerator TransitionColorChildren(Color startColor, Color newColor, float fadeTime)
    {
        Color lerpedColor;
        float timer = 0.0f;
        bool red = true;

        while (timer < fadeTime)
        {
            float timeElapsed = timer / fadeTime;

            lerpedColor = Color.Lerp(startColor, newColor, timeElapsed);
            if(red)
            {
                SetAllChildColors(lerpedColor);
                red = false;
            } else
            {
                lerpedColor.r = 0;
                lerpedColor.g = 0;
                lerpedColor.b = 1;
                SetAllChildColors(lerpedColor);
                red=true;
            }

            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }

        Destroy(this.gameObject);
    }
}
