using System;
using System.Collections;
using UnityEngine;

public class UIThingView : MonoBehaviour
{
	[SerializeField] TMPro.TMP_Text text;
	public	 void View(Thing thing)
	{
		text.text =""+ thing.Category;
	}
}