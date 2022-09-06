using UnityEngine;
using System.Collections;
using GameEnums;

public class PlayerInputManager : MonoBehaviour
{
	[SerializeField] UIOrganizer uiOrganizer;

	UIEnums.FEEDBACK selectedCommand = UIEnums.FEEDBACK.NONE;
	Direction directionToBuild = Direction.DOWN;

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
		uiOrganizer.CancellLastInput();
	}

	private void hdrBttnFeedbackString(string feedback)
	{
		Debug.Log("playerInputManager received : " + feedback);
		//throw new NotImplementedException();
		var value = UIEnums.ToEnum(feedback);
		//hdrBttnFeedback(feedbackEnum);
		if (value == UIEnums.FEEDBACK.TASKS_HAUL)
		{
			WorldController.SetCommand(WorldController.Command.HAUL);

		}
		else if (value == UIEnums.FEEDBACK.ZONES_STOCKPILE)
		{
			WorldController.SetCommand(WorldController.Command.STOCKPILE);

		}
		else if(value == UIEnums.FEEDBACK.CANCELL)
		{
			WorldController.SetCommand(WorldController.Command.NONE);
		}

		if (value.isBUILDS())
		{
			switch (value)
			{
				case UIEnums.FEEDBACK.BUILD_WALL:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: CATEGORY.WALL);
					break;
				case UIEnums.FEEDBACK.BUILD_DOOR:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: CATEGORY.DOOR);
					break;
				case UIEnums.FEEDBACK.BUILD_ROOF:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: CATEGORY.ROOF);
					break;
				case UIEnums.FEEDBACK.BUILD_BED:
					WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: CATEGORY.BED);
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
			directionToBuild = (Direction)(((int)directionToBuild + 2) % 8);
			Debug.Log("R pressed " + dirBefore +"->"+ directionToBuild);
			WorldController.SetBuildingDirection(directionToBuild);

		}
	}

}
