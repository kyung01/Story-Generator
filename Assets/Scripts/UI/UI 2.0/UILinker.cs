using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UILinker : MonoBehaviour
{
	public enum FEEDBACK { 
	FOOTER_TASK_SELECTED,
	FOOTER_ZONE_SELECTED,
	END,
		STOCKPILE_SELECTED,
		HOUSEMENU_SELECTED,
		HOUSE_LIVINGROOM_SELECTED,
		HOUSE_BATHROOM_SELECTED,
		HOUSE_BEDROOM_SELECTED
	}
	public delegate void DEL_FEEDBACK(FEEDBACK feedback);

	public List<DEL_FEEDBACK> OnFeedback = new List<DEL_FEEDBACK>();
	public void raiseFeedback(FEEDBACK f)
	{
		foreach (var hdr in OnFeedback) hdr(f);
	}

	[SerializeField] Footer footer;
	[SerializeField] TaskMenu taskMenu;
	[SerializeField] AnyAddRemoveButton taskMenu_edit;

	[SerializeField] ZoneMenu zoneMenu;
	[SerializeField] AnyAddRemoveButton stockpile_edit;
	[SerializeField] HouseRoomMenu houseroomMenu;
	[SerializeField] AnyAddRemoveButton houseroomMenu_edit;

	HouseRoomMenu.SELECTED roomSelected;
	private TaskMenu.SELECTED selectedTask;

	// Sta
	// rt is called before the first frame update
	void Start()
	{
		footer.bttnTasks.onClick.AddListener(hdrFooterTaskClicked);
		footer.bttnZones.onClick.AddListener(hdrFooterZonesClicked);

		zoneMenu.OnSelected.Add(hdrZoneMenuSelected);
		taskMenu.OnSelected.Add(hdrTaskSelected);
		houseroomMenu.OnSelected.Add(hdrHouseRoomMenuSelected);

		stockpile_edit.OnSelected.Add(hdrStockpileEditSelected);
		taskMenu_edit.OnSelected.Add(hdrTaskMenuEditSelected);
		houseroomMenu_edit.OnSelected.Add(hdrHouseRoomMenuEditSelected);
		/*
		taskMenu.bttnAny.onClick.hdr(taskMenuAnyClicked);
		taskMenu.bttnHaul.onClick.hdr(taskMenuHaulClicked);
		zoneMenu.bttnAny.onClick.hdr(zoneMenuAnyClicked);
		zoneMenu.bttnHouse.onClick.hdr(zoneMenuHouseClicked);
		zoneMenu.bttnStockpile.onClick.hdr(zoneMenuStockpileClicked);
		 * */


	}

	private void hdrHouseRoomMenuEditSelected(AnyAddRemoveButton.SELECTED sel)
	{
		switch (sel)
		{
			case AnyAddRemoveButton.SELECTED.ANY:
				break;
			case AnyAddRemoveButton.SELECTED.ADD:
				break;
			case AnyAddRemoveButton.SELECTED.REMOVE:
				break;
			default:
				break;
		}
	}

	private void hdrTaskMenuEditSelected(AnyAddRemoveButton.SELECTED sel)
	{
		switch (sel)
		{
			case AnyAddRemoveButton.SELECTED.ANY:
				break;
			case AnyAddRemoveButton.SELECTED.ADD:
				break;
			case AnyAddRemoveButton.SELECTED.REMOVE:
				break;
			default:
				break;
		}
	}

	private void hdrStockpileEditSelected(AnyAddRemoveButton.SELECTED sel)
	{
		switch (sel)
		{
			case AnyAddRemoveButton.SELECTED.ANY:
				break;
			case AnyAddRemoveButton.SELECTED.ADD:
				break;
			case AnyAddRemoveButton.SELECTED.REMOVE:
				break;
			default:
				break;
		}
	}

	private void hdrTaskSelected(TaskMenu.SELECTED s)
	{
		this.selectedTask = s;
		switch (s)
		{
			case TaskMenu.SELECTED.HAUL:
				this.taskMenu_edit.Open();
				break;
			default:
				break;
		}
	}
	#region Footer
	private void hdrFooterTaskClicked()
	{
		if (!taskMenu.IsActive)
		{
			closeAllMenus();
			taskMenu.Open();
			raiseFeedback(FEEDBACK.FOOTER_TASK_SELECTED);
		}
		else
		{
			closeAllMenus();

		}
	}
	private void hdrFooterZonesClicked()
	{
		if (!zoneMenu.IsActive)
		{
			closeAllMenus();
			zoneMenu.Open();
			raiseFeedback(FEEDBACK.FOOTER_ZONE_SELECTED);
		}
		else
		{
			closeAllMenus();

		}
	}
	#endregion
	void closeAllMenus()
	{
		this.taskMenu.Close();
		this.taskMenu_edit.Close();
		closeZoneMenus();
		closeAllZoneMenuSelectMenus();
	}

	void closeZoneMenus()
	{
		zoneMenu.Close();
		houseroomMenu.Close();
		houseroomMenu_edit.Close();
		stockpile_edit.Close();

	}
	void closeAllZoneMenuSelectMenus()
	{
		stockpile_edit.Close();
		houseroomMenu.Close();
		houseroomMenu_edit.Close();

	}
	private void hdrZoneMenuSelected(ZoneMenu.SELECTED sel)
	{
		switch (sel)
		{
			case ZoneMenu.SELECTED.HOUSE:
				closeAllZoneMenuSelectMenus();
				houseroomMenu.Open();
				raiseFeedback(FEEDBACK.HOUSEMENU_SELECTED);
				break;
			case ZoneMenu.SELECTED.STOCKPILE:
				closeAllZoneMenuSelectMenus();
				stockpile_edit.Open();
				raiseFeedback(FEEDBACK.STOCKPILE_SELECTED);
				break;
			default:
				break;
		}
	}

	private void hdrHouseRoomMenuSelected(HouseRoomMenu.SELECTED sel)
	{
		houseroomMenu_edit.Open();
		roomSelected = sel;
		switch (sel)
		{
			case HouseRoomMenu.SELECTED.BEDROOM:
				raiseFeedback(FEEDBACK.HOUSE_BEDROOM_SELECTED);
				break;
			case HouseRoomMenu.SELECTED.BATHROOM:
				raiseFeedback(FEEDBACK.HOUSE_BATHROOM_SELECTED);
				break;
			case HouseRoomMenu.SELECTED.LIVINGROOM:
				raiseFeedback(FEEDBACK.HOUSE_LIVINGROOM_SELECTED);
				break;
			default:
				break;
		}
	}





	// Update is called once per frame
	void Update()
	{

	}
}
