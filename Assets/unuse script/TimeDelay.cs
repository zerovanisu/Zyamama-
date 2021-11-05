using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDelay : MonoBehaviour
{
    private GameObject Ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            StartCoroutine(Delay());
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);

       //UI ÇÃÅ@éûä‘
    }

}
