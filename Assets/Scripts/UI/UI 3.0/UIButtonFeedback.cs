using System;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonFeedback : MonoBehaviour
{
	[SerializeField] UIEnums.FEEDBACK feedback;
	public List<UIEnums.DEL_FEEDBACK> OnFeedback = new List<UIEnums.DEL_FEEDBACK>();

	private void Awake()
	{
		this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(hdrOnClick);
	}

	private void hdrOnClick()
	{
		var bttnLinked = GetComponent<UIButtonLinked>();
		if(bttnLinked == null)
		{
			OnFeedback.Raise(feedback);
		}
		else
		{
			if (bttnLinked.IsEnalbed)
			{
				OnFeedback.Raise(feedback);
			}
			else
			{
			}
		}
	}
}
