using System;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonFeedback : MonoBehaviour
{
	public delegate void DelFeedbackString(string s);
	//[SerializeField] UIEnums.FEEDBACK feedback;
	[SerializeField] string feedbackString;

	public List<DelFeedbackString> OnFeedbackString = new List<DelFeedbackString>();
	//public List<UIEnums.DEL_FEEDBACK> OnFeedback = new List<UIEnums.DEL_FEEDBACK>();

	private void Awake()
	{
		this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(hdrOnClick);
	}

	private void hdrOnClick()
	{
		var bttnLinked = GetComponent<UIButtonLinked>();

		for(int i = 0; i < OnFeedbackString.Count; i++)
		{
			OnFeedbackString[i](feedbackString);
			if (bttnLinked.IsEnalbed)
			{
				OnFeedbackString[i](feedbackString);
			}
		}
		/*
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
		 * */
	}
}
