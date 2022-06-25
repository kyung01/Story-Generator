using UnityEngine;
using System.Collections;

public class PlayerInputManager : MonoBehaviour
{
	public enum STATE
	{
	}
	private void hdrSelectedWorld(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		WorldController.Select(new Vector2(xBegin, yBegin), new Vector2(xEnd, yEnd));
	}


}
