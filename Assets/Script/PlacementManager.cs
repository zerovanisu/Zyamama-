using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlacementManager : MonoBehaviour
{
    [Header("各配置場所とパーツを格納")]
    [SerializeField]
    public GameObject[] Generation, Parts;

    [Header("作業台を格納")]
    public GameObject Table;

    [Header("パーツの数")]
    public int Parts_No;

    [Header("作業台の数")]
    [SerializeField]
    int TableNo;

    [Header("生成場所(中心)")]
    [SerializeField]
    Vector3 Range_L, Range_R;

    [Header("生成場所(範囲)")]
    [SerializeField]
    float Range_x, Range_z;

    private void Awake()
    {
        //パーツを幾つ持ってるか確認
        Parts_No = Parts.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        Parts_Create();
        Table_Create();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Parts_Create()
    {
        //配置場所とパーツの組み合わせをランダムにする
        Parts = Parts.OrderBy(a => Guid.NewGuid()).ToArray();

        //パーツを配置場所に生成
        for (int i = 0; i < Parts_No; i++)
        {
            Instantiate(Parts[i], Generation[i].transform.position, Quaternion.identity);
        }
    }

    //作業台生成
    void Table_Create()
    {
        //左側の生成
        Vector3 posL;

        posL.x = UnityEngine.Random.Range(Range_L.x + Range_x, Range_L.x + -Range_x);
        posL.z = UnityEngine.Random.Range(Range_L.z + Range_z, Range_L.z + -Range_z);
        posL.y = Table.transform.position.y;

        Instantiate(Table, posL, Quaternion.identity);

        //右側の生成
        Vector3 posR;

        posR.x = UnityEngine.Random.Range(Range_R.x + Range_x, Range_R.x + -Range_x);
        posR.z = UnityEngine.Random.Range(Range_R.z + Range_z, Range_R.z + -Range_z);
        posR.y = Table.transform.position.y;

        Instantiate(Table, posR, Quaternion.identity);
    }
}
