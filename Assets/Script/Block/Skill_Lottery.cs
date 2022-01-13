using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lottery : MonoBehaviour
{
    [Header("ボールの加速スキルを抽選")]
    [SerializeField]
    GameObject[] Block_1;

    [Header("ボールの分身スキルを抽選")]
    [SerializeField]
    GameObject[] Block_2;

    [Header("制限時間スキルを抽選")]
    [SerializeField]
    GameObject[] Block_3;

    //各抽選機にいくつブロックが入ってるかを保存する変数
    private int Count1, Count2, Count3;
    void Awake()
    {
        //各スキルにいくつブロックがあるのかを保存
        Count1 = Block_1.Length;
        Count2 = Block_2.Length;
        Count3 = Block_3.Length;

        //抽選した番号を保存
        int PickUp1 = Random.Range(0, Count1);
        int PickUp2 = Random.Range(0, Count2);
        int PickUp3 = Random.Range(0, Count3);
        
        Block_1[PickUp1].GetComponent<BlockManager>().Skill_Number = 1;
        Block_2[PickUp2].GetComponent<BlockManager>().Skill_Number = 2;
        Block_3[PickUp3].GetComponent<BlockManager>().Skill_Number = 3;
    }
}
