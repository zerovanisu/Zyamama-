using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Director : MonoBehaviour
{
    [Header("�������Ԃ�ύX�ł����")]
    [SerializeField]
    private float Timmer;

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

    [Header("���������p�̕ϐ�")]
    [SerializeField]
    private GameObject Generation, Doctor, Hand, Zyama;
    //[SerializeField]
    //private Text Count_Text;

    // Start is called before the first frame update
    void Start()
    {
        Generation = GameObject.Find("Generation");//�p�[�c�i�[������擾
        Hand = Doctor.GetComponent<DoctorManager>().Hand;//����擾
        Parts_No = Generation.GetComponent<PlacementManager>().Parts_No;//�S�̂̃p�[�c�����擾
    }

    // Update is called once per frame
    void Update()
    {
        Timmer -= Time.deltaTime;

        //���ꂼ��̃��C�t���擾(�X�V)
        Life_Doctor = Doctor.GetComponent<DoctorManager>().Life_Doctor;
        Life_Zyama = Zyama.GetComponent<Jamma>().Life_Zyama;

        //�p�[�c�̊i�[�����S�̂̃p�[�c���ɂȂ�����
        if(Point == Parts_No)
        {
            Doctor_Win = true;//���m�̏����t���O�𗧂Ă�
            Debug.Log("���m����");
        }

        //���m�̃��C�t��0�ɂȂ�����
        if(Life_Doctor <= 0)
        {
            Zyama_Win = true;
            Debug.Log("�W���}�}�[�̏���");
        }

        //�W���}�}�[�̃��C�t��0�ɂȂ�����
        if(Life_Zyama <= 0)
        {
            Doctor_Win = true;
            Debug.Log("���m�̏���");
        }
    }

    //private void FixedUpdate()
    //{
        //�^�C�}�[�̍X�V
    //    Count_Text.text = "�c�莞�� " + Timmer.ToString("F2");
   // }

    private void OnTriggerEnter(Collider other)
    {
        //�p�[�c������ɐG�ꂽ��
        if(other.gameObject.tag == "Robot")
        {
            Point += 1;//�i�[�������Z

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

            Destroy(other.gameObject);//�p�[�c������
            Debug.Log("���m�̃|�C���g" + Point + "/" + Parts_No);
        }
    }
}
