using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{

    public GameObject headMid;
    public GameObject headFinal;
    public GameObject core;
    public bool weakSpot;
    // Start is called before the first frame update
    void Start()
    {
        weakSpot = false;
    }

    // Update is called once per frame
    void Update()
    {
        headFinal.SetActive(weakSpot);
        headMid.SetActive(!weakSpot);
    }
}
