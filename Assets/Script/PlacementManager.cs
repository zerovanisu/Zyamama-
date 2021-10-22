using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] Generation, Parts;//各配置場所とパーツを格納

    public int Parts_No;//パーツの数

    private void Awake()
    {
        //パーツを幾つ持ってるか確認
        Parts_No = Parts.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        //配置場所とパーツの組み合わせをランダムにする
        Parts = Parts.OrderBy(a => Guid.NewGuid()).ToArray();

        //パーツを配置場所に生成
        for (int i = 0; i < Parts_No ; i++)
        {
            Instantiate(Parts[i],Generation[i].transform.position,Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
