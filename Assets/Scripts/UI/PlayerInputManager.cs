using UnityEngine;
using System.Collections;
using System;

public class PlayerInputManager : MonoBehaviour
{
	[SerializeField] UIOrganizer uiOrganizer;

	UIEnums.FEEDBACK selectedCommand = UIEnums.FEEDBACK.NONE;

	public void Awake()
	{
		uiOrganizer.OnBttnFeedback.Add(hdrBttnFeedback);
		UISelectBox.OnSelectedEnd.Add(hdrSelectedFinal);
	}

	private void hdrSelectedFinal(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		WorldController.Select(new Vector2(xBegin, yBegin), new Vector2(xEnd, yEnd));
	}


	private void hdrCancell()
	{
		WorldController.CancellCurrentAction();
	}

	private void hdrBttnFeedback(UIEnums.FEEDBACK value)
	{
		if (value == UIEnums.FEEDBACK.TASKS_HAUL)
		{
			WorldController.SetCommand(WorldController.Command.HAUL);

		}
		if (value == UIEnums.FEEDBACK.ZONES_STOCKPILE)
		{
			WorldController.SetCommand(WorldController.Command.STOCKPILE);

		}
		if (value.isBUILDS())
		{
			switch (value)
			{
				case UIEnums.FEEDBACK.BUILDS_WALL:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: Thing.TYPE.WALL);
					break;
				case UIEnums.FEEDBACK.BUILDS_DOOR:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: Thing.TYPE.DOOR);
					break;
				case UIEnums.FEEDBACK.BUILDS_ROOF:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: Thing.TYPE.ROOF);
					break;
				default:
					break;
			}

		}
	}

	private void hdrSelectedWorld(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		WorldController.Select(new Vector2(xBegin, yBegin), new Vector2(xEnd, yEnd));
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			hdrCancell();
		}
	}

}
