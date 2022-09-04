using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyAddRemoveButton : UIElementBase
{
	public enum SELECTED { ANY,ADD,REMOVE }
	public delegate void DEL_SELECTED(SELECTED sel);
	public List<DEL_SELECTED> OnSelected = new List<DEL_SELECTED>();

	public UnityEngine.UI.Button bttnAny;
	public UnityEngine.UI.Button bttnAdd;
	public UnityEngine.UI.Button bttnRemove;
	private void Awake()
	{
		bttnAny.onClick.AddListener(() => { raise(SELECTED.ANY); });
		bttnAdd.onClick.AddListener(() => { raise(SELECTED.ADD); });
		bttnRemove.onClick.AddListener(() => { raise(SELECTED.REMOVE); });
	}

	private void raise(SELECTED aNY)
	{
		foreach(var hdr in OnSelected)
		{
			hdr(aNY);
		}
	}
}