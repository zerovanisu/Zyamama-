using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("表示している画像の番号")]
    [SerializeField]
    int Number;

    [Header("表示画像")]
    [SerializeField]
    GameObject[] TitleImage;

    [Header("メッセージのtextを入れる")]
    [SerializeField]
    Text Message;

    [Header("文字の点滅速度")]
    [SerializeField]
    float TextSpeed;

    [Header("画面を暗転させるパネル")]
    [SerializeField]
    Image BlackImage;

    [Header("押してからシーンを移動させる暗転までの速度")]
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
        //説明画像切り替え
        if (Change_Scene == false)
        {
            if (Input.GetButtonDown("○_Button"))
            {
                if(Number < TitleImage.Length -1)
                {
                    Number += 1;
                    No_Change();
                }
                else
                {
                    //ゲームシーンに移行
                    Change_Scene = true;
                }
            }
            if (Input.GetButtonDown("×_Button") && Number > 0)
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

    //画像切り替え
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
            Message.text = "○ボタンでスタート";
        }
        else
        {
            Message.text = "";
        }
    }

    //文字点滅
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
