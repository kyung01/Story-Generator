using GameEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UILinkerOLD : MonoBehaviour
{
	public enum FEEDBACK { 
		NONE,
		FOOTER_TASK_SELECTED,
		FOOTER_ZONE_SELECTED,

		TASK_HAUL,
		
		STOCKPILE_SELECTED,
		HOUSING_SELECTED,

		HOUSING_HOUSE_SELECTED,
		HOUSING_LIVINGROOM_SELECTED,
		HOUSING_BATHROOM_SELECTED,
		HOUSING_BEDROOM_SELECTED,

		ANY,
		ADD,
		REMOVE,

		END
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
	[SerializeField] HousingMenu housingMenu;
	[SerializeField] AnyAddRemoveButton houseroomMenu_edit;

	HousingMenu.SELECTED roomSelected;
	private TaskMenu.SELECTED selectedTask;

	// Sta
	// rt is called before the first frame update
	void Start()
	{
		footer.bttnTasks.onClick.AddListener(hdrFooterTaskClicked);
		footer.bttnZones.onClick.AddListener(hdrFooterZonesClicked);

		zoneMenu.OnSelected.Add(hdrZoneMenuSelected);
		taskMenu.OnSelected.Add(hdrTaskSelected);
		housingMenu.OnSelected.Add(hdrHouseRoomMenuSelected);

		stockpile_edit.OnSelected.Add(hdrEdit);
		taskMenu_edit.OnSelected.Add(hdrEdit);
		houseroomMenu_edit.OnSelected.Add(hdrEdit);
		/*
		taskMenu.bttnAny.onClick.hdr(taskMenuAnyClicked);
		taskMenu.bttnHaul.onClick.hdr(taskMenuHaulClicked);
		zoneMenu.bttnAny.onClick.hdr(zoneMenuAnyClicked);
		zoneMenu.bttnHouse.onClick.hdr(zoneMenuHouseClicked);
		zoneMenu.bttnStockpile.onClick.hdr(zoneMenuStockpileClicked);
		 * */


	}

	private void hdrEdit(AnyAddRemoveButton.SELECTED sel)
	{
		switch (sel)
		{
			case AnyAddRemoveButton.SELECTED.ANY:
				raiseFeedback(FEEDBACK.ANY);
				break;
			case AnyAddRemoveButton.SELECTED.ADD:
				raiseFeedback(FEEDBACK.ADD);
				break;
			case AnyAddRemoveButton.SELECTED.REMOVE:
				raiseFeedback(FEEDBACK.REMOVE);
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
				raiseFeedback(FEEDBACK.TASK_HAUL);
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
			raiseFeedback(FEEDBACK.NONE);
			closeAllMenus();

		}
	}
	private void hdrFooterZonesClicked()
	{
		if (!zoneMenu.IsActive)
		{
			//Open
			closeAllMenus();
			zoneMenu.Open();
			raiseFeedback(FEEDBACK.FOOTER_ZONE_SELECTED);
		}
		else
		{
			//Close
			raiseFeedback(FEEDBACK.NONE);
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
		housingMenu.Close();
		houseroomMenu_edit.Close();
		stockpile_edit.Close();

	}
	void closeAllZoneMenuSelectMenus()
	{
		stockpile_edit.Close();
		housingMenu.Close();
		houseroomMenu_edit.Close();

	}
	private void hdrZoneMenuSelected(ZoneMenu.SELECTED sel)
	{
		switch (sel)
		{
			case ZoneMenu.SELECTED.HOUSE:
				closeAllZoneMenuSelectMenus();
				housingMenu.Open();
				raiseFeedback(FEEDBACK.HOUSING_SELECTED);
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

	private void hdrHouseRoomMenuSelected(HousingMenu.SELECTED sel)
	{
		houseroomMenu_edit.Open();
		roomSelected = sel;
		switch (sel)
		{
			case HousingMenu.SELECTED.HOUSE:
				raiseFeedback(FEEDBACK.HOUSING_HOUSE_SELECTED);
				break;
			case HousingMenu.SELECTED.BEDROOM:
				raiseFeedback(FEEDBACK.HOUSING_BEDROOM_SELECTED);
				break;
			case HousingMenu.SELECTED.BATHROOM:
				raiseFeedback(FEEDBACK.HOUSING_BATHROOM_SELECTED);
				break;
			case HousingMenu.SELECTED.LIVINGROOM:
				raiseFeedback(FEEDBACK.HOUSING_LIVINGROOM_SELECTED);
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
