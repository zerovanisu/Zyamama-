using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Director : MonoBehaviour
{
    [Header("制限時間を変更できるよ")]
    [SerializeField]
    private float Timmer;

    [Header("パーツ格納数")]
    [SerializeField]
    private int Point = 0;

    [Header("全体のパーツ数")]
    public int Parts_No;

    [Header("博士の勝利フラグ")]
    public bool Doctor_Win = false;

    [Header("ジャママーの勝利フラグ")]
    public bool Zyama_Win = false;

    [Header("ライフ")]
    [SerializeField]
    private int Life_Doctor, Life_Zyama;

    [Header("Life画像")]
    [SerializeField]
    Image[] Lifeimage_D,Lifeimage_Z;

    [Header("内部処理用の変数")]
    [SerializeField]
    private GameObject Generation, Doctor, Hand, Zyama, Robots;
    [SerializeField]
    private Text Count_Text;
    public bool BlockBreake;//ジャママーのブロック破壊音を鳴らすフラグ

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
        //カウントが終わっていない時
        if(Timmer >= 0)
        {
            //カウントを進める
            Timmer -= Time.deltaTime;
        }
        //カウントが終わったら
        else
        {
            //ジャママーの勝ちにする
            Zyama_Win = true;
        }

        //それぞれのライフを取得(更新)
        Life_Doctor = Doctor.GetComponent<DoctorManager>().Life_Doctor;
        Life_Zyama = Zyama.GetComponent<Jamma>().Life_Zyama;

        //パーツの格納数が全体のパーツ数になった時
        if(Point == Parts_No)
        {
            Doctor_Win = true;//博士の勝利フラグを立てる
        }

        //博士のライフが0になったら
        if(Life_Doctor <= 0)
        {
            Zyama_Win = true;
        }

        //ジャママーのライフが0になったら
        if(Life_Zyama <= 0)
        {
            Doctor_Win = true;
        }

        //オブジェクトが消えると音を鳴らせないので代わりにブロック破壊音を鳴らす
        if (BlockBreake == true)
        {
            Sound_Manager.Instance.PlaySE(SE.Break_1,0.6f,0);
            Sound_Manager.Instance.PlaySE(SE.Break_2,0.6f,0);
            BlockBreake = false;
        }
    }

    private void FixedUpdate()
    {
        if(Timmer >= 0)
        {
            //タイマーの更新
            Count_Text.text = Timmer.ToString("F0");
        }
        else
        {
            if(Doctor_Win == true)
            {
                Count_Text.text = "博士の勝ち！";
            }
            else if(Zyama_Win == true)
            {
                Count_Text.text = "ジャママーの勝ち！";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //パーツが判定に触れた時
        if(other.gameObject.tag == "Robot")
        {
            Point += 1;//格納数を加算

            Robots.GetComponent<RobotManager>().CreateRobot(other.gameObject);//他のパーツと合体させる

            //博士が何かしらのスキルを持っていた場合スキル発動音を鳴らす
            if (Doctor.GetComponent<DoctorManager>().SkillName != null && Doctor.GetComponent<DoctorManager>().SkillOn == false)
            {
                Sound_Manager.Instance.PlaySE(SE.SkillGet_D,1,0);
            }

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

            Debug.Log("博士のポイント" + Point + "/" + Parts_No);
        }
    }
}
