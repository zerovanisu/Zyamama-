using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Icone : MonoBehaviour
{
    [Header("�J�v�Z��")]
    [SerializeField]
    GameObject Capsule;

    [Header("�X�L���ԍ�")]
    [SerializeField]
    int Skill_No;

    [Header("�A�C�R���̐F��ύX�ł����")]
    [SerializeField]
    Color Blue, Yellow, Red, Green;

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

        //�X�L���ɍ��킹�ĐF��ς���
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
            case 4:
                Sr.color = Green;
                break;
        }
    }
}
