using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseRoomMenu : UIElementBase
{
	public enum SELECTED { BEDROOM,BATHROOM,LIVINGROOM}
	public delegate void DEL_SELECTED(SELECTED sel);
	public List<DEL_SELECTED> OnSelected = new List<DEL_SELECTED>();
	
	public UnityEngine.UI.Button bttnBedRoom;
	public UnityEngine.UI.Button bttnBathRoom;
	public UnityEngine.UI.Button bttnLivingRoom;

	private void Awake()
	{
		bttnBedRoom.onClick.AddListener(() => { raiseSelected(SELECTED.BEDROOM); });
		bttnBathRoom.onClick.AddListener(() => { raiseSelected(SELECTED.BATHROOM); });
		bttnLivingRoom.onClick.AddListener(() => { raiseSelected(SELECTED.LIVINGROOM); });
	}
	void raiseSelected(SELECTED selected)
	{
		foreach (var hdr in OnSelected) hdr(selected);

	}
}