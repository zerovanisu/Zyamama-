using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifedrain : MonoBehaviour
{
    void OnCollisionEnter(Collision collider)
    {
        GameHealthControl.health -= 1;
    }
}
