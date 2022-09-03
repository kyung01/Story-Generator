using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;


public class CollisionModel {
	Vector2 pos;
	float weight;
}

public class Structure : Thing
{
	bool isInstalled = false;
	public bool IsInstalled { get { return this.isInstalled; } }

	int width = 1;
	int height = 1;
	public int Width { get { return this.width; } }
	public int Height { get { return this.height; } }


	public Structure():base(TYPE.STRUCTURE)
	{
		this.T = TYPE.STRUCTURE;
	}

	public void Install()
	{
		this.isInstalled = true;
	}
	public void Uninstall()
	{
		this.isInstalled = false;
	}
}
