using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDirector : MonoBehaviour
{
    [SerializeField]
    private int Point = 0;

    public int Parts_No;
    public GameObject Generation, Doctor,Hand;
    public bool Doctor_Win = false;

    // Start is called before the first frame update
    void Start()
    {
        Generation = GameObject.Find("Generation");
        Hand = Doctor.GetComponent<DoctorManager>().Hand;
        Parts_No = Generation.GetComponent<PlacementManager>().Parts_No;//パーツ数を取得
    }

    // Update is called once per frame
    void Update()
    {
        if(Point == Parts_No)
        {
            Doctor_Win = true;
            Debug.Log("博士勝ち");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Robot")
        {
            Point += 1;

            //フラグや持ち物を代わりにリセット
            Doctor.GetComponent<DoctorManager>().Parts
                = Hand.GetComponent<DoctorHand>().Parts
                = null;
            Doctor.GetComponent<DoctorManager>().Catching
                = false;
            Hand.GetComponent<DoctorHand>().OnParts
                = false;

            Destroy(other.gameObject);
        }
    }
}
