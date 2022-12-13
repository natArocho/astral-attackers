using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    public GameObject explosion;
    public GameObject ufoModel;
    private BoxCollider ufoHitbox;
    private int POINT_VAL = 200;
    private Vector3 startPos;
    private float speed = 10.0f;
    private bool notHit;

    // Start is called before the first frame update
    void Start()
    {   
        ufoHitbox = GetComponent<BoxCollider>();
        notHit = true;
        explosion.SetActive(false);
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void moveUFO()
    {
        StartCoroutine(waitForUFO());
    }

    public void stopUFO()
    {
        notHit = true;
        explosion.SetActive(false);
        ufoModel.SetActive(true);
        ufoHitbox.enabled = true;
        transform.position = startPos;
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            // destroy myself
            explosion.SetActive(true);
            ufoModel.SetActive(false);
            ufoHitbox.enabled = false;

            SoundManager.S.MakeEnemyHit();

            GameManager.S.updateScore(POINT_VAL);

            if(!Player.bigBullet) Destroy(collision.gameObject);

            notHit = false;


        }
    }

    public IEnumerator waitForUFO()
    {
        yield return new WaitForSeconds(8.0f); // wait a bit before UFO appears
        SoundManager.S.MakeUFO();
        while (notHit)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += (speed * Time.deltaTime);

            transform.position = currentPosition;
            yield return new WaitForSeconds(.01f);
        }
    }
}
