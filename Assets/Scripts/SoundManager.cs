using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S; //singleton def
    private AudioSource audio;
     
    public GameObject enemyHit;
    public AudioClip[] enemyAdvance;
    private int advanceIndex = 0;

    public AudioClip bulletHit;
    public AudioClip deathSound;
    public AudioClip shieldBreak;
    public AudioClip groundHit;
    public AudioClip ufo;
    public AudioClip oneUp;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip buddyHolly;

    public AudioSource bgm;

    private void Awake()
    {
        S = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeEnemySound()
    {
        if (advanceIndex >= enemyAdvance.Length) {advanceIndex = 0;}
        audio.PlayOneShot(enemyAdvance[advanceIndex], .3f);
        advanceIndex++;

    }

    public void MakeEnemyHit()
    {
        GameObject deathSound = Instantiate(enemyHit, transform);
        Destroy(deathSound, 5.0f);
    }

    public void MakeBulletHit()
    {
        audio.PlayOneShot(bulletHit, .7f);
    }

    public void MakeDeathSound()
    {
        audio.PlayOneShot(deathSound);
    }

    public void MakeShieldBreak()
    {
        audio.PlayOneShot(shieldBreak, .5f);
    }

    public void MakeGroundHit()
    {
        audio.PlayOneShot(groundHit, .5f);
    }

    public void MakeUFO()
    {
        audio.PlayOneShot(ufo, .7f);
    }

    public void MakeOneUp()
    {
        audio.PlayOneShot(oneUp, .7f);
    }

    public void MakeWinSound()
    {
        audio.PlayOneShot(win, .7f);
    }

    public void MakeLoseSound()
    {
        audio.PlayOneShot(lose, .7f);
    }

    public void PlayBuddyHolly()
    {
        audio.time = 114f;
        audio.Play();
    }
    public void StopAllSounds()
    {
        //stop ambient noise
        bgm.Pause();

        //stop all child sounds
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
