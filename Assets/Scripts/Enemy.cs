using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyBomb;
    public int pointValue;
    private Transform parent;
    private MotherShip ship;

    public GameObject frame0;
    public GameObject frame1;
    public GameObject explode;

    public int hitPoints;

    //explosive force
    public float explosionForce;
    public float explosionRadius;

    private static int collisionCount;

    public bool boss;

    private Renderer renderer;

    private void Awake()
    {
        frame0.SetActive(true);
        frame1.SetActive(false);
        explode.SetActive(false);
        collisionCount = 0;
    }

    private void Start()
    {
        parent = transform.parent;
        if (!boss) ship = parent.GetComponent<MotherShip>();
        renderer = GetComponent<Renderer>();
        StartCoroutine(NextStage());

    }

    private void Update()
    {
       if (collisionCount > 1)
        {
            GameManager.S.lives = GameManager.S.lives + collisionCount - 1;
            collisionCount = 0;

        }
        SetAllChildColors(GetColor());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            SoundManager.S.MakeEnemyHit();
            hitPoints--;
            if(!Player.bigBullet || hitPoints != 0) Destroy(collision.gameObject);
            if (hitPoints == 0)
            {
                // destroy myself
                Explode();

                if(!boss) ship.timeBetweenSteps -= (ship.timeBetweenSteps / ship.enemyCount);

                GameManager.S.updateScore(pointValue);

                Destroy(this.gameObject);
            }

        } 
        if (collision.transform.tag == "PlayerGround") //player ground is slightly highter than actual ground
        {
            collisionCount++;
            GameManager.S.PlayerDead();
        }
    }

    public void DropABomb()
    {
        GameObject bomb = Instantiate(enemyBomb, transform.position + Vector3.down, Quaternion.identity);

        if (ship.boss)
        {
            Enemy enemyScript = bomb.GetComponent<Enemy>();
            if (enemyScript)
            {
                enemyScript.boss = true;
                StartCoroutine(DropEnemy(bomb));
            }
        }
        
        else Destroy(bomb, 2.0f);
    }

    public IEnumerator DropEnemy(GameObject enemy) 
    {
        int debugNum = Random.Range(0, 10); 
        while (enemy)
        {
            Debug.Log("Still going"+ transform.position+" "+debugNum);
            enemy.gameObject.transform.position += 2*Vector3.down;
            yield return new WaitForSeconds(2.0f);
        }
    }

    public IEnumerator NextStage()
    {
        while (true)
        {
            SwapFrames();
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void SwapFrames()
    {
        frame0.SetActive(!frame0.activeSelf);
        frame1.SetActive(!frame1.activeSelf);
    }

    private void Explode()
    {
        frame0.SetActive(false);
        frame1.SetActive(false);
        explode.SetActive(true);
        SetAllChildColors(GetColor());
        explode.GetComponent<EnemyFadeOut>().fadeOut = true;

        explode.transform.parent = null;
        
        Vector3 explosionPoint = explode.transform.position + Vector3.back*2;

        Rigidbody[] cubes = explode.GetComponentsInChildren<Rigidbody>();    


        foreach (Rigidbody rb in cubes)
        {
            rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
        }
    }

    private Color GetColor()
    {
        Color colorVal = new Color();

       switch (hitPoints)
        {
            case 1:
                colorVal.r = 1f;
                colorVal.g = 181f/255f;
                colorVal.b = 136f/255f;
                colorVal.a = 1f;
                break;
            case 2:
                colorVal.r = 1f;
                colorVal.g = 124f/255f; ;
                colorVal.b = 45f/255f;
                colorVal.a = 1f; 
                break;
            case 3:
                colorVal.r = 212f/255f;
                colorVal.g = 47f/255f; ;
                colorVal.b = 0f;
                colorVal.a = 1f;
                break;
            default:
                colorVal = Color.red;
                break;

        }
        return colorVal;
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

        while (timer < fadeTime)
        {
            float timeElapsed = timer / fadeTime;

            lerpedColor = Color.Lerp(startColor, newColor, timeElapsed);

            //renderer.material.SetColor("_Color", lerpedColor);
            SetAllChildColors(lerpedColor);

            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }
    }
}
