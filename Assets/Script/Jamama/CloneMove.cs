using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMove : MonoBehaviour
{   
    [Header("移動速度が変更できるよ")]
    public float Speed;
    private Rigidbody rb;

    //スティック入力を格納する変数
    float Horizontal;

    Vector3 direction;//移動量を格納する変数
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         direction = new Vector3(Horizontal, 0, 0).normalized * Speed;
         rb.velocity = direction;
    }
}
