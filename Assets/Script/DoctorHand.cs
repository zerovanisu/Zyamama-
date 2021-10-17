using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorHand : MonoBehaviour
{
    [SerializeField]
    public bool OnParts = false;
    public GameObject Parts;

    Rigidbody Rb;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Robot")
        {
            OnParts = true;
            Parts = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Robot")
        {
            OnParts = false;
            Parts = null;
        }
    }
}