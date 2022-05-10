using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singletone class
public class UIPostRenderer : MonoBehaviour
{
	static int colorsMax = 100;
	Color[] colors;
	public static Color GetColor(int n)
	{
		return me.colors[n % colorsMax];
	}

	public class RenderSquareCall
	{
		public bool isColored = false;
		public Color color;
		public Vector3
			from,
			to;
	}
	static UIPostRenderer me;

	// Draws a line from "startVertex" var to the curent mouse position.
	static List<RenderSquareCall> ListRenderSquareLines = new List<RenderSquareCall>();
	static List<RenderSquareCall> ListRenderSquares = new List<RenderSquareCall>();
	[SerializeField]
	Material mat;
	Vector3 startVertex;
	Vector3 mousePos;
	public static Color GetRandomColor()
	{
		var c = GetRandomColorRaw();
		return new Color(c.r, c.g, c.b,1);
	}
	public static Color GetRandomColorRaw()
	{
		int n = Random.Range(0, 3);
		float f1 = Random.Range(0, 1.0f);
		float f2 = 1 - f1;
		switch (n)
		{
			default:
			case 0:
				return new Color(1, f1, f2);
				break;
			case 1:
				return new Color(f1, 1, f2);
				break;
			case 2:
				return new Color(f1, f2, 1);
				break;
		}
	}

	private void Awake()
	{
		me = this;
		colors = new Color[colorsMax];
		for(int i = 0; i< colorsMax; i++)
		{
			colors[i] = GetRandomColor();
		}
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
		//Debug.Log("Post Renderer " + ListRenderSquareLines.Count);
		if (!mat)
		{
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho();

		GL.Begin(GL.TRIANGLES);
		for (int i = 0; i < ListRenderSquares.Count; i++)
		{
			var renderSquareCall = ListRenderSquares[i];
			//Debug.Log(renderSquareCall.color);
			if (!renderSquareCall.isColored)
			{
				GL.Color(GetColor(i));
			}
			else if(renderSquareCall.isColored ) {
				GL.Color(renderSquareCall.color);

			}
			//Debug.Log("Post Renderer " + renderSquareCall.from + " " + renderSquareCall.to);
			GL.Vertex(renderSquareCall.from);
			GL.Vertex(new Vector3(renderSquareCall.to.x, renderSquareCall.from.y, 0));
			GL.Vertex(renderSquareCall.to);
			GL.Vertex(renderSquareCall.to);
			GL.Vertex(new Vector3(renderSquareCall.from.x, renderSquareCall.to.y, 0));
			GL.Vertex(renderSquareCall.from);
		}
		GL.End();
		GL.Begin(GL.LINES);

		for (int i = 0; i < ListRenderSquareLines.Count; i++)
		{
			var renderSquareCall = ListRenderSquareLines[i];
			if (!renderSquareCall.isColored)
			{

				GL.Color(GetColor(i));
			}
			else
			{
				GL.Color(renderSquareCall.color);
			}
			GL.Vertex(renderSquareCall.from);
			GL.Vertex(new Vector3(renderSquareCall.to.x, renderSquareCall.from.y, 0));
			GL.Vertex(renderSquareCall.from);
			GL.Vertex(new Vector3(renderSquareCall.from.x, renderSquareCall.to.y, 0));
			GL.Vertex(renderSquareCall.to);
			GL.Vertex(new Vector3(renderSquareCall.to.x, renderSquareCall.from.y, 0));
			GL.Vertex(renderSquareCall.to);
			GL.Vertex(new Vector3(renderSquareCall.from.x, renderSquareCall.to.y, 0));
		}
		foreach (var renderSquareCall in ListRenderSquareLines)
		{

		}

		//GL.Vertex(startVertex);
		//GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
		GL.End();

		GL.PopMatrix();
		ListRenderSquareLines.Clear();
		ListRenderSquares.Clear();
	}



	public static void RenderSquare(Color color, Vector2 screenSpaceFrom, Vector2 screenSpaceTo)
	{
		screenSpaceFrom = new Vector3(screenSpaceFrom.x, screenSpaceFrom.y, 0);
		screenSpaceTo = new Vector3(screenSpaceTo.x, screenSpaceTo.y, 0);
		var r = new RenderSquareCall() { color = color, from = screenSpaceFrom, to = screenSpaceTo };
		if (color != null)
		{
			r.isColored = true;

		}
		ListRenderSquares.Add(r);
	}
	
	public static void RenderSquareLines(Vector3 screenSpaceFrom, Vector3 screenSpaceTo)
	{
		screenSpaceFrom = new Vector3(screenSpaceFrom.x, screenSpaceFrom.y, 0);
		screenSpaceTo = new Vector3(screenSpaceTo.x, screenSpaceTo.y, 0);
		//Debug.Log("Screen space from" + screenSpaceFrom);
		//Debug.Log("Screen space to" + screenSpaceTo);

		ListRenderSquareLines.Add(new RenderSquareCall() { color = GetRandomColor(), from = screenSpaceFrom, to = screenSpaceTo });
	}
}
