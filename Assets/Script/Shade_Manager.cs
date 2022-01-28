using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shade_Manager : MonoBehaviour
{
    [Header("ゲームディレクター")]
    [SerializeField]
    GameObject GameDirector;

    [Header("シェード")]
    [SerializeField]
    Image Shade;

    [Header("シェード速度")]
    [SerializeField]
    float Shade_Speed;

    float Color_R, Color_G, Color_B, Color_A;

    bool Change_In, Change_Out, Change_Start;

    void Start()
    {
        //rgbaを保存
        Color_R = Shade.color.r;
        Color_G = Shade.color.g;
        Color_B = Shade.color.b;

        Shade.color = new Color(Color_R, Color_G, Color_B, 0);

        Shade.gameObject.SetActive(false);

        Change_In = Change_Out = false;
    }

    void Update()
    {
        if(GameDirector.GetComponent<Game_Director>().Victory_Time <= 0)
        {
            if(Change_Start == false)
            {
                Shade.gameObject.SetActive(true);
                Shade.color = new Color(Color_R, Color_G, Color_B, 0);
                Change_Start = true;
            }


            if(Change_Start && Change_In == false)
            {
                Shade_In();
            }

            if(Change_Out == true)
            {
                Shade_Out();
            }
        }
    }

    void Shade_In()
    {
        if(Shade.color.a < 1)
        {
            Color_A += Shade_Speed;
        }
        else if(Shade.color.a >= 1)
        {
            Change_In = true;
            Change_Out = true;

            GameDirector.GetComponent<Game_Director>().GameSet = true;
        }

        Shade.color = new Color(Color_R,Color_G,Color_B,Color_A);
    }

    void Shade_Out()
    {
        if(Shade.color.a > 0)
        {
            Color_A -= Shade_Speed;
        }
        else
        {
            Change_Out = true;
        }

        Shade.color = new Color(Color_R, Color_G, Color_B, Color_A);
    }
}
