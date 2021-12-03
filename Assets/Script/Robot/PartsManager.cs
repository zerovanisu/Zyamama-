using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public GameObject Hand = null;//G‚ê‚½è‚ğŠi”[‚·‚é•Ï”
    public bool Catching = false;//’Í‚Ü‚ê‚Ä‚¢‚é‚©‚ğ”»’è‚·‚é•Ï”

    public float PosX,PosY,PosZ = 0;//è‚ÌÀ•W‚ğŠi”[‚·‚é•Ï”

    [SerializeField]
    private float High = 0;//‚¿ã‚°‚é‚‚³
    public string SkillName;//ƒXƒLƒ‹‚Ìí—Ş‚ğŠi”[‚·‚é•Ï”

    Rigidbody rb;
    BoxCollider bc;
    Vector3 FarstPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        FarstPos = this.transform.position;//‰ŠúˆÊ’u‚ğ‹L‰¯
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
            //‚¿ã‚°‚éˆ—
            PosY = High;
            
            //À•W‚ğ”½‰f
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

    private void OnCollisionEnter(Collision collision)
    {
        //G‚ê‚½‚Ì‚ª’n–Ê‚¾‚Á‚½‚ç(—‚Æ‚³‚ê‚½‚ç)
        if (collision.gameObject.tag == "Ground")
        {
            //‰ŠúˆÊ’u‚É–ß‚·
            this.transform.position = FarstPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Catching == true)
        {
            //G‚ê‚½‚Ì‚ªì‹Æ‘ä‚¾‚Á‚½‚ç
            if (other.gameObject.tag == "CreateTable")
            {
                //”­“®‚Å‚«‚éƒXƒLƒ‹‚Ìí—Ş‚ğ‹L‰¯
                SkillName = other.gameObject.GetComponent<TableManager>().SkillName;
            }
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

        if (other.gameObject.tag == "CreateTable")
        {
            SkillName = null;
        }
    }
}
