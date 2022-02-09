using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    //[SerializeField]
   // public int SkillUp;
    public string Skill_Name;

    public int Skill_Number;

    [SerializeField]
    GameObject Def, Red, Blue, Yellow;

    // Start is called before the first frame update
    void Start()
    {
        Def.SetActive(false);
        Red.SetActive(false);
        Blue.SetActive(false);
        Yellow.SetActive(false);

        switch (Skill_Number)
        {
            case 0:
                Skill_Name = null;
                Def.SetActive(true);
                break;
            case 1:
                Skill_Name = "Avatar";
                Blue.SetActive(true);
                break;
            case 2:
                Skill_Name = "MultipleBall";
                Yellow.SetActive(true);
                break;
            case 3:
                Skill_Name = "TimeFast";
                Red.SetActive(true);
                break;
        }
    }
}
