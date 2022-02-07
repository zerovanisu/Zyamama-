using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    //[Header("�X�L��ID�Ƃ��̃X�L���̖��O(�F)���\��������")]
    [SerializeField]
    private int SkillType;//�X�L��ID
    public string SkillName;//�X�L���̖��O(�F)

    [Header("�A�C�R���̐F��ύX�ł����")]
    [SerializeField]
    Color Blue, Yellow, Red;

    [Header("���������p�ϐ��`�G��Ȃ��łˁ`")]
    [SerializeField]
    GameObject Icon;
    Material IconeColor;

    private void Awake()
    {
        //�X�L��ID�̒��I
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
        //�X�L����ID�ɑ΂��Ăǂ̃X�L���������蓖�Ă�
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
        //�A�C�R���̐F���X�L���̐F�ɕύX
        TableColor();
    }

    //�A�C�R���̐F���X�L���̐F�ɕύX���鏈��
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
