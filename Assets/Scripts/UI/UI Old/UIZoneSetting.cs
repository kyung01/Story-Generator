using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIZoneSetting : MonoBehaviour
{
	[SerializeField] UnityEngine.UI.Toggle PREFAB_TOGGLE;
	[SerializeField] GameObject content;
	private void Awake()
	{
		var values = Enum.GetValues(typeof(Game.CATEGORY));

		foreach (var t in values)
		{
			var toggle = Instantiate(PREFAB_TOGGLE);
			toggle.name = ""+t;
			toggle.GetComponentInChildren<UnityEngine.UI.Text>().text = "" + t;
			toggle.transform.SetParent( content.transform);
			toggle.isOn = false;
		}
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void Init()
	{

	}

	void clear()
	{
		foreach(Transform t in content.transform)
		{
			GameObject.Destroy(t.gameObject);
		}
	}

	internal void Link(Zone zone)
	{
		clear();
	}

	internal void Unlink()
	{
	}
}
