using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public GameObject Hand;//�G�ꂽ����i�[����ϐ�
    public bool Catching = false;//�͂܂�Ă��邩�𔻒肷��ϐ�

    public float PosX,PosY,PosZ = 0;//��̍��W���i�[����ϐ�

    [SerializeField]
    private float High = 0;//�����グ�鍂��
    public string SkillName;//�X�L���̎�ނ��i�[����ϐ�

    Rigidbody rb;
    BoxCollider bc;
    Vector3 FarstPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        FarstPos = this.transform.position;//�����ʒu���L��
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //�ړ�����
    void Move()
    {
        //�͂܂�Ă��邩�𔻒�
        if (Catching == true)
        {
            //���W�𔽉f
            this.transform.position = Hand.transform.position;

            //��]��ʒu���ꂪ�N���Ȃ��悤�Ɏ~�߂鏈��
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            //�����������ꎞ��~
            bc.isTrigger = true;
        }
        else
        {
            if(Hand != null)
            {
                //��̏���j��
                Hand = null;
            }

            //�����������ĉғ�
            bc.isTrigger = false;

            //�ʒu���ꏈ�����Ē���
            rb.constraints = RigidbodyConstraints.FreezeRotation
                | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�G�ꂽ�̂��n�ʂ�������(���Ƃ��ꂽ��)
        if (collision.gameObject.tag == "Ground")
        {
            //�����ʒu�ɖ߂�
            this.transform.position = FarstPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Catching == true)
        {
            //�G�ꂽ�̂���Ƒ䂾������
            if (other.gameObject.tag == "CreateTable")
            {
                //�����ł���X�L���̎�ނ��L��
                SkillName = other.gameObject.GetComponent<TableManager>().SkillName;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //�G��Ă���̂��肾������
        if(other.gameObject.name == "Hand")
        {
            //��̏����擾
            Hand = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CreateTable")
        {
            SkillName = null;
        }
    }
}
