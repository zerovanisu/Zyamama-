using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //[Header("�\�����Ă���摜�̔ԍ�")]
    [SerializeField]
    int Number;

    //[Header("�\���摜")]
    [SerializeField]
    GameObject[] TitleImage;

    [Header("���b�Z�[�W��text������")]
    [SerializeField]
    Text Message;

    [Header("�����̓_�ő��x")]
    [SerializeField]
    float TextSpeed;

    [Header("��ʂ��Ó]������p�l��")]
    [SerializeField]
    Image BlackImage;

    [Header("�����Ă���V�[�����ړ�������Ó]�܂ł̑��x")]
    [SerializeField]
    float ChangeTime;

    float Alpha,Alpha_2;
    bool A_judge;
    bool Change_Scene;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        Number = 0;
        Alpha = Message.color.a;
        No_Change();
    }

    // Update is called once per frame
    void Update()
    {
        //�����摜�؂�ւ�
        if (Change_Scene == false)
        {
            if (Input.GetButtonDown("��_Button"))
            {
                if(Number < TitleImage.Length -1)
                {
                    Number += 1;
                    No_Change();
                }
                else
                {
                    //�Q�[���V�[���Ɉڍs
                    Change_Scene = true;
                }
            }
            if (Input.GetButtonDown("�~_Button") && Number > 0)
            {
                Number -= 1;
                No_Change();
            }
        }
        
    }

    private void FixedUpdate()
    {
        A_Change();
    }

    //�摜�؂�ւ�
    void No_Change()
    {
        for (i = 0; i < TitleImage.Length; i++)
        {
            if (i == Number)
            {
                TitleImage[i].SetActive(true);
            }
            else
            {
                TitleImage[i].SetActive(false);
            }
        }

        if(Number == 0)
        {
            Message.text = "���{�^���ŃX�^�[�g";
        }
        else
        {
            Message.text = "";
        }
    }

    //�����_��
    void A_Change()
    {
        if (Change_Scene == false)
        {
            Message.color = new Color(Message.color.r, Message.color.g, Message.color.b, Alpha);

            if (Alpha >= 1)
            {
                A_judge = true;
            }
            else if (Alpha <= 0)
            {
                A_judge = false;
            }

            if (A_judge == true)
            {
                Alpha -= TextSpeed;
            }
            else
            {
                Alpha += TextSpeed;
            }
        }
        else
        {
            Alpha_2 += ChangeTime;

            BlackImage.color = BlackImage.color = new Color(BlackImage.color.r, BlackImage.color.g, BlackImage.color.b, Alpha_2);

            if (Alpha_2 >= 1)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
