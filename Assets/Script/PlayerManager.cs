using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
	public float Speed;
	
	float MoveX = 0f;
	float MoveZ = 0f;
	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		MoveX = Input.GetAxis("Horizontal") * Speed;
		MoveZ = Input.GetAxis("Vertical") * Speed;
		Vector3 direction = new Vector3(MoveX, 0, MoveZ);
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector3(MoveX, 0, MoveZ);
	}
}
