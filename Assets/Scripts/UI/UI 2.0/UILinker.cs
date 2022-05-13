using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UILinker : MonoBehaviour
{
	[SerializeField] Footer footer;
	[SerializeField] TaskMenu taskMenu;
	[SerializeField] AnyAddRemoveButton taskMenu_edit;

	[SerializeField] ZoneMenu zoneMenu;
	[SerializeField] AnyAddRemoveButton stockpile_edit;
	[SerializeField] HouseRoomMenu houseroomMenu;
	[SerializeField] AnyAddRemoveButton houseroomMenu_edit;
	// Sta
	// rt is called before the first frame update
	void Start()
	{
		footer.bttnTasks.onClick.AddListener(hdrFooterTaskClicked);
		footer.bttnZones.onClick.AddListener(hdrZonesClicked);
		/*
		taskMenu.bttnAny.onClick.hdr(taskMenuAnyClicked);
		taskMenu.bttnHaul.onClick.hdr(taskMenuHaulClicked);
		zoneMenu.bttnAny.onClick.hdr(zoneMenuAnyClicked);
		zoneMenu.bttnHouse.onClick.hdr(zoneMenuHouseClicked);
		zoneMenu.bttnStockpile.onClick.hdr(zoneMenuStockpileClicked);
		 * */


	}

	private void hdrZonesClicked()
	{
		throw new NotImplementedException();
	}

	private void hdrFooterTaskClicked()
	{
		throw new NotImplementedException();
	}


	// Update is called once per frame
	void Update()
	{

	}
}
