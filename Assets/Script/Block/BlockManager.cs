using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    //[SerializeField]
   // public int SkillUp;
   public string Skill_Name;
    public int Skill_Number;
    // Start is called before the first frame update
    void Start()
    {
        switch (Skill_Number)
        {
            case 0:
                Skill_Name = null;
                break;
            case 1:
                Skill_Name = "SpeedBoost";
                break;
            case 2:
                Skill_Name = "MultipleBall";
                break;
            case 3:
                Skill_Name = "TimeFast";
                break;
            case 4:
                Skill_Name = "JammaClone";
                break;
        }
    }
}
