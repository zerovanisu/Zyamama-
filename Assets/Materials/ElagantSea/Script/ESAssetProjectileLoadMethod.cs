using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESAssetProjectileLoadMethod : MonoBehaviour {

	public GameObject[] EffectPrefabs;
	public GameObject CameraPrefabs;
	public Camera mainCamera;

	private  Animator a_animator;

	private int currentNum;
	private GameObject currentInstance=null;
	private string textToEdit="defo";
	public bool isChecked = true;

	void Start () {
		
		a_animator =  CameraPrefabs.GetComponent<Animator>();
		ChangeCurrent(0);
	}


	private void OnGUI()
	{
		
		if (GUI.Button(new Rect(5,5,200,30), "PREVIOUS EFFECT"))
		{
			ChangeCurrent(-1);
		}

		if (GUI.Button(new Rect(205, 5, 200, 30), "NEXT EFFECT"))
		{
			ChangeCurrent(+1);
		}

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleCenter;

		textToEdit = GUI.TextField(new Rect(5, 40, 400, 30), textToEdit,50,myStyle);

		Rect rect1 = new Rect(5, 70, 400, 30);
		isChecked = GUI.Toggle(rect1, isChecked, "Camera rotation");

		if (isChecked) {
			a_animator.speed = 1;
		} else {	
			a_animator.speed = 0;
		}
	}


	void ChangeCurrent(int num) {

		currentNum += num;
		if (currentNum > EffectPrefabs.Length - 1){
			currentNum = 0;
		}else if (currentNum < 0){
		
			currentNum = EffectPrefabs.Length - 1;

		}

		textToEdit = EffectPrefabs [currentNum].name;

		if(currentInstance!=null) {
			Destroy(currentInstance);
		}
		
		//currentInstance = Instantiate(EffectPrefabs[currentNum], transform.position, transform.rotation) as GameObject;
				
	}

	void Update () {	
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		Plane ground = new Plane (Vector3.up, Vector3.zero);
		float rayDistance;
		if (ground.Raycast (ray, out rayDistance)) {
			Vector3 point = ray.GetPoint (rayDistance);
			Vector3 correctPoint = new Vector3 (point.x, transform.position.y, point.z);
			transform.LookAt (correctPoint);
		}

		if (Input.GetMouseButtonDown (0)) {
			GameObject projectile = Instantiate (EffectPrefabs [currentNum], transform.position, transform.rotation);
		}

	}

}
