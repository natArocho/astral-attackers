using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{
    public int stepsToSide;
    public float sideStepUnits;
    public float downStepUnits;
    public float timeBetweenSteps;
    public float enemyCount;

    public bool boss;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0 && GameManager.S.gameState != GameState.gameWon) {
            StopTheAttack();
            GameManager.S.GameWon();
        }
        else if (transform.childCount == 1)
        {
            if (boss)
            {
                BossHead head = GetComponentInChildren<BossHead>();
                head.weakSpot = true;
            } else timeBetweenSteps = 0.1f; //go really fast when 1 enemy left
        }
    }

    public void StartTheAttack()
    {
        StartCoroutine(MoveMother());
        StartCoroutine(SendABomb());
    }

    public void StopTheAttack()
    {
        StopAllCoroutines();
    }

    public IEnumerator MoveMother()
    {
        Vector3 moveVector = Vector3.right * sideStepUnits;

        while(transform.childCount > 0)
        {
            for (int i = 0; i < stepsToSide; i++)
            {   
                SoundManager.S.MakeEnemySound();
        

                transform.position += moveVector;

                yield return new WaitForSeconds(timeBetweenSteps);
            }

            transform.position += Vector3.down * downStepUnits;

            moveVector *= -1.0f;

            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }

    public IEnumerator SendABomb()
    {
        float timeBetweenBombs = 1.0f;

        bool isRunning = true; 
        
        while (isRunning)
        {
            //see how many children
            int enemyCount = transform.childCount;

            if (enemyCount > 0)
            {
                //pick one
                int enemyIndex = Random.Range(0, enemyCount);

                //send bomb
                Transform thisEnemy = transform.GetChild(enemyIndex);

                Enemy enemyScript = thisEnemy.GetComponent<Enemy>();

                if (enemyScript)
                {
                    enemyScript.DropABomb();
                }
            }
            else isRunning = false;

            yield return new WaitForSeconds(timeBetweenBombs);
        }
    }
}
