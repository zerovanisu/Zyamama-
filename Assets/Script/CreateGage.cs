using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGage : MonoBehaviour
{
    [Header("追う対象(博士)")]
    [SerializeField]
    GameObject Doctor;

    [Header("博士が作業中か")]
    [SerializeField]
    public bool Create_Now;

    private RectTransform MyRectTfm;//この画像の座標

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

    void Start()
    {
        MyRectTfm = GetComponent<RectTransform>();//現在の座標を保存
        GageImage.fillAmount = BuckGage.fillAmount = 0;//画像をリセット(非表示)
    }

    void Update()
    {
        //ターゲットを追う
        MyRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, Doctor.transform.position + offset);

        Create_Now = Doctor.GetComponent<DoctorManager>().Create_now;

        if (Doctor.GetComponent<DoctorManager>().Create_now == true)
        {
            //ゲージ画像に作業時間を反映させる
            GageImage.fillAmount = Doctor.GetComponent<DoctorManager>().Createnow_Time / Doctor.GetComponent<DoctorManager>().Create_Time;

            if (GageImage.fillAmount > 0)
            {
                BuckGage.fillAmount = 1;
            }
            else
            {
                BuckGage.fillAmount = 0;
                GageImage.fillAmount = BuckGage.fillAmount = 0;//画像をリセット(非表示)
            }
        }

        Image_Change();
    }

    void Image_Change()
    {
        if(Create_Now == true)
        {
            GageImage.gameObject.SetActive(true);
            BuckGage.gameObject.SetActive(true);
        }
        else
        {
            GageImage.gameObject.SetActive(false);
            BuckGage.gameObject.SetActive(false);
        }
    }
}
