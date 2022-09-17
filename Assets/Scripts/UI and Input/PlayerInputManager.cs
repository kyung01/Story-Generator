using UnityEngine;
using System.Collections;
using GameEnums;
using System.Collections.Generic;
using static UIEnums;
using System;

public class PlayerInputManager : MonoBehaviour
{
	[SerializeField] UIOrganizer uiOrganizer;

	FEEDBACK selectedCommand = UIEnums.FEEDBACK.NONE;
	ThingCategory thingToBuild = ThingCategory.UNDEFINED;
	Direction directionToBuild = Direction.DOWN;
	ZoneCategory zoneToBuild = ZoneCategory.NONE;
	WorldController.Command controllerCommandSelected = WorldController.Command.NONE;

	public void Awake()
	{
		uiOrganizer.OnBttnFeedbackString.Add(hdrBttnFeedbackString);
		UISelectBox.OnSelectedEnd.Add(hdrSelectedFinal);
	}

	private void hdrSelectedFinal(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		switch (controllerCommandSelected)
		{
			case WorldController.Command.NONE:
				break;
			case WorldController.Command.BUILD:
				WorldController.SetCommand(WorldController.Command.BUILD, thingToBuild: this.thingToBuild);
				break;
			case WorldController.Command.HAUL:
				WorldController.SetCommand(WorldController.Command.HAUL);
				break;
			case WorldController.Command.ZONE:
				WorldController.SetCommand(WorldController.Command.ZONE, zoneToBuild: this.zoneToBuild);
				break;
			case WorldController.Command.END:
				break;
			default:
				break;
		}
		WorldController.Select(new Vector2(xBegin, yBegin), new Vector2(xEnd, yEnd));
	}


	private void hdrCancell()
	{
		WorldController.CancellCurrentAction();
		WorldController.UnSelect();
		uiOrganizer.CancellLastInput();
	}

	void clearSettings()
	{
		this.thingToBuild = ThingCategory.UNDEFINED;
		this.zoneToBuild = ZoneCategory.NONE;
		this.selectedCommand = FEEDBACK.NONE;
	}

	static readonly string FOOTER_BUILD = "FOOTER_BUILD";
	static readonly string FOOTER_ZONE_OPEN = "FOOTER_ZONE_OPEN";
	static readonly string FOOTER_ZONE_CLOSE = "FOOTER_ZONE_CLOSE";
	static readonly string FOOTER_TASK = "FOOTER_BUILD";
	private void hdrBttnFeedbackString(string feedbackString)
	{
		clearSettings();
		Debug.Log("playerInputManager received : " + feedbackString);
		//throw new NotImplementedException();
		Dictionary<string, WorldController.Command> feedbackStringToWorldControllerCOmmand = new Dictionary<string, WorldController.Command>() {
			{"BUILD_WALL",WorldController.Command.BUILD  },
			{"BUILD_DOOR",WorldController.Command.BUILD  },
			{"BUILD_ROOF",WorldController.Command.BUILD  },
			{"BUILD_BED",WorldController.Command.BUILD  },

			{"ZONE_HOUSING",WorldController.Command.ZONE  },
			{"ZONE_HOUSING_LIVINGROOM",WorldController.Command.ZONE },
			{"ZONE_HOUSING_BEDROOM",WorldController.Command.ZONE  },
			{"ZONE_HOUSING_BATHROOM",WorldController.Command.ZONE },
			{"ZONE_STOCKPILE",WorldController.Command.ZONE  },

			{"TASK_HAUL",WorldController.Command.HAUL  },
			{"CANCELL",WorldController.Command.NONE  }

		};
		Dictionary<string, ThingCategory> buildDictionary = new Dictionary<string, ThingCategory>() {
			{"BUILD_WALL",ThingCategory.WALL  },
			{"BUILD_DOOR",ThingCategory.DOOR  },
			{"BUILD_ROOF",ThingCategory.ROOF  },
			{"BUILD_BED", ThingCategory.BED  },
		};
		Dictionary<string, ZoneCategory> zoneDictionary = new Dictionary<string, ZoneCategory>() {
			{"ZONE_HOUSING",ZoneCategory.HOUSING  },
			{"ZONE_HOUSING_LIVINGROOM",ZoneCategory.HOUSING_LIVINGROOM  },
			{"ZONE_HOUSING_BEDROOM",ZoneCategory.HOUSING_BEDROOM  },
			{"ZONE_HOUSING_BATHROOM",ZoneCategory.HOUSING_BATHROOM  },
			{"ZONE_STOCKPILE",ZoneCategory.STOCKPILE  }
		};

		if(feedbackString == FOOTER_ZONE_OPEN)
		{
			GameRenderer.Instance.SetZoneRendereEnabledTo(true);
			//UIMain.SetZoneRenderer(true);

		}else if(feedbackString == FOOTER_ZONE_CLOSE)
		{
			//UIMain.SetZoneRenderer(false);
			GameRenderer.Instance.SetZoneRendereEnabledTo(false);

		}


		if (feedbackStringToWorldControllerCOmmand.ContainsKey(feedbackString))
		{


			this.controllerCommandSelected = feedbackStringToWorldControllerCOmmand[feedbackString];
			if (controllerCommandSelected == WorldController.Command.BUILD)
			{
				this.thingToBuild = buildDictionary[feedbackString];
				return;
			}
			else if (controllerCommandSelected == WorldController.Command.HAUL)
			{
				WorldController.SetCommand(WorldController.Command.HAUL);
			}
			else if (controllerCommandSelected == WorldController.Command.ZONE)
			{
				this.zoneToBuild = zoneDictionary[feedbackString];
			}
		}
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
			Debug.Log("R pressed " + dirBefore + "->" + directionToBuild);
			WorldController.SetBuildingDirection(directionToBuild);

		}
	}

}
