using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CAIModel
{

	public List<Vector2> collisionMap;
	public List<Vector3> avoidanceMap;
	public List<Vector2> installationMap;

	public void setCollisionMap(params Vector2[] data)
	{
		collisionMap = new List<Vector2>(data.ToList());

	}
	public void setInstallationMap(params Vector2[] data)
	{
		installationMap = new List<Vector2>(data.ToList());
	}
	public void setAvoidanceMap(params Vector3[] data)
	{
		avoidanceMap = new List<Vector3>(data.ToList());

	}
}