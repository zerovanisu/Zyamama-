using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class DoctorManager : MonoBehaviour
{
    [SerializeField]
	public float Speed;
	public bool OnParts;
	public bool Catching;
	public GameObject Hand;
	public GameObject Parts;

	float MoveX = 0f;
	float MoveZ = 0f;
	Rigidbody rb;

	float Horizontal;
	float Vertical;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		Horizontal = Input.GetAxis("Horizontal_Dr");
		Vertical = Input.GetAxis("Vertical_Dr");
		Move();
		Turn();
		Catch();
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector3(MoveX, 0, MoveZ);
	}

	void Move()
    {
		MoveX = Horizontal * Speed;
		MoveZ = Vertical * Speed;
		Vector3 direction = new Vector3(MoveX, 0, MoveZ);
	}

	void Turn()
	{
		if (Horizontal != 0 || Vertical != 0)
		{
			var direction = new Vector3(Horizontal, 0, Vertical);
			transform.localRotation = Quaternion.LookRotation(direction);
		}
	}

	void Catch()
    {
		if(Input.GetButtonDown("Åõ_Button"))
        {
			OnParts = Hand.GetComponent<DoctorHand>().OnParts;
			if(OnParts == true && Catching == false)
            {
				Parts = Hand.GetComponent<DoctorHand>().Parts;
				Catching = true;
				//íÕÇﬁèàóù
            }
			else if(Catching == true)
            {
				Catching = false;
				//ó£Ç∑èàóù
            }
        }
    }
}
