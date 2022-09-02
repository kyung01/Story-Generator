using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClock : MonoBehaviour
{
	static UIClock Instance;

	public static void Display(System.DateTime dateTime)
	{
		Instance.display(dateTime);
	}
	[SerializeField] TMPro.TMP_Text textYearMonthDay;
	[SerializeField] TMPro.TMP_Text textDayOfWeek;
	[SerializeField] UnityEngine.GameObject horNeedle;
	[SerializeField] UnityEngine.GameObject minNeedle;
	[SerializeField] UnityEngine.GameObject secNeedle;

	[SerializeField] UnityEngine.UI.Button bttnPause;
	[SerializeField] UnityEngine.UI.Button bttnNormalSpeed;
	[SerializeField] UnityEngine.UI.Button bttnFastSpeed;
	[SerializeField] UnityEngine.UI.Button bttnFasterSpeed;
	[SerializeField] UnityEngine.UI.Button bttnFlyingSpeed;

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;

		bttnPause.onClick.AddListener(hdrOnBttnPause);
		bttnNormalSpeed.onClick.AddListener(hdrOnBttnNormalSpeed);
		bttnFastSpeed.onClick.AddListener(hdrOnBttnFastSpeed);
		bttnFasterSpeed.onClick.AddListener(hdrOnBttnFasterSpeed);
		bttnFlyingSpeed.onClick.AddListener(hdrOnBttnFlyingSpeed);

	}

	private void hdrOnBttnFlyingSpeed()
	{
		Game.SetTimeScale(21601);
	}

	private void hdrOnBttnFasterSpeed()
	{
		Game.SetTimeScale(361);
	}

	private void hdrOnBttnFastSpeed()
	{
		Game.SetTimeScale(61);
	}

	private void hdrOnBttnNormalSpeed()
	{
		Game.SetTimeScale(1);
	}

	private void hdrOnBttnPause()
	{
		Game.SetTimeScale(0);
	}

	public void display(System.DateTime dateTime)
	{
		this.textYearMonthDay.text = dateTime.Year + "/" + dateTime.Month + "/" + dateTime.Day;
		this.textDayOfWeek.text = ""+dateTime.DayOfWeek;
		this.horNeedle.transform.rotation = Quaternion.Euler(0, 0, dateTime.Hour / 12.0f * -360);
		this.minNeedle.transform.rotation = Quaternion.Euler(0, 0, dateTime.Minute / 60.0f * -360);
		this.secNeedle.transform.rotation = Quaternion.Euler(0, 0, dateTime.Second / 60.0f * -360);

	}
	// Update is called once per frame
	void Update()
	{

	}
}
