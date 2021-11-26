using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSkill : MonoBehaviour
{
    [SerializeField]
    public GameObject[] tag1_Objects;
    [SerializeField]
    public int Skillnumber;
    public int chooseNum;
    // Start is called before the first frame update
    void Start()
    {

        //tag1_Objects = GameObject.FindGameObjectsWithTag("tag1");

        /* for(int i = 0; i < 4; i++)
         {
             Random.Range(0, tag1_Objects.Length);
             /*Debug.Log(tag1_Objects[i]);
             if(gameObject.tag == "Die")
             {
                Destroy(gameObject);
             }
         }*/
        for (int i = 0; i < 3; i++)
        {
            chooseNum = Random.Range(0, tag1_Objects.Length - 1);
            tag1_Objects[chooseNum].GetComponent<BlockManger>().Skillnumber = i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        


        /*if(num == 1)
        {
            gameObject.tag = "tag1";
        }
        else if( num == 2)
        {
            gameObject.tag = "Die";
        }
       else  if( num == 3)
        {
            gameObject.tag = "Wow";
        }
       
        Debug.Log(num);*/
        /*switch (num)
         {
             case 1:
                 gameObject.tag = "tag1";
                 break;
             case 2:
                 gameObject.tag = "Die";
                 break;
             case 3:
                 gameObject.tag = "Wow";
                 break;
             default:
                 Debug.Log("Default");
                 break;
         }*/
    }
}