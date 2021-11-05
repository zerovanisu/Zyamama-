using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clone : MonoBehaviour
{
    private bool istouching;
    // Start is called before the first frame update
    void Start()
    {
        istouching = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameObject.tag)
        {
            case "Multipleball":
                if (istouching == true)
                {
                    Instantiate(this.gameObject, transform.position, Quaternion.identity);
                }
                break;
        }
    }
    
}
