using System.Collections;
using UnityEngine;

public class UIElementBase : MonoBehaviour
{
	public bool IsActive
	{
		get { return this.gameObject.activeSelf; }
	}
	public void Close()
	{
		this.gameObject.SetActive(false);
	}
	public void Open()
	{
		this.gameObject.SetActive(true);
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}