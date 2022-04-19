using System.Collections;
using UnityEngine;

public class DoorRenderer : MonoBehaviour
{


	public void RenderThing(Thing thing)
	{
		this.transform.position = new Vector3(thing.X, thing.Y, 0);
	}
}