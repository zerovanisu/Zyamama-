using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammaClone : MonoBehaviour
{
    public float Speed;
    private Rigidbody rb;
    float Horizontal;

    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal_Ja");
    }

    private void FixedUpdate()
    {
        direction = new Vector3(Horizontal, 0, 0).normalized * Speed;
        rb.velocity = direction;
    }
}
