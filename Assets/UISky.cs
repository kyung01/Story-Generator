using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISky : MonoBehaviour
{
	static UISky Instance;
	public static void SetTime(float seconds)
	{
		//Debug.Log(seconds);
		Instance.transform.rotation = Quaternion.Euler(0, 360.0f * seconds / 86400.0f ,0);
	}

	private void Awake()
	{
		Instance = this;
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
