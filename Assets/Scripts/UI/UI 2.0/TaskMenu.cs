
using System.Collections.Generic;
using UnityEngine;


public class TaskMenu : UIElementBase
{
	public enum SELECTED { HAUL,
		ANY
	}
	public delegate void DEL_SELECTED(SELECTED s);
	public List<DEL_SELECTED> OnSelected = new List<DEL_SELECTED>();
	public UnityEngine.UI.Button bttnAny;
	public UnityEngine.UI.Button bttnHaul;
	private void Awake()
	{
		bttnAny.onClick.AddListener(() => { raiseSelected(SELECTED.ANY); });
		bttnHaul.onClick.AddListener(() => { raiseSelected(SELECTED.HAUL); });
	}
	void raiseSelected(SELECTED s)
	{
		foreach (var hdr in OnSelected) hdr(s);
	}
}