using GameEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class StaticMethods
{
	public static void Raise(this List<UIButtonLinked.DEL_SetOpen> me, bool value)
	{
		foreach(var d in me)
		{
			d(value);
		}
	}
}

public class UIButtonLinked : MonoBehaviour
{
	public delegate void DEL_SetOpen(bool isOpen);
	public List<DEL_SetOpen> OnSetOpen = new List<DEL_SetOpen>();
	


	[SerializeField] List<UIButtonBundle> bundlesOpen;

	bool isEnabled = false;
	public bool IsEnalbed { get { return this.isEnabled; } }
	static Color colorSelected = Color.yellow;
	static Color colorUnSelected = Color.white;

	private void Awake()
	{
		this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(hdrOnClick);
	}

	private void hdrOnClick()
	{
		SetOpen(!isEnabled);
	}

	public void SetOpen(bool isOpen)
	{
		if(this.isEnabled == isOpen)
		{
			foreach (var b in bundlesOpen)
			{
				b.SetOpen(isOpen);
			}
			return;
		}
		isEnabled = isOpen;
		this.GetComponent<UnityEngine.UI.Image>().color = (isOpen) ? colorSelected : colorUnSelected;
		OnSetOpen.Raise(isOpen);
		//Debug.Log(this.gameObject.name + " " +isOpen);
		foreach(var b in bundlesOpen)
		{
			b.SetOpen(isOpen);
		}
	}
}