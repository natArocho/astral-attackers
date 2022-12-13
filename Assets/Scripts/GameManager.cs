using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState {getReady, playing, oops, gameOver, gameWon};

public class GameManager : MonoBehaviour
{
    //Signleton Declaration
    public static GameManager S;

    //Game State 
    public GameState gameState;

    //UI Variables
    public TextMeshProUGUI messageOverlay;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    //game vars
    public int lives;
    private int livesStart = 3;
    private int score;
    private int oneUpScore; //score we need for 1 up to appear 

    //attacker vars
    //blic GameObject mocherShip;
    private GameObject currentMotherShip;

    //public GameObject player;
    //private GameObject curPlayer;

    public GameObject shield;
    //private GameObject curShield;

    public GameObject ufo;
    private Ufo ufoScript;
    private bool ufoMove;
    //private GameObject curUfo;

    public GameObject oneUp;
    public GameObject bigBullet;
    public GameObject playerShield;

    public GameObject restartBtn;
    public GameObject weezer; //this is so dumb lmao

    private bool gameBeat;

    private void Awake()
    {
        gameBeat = false;
        ufoScript = ufo.GetComponent<Ufo>();
        score = 0;
        ufoMove = false;
        lives = livesStart;
        oneUpScore = 1000;
        if (GameManager.S)
        {
            //singleton exists, delete
            Destroy(this.gameObject);
        }   else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Shields.S);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.playing)
        {
            if (score >= oneUpScore) //give 1-up every 2000 points 
            {
                oneUpScore += 1000;
                createPowerUp();
            }
            if (!ufoMove)
            {
                ufoMove = true;
                ufoScript.moveUFO();
            }
        } else 
        {
            ufoMove = false;
            ufoScript.stopUFO();
        }
        if(!gameBeat) restartBtn.SetActive(gameState == GameState.gameOver);
    }

    public void createPowerUp()
    {
        int selPowUp = Random.Range(0, 3);
        GameObject powUp = null;
        switch (selPowUp)
        {
            case 0: powUp = oneUp; break;
            case 1: powUp = bigBullet; break;
            case 2: powUp = playerShield; break;
            default: powUp = oneUp; break;
        }
        GameObject extraLife = Instantiate(powUp);
        Vector3 oneUpPos = new Vector3(0, 20, 0);
        extraLife.transform.position = oneUpPos;    
    }

    public void updateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void updateLives()
    {
        lives++;
        livesText.text = "Lives: " + lives;
    }

    public void StartANewGame()
    {

        if (lives == 0)
        {
            lives = livesStart;
            score = 0;
            oneUpScore = 1000;
        }
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
        SoundManager.S.bgm.Play();

        //reset round
        ResetRound();
    }

    private void StartRound()
    {
        gameState = GameState.playing;
        currentMotherShip.GetComponent<MotherShip>().StartTheAttack();
    }

    public void ResetRound()
    {
        currentMotherShip = Instantiate(LevelManager.S.motherShip);

        gameState = GameState.getReady;

        //start get read coroutine
        StartCoroutine(GetReadyState());
    }

    private void GameOver()
    {
        messageOverlay.enabled = true;
        messageOverlay.text = "Game Over!";
        scoreText.text = "Final Score: " + score;
        gameState = GameState.gameOver;
        SoundManager.S.MakeLoseSound();
        SoundManager.S.bgm.Stop();
    }

    public void GameWon()
    {
        if(LevelManager.S.finalScene)
        {
            SoundManager.S.PlayBuddyHolly();
        }
        else SoundManager.S.MakeWinSound();
        SoundManager.S.bgm.Pause();
        StartCoroutine(LevelComplete());

    }

    public IEnumerator GetReadyState()
    {
        //turn on message 
        messageOverlay.enabled = true;
        //messageOverlay.text = "Get Ready!!!";
        messageOverlay.text = "" + LevelManager.S.levelName + "\nGet Ready!!!";

        //pause for 2 sec
        yield return new WaitForSeconds(2.0f);

        // turn off message
        messageOverlay.enabled = false;

        //start game
        StartRound();
    }

    public void PlayerDead()
    {
        SoundManager.S.StopAllSounds();
        SoundManager.S.MakeDeathSound();

        Player.bigBullet = false;
        Player.shieldUp = false;

        //remove a life 
        lives--;
        Debug.Log("Lives Left = " + lives);
        //livesText.text = "Lives: " + lives;

        //go to oops
        StartCoroutine(OopsState());
    }

    public IEnumerator OopsState()
    {
        gameState = GameState.oops;

        //stop attack
        currentMotherShip.GetComponent<MotherShip>().StopTheAttack();
        yield return new WaitForSeconds(0.1f);
        livesText.text = "Lives: " + lives;

        //Text message
        messageOverlay.enabled = true;
        messageOverlay.text = "Lives Remaining: " + lives;

        //wait
        yield return new WaitForSeconds(2.0f);

        messageOverlay.enabled= false;

        // do we continue?
        if (lives > 0) {
            SoundManager.S.bgm.UnPause();
            //ResetRound();
            LevelManager.S.RestartLevel();
        }
        else { GameOver(); }
        
    }

    public IEnumerator LevelComplete()
    {
        gameState = GameState.gameWon;
        messageOverlay.enabled = true;
        if (LevelManager.S.finalScene)
        {
            messageOverlay.text = "Game Won! Congrats!";
            scoreText.text = "Final Score: " + score;
            gameBeat = true;
        }
        else messageOverlay.text = "Round Complete!";

        yield return new WaitForSeconds(2.0f);

        if (!LevelManager.S.finalScene) LevelManager.S.RoundWin();
    }
}
