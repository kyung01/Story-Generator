using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UISelectBoxExtensions {
	public static void Raise(this List<UISelectBox.DEL_SELECTED> list, int xBegin, int yBegin, int xEnd, int yEnd)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i](xBegin, yBegin, xEnd, yEnd);
		}
	}
}

public class UISelectBox : MonoBehaviour
{
	public delegate void DEL_SELECTED(int xBegin, int yBegin, int xEnd, int yEnd);

	public static List<DEL_SELECTED> OnSelectedBegin = new List<DEL_SELECTED>();
	public static List<DEL_SELECTED> OnSelectedMid = new List<DEL_SELECTED>();
	public static List<DEL_SELECTED> OnSelectedEnd = new List<DEL_SELECTED>();


	bool isDraw = false;
	Vector2 rectRoundedBegin = new Vector2();
	Vector2 rectRoundedEnd = new Vector2();
	Vector2 rectRawBegin = new Vector2();
	Vector2 rectRawEnd = new Vector2();


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
		return new Vector2(pWorld.x, pWorld.y);

	}
	Vector2 screenWorldPositionRounded()
	{
		var pWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector2(Mathf.RoundToInt(pWorld.x),Mathf.RoundToInt( pWorld.y));

	}
	void updateRender()
	{
		if (!isDraw) return;
		Vector2 drawBegin, drawEnd;
		Vector2 drawLightBegin, drawLightEnd;
		EasyTools.EzT.Separte(rectRoundedBegin, rectRoundedEnd, out drawBegin, out drawEnd);
		EasyTools.EzT.Separte(rectRawBegin, rectRawEnd, out drawLightBegin, out drawLightEnd);

		float w = Mathf.Abs(drawBegin.x - drawEnd.x);
		float h = Mathf.Abs(drawBegin.y - drawEnd.y);
		if (w == 0)
		{
			//rectEnd = rectEnd + new Vector2(1,0);
		}
		else if (h == 0)
		{
			//rectEnd = rectEnd + new Vector2(0, 1);

		}
		UIPostRenderer.Render_Line_Squares(Color.white, 
			new Vector3(drawBegin.x - 0.5f, drawBegin.y - 0.5f, 0), 
			new Vector3(drawEnd.x + 0.5f, drawEnd.y + 0.5f, 0)
			);
		UIPostRenderer.Render_Line_Squares(new Color(1,1,1,0.5f), drawLightBegin, drawLightEnd);
		//Debug.Log("Drwaing");
	}

	void updateRaiseEvents()
	{
		//var rectBegin = this.rectBegin;
		//var rectEnd = this.rectEnd;
		var vec2Begin = new Vector2(Mathf.Min(rectRoundedBegin.x, rectRoundedEnd.x), Mathf.Min(rectRoundedBegin.y, rectRoundedEnd.y));
		var vec2End = new Vector2(Mathf.Max(rectRoundedBegin.x, rectRoundedEnd.x), Mathf.Max(rectRoundedBegin.y, rectRoundedEnd.y));

		if (Input.GetMouseButtonDown(0))
		{
			OnSelectedBegin.Raise(Mathf.RoundToInt(vec2Begin.x), Mathf.RoundToInt(vec2Begin.y), Mathf.RoundToInt(vec2End.x), Mathf.RoundToInt(vec2End.y));
		}
		else if (Input.GetMouseButtonUp(0))
		{
			OnSelectedEnd.Raise(Mathf.RoundToInt(vec2Begin.x), Mathf.RoundToInt(vec2Begin.y), Mathf.RoundToInt(vec2End.x), Mathf.RoundToInt(vec2End.y));
		}
		else
		{
			OnSelectedMid.Raise(Mathf.RoundToInt(vec2Begin.x), Mathf.RoundToInt(vec2Begin.y), Mathf.RoundToInt(vec2End.x), Mathf.RoundToInt(vec2End.y));
		}
	}
	void updateRectPositions()
	{

		if (Input.GetMouseButtonDown(0))
		{
			isDraw = true;
			rectRoundedBegin = screenWorldPositionRounded();
			rectRawBegin = screenWorldPosition();
		}
		rectRoundedEnd = screenWorldPositionRounded();
		rectRawEnd = screenWorldPosition();

	}
	private void Update()
	{
		if (isMouseOverUI())
		{
			return;
		}
		updateRectPositions();


		if (Input.GetMouseButtonUp(0))
		{
			isDraw = false;
		}

		updateRender();
		updateRaiseEvents();


	}
}