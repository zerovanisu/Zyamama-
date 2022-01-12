using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal_Ja");
        float zDirection = Input.GetAxis("Vertical_Ja");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

        transform.position += moveDirection * speed;
    }

    private void OnCollisionEnter(Collision hit)
    {
        switch(hit.gameObject.tag)
        {
            case "BallSpeed":
            speed = 20f;
            Destroy(gameObject,10);
            break;

            case "Multiple":
            Destroy(hit.gameObject);
            Instantiate(this.gameObject, transform.position, Quaternion.identity);
            Destroy(gameObject,10);
            break;

            case"TimeSpeed":
            gameObject.GetComponent<TimeControl>().fastForward = true;
            break;
        }
    }
}
