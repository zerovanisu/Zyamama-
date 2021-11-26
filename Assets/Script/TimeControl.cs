using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TimeControl : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 100f;

    public float fastForwardMultipler = 5f;
    public bool fastForward;

    [SerializeField] Text countDownText;
    // creat canvas text and input 
    

    void Start()
    {
        currentTime = startingTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countDownText.text = currentTime.ToString("0");

        if(fastForward == true )
        {
            currentTime -= 5 * Time.deltaTime;
        }
    }
}
