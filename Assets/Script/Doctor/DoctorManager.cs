using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorManager : MonoBehaviour
{
	[Header("�������Ԃ��̔���")]
	public bool Frieze = false;

	[Header("���C�t����ύX�ł����")]
	public int Life_Doctor;

	[Header("���m�̈ړ����x��ύX�ł����")]
	public float Speed;
	
	[Header("�p�[�c�ɐG��Ă��邩�̔���")]
	public bool OnParts;

	[Header("�p�[�c��͂�ł��邩�̔���")]
	public bool Catching;

	[Header("��Ƒ�ɐG��Ă��邩�̔���")]
	public bool OnTable = false;

	[Header("�X�L�����������Ă��邩�̔���")]
	public bool SkillOn = false;

	[Header("�X�L���̔������Ԃ�ύX�ł����")]
	[SerializeField]
	private float Blue_Time, Yellow_Time, Red_Time;

	[Header("�������̃X�L���̎c�莞��")]
	[SerializeField]
	private float SkillTime;

	[Header("�p�[�c���H�̍�Ǝ��Ԃ�ύX�ł����")]
	[SerializeField]
	public float Create_Time;

	[SerializeField]
	GameObject[] Life;

	[Header("�����p�ϐ��`�G��Ȃ��łˁ`")]
	[SerializeField]
	private float StickSafety;//�R���g���[���[�̔����͂��ǂ��܂ŏȂ���
	public string SkillName;//��������X�L���̎�ނ��i�[����ϐ�
	public GameObject Hand;//����i�[����ϐ�
	public GameObject Parts;//�G��Ă���(�͂�ł���)�p�[�c���i�[����ϐ�
	public bool Skill_Keep;//�p�[�c�����H������
	public GameObject Zyama;//�W���}�}�[���i�[����ϐ�

	private bool Create_now;//��Ƃ��Ă��鎞��true
	public float Createnow_Time = 0;//��Ǝ��Ԃ𑪂�p�̕ϐ�
	Rigidbody rb;
	Animator Am;

	//�ŏI���͂�ۑ�����ϐ�
	Quaternion LastRotation;

	//�X�e�B�b�N���͂��i�[����ϐ�
	float Horizontal;
	float Vertical;

	Vector3 direction;//�ړ��ʂ��i�[����ϐ�

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		Am = GetComponent<Animator>();
		SkillName = null;
	}

	//���͌n�͂�����
	void Update()
	{
		//Frieze��true�̊�(�|�[�Y���ƒ�)�͑��삪�ł��Ȃ�
		if(Frieze == false)
        {
			//�X�e�B�b�N���͂��󂯎��
			Horizontal = Input.GetAxis("Horizontal_Dr");
			Vertical = Input.GetAxis("Vertical_Dr");
			
			//�ړ��ʂ̌v�Z
			direction = new Vector3(Horizontal, 0, Vertical).normalized * Speed;
			
			//�����̐؂�ւ�
			Turn();

			//�p�[�c��͂ޏ���
			Catch();

			//�p�[�c�ɐG��Ă邩���擾
			OnParts = Hand.GetComponent<DoctorHand>().OnParts;

			//�X�L���{�^���������ꂽ��
			if (Input.GetButtonDown("��_Button"))
			{
				Create();
			}
		}

		if (Life_Doctor == 1)
		{
			Destroy(Life[1]);
		}
		if (Life_Doctor == 0)
		{
			Destroy(Life[0]);
		}
	}

	//���s�n�͂�����
	void FixedUpdate()
	{
		if (Frieze == false)
		{
			//���͂��ɋ͂��ȏꍇ�͈ړ��ʂ��Ȃ���
			if (Horizontal >= StickSafety || Vertical >= StickSafety || Horizontal <= -StickSafety || Vertical <= -StickSafety)
			{
				Am.SetBool("Walk",true);

				//�ړ��ʂ�U�蓖�Ă�(���ۂɈړ�������)����
				rb.velocity = direction;
			}
			else
            {
				Am.SetBool("Walk", false);

				rb.velocity = new Vector3 (0, 0, 0);
			}
		}
		else
		{
			rb.velocity = new Vector3(0, 0, 0);
		}

		//�X�L�������s
		if (SkillOn == true)
		{
			Skill();
		}

		//��ƒ��̃t���O���I���̎�
		if (Create_now == true)
		{
			Am.SetBool("Build",true);

			//������󂯕t���Ȃ��悤�ɒ�~�t���O�𗧂Ă�
			Frieze = true;

			//��Ǝ��Ԃ̃J�E���g�_�E��
			Createnow_Time -= Time.deltaTime;

			//���Ԃ��߂�����t���O�̃��Z�b�g
			if (Createnow_Time <= 0)
			{
				Am.SetBool("Build", false);

				Create_now = false;//��ƒ��t���O
				Frieze = false;//��~�t���O
				Skill_Keep = true;//���H��p�[�c�̎擾��
			}
		}
	}

	//�����̕ύX
	void Turn()
	{
		//���͂���Ă��鎞(�ɋ͂��ȓ��͂͏Ȃ�)
		if (Horizontal >= StickSafety || Vertical >= StickSafety || Horizontal <= -StickSafety || Vertical <= -StickSafety)
		{
			//������ύX
			var direction = new Vector3(Horizontal, 0, Vertical);
			transform.localRotation = Quaternion.LookRotation(direction);

			//������������ۑ�
			LastRotation = transform.localRotation;
		}
		else
        {
			transform.localRotation = LastRotation;
		}
	}

	//�p�[�c��͂ޏ���
	void Catch()
    {
		if(Input.GetButtonDown("��_Button"))
        {
			//�G��Ă��邯�ǒ͂�ł͂��Ȃ��Ƃ�(�͂�)
			if(OnParts == true && Catching == false)
            {
				//�ǂ̃p�[�c�������Ă��邩���󂯎��
				Parts = Hand.GetComponent<DoctorHand>().Parts;

				//�p�[�c�ɒ͂�ł��锻��𑗂�
				Catching = Parts.GetComponent<PartsManager>().Catching = true;
            }
			//�͂�ł��鎞(����)
			else if(Catching == true)
			{
				//�p�[�c�̒͂�ł��锻���������(����)
				Catching = Parts.GetComponent<PartsManager>().Catching = false;

				//�����Ă�p�[�c�������Ȃ���Ԃɂ���
				Parts = null;
			}
        }

		if(Catching == true)
		{
			Am.SetBool("Hold", true);
		}
		else
        {
			Am.SetBool("Hold", false);
		}
    }

	void Create()
    {
		//�p�[�c�������Ă���A�X�L���������ł͂Ȃ��A��Ƒ�ɐG��Ă���A��Ƃ��I�����p�[�c�������Ă��Ȃ���
		if (Catching == true && SkillOn == false && OnTable == true && Skill_Keep == false)
		{
			SkillName = Hand.GetComponent<DoctorHand>().SkillName;//�X�L�����擾

			//�X�L���̔������Ԃ��擾
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
			Createnow_Time = Create_Time;//��Ɨp�̃J�E���g�_�E����ݒ�E�Đݒ�
			Create_now = true;//��ƒ��̃t���O���I���ɂ���(�I���̊Ԃ͓����Ȃ�)
		}
	}

	//�X�L���S�ʂ̏���
	void Skill()
	{
		//�X�L�����Ԃ̃J�E���g�_�E��
		SkillTime -= Time.deltaTime;

		//�����X�L���̑I��
		switch (SkillName)
        {
			case null:
				break;

			case "Blue":
				Blue_Skill();
				break;

			case "Yellow":
				Yellow_Skill();
				break;

			case "Red":
				Red_Skill();
				break;
        }
		
		//���Ԃ��߂�����X�L���t���O���I�t�ɂ���
		if (SkillTime <= 0)
		{
			SkillOn = false;
			SkillName = null;

			Zyama.GetComponent<Jamma>(). Frieze = false;
		}
	}

	void Blue_Skill()//�X�L��
	{

	}

	void Yellow_Skill()//���X�L��
    {
		Zyama.GetComponent<Jamma>().Frieze = true;
	}

	void Red_Skill()//�ԃX�L��
    {
		//���C���}�V���i�u���b�N�j���{�[�����͂˕Ԃ��悤�ɂȂ�
	}
}
