using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Director : MonoBehaviour
{
    [Header("�Q�[���J�n�̃J�E���g�_�E��")]
    [SerializeField]
    float StartTime_Max;

    [Header("�ŏ��̃J�E���g�_�E���p�̃e�L�X�g")]
    [SerializeField]
    Text StartCount_Text;

    [Header("�������Ԃ�ύX�ł����")]
    [SerializeField]
    public float Timer;

    [Header("�p�[�c�i�[��")]
    [SerializeField]
    private int Point = 0;

    [Header("�S�̂̃p�[�c��")]
    public int Parts_No;

    [Header("���m�̏����t���O")]
    public bool Doctor_Win = false;

    [Header("�W���}�}�[�̏����t���O")]
    public bool Zyama_Win = false;

    [Header("���C�t")]
    [SerializeField]
    private int Life_Doctor, Life_Zyama;

    [Header("Life�摜")]
    [SerializeField]
    Image[] Lifeimage_D,Lifeimage_Z;

    [Header("�������[�V�����p�J����")]
    [SerializeField]
    GameObject Doctor_Camera, Jamama_Camera;

    [Header("�f�t�H���g�̃J����")]
    [SerializeField]
    GameObject Main_Camera;

    [Header("�V�F�[�h�C�����n�܂�܂ł̎���")]
    [SerializeField]
    float Victory_Time_Max;

    [Header("�V�F�[�h�A�E�g���I����Ă��瓮���n�߂�܂ł̎���")]
    [SerializeField]
    float Motion_Time_Max;

    [Header("�u���b�N�̐�")]
    [SerializeField]
    int BlockCount_Max;

    [Header("���������p�̕ϐ�")]
    [SerializeField]
    private GameObject Generation, Doctor, Hand, Zyama, Robots;
    [SerializeField]
    Text JudgeText, Count_Text;

    [System.NonSerialized]
    public bool Block_Breake;//�W���}�}�[�̃u���b�N�j�󉹂�炷�t���O

    [System.NonSerialized]
    public bool Time_SKill;//�W���}�}�[�̎��ԉ����X�L�����������Ă��邩�̃t���O

    [System.NonSerialized] 
    public float Victory_Time, Motion_Time;

    [System.NonSerialized]
    public bool GameSet;
    Animator Zamama_Anim;
    float StartTime;
    bool Started;
    public int BlockCount;

    // Start is called before the first frame update
    void Start()
    {
        Started = false;
        Zyama.GetComponent<Jamma>().Frieze = Doctor.GetComponent<DoctorManager>().Frieze = true;
        StartTime = StartTime_Max;
        Generation = GameObject.Find("Generation");//�p�[�c�i�[������擾
        Hand = Doctor.GetComponent<DoctorManager>().Hand;//����擾
        Parts_No = Generation.GetComponent<PlacementManager>().Parts_No;//�S�̂̃p�[�c�����擾
        Doctor_Camera.SetActive(false);Jamama_Camera.SetActive(false);//�����p�J������؂�
        Main_Camera.SetActive(true);

        GameSet = false;

        Zamama_Anim = Zyama.GetComponent<Animator>();

        Victory_Time = Victory_Time_Max;
        Motion_Time = Motion_Time_Max;

        Sound_Manager.Instance.PlayBGM(BGM.Game_BGM);
        BlockCount = BlockCount_Max;
    }

    // Update is called once per frame
    void Update()
    {
        if(Started == true)
        {
            if(StartTime >= 0)
            {
                StartTime -= Time.deltaTime;
            }
            else
            {
                StartCount_Text.gameObject.SetActive(false);
            }

            //�^�C�}�[����
            Count();
        }
        else
        {
            StartTime -= Time.deltaTime;

            if(StartTime <= 0.5f)
            {
                StartCount_Text.text = "START";
            }
            else
            {
                StartCount_Text.text = StartTime.ToString("F0");
            }

            if (StartTime <= 0)
            {
                Started = true;

                Zyama.GetComponent<Jamma>().Frieze = Doctor.GetComponent<DoctorManager>().Frieze = false;

                StartTime = 0.5f;
            }
        }

        //���ꂼ��̃��C�t���擾(�X�V)
        Life_Doctor = Doctor.GetComponent<DoctorManager>().Life_Doctor;
        Life_Zyama = Zyama.GetComponent<Jamma>().Life_Zyama;

        //�p�[�c�̊i�[�����S�̂̃p�[�c���ɂȂ�����
        if(Point == Parts_No)
        {
            Doctor_Win = true;//���m�̏����t���O�𗧂Ă�
        }

        //�I�u�W�F�N�g��������Ɖ���点�Ȃ��̂ő���Ƀu���b�N�j�󉹂�炷
        if (Block_Breake == true)
        {
            Sound_Manager.Instance.PlaySE(SE.Break_1,0.6f,0);
            Sound_Manager.Instance.PlaySE(SE.Break_2,0.6f,0);
            Block_Breake = false;
        }

        //�^�C�g���ɖ߂�
        if (Doctor_Win == true || Zyama_Win == true)
        {
            if (Input.GetButtonDown("�~_Button"))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
        
        if(BlockCount == 0)
        {
            Zyama_Win = true;
        }

        Juge();
    }

    private void FixedUpdate()
    {
        if (Timer >= 0)
        {
            //�^�C�}�[�̍X�V
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

    //�^�C�}�[����
    void Count()

    {
        //�J�E���g���I����Ă��Ȃ��� & �W���}�}�[�̃X�L�����������Ă��Ȃ��Ƃ�
        if (Timer > 0 && Time_SKill == false)
        {
            //�J�E���g��i�߂�
            Timer -= Time.deltaTime;
        }
        //�J�E���g���I�������
        if (Timer <= 0)
        {
            //�W���}�}�[�̏����ɂ���
            Zyama_Win = true;
        }
    }

    void Juge()
    {
        //���m�̃��C�t��0�ɂȂ�����
        if (Life_Doctor <= 0)
        {
            Zyama_Win = true;
        }

        //�W���}�}�[�̃��C�t��0�ɂȂ�����
        if (Life_Zyama <= 0)
        {
            Doctor_Win = true;
        }

        if (Doctor_Win == true || Zyama_Win == true)
        {
            Zyama.GetComponent<Jamma>().Frieze = Doctor.GetComponent<DoctorManager>().Frieze = true;
            
            //�J�ڂ̃J�E���g�_�E��
            Victory_Time -= Time.deltaTime;

            if(Victory_Time > 0)
            {
                JudgeText.text = "�Q�[���Z�b�g�I";
            }
        }
    }

    public void Victory()
    {
        ////�J�����؂�ւ�////
        Main_Camera.SetActive(false);

        //�Ó]����̏���
        if(Doctor_Win == true)
        {
            Doctor_Camera.SetActive(true);
            Robots.GetComponent<RobotManager>().GameSet = true;
            Robots.transform.position = new Vector3(0, 0, 2);
            Doctor.transform.position = new Vector3(0, -2, 0);
        }
        else if(Zyama_Win == true)
        {
            Jamama_Camera.SetActive(true);
            Zyama.transform.position = new Vector3(0, Zyama.transform.position.y, Zyama.transform.position.z);
            Zyama.transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
        }

        //���΂炭�Ԃ��󂯂�
        Motion_Time -= Time.deltaTime;

        //��莞�Ԍo�ߌ�
        if(Motion_Time > 0)
        {
            JudgeText.text = "";
        }

        //�v���C���[�̏���
        if (Doctor_Win == true && Zyama_Win == false)
        {
            //���m�����������̋���
            if(Motion_Time <= 0)
            {
                JudgeText.text = "���m�̏����I";
                Robots.GetComponent<RobotManager>().Motion_On = true;
            }
        }
        else if (Zyama_Win == true && Doctor_Win == false)
        {
            //�W���}�}�[�����������̋���
            if (Motion_Time <= 0)
            {
                JudgeText.text = "�W���}�}�[�̏����I";
                Zyama.GetComponent<Jamma>().Am.SetBool("Win",true);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //�p�[�c������ɐG�ꂽ��
        if(other.gameObject.tag == "Robot")
        {
            Point += 1;//�i�[�������Z

            Robots.GetComponent<RobotManager>().CreateRobot(other.gameObject);//���̃p�[�c�ƍ��̂�����

            //���m����������̃X�L���������Ă����ꍇ�X�L����������炷
            if (Doctor.GetComponent<DoctorManager>().SkillName != null && Doctor.GetComponent<DoctorManager>().SkillOn == false)
            {
                Sound_Manager.Instance.PlaySE(SE.SkillGet_D,1,0);
            }

            //���m�B�̃t���O�⎝���������Ƀ��Z�b�g
            Doctor.GetComponent<DoctorManager>().Parts
                = Hand.GetComponent<DoctorHand>().Parts
                = null;
            Doctor.GetComponent<DoctorManager>().Catching
                = false;
            Hand.GetComponent<DoctorHand>().OnParts
                = false;

            Doctor.GetComponent<DoctorManager>().SkillOn = true;//���m�̃X�L���𔭓�������
            Doctor.GetComponent<DoctorManager>().Skill_Keep = false;//���m�̉��H��p�[�c�擾������Z�b�g

            //Debug.Log("���m�̃|�C���g" + Point + "/" + Parts_No);
        }
    }
}
