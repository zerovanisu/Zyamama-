using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorHand : MonoBehaviour
{
    [SerializeField]
    public bool OnParts = false;//パーツに触れているかの判定
    public GameObject Parts;//触れているパーツを格納する変数
    public string SkillName;
    public GameObject Doctor;

    Rigidbody Rb;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //パーツを持っていない時は持っていない判定にする
        if(Parts == null)
        {
            OnParts = false;
        }
    }

    //パーツに触れている時、どのパーツに触れているか
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Robot")
        {
            //触れている判定に切り替える
            OnParts = true;

            //触れているパーツを格納
            Parts = other.gameObject;

            SkillName = other.gameObject.GetComponent<RobotManager>().SkillName;
        }
        if(other.gameObject.tag == "CreateTable")
        {
            //作業台に触れている判定に書き換える
            Doctor.GetComponent<DoctorManager>().OnTable = true;
        }
    }

    //パーツを離した時
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Robot")
        {
            //取得パーツを空(何も持っていない状態)にする
            Parts = null;
        }

        if (other.gameObject.tag == "CreateTable")
        {
            //作業台に触れていない判定に書き換える
            Doctor.GetComponent<DoctorManager>().OnTable = false;
        }
    }
}
