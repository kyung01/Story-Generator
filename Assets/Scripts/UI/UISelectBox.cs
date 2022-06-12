using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectBox : MonoBehaviour
{
	public delegate void DEL_SELECTED(int xBegin, int yBegin, int xEnd, int yEnd);
	public static List<DEL_SELECTED> OnSelected = new List<DEL_SELECTED>();

	void raiseOnSelected(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		for(int i = 0; i < OnSelected.Count; i++)
		{
			OnSelected[i](xBegin, yBegin, xEnd, yEnd);
		}
	}

	bool isDraw = false;
	Vector2 rectBegin = new Vector2();
	Vector2 rectEnd = new Vector2();
	bool isMouseOverUI()
	{
		return IsMouseOverUIElement(hprGetEventSystemRaycastResults());
	}

	List<RaycastResult> hprGetEventSystemRaycastResults()
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = Input.mousePosition;
		List<RaycastResult> raysastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raysastResults);
		return raysastResults;
	}

	bool IsMouseOverUIElement(List<RaycastResult> eventSystemRaysastResults)
	{
		for (int index = 0; index < eventSystemRaysastResults.Count; index++)
		{
			RaycastResult curRaysastResult = eventSystemRaysastResults[index];
			if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
				return true;
		}
		return false;
	}
	Vector2 screenWorldPosition()
	{
		var pWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//var pWorldRefined = new Vector3(Mathf.RoundToInt(pWorld.x), Mathf.RoundToInt(pWorld.y), 0);
		//pWorldRefined += new Vector3(-0.5f,-0.5f,0);
		//var pViewport = Camera.main.WorldToViewportPoint(pWorldRefined);
		return new Vector2(Mathf.RoundToInt(pWorld.x),Mathf.RoundToInt( pWorld.y));

	}
	private void Update()
	{
		if (isMouseOverUI())
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			isDraw = true;
			rectBegin = screenWorldPosition();
		}
		rectEnd = screenWorldPosition();


		if (isDraw)
		{
			var r1 = rectBegin;
			var r2 = rectEnd;
			var drawbegin = new Vector2(Mathf.Min(r1.x, r2.x), Mathf.Min(r1.y, r2.y));
			var drawEnd = new Vector2(Mathf.Max(r1.x, r2.x), Mathf.Max(r1.y, r2.y));

			float w = Mathf.Abs(drawbegin.x - drawEnd.x);
			float h = Mathf.Abs(drawbegin.y - drawEnd.y);
			if(w==0)
			{
				//rectEnd = rectEnd + new Vector2(1,0);
			}
			else if( h == 0)
			{
				//rectEnd = rectEnd + new Vector2(0, 1);

			}
			var p1 = Camera.main.WorldToViewportPoint(new Vector3(drawbegin.x-0.5f, drawbegin.y - 0.5f, 0));
			var p2 = Camera.main.WorldToViewportPoint(new Vector3(drawEnd.x + 0.5f, drawEnd.y + 0.5f, 0));
			UIPostRenderer.RenderSquareLines( p1, p2);
			//Debug.Log("Drwaing");
		}
		if (Input.GetMouseButtonUp(0))
		{
			isDraw = false;

			var r1 = rectBegin;
			var r2 = rectEnd;
			var vec2Begin = new Vector2(Mathf.Min(r1.x, r2.x), Mathf.Min(r1.y, r2.y));
			var vec2End = new Vector2(Mathf.Max(r1.x, r2.x), Mathf.Max(r1.y, r2.y));

			//Debug.Log("Selected " + r1 + " " + r2);
			raiseOnSelected(Mathf.RoundToInt(vec2Begin.x), Mathf.RoundToInt(vec2Begin.y), Mathf.RoundToInt(vec2End.x), Mathf.RoundToInt(vec2End.y));

		}


	}
}