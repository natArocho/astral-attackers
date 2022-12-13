using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    private GameObject bullet;
    public bool demo;

    // Start is called before the first frame update
    void Start()
    {
        bullet = transform.gameObject;
        rb = GetComponent<Rigidbody>();
        rb.velocity = (Vector3.up * speed);
    }

    void Update()
    {
        //Do this so bullets from prev rounds cant destroy new enemies
        if (!demo) bullet.SetActive(GameManager.S.gameState == GameState.playing);
        rb.velocity = (Vector3.up * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "EnemyBomb":
                Transform parent = transform.parent;

                SoundManager.S.MakeBulletHit();
                GameManager.S.updateScore(25); //Give extra points if we hit a bomb

                if(!Player.bigBullet) Destroy(this.gameObject);

                Destroy(collision.gameObject);
                break;
            case "OneUp":
                SoundManager.S.MakeOneUp();
                GameManager.S.updateLives();

                if (!Player.bigBullet) Destroy(this.gameObject);
                Destroy(collision.gameObject);
                break;
            case "PlayerShield":
                SoundManager.S.MakeOneUp();
                Player.shieldUp = true;

                if (!Player.bigBullet) Destroy(this.gameObject);
                Destroy(collision.gameObject);
                break;
            case "BigBullet":
                SoundManager.S.MakeOneUp();

                if (!Player.bigBullet) Destroy(this.gameObject);
                Player.bigBullet = true;
                Destroy(collision.gameObject);
                break;
            case "BossHead":
                Destroy(this.gameObject);
                break;
            default: break;
        }
    }
}
