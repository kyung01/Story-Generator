using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	Vector3 targetPosition;
	private void Awake()
	{
		targetPosition = this.transform.position;
	}
	// Start is called before the first frame update
	void Start()
	{

	}
	Vector3 velocity = new Vector3();
	float moveSpeed = 5;

	// Update is called once per frame
	void Update()
	{
		Vector3 dir = new Vector3();
		var keys = new Dictionary<KeyCode, Vector3>() {
			{KeyCode.A, Vector3.left},
			{KeyCode.S, Vector3.back},
			{KeyCode.D, Vector3.right},
			{KeyCode.W, Vector3.forward}
		};
		foreach(var pair in keys)
		{
			if (Input.GetKey(pair.Key)) dir += pair.Value;
		}
		dir.Normalize();
		
		this.targetPosition += dir* moveSpeed * Time.deltaTime;
		//Debug.Log(dir + " " + moveSpeed);
		this.transform.position= Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, .1f);
	}
}
