using StoryGenerator.World.Things.Actors;
using System.Collections;
using UnityEngine;

public class UIPersonView : MonoBehaviour
{
	[SerializeField] TMPro.TMP_Text text;

	[SerializeField] UnityEngine.UI.Button bttnBio;
	[SerializeField] UnityEngine.UI.Button bttnChar;
	[SerializeField] UnityEngine.UI.Button bttnNeeds;
	[SerializeField] UnityEngine.UI.Button bttnState;
	[SerializeField] UnityEngine.UI.Button bttnValues;
	[SerializeField] UnityEngine.UI.Button bttnLog;


	Person person;

	private void Awake()
	{
		
	}

	public void View(Person person)
	{
		this.person = person;
		ViewBio(person);
	}

	void ViewBio(Person person)
	{
	}

	void ViewCharacterestics(Person person)
	{

	}

	void ViewNeeds(Person person)
	{

	}

	void ViewState(Person person)
	{

	}

	void ViewValues(Person person)
	{

	}

	void ViewLog(Person person)
	{

	}
}