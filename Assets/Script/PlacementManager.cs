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
    
    public GameObject Table;
    Vector3 TableSize;
    [SerializeField]
    int TableNo;

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

    void Table_Create()
    {
        //作業台の半径を代入
        TableSize = Table.transform.localScale / 2;

        //作業台を生成
        for (int i = 0; i < TableNo; i++)
        {
            Vector3 pos = UnityEngine.Random.insideUnitCircle * 6;
            pos.z = pos.y;
            pos.y = TableSize.y;

            if (!Physics.CheckBox(pos, TableSize, Quaternion.identity, 1 << 12))
            {
                Instantiate(Table, pos, Quaternion.identity);
                break;
            }
        }
    }
}
