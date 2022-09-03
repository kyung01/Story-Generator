using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMovableWindow : MonoBehaviour
{
	[SerializeField]
	public Canvas canvas;
	//[SerializeField] UnityEngine.UI.Image image;
	// Start is called before the first frame update
	void Start()
	{


	}
	Vector2 offset;

	public void ClickHandler(BaseEventData data)
	{
		PointerEventData pointerEvenetData = (PointerEventData)data;
		offset =  new Vector2(this.transform.position.x, this.transform.position.y)- pointerEvenetData.position;

	}
	public void DragHandler(BaseEventData data)
	{
		PointerEventData pointerEvenetData = (PointerEventData)data;
		var relative = pointerEvenetData.position - new Vector2(this.transform.position.x,this.transform.position.y);
		Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			(RectTransform)canvas.transform,
			pointerEvenetData.position+ offset,
			canvas.worldCamera,
			out position);
		transform.position = canvas.transform.TransformPoint(position);

	}

	// Update is called once per frame
	void Update()
	{

	}
}
