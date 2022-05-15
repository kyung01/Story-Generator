
using System.Collections.Generic;
using UnityEngine;


public class ZoneMenu : UIElementBase
{
	public enum SELECTED { HOUSE,STOCKPILE}
	public delegate void DEL_SELECTED(SELECTED sel);
	public List<DEL_SELECTED> OnSelected = new List<DEL_SELECTED>();

	public UnityEngine.UI.Button bttnAny;
	public UnityEngine.UI.Button bttnHouse;
	public UnityEngine.UI.Button bttnStockpile;

	private void Awake()
	{
		bttnHouse.onClick.AddListener(() => { raseSelected(SELECTED.HOUSE); });
		bttnStockpile.onClick.AddListener(() => { raseSelected(SELECTED.STOCKPILE); });
	}
	void raseSelected(SELECTED sel)
	{
		foreach(var hdr in OnSelected)
		{
			hdr(sel);
		}
	}
}