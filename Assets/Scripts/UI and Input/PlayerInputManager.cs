using UnityEngine;
using System.Collections;
using System;

public class PlayerInputManager : MonoBehaviour
{
	[SerializeField] UIOrganizer uiOrganizer;

	UIEnums.FEEDBACK selectedCommand = UIEnums.FEEDBACK.NONE;
	Game.Direction directionToBuild = Game.Direction.DOWN;

	public void Awake()
	{
		uiOrganizer.OnBttnFeedbackString.Add(hdrBttnFeedbackString);
		UISelectBox.OnSelectedEnd.Add(hdrSelectedFinal);
	}

	private void hdrSelectedFinal(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		WorldController.Select(new Vector2(xBegin, yBegin), new Vector2(xEnd, yEnd));
	}


	private void hdrCancell()
	{
		WorldController.CancellCurrentAction();
		WorldController.UnSelect();
	}

	private void hdrBttnFeedbackString(string feedback)
	{
		//throw new NotImplementedException();
		var feedbackEnum = UIEnums.ToEnum(feedback);
		hdrBttnFeedback(feedbackEnum);
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
				case UIEnums.FEEDBACK.BUILD_WALL:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: Game.CATEGORY.WALL);
					break;
				case UIEnums.FEEDBACK.BUILD_DOOR:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: Game.CATEGORY.DOOR);
					break;
				case UIEnums.FEEDBACK.BUILD_ROOF:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: Game.CATEGORY.ROOF);
					break;
				case UIEnums.FEEDBACK.BUILD_BED:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: Game.CATEGORY.BED);
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
		if (Input.GetKeyDown(KeyCode.R))
		{
			var dirBefore = directionToBuild;
			directionToBuild = (Game.Direction)(((int)directionToBuild + 2) % 8);
			Debug.Log("R pressed " + dirBefore +"->"+ directionToBuild);
			WorldController.SetBuildingDirection(directionToBuild);

		}
	}

}
