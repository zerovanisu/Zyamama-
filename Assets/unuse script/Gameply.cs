using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameply : MonoBehaviour
{
    public GameObject ballOriginal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        CreatBall(1);
    }
    // Update is called once per frame
    void CreatBall(int coinsNum)
    {
        for ( int i = 0; i < coinsNum; i++)
        {
            GameObject BallClone = Instantiate(ballOriginal/*, new Vector3(i, ballOriginal.transform.position.y, i ), ballOriginal.transform.rotation*/);
        }
    }
}
