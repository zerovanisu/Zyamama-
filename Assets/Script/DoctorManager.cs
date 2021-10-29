using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorManager : MonoBehaviour
{
	[Header("動ける状態かの判定")]
	public bool Frieze = false;

	[Header("博士の移動速度を変更できるよ")]
	public float Speed;
	
	[Header("パーツに触れているかの判定")]
	public bool OnParts;

	[Header("パーツを掴んでいるかの判定")]
	public bool Catching;

	[Header("スキルが発動しているかの判定")]
	public bool SkillOn = false;

	[Header("スキルの発動時間を変更できるよ")]
	[SerializeField]
	private float Blue_Time, Yellow_Time, Red_Time;

	[Header("発動中のスキルの残り時間")]
	[SerializeField]
	private float SkillTime = 0;

	[Header("パーツ加工の作業時間を変更できるよ")]
	[SerializeField]
	private float Create_Time = 0;

	[Header("処理用変数～触らないでね～")]
	public string SkillName;//発動するスキルの種類を格納する変数
	public GameObject Hand;//手を格納する変数
	public GameObject Parts;//触れている(掴んでいる)パーツを格納する変数
	public bool Skill_Keep;//パーツを加工したか
	private bool Create_now;//作業している時はtrue
	private float Createnow_Time = 0;//作業時間を測る用の変数
	Rigidbody rb;

	//スティック入力を格納する変数
	float Horizontal;
	float Vertical;

	Vector3 direction;//移動量を格納する変数

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	//入力系はこっち
	void Update()
	{
		//Friezeがtrueの間(ポーズや作業中)は操作ができない
		if(Frieze == false)
        {
			//スティック入力を受け取る
			Horizontal = Input.GetAxis("Horizontal_Dr");
			Vertical = Input.GetAxis("Vertical_Dr");
			Move();
			Turn();
			Catch();

			//パーツに触れてるかを取得
			OnParts = Hand.GetComponent<DoctorHand>().OnParts;

			//スキルボタンを押されたら
			if (Input.GetButtonDown("X_Button"))
			{
				//パーツを持っている、スキル発動中ではない、作業を終えたパーツを持っていない時
				if (Catching == true && SkillOn == false && Skill_Keep == false)
				{
					SkillName = Hand.GetComponent<DoctorHand>().SkillName;//スキルを取得
					
					//スキルの発動時間を取得
					switch (SkillName)
					{
						case "Blue":
							SkillTime = Blue_Time;
							break;

						case "Yellow":
							SkillTime = Yellow_Time;
							break;

						case "Red":
							SkillTime = Red_Time;
							break;
					}

					Createnow_Time = Create_Time;//作業用のカウントダウンを設定・再設定
					Create_now = true;//作業中のフラグをオンにする(オンの間は動けない)
				}
			}
		}
	}

	//実行系はこっち
	void FixedUpdate()
	{
		//移動量を振り当てる(実際に移動させる)処理
		rb.velocity = direction;

		//スキルを実行
		if(SkillOn == true)
        {
			Skill();
        }

		//作業中のフラグがオンの時
		if (Create_now == true)
		{
			//動作を受け付けないように停止フラグを立てる
			Frieze = true;

			//作業時間のカウントダウン
			Createnow_Time -= Time.deltaTime;

			//時間が過ぎたらフラグのリセット
			if (Createnow_Time <= 0)
			{
				Create_now = false;//作業中フラグ
				Frieze = false;//停止フラグ
				Skill_Keep = true;//加工後パーツの取得状況
			}
		}
	}

	//移動量の計算
	void Move()
    {
		direction = new Vector3(Horizontal, 0, Vertical).normalized * Speed;
	}

	//向きの変更
	void Turn()
	{
		if (Horizontal != 0 || Vertical != 0)
		{
			var direction = new Vector3(Horizontal, 0, Vertical);
			transform.localRotation = Quaternion.LookRotation(direction);
		}
	}

	//パーツを掴む処理
	void Catch()
    {
		if(Input.GetButtonDown("A_Button"))
        {
			//触れているけど掴んではいないとき(掴む)
			if(OnParts == true && Catching == false)
            {
				//どのパーツを持っているかを受け取る
				Parts = Hand.GetComponent<DoctorHand>().Parts;

				//パーツに掴んでいる判定を送る
				Catching = Parts.GetComponent<RobotManager>().Catching = true;
            }
			//掴んでいる時(離す)
			else if(Catching == true)
            {
				//パーツの掴んでいる判定を取り消す(離す)
				Catching = Parts.GetComponent<RobotManager>().Catching = false;

				//持ってるパーツを何もない状態にする
				Parts = null;
			}
        }
    }

	//スキル全般の処理
	void Skill()
	{
		//スキル時間のカウントダウン
		SkillTime -= Time.deltaTime;

		//発動スキルの選別
		switch (SkillName)
        {
			case null:
				break;

			case "Blue":
				Bule_Skill();
				break;

			case "Yellow":
				Yellow_Skill();
				break;

			case "Red":
				Red_Skill();
				break;
        }
		
		//時間が過ぎたらスキルフラグをオフにする
		if (SkillTime <= 0)
		{
			SkillOn = false;
			SkillName = null;
		}
	}

	void Bule_Skill()//青スキル
	{
		//ジャママーのボールを打ち返すことが出来る
	}

	void Yellow_Skill()//黄スキル
    {
		//ジャママーの動きを止めることが出来る
	}

	void Red_Skill()//赤スキル
    {
		//メインマシン（ブロック）がボールをはね返すようになる
	}
}
