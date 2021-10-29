using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDirector : MonoBehaviour
{
    [SerializeField]
    private int Point = 0;//パーツ格納数

    public int Parts_No;//全体のパーツ数
    public GameObject Generation, Doctor,Hand;//[パーツ格納場所の判定、博士、手]を格納する変数
    public bool Doctor_Win = false;//博士の勝利フラグ

    // Start is called before the first frame update
    void Start()
    {
        Generation = GameObject.Find("Generation");//パーツ格納判定を取得
        Hand = Doctor.GetComponent<DoctorManager>().Hand;//手を取得
        Parts_No = Generation.GetComponent<PlacementManager>().Parts_No;//全体のパーツ数を取得
    }

    // Update is called once per frame
    void Update()
    {
        //パーツの格納数が全体のパーツ数になった時
        if(Point == Parts_No)
        {
            Doctor_Win = true;//博士の勝利フラグを立てる
            Debug.Log("博士勝ち");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //パーツが判定に触れた時
        if(other.gameObject.tag == "Robot")
        {
            Point += 1;//格納数を加算

            //博士達のフラグや持ち物を代わりにリセット
            Doctor.GetComponent<DoctorManager>().Parts
                = Hand.GetComponent<DoctorHand>().Parts
                = null;
            Doctor.GetComponent<DoctorManager>().Catching
                = false;
            Hand.GetComponent<DoctorHand>().OnParts
                = false;

            Doctor.GetComponent<DoctorManager>().SkillOn = true;//博士のスキルを発動させる
            Doctor.GetComponent<DoctorManager>().Skill_Keep = false;//博士の加工後パーツ取得状をリセット

            Destroy(other.gameObject);//パーツを消去
            Debug.Log("博士のポイント" + Point + "/" + Parts_No);
        }
    }
}
