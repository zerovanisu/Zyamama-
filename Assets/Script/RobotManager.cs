using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public GameObject Hand = null;
    public bool Catching = false;

    public float PosX,PosY,PosZ = 0;

    [SerializeField]
    private float limit,High = 0;

    Rigidbody rb;
    BoxCollider bc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //ˆÚ“®ˆ—
    void Move()
    {
        //’Í‚Ü‚ê‚Ä‚¢‚é‚©‚ğ”»’è
        if (Catching == true)
        {
            //è‚ÌÀ•W(‚‚³ˆÈŠO)‚ğæ“¾A‚‚³‚ÍŒ»İ’n‚ğ‚»‚Ì‚Ü‚Ü”½‰f
            PosX = Hand.transform.position.x;
            PosY = this.transform.position.y;
            PosZ = Hand.transform.position.z;

            //‚‚³‚ª’á‚©‚Á‚½‚ç‚¿ã‚°‚é
            if(PosY < limit)
            {
                PosY = High;
            }
            this.transform.position = new Vector3(PosX, PosY, PosZ);

            //‰ñ“]‚âˆÊ’u‚¸‚ê‚ª‹N‚«‚È‚¢‚æ‚¤‚É~‚ß‚éˆ—
            rb.constraints = RigidbodyConstraints.FreezeRotation
                | RigidbodyConstraints.FreezePositionY;

            //•¨—ˆ—‚ğˆê’â~
            bc.isTrigger = true;
        }
        else
        {
            //•¨—ˆ—‚ğÄ‰Ò“­
            bc.isTrigger = false;

            //ˆÊ’u‚¸‚êˆ—‚ğÄ’²®
            rb.constraints = RigidbodyConstraints.FreezeRotation
                | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //G‚ê‚Ä‚¢‚é‚Ì‚ªè‚¾‚Á‚½‚ç
        if(other.gameObject.name == "Hand")
        {
            //è‚Ìî•ñ‚ğæ“¾
            Hand = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //—£‚ê‚½‚à‚Ì‚ªè‚¾‚Á‚½‚ç(—£‚³‚ê‚½‚ç)
        if(other.gameObject == Hand)
        {
            //è‚Ìî•ñ‚ğ”jŠü
            Hand = null;
        }
    }
}
