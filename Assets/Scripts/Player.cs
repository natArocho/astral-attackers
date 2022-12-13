using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float MAX_OFFSET;

    public GameObject bulletPrefab;
    public GameObject bigBulPrefab;
    public GameObject shield;

    public bool demo;

    public static bool shieldUp; //true when we grab sheild power up
    public static bool bigBullet;
    // Start is called before the first frame update
    void Start()
    {
        shieldUp = false;
        bigBullet = false;
        //Transform parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        shield.SetActive(shieldUp);
        if (demo || GameManager.S.gameState == GameState.playing)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += (Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime);

            currentPosition.x = Mathf.Clamp(currentPosition.x, -MAX_OFFSET, MAX_OFFSET);

            transform.position = currentPosition;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireBullet();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "EnemyBomb":
                if (shieldUp)
                {
                    shieldUp = false;
                    Destroy(collision.gameObject);
                }
                else
                {
                    // destroy myself
                    Destroy(this.gameObject);

                    Destroy(collision.gameObject);

                    GameManager.S.PlayerDead();
                }
                break;
            case "OneUp":
                SoundManager.S.MakeOneUp();
                GameManager.S.updateLives();

                Destroy(collision.gameObject);
                break;
            case "PlayerShield":
                SoundManager.S.MakeOneUp();
                shieldUp = true;

                Destroy(collision.gameObject);
                break;
            case "BigBullet":
                SoundManager.S.MakeOneUp();
                bigBullet = true;

                Destroy(collision.gameObject);
                break;
            default: break;
        }
    }

    void FireBullet()
    {
        GameObject curBullet = bigBullet ? bigBulPrefab : bulletPrefab;
        GameObject bullet = Instantiate(curBullet, (transform.position + Vector3.up), Quaternion.identity);

        bullet.transform.parent = transform.parent;
        bullet.GetComponent<Bullet>().demo = demo;

        Destroy(bullet, 5.0f);

    }

}
