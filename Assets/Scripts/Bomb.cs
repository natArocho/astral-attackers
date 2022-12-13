using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameObject bomb;
    void Start()
    {
        bomb = transform.gameObject;
    }

    void Update()
    {
        //Do this so bombs from prev rounds cant destroy blocks
        bomb.SetActive(GameManager.S.gameState == GameState.playing);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {

            SoundManager.S.MakeGroundHit();
            Destroy(this.gameObject);

        }
    }
}
