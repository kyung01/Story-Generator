using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Structure : Thing
{
	bool isInstalled = false;
	public bool IsInstalled { get { return this.isInstalled; } }

	int width = 1;
	int height = 1;
	public int Width { get { return this.width; } }
	public int Height { get { return this.height; } }
	public Structure()
	{
		this.type = TYPE.STRUCTURE;
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
