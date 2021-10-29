using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [Header("スキルIDとそのスキルの名前(色)が表示されるよ")]
    [SerializeField]
    private int SkillType;//スキルID
    public string SkillName;//スキルの名前(色)

    [Header("アイコンの色を変更できるよ")]
    [SerializeField]
    Color Blue, Yellow, Red;

    [Header("内部処理用変数～触らないでね～")]
    [SerializeField]
    GameObject Icon;
    Material IconeColor;

    private void Awake()
    {
        //スキルIDの抽選
        SkillType = Random.Range(1, 4);
    }

    // Start is called before the first frame update
    void Start()
    {
        IconeColor = Icon.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //スキルのIDに対してどのスキルかを割り当てる
        switch (SkillType)
        {
            case 0:
                SkillName = null;
                break;
            case 1:
                SkillName = "Blue";
                break;
            case 2:
                SkillName = "Yellow";
                break;
            case 3:
                SkillName = "Red";
                break;
        }
        //アイコンの色をスキルの色に変更
        TableColor();
    }

    //アイコンの色をスキルの色に変更する処理
    void TableColor()
    {
        switch (SkillName)
        {
            case "Blue":
                IconeColor.color = Blue;
                break;
            case "Yellow":
                IconeColor.color = Yellow;
                break;
            case "Red":
                IconeColor.color = Red;
                break;
        }
    }
}
