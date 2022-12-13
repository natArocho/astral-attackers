using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyBomb" || collision.transform.tag == "PlayerBullet")
        {

            SoundManager.S.MakeShieldBreak();

            Destroy(this.gameObject);

            if (collision.transform.tag == "EnemyBomb" || !Player.bigBullet) Destroy(collision.gameObject);

        }
    }
}
