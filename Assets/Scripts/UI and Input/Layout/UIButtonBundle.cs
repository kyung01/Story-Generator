using GameEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonBundle : MonoBehaviour
{
    [SerializeField] List<UIButtonLinked> buttons;

	private void Awake()
	{
		buttons = new List<UIButtonLinked>(GetComponentsInChildren<UIButtonLinked>());
		foreach(var b in buttons)
		{
			b.OnSetOpen.Add((bool isOpen) => { hdrButtonSetOpen(b, isOpen); });
		}

	}


	private void hdrButtonSetOpen(UIButtonLinked button, bool isOpen)
	{
		if (!isOpen) return;
		foreach(var b in buttons)
		{
			if (b == button) continue;
			b.SetOpen(false);

		}
	}

	public void SetOpen(bool isOpen)
	{
		this.gameObject.SetActive(isOpen);

		foreach (var b in buttons)
		{
			b.SetOpen(false);
		}

	}
}
