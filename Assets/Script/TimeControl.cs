using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TimeControl : MonoBehaviour
{
   float currentTime = 0f;
    float startingTime = 100f;

    [SerializeField]
    public float fastForwardMultipler = 1f;
    [SerializeField]
    public bool fastForward = true;
    [SerializeField] Text countDownText;

    public delegate void MyDelegate();
    public static MyDelegate moveTime;

    void Start()
    {
        currentTime = startingTime;
        moveTime = MoveTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countDownText.text = "残り時間" + currentTime.ToString("0");
    }

    private void MoveTime()
    {
        if(fastForward == true )
        {
            currentTime -= fastForwardMultipler * Time.deltaTime;
        }
    }
}
