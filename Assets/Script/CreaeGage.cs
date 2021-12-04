using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreaeGage : MonoBehaviour
{
    [Header("追う対象(博士)")]
    [SerializeField]
    private Transform targetTfm;

    private RectTransform myRectTfm;//この画像の座標

    [Header("作業時間")]
    public float CreateTime;

    [Header("ゲージの画像")]
    [SerializeField]
    private Image GageImage;

    [Header("ゲージの画像(背景)")]
    [SerializeField]
    private Image BuckGage;

    private Vector3 offset = new Vector3(0, 1.5f, 0);

    private bool Downnow;

    GameObject Doctor;

    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();//現在の座標を保存
        GageImage.fillAmount = BuckGage.fillAmount = 0;//画像をリセット(非表示)
        Doctor = GameObject.Find("Doctor");
    }

    void Update()
    {
        //ターゲットを追う
        myRectTfm.position　= RectTransformUtility.WorldToScreenPoint(Camera.main, targetTfm.position + offset);

        //ゲージ画像に作業時間を反映させる
        GageImage.fillAmount = Doctor.GetComponent<DoctorManager>().Createnow_Time / Doctor.GetComponent<DoctorManager>().Create_Time;
        
        if (GageImage.fillAmount > 0)
        {
            BuckGage.fillAmount = 1;
        }
        else
        {
            BuckGage.fillAmount = 0;
        }
    }
}
