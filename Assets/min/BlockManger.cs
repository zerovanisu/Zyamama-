using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManger : MonoBehaviour
{
    //[SerializeField]
   // public int SkillUp;
   public string Skillname;
    public int Skillnumber;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        switch (Skillnumber)
        {
            case 0:
                Skillname = null;
                break;
            case 1:
                Skillname = "SpeedBoost";
                break;
            case 2:
                Skillname = "MultipleBall";
                break;
            case 3:
                Skillname = "TimeFast";
                break;
        }
    }
}
