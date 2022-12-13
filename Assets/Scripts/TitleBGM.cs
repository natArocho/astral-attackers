using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGM : MonoBehaviour
{
    public static TitleBGM S;

    private void Awake()
    {
        if (TitleBGM.S) Destroy(this.gameObject);
        else S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void goToGame()
    {
        //destroy bgm when going to start game
        Destroy(this.gameObject);
    }
}
