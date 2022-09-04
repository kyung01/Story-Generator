using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CAIModel
{

	List<Vector2> collisionMap = new List<Vector2>();
	List<Vector3> avoidanceMap = new List<Vector3>();
	List<Vector2> installationMap = new List<Vector2>();
	public CAIModel()
	{

	}
	public CAIModel(List<Vector2> collision, List<Vector2> installation, List<Vector3> avoidance)
	{
		this.collisionMap = collision;
		this.avoidanceMap = avoidance;
		this.installationMap = installation;
	}

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
	//https://stackoverflow.com/questions/2259476/rotating-a-point-about-another-point-2d
	Vector2 rotate_point(Vector2 p,float angleRaw)
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
		//p.x = xnew + cx;
		//p.y = ynew + cy;
		return p;
	}
	public List<Vector2> GetCollisionMap(int rotation)
	{
		return getRotatedVersion(this.collisionMap, rotation);
	}
	public List<Vector2> GetInstallationMap(int rotation)
	{
		return getRotatedVersion(this.installationMap, rotation);
	}

	public List<Vector3> GetAvoidanceMap(int rotation)
	{
		return getRotatedVersion(this.avoidanceMap, rotation);
	}
	List<Vector2> getRotatedVersion(List<Vector2> vec2List, int rotation)
	{
		List<Vector2> newVec2List = new List<Vector2>();
		foreach(var v in vec2List)
		{
			newVec2List.Add(rotate_point( v,rotation*45.0f));
		}
		return newVec2List;

	}
	List<Vector3> getRotatedVersion(List<Vector3> vec3List, int rotation)
	{
		var newVec3List = new List<Vector3>();
		foreach (var v in vec3List)
		{
			var p = rotate_point(new Vector2(v.x, v.y), rotation * 45.0f);
			newVec3List.Add(new Vector3(p.x,p.y,v.z) );
		}
		return newVec3List;
	}
}