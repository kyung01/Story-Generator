using System.Collections;
using UnityEngine;

public class DoorRenderer : MonoBehaviour
{
	Door door;
	[SerializeField] GameObject doorLeft, doorRight;

	public void RenderThing(Thing thing)
	{
		this.door = (Door)thing;
		this.transform.position = new Vector3(thing.X, thing.Y, 0);
	}

	private void Update()
	{
		doorLeft.transform.localPosition = new Vector3(-0.25f - 0.5f * (door.OpenLevel), 0, 0);
		doorRight.transform.localPosition = new Vector3(0.25f + 0.5f * (door.OpenLevel), 0, 0);

	}
}