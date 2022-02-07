using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Director : MonoBehaviour
{
    [Header("制限時間を変更できるよ")]
    [SerializeField]
    public float Timer;

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

    [Header("勝利モーション用カメラ")]
    [SerializeField]
    GameObject Doctor_Camera, Jamama_Camera;

    [Header("デフォルトのカメラ")]
    [SerializeField]
    GameObject Main_Camera;

    [Header("シェードインが始まるまでの時間")]
    [SerializeField]
    float Victory_Time_Max;

    [Header("シェードアウトが終わってから動き始めるまでの時間")]
    [SerializeField]
    float Motion_Time_Max;

    [Header("内部処理用の変数")]
    [SerializeField]
    private GameObject Generation, Doctor, Hand, Zyama, Robots;
    [SerializeField]
    Text JudgeText, Count_Text;

    [System.NonSerialized]
    public bool Block_Breake;//ジャママーのブロック破壊音を鳴らすフラグ

    [System.NonSerialized]
    public bool Time_SKill;//ジャママーの時間加速スキルが発動しているかのフラグ

    [System.NonSerialized] 
    public float Victory_Time, Motion_Time;

    [System.NonSerialized]
    public bool GameSet;
    Animator Zamama_Anim;

    // Start is called before the first frame update
    void Start()
    {
        Generation = GameObject.Find("Generation");//パーツ格納判定を取得
        Hand = Doctor.GetComponent<DoctorManager>().Hand;//手を取得
        Parts_No = Generation.GetComponent<PlacementManager>().Parts_No;//全体のパーツ数を取得
        Doctor_Camera.SetActive(false);Jamama_Camera.SetActive(false);//勝利用カメラを切る
        Main_Camera.SetActive(true);

        GameSet = false;

        Zamama_Anim = Zyama.GetComponent<Animator>();

        Victory_Time = Victory_Time_Max;
        Motion_Time = Motion_Time_Max;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマー処理
        Count();

        //それぞれのライフを取得(更新)
        Life_Doctor = Doctor.GetComponent<DoctorManager>().Life_Doctor;
        Life_Zyama = Zyama.GetComponent<Jamma>().Life_Zyama;

        //パーツの格納数が全体のパーツ数になった時
        if(Point == Parts_No)
        {
            Doctor_Win = true;//博士の勝利フラグを立てる
        }

        //オブジェクトが消えると音を鳴らせないので代わりにブロック破壊音を鳴らす
        if (Block_Breake == true)
        {
            Sound_Manager.Instance.PlaySE(SE.Break_1,0.6f,0);
            Sound_Manager.Instance.PlaySE(SE.Break_2,0.6f,0);
            Block_Breake = false;
        }

        //タイトルに戻る
        if (Doctor_Win == true || Zyama_Win == true)
        {
            if (Input.GetButtonDown("×_Button"))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        Juge();
    }

    private void FixedUpdate()
    {
        if (Timer >= 0)
        {
            //タイマーの更新
            Count_Text.text = Timer.ToString("F0");
        }
        else
        {
            Count_Text.text = "0:00";
        }

        if (GameSet == true)
        {
            Victory();
        }
    }

    //タイマー処理
    void Count()

    {
        //カウントが終わっていない時 & ジャママーのスキルが発動していないとき
        if (Timer > 0 && Time_SKill == false)
        {
            //カウントを進める
            Timer -= Time.deltaTime;
        }
        //カウントが終わったら
        if (Timer <= 0)
        {
            //ジャママーの勝ちにする
            Zyama_Win = true;
        }
    }

    void Juge()
    {
        //博士のライフが0になったら
        if (Life_Doctor <= 0)
        {
            Zyama_Win = true;
        }

        //ジャママーのライフが0になったら
        if (Life_Zyama <= 0)
        {
            Doctor_Win = true;
        }

        if (Doctor_Win == true || Zyama_Win == true)
        {
            Zyama.GetComponent<Jamma>().Frieze = Doctor.GetComponent<DoctorManager>().Frieze = true;
            
            //遷移のカウントダウン
            Victory_Time -= Time.deltaTime;

            if(Victory_Time > 0)
            {
                JudgeText.text = "ゲームセット！";
            }
        }
    }

    public void Victory()
    {
        ////カメラ切り替え////
        Main_Camera.SetActive(false);

        //暗転直後の処理
        if(Doctor_Win == true)
        {
            Doctor_Camera.SetActive(true);
            Robots.GetComponent<RobotManager>().GameSet = true;
        }
        else if(Zyama_Win == true)
        {
            Jamama_Camera.SetActive(true);
            Zyama.transform.position = new Vector3(0, Zyama.transform.position.y, Zyama.transform.position.z);
            Zyama.transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
        }

        //しばらく間を空ける
        Motion_Time -= Time.deltaTime;

        //一定時間経過後
        if(Motion_Time > 0)
        {
            JudgeText.text = "";
        }

        //プレイヤーの処理
        if (Doctor_Win == true && Zyama_Win == false)
        {
            //博士が勝った時の挙動
            if(Motion_Time <= 0)
            {
                JudgeText.text = "博士の勝ち！";
                Robots.GetComponent<RobotManager>().Motion_On = true;
            }
        }
        else if (Zyama_Win == true && Doctor_Win == false)
        {
            //ジャママーが勝った時の挙動
            if (Motion_Time <= 0)
            {
                JudgeText.text = "ジャママーの勝ち！";
                Zyama.GetComponent<Jamma>().Am.SetBool("Win",true);
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
