using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EasyMath
{
	//https://stackoverflow.com/questions/2259476/rotating-a-point-about-another-point-2d

	static public Vector2 rotate_point(Vector2 p, float angleRaw)
	{
		float angle = -angleRaw * Mathf.PI / 180.0f;

		float s = Mathf.Sin(angle);
		float c = Mathf.Cos(angle);

		// translate point back to origin:
		//p.x -= cx;
		//p.y -= cy;

		// rotate point
		float xnew = p.x * c - p.y * s;
		float ynew = p.x * s + p.y * c;

		// translate point back:
		p.x = xnew;// + cx;
		p.y = ynew;// + cy;
		return p;
	}
}