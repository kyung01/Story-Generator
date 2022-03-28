using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singletone class
public class UIPostRenderer : MonoBehaviour
{
	public struct RenderSquareCall
	{
		public Vector3 from, to;
	}
	static UIPostRenderer me;

	// Draws a line from "startVertex" var to the curent mouse position.
	static List<RenderSquareCall> listRenderSquare = new List<RenderSquareCall>();
	[SerializeField]
	Material mat;
	Vector3 startVertex;
	Vector3 mousePos;

	private void Awake()
	{
		me = this;
	}
	void Start()
	{
		startVertex = Vector3.zero;
	}

	void Update()
	{
		mousePos = Input.mousePosition;
		// Press space to update startVertex
		if (Input.GetKeyDown(KeyCode.Space))
		{
			startVertex = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);
		}
	}
	void renderLine(Vector3 from, Vector3 to)
	{

	}
	void renderSquare(Vector3 from, Vector3 to)
	{

	}

	void OnPostRender()
	{
		//Debug.Log("Post Renderer " + listRenderSquare.Count);
		if (!mat)
		{
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho();

		GL.Begin(GL.LINES);

		GL.Color(Color.red);
		for (int i = 0; i < listRenderSquare.Count; i++)
		{
			var renderSquareCall = listRenderSquare[i];
			GL.Vertex(renderSquareCall.from);
			GL.Vertex(new Vector3(renderSquareCall.to.x, renderSquareCall.from.y, 0));
			GL.Vertex(renderSquareCall.from);
			GL.Vertex(new Vector3(renderSquareCall.from.x, renderSquareCall.to.y, 0));
			GL.Vertex(renderSquareCall.to);
			GL.Vertex(new Vector3(renderSquareCall.to.x, renderSquareCall.from.y, 0));
			GL.Vertex(renderSquareCall.to);
			GL.Vertex(new Vector3(renderSquareCall.from.x, renderSquareCall.to.y, 0));
		}
		foreach (var renderSquareCall in listRenderSquare)
		{

		}

		//GL.Vertex(startVertex);
		//GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
		GL.End();

		GL.PopMatrix();
		listRenderSquare.Clear();
	}



	public static void RenderSquare(Vector3 screenSpaceFrom, Vector3 screenSpaceTo)
	{
		screenSpaceFrom = new Vector3(screenSpaceFrom.x, screenSpaceFrom.y, 0);
		screenSpaceTo = new Vector3(screenSpaceTo.x, screenSpaceTo.y, 0);
		//Debug.Log("Screen space from" + screenSpaceFrom);
		//Debug.Log("Screen space to" + screenSpaceTo);

		listRenderSquare.Add(new RenderSquareCall() { from = screenSpaceFrom, to = screenSpaceTo });
	}
}
