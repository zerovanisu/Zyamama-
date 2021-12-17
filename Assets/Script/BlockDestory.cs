using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestory : MonoBehaviour
{
    public bool istouching;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(istouching == true)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ball")
        {
            istouching = true;
        }
    }
}
