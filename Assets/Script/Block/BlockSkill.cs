using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSkill : MonoBehaviour
{
    [SerializeField]
    public GameObject[] tag1_Objects;
    [SerializeField]
    public int Skillnumber;
    public int chooseNum;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            chooseNum = Random.Range(0, tag1_Objects.Length - 1);
            tag1_Objects[chooseNum].GetComponent<BlockManager>().Skill_Number = i;
        }

    }

    void Update()
    {

    }
}