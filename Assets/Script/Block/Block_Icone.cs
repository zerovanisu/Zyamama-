using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Icone : MonoBehaviour
{
    [Header("カプセル")]
    [SerializeField]
    GameObject Capsule;

    [Header("スキル番号")]
    [SerializeField]
    int Skill_No;

    [Header("アイコンの色を変更できるよ")]
    [SerializeField]
    Color Blue, Yellow, Red;

    [Header("SpriteRenderer")]
    [SerializeField]
    SpriteRenderer Sr;

    [Header("Canvas")]
    [SerializeField]
    private Canvas Icone_Canvas;

    void Start()
    {
        Icone_Canvas.worldCamera = Camera.main;

        Skill_No = Capsule.GetComponent<BlockManager>().Skill_Number;

        //スキルに合わせて色を変える
        switch (Skill_No)
        {
            case 0:
                this.gameObject.SetActive(false);
                break;
            case 1:
                Sr.color = Blue;
                break;
            case 2:
                Sr.color = Yellow;
                break;
            case 3:
                Sr.color = Red;
                break;
        }
    }
}
