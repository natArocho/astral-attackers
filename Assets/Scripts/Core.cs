using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    // Start is called before the first frame update
    private int hitPoints;
    private int pointValue;
    public GameObject explosion;
    public Material pulseBlue;
    private Color pulseColor;
    private bool pulser;
    void Start()
    {
        hitPoints = 3;
        pointValue = 300;
        pulseColor = pulseBlue.color;
        StartCoroutine(pulse());
        pulser = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            SoundManager.S.MakeEnemyHit();
            hitPoints--;
            pulser = true;
            if (!Player.bigBullet || hitPoints != 0) Destroy(collision.gameObject);
            if (hitPoints == 0)
            {
                // destroy myself
                explosion.SetActive(true);

                GameManager.S.updateScore(pointValue);

                Destroy(this.transform.parent.parent.gameObject);
                Destroy(this.transform.parent.gameObject);
                Destroy(this.gameObject);
            }

        }
        else if (collision.transform.tag == "PlayerGround") //player ground is slightly highter than actual ground
        {
            GameManager.S.PlayerDead();
        }
    }

    public IEnumerator pulse()
    {
        Color color = pulseColor;
        while (true)
        {
            Debug.Log(color);
            SetAllChildColors(color);
            yield return new WaitForSeconds(1.0f);
            SetAllChildColors(Color.red);
            yield return new WaitForSeconds(0.5f);
            if (pulser)
            {
                float timer = 0f;
                while (timer < 2.0f)
                {
                    Debug.Log(color);
                    SetAllChildColors(color);
                    yield return new WaitForSeconds(0.2f);
                    SetAllChildColors(Color.red);
                    yield return new WaitForSeconds(0.2f);
                    timer += Time.deltaTime;
                }
                pulser = false;
            }
        }

    }

    public void SetAllChildColors(Color colorValue)
    {
        Renderer[] cubelist = GetComponentsInChildren<Renderer>();

        foreach (Renderer cube in cubelist)
        {
            //Debug.Log("doing thing " + newCol);
            cube.material.SetColor("_Color", colorValue);
        }
    }
}
