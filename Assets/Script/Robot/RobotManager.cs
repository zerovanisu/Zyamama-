using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [Header("各パーツを入れてね")]
    [SerializeField]
    public GameObject Head, Arm_L, Arm_R, Leg_L, Leg_R;

    // Start is called before the first frame update
    void Start()
    {
        //最初に各パーツを非表示にする
        Head.SetActive(false);
        Arm_L.SetActive(false);
        Arm_R.SetActive(false);
        Leg_L.SetActive(false);
        Leg_R.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //パーツを合体させる処理
    public void CreateRobot(GameObject Parts)
    {
        //クローンのオブジェクトなので(Clone)の文字を空白に変えてパーツ名と同じにする
        string chackname = Parts.name.Replace("(Clone)", "");

        switch (chackname)
        {
            case "Parts_Head":
                Head.SetActive(true);
                break;

            case "Parts_Arm_L":
                Arm_L.SetActive(true);
                break;

            case "Parts_Arm_R":
                Arm_R.SetActive(true);
                break;

            case "Parts_Leg_L":
                Leg_L.SetActive(true);
                break;

            case "Parts_Leg_R":
                Leg_R.SetActive(true);
                break;
        }

        Destroy(Parts);//手に持ってるパーツを消す
    }
}
