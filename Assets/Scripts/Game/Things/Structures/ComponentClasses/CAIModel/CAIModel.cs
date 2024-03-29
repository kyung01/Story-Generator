﻿using GameEnums;
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

	public List<Vector2> GetCollisionMap(ThingWithPhysicalPresence thing)
	{
		return getRotatedVersion(this.collisionMap, thing);
	}
	public List<Vector2> GetInstallationMap(ThingWithPhysicalPresence thing)
	{
		return getRotatedVersion(this.installationMap, thing);
	}

	public List<Vector3> GetAvoidanceMap(ThingWithPhysicalPresence thing)
	{
		return getRotatedVersion(this.avoidanceMap, thing);
	}

	List<Vector2> getRotatedVersion(List<Vector2> vec2List, ThingWithPhysicalPresence thing)
	{
		List<Vector2> newVec2List = new List<Vector2>();
		foreach (var v in vec2List)
		{
			newVec2List.Add(EasyMath.rotate_point(v, (int)thing.DirectionFacing * 45.0f) + thing.XY);
		}
		return newVec2List;

	}

	List<Vector2> getRotatedVersion(List<Vector2> vec2List, int rotation)
	{
		List<Vector2> newVec2List = new List<Vector2>();
		foreach(var v in vec2List)
		{
			newVec2List.Add(EasyMath.rotate_point( v,rotation*45.0f));
		}
		return newVec2List;

	}
	List<Vector3> getRotatedVersion(List<Vector3> vec3List, ThingWithPhysicalPresence thing)
	{
		var newVec3List = new List<Vector3>();
		foreach (var v in vec3List)
		{
			var p = EasyMath.rotate_point(new Vector2(v.x, v.y), (int)thing.DirectionFacing * 45.0f) + thing.XY;
			newVec3List.Add(new Vector3(p.x, p.y, v.z));
		}
		return newVec3List;
	}
	List<Vector3> getRotatedVersion(List<Vector3> vec3List, int rotation)
	{
		var newVec3List = new List<Vector3>();
		foreach (var v in vec3List)
		{
			var p = EasyMath.rotate_point(new Vector2(v.x, v.y), rotation * 45.0f);
			newVec3List.Add(new Vector3(p.x,p.y,v.z) );
		}
		return newVec3List;
	}
}