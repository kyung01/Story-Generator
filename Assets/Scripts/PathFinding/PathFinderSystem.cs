using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinder
{

	public class PathFinderSystem
	{
		int pathID = 0;

		List<List<Cell>> cells = new List<List<Cell>>();
		List<List<KPath>> paths = new List<List<KPath>>();
		List<KPath> availablePaths = new List<KPath>();
		int maxWidth = 0;
		int maxHeight = 0;
		public void Init(int width, int height)
		{
			this.maxWidth = width;
			this.maxHeight = height;
			for (int i = 0; i < width; i++)
			{
				cells.Add(new List<Cell>());
				for (int j = 0; j < height; j++)
				{
					cells[i].Add(new Cell());
				}
			}
			for (int i = 0; i < width; i++)
			{
				paths.Add(new List<KPath>());
				for (int j = 0; j < height; j++)
				{
					paths[i].Add(new KPath(i, j));
				}
			}
			Vector2[] neighbourIndexs = new Vector2[4] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, +1) };
			Vector2[] neighbourDiagonalIndexs = new Vector2[4] { new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1) };

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					List<KPath> neighbours = paths[i][j].neighbours;
					for (int k = 0; k < 4; k++)
					{
						int x = i + (int)neighbourIndexs[k].x;
						int y = j + (int)neighbourIndexs[k].y;
						if (x < 0 || x >= width || y < 0 || y >= height)
						{
							continue;
						}
						neighbours.Add(paths[x][y]);
					}
				}
			}
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					List<KPath> neighbours = paths[i][j].neighboursDiagonal;
					for (int k = 0; k < 4; k++)
					{
						int x = i + (int)neighbourDiagonalIndexs[k].x;
						int y = j + (int)neighbourDiagonalIndexs[k].y;
						if (x < 0 || x >= width || y < 0 || y >= height)
						{
							continue;
						}
						neighbours.Add(paths[x][y]);
					}
				}
			}
		}

		public bool isCellOccupied(int x, int y)
		{
			return cells[x][y].isOccupied;
		}

		public void setCellWeight(int x, int y, float weight)
		{
			cells[x][y].Weight = weight;
		}
		public void addCellWeightInt(int x, int y, int weight)
		{
			cells[x][y].weightInt += weight;
		}
		public void setCellOccupied(int x, int y, bool isOccupied)
		{
			cells[x][y].isOccupied = isOccupied;
		}

		public void Log(object obj)
		{
			//Debug.Log(obj);
		}

		float hprCalculatePathWeight(Vector2 from, Vector2 to, Vector2 destination)
		{
			return (to - from).SqrMagnitude() + (destination - to).SqrMagnitude();
		}
		bool hprIsDestinationReached(Vector2 path, Vector2 destination)
		{
			return (int)path.x == (int)destination.x && (int)path.y == (int)destination.y;
		}
		void hprUpdateNeighbouringPaths(KPath path, List<KPath> availblePaths)
		{

			for (int i = 0; i < path.neighbours.Count; i++)
			{
				var neighbour = path.neighbours[i];
				var neighbourCell = cells[neighbour.x][neighbour.y];
				if (neighbour.id != pathID)
				{
					if (neighbourCell.isOccupied)
					{
						continue;
					}
					//Normally path -> the neighbouring path has distance of 1 or sqrt(2) but sometimes, a teleporter can be created
					//And teleporter can be used to shrink the distance between two points

					float distanceWeight = (neighbour.vec2 - path.vec2).sqrMagnitude;
					//Debug.Log("resetting a path with...");
					//Debug.Log(cells[path.x][path.y].Weight + " " + cells[neighbour.x][neighbour.y].Weight + " " + distanceWeight);


					neighbour.reset(pathID,
						//path.weight +
						//cells[path.x][path.y].Weight +
						cells[neighbour.x][neighbour.y].Weight + distanceWeight, null, null);

					var neighBourCopy = neighbour.GetCopy();
					removeThisFromNeighbours(neighBourCopy, path);
					neighBourCopy.weight += neighbour.weight;
					neighBourCopy.before = path;
					availblePaths.Add(neighBourCopy);

				}
				else
				{
					//the neighbouring path is already updated 
				}
			}
			for (int i = 0; i < path.neighboursDiagonal.Count; i++)
			{
				var neighbourD = path.neighboursDiagonal[i];
				var neighbourCell = cells[neighbourD.x][neighbourD.y];
				int moveX = neighbourD.x - path.x;
				int moveY = neighbourD.y - path.y;
				var neighbourCell_corner_X = cells[neighbourD.x - moveX][neighbourD.y];
				var neighbourCell_corner_Y = cells[neighbourD.x][neighbourD.y - moveY];

				if (neighbourD.id != pathID && !neighbourCell.isOccupied &&
					!neighbourCell_corner_X.isOccupied && !neighbourCell_corner_Y.isOccupied)
				{
					//Normally path -> the neighbouring path has distance of 1 or sqrt(2) but sometimes, a teleporter can be created
					//And teleporter can be used to shrink the distance between two points

					//Debug.Log("resetting a path(diagonal) with...");
					//Debug.Log(cells[path.x][path.y].Weight + " " + cells[neighbourD.x][neighbourD.y].Weight + " " + (neighbourD.vec2 - path.vec2).sqrMagnitude);

					neighbourD.reset(pathID,
						//path.weight+
						//cells[path.x][path.y].Weight +
						cells[neighbourD.x][neighbourD.y].Weight + (neighbourD.vec2 - path.vec2).sqrMagnitude, null, null);
					
					var neighBourDCopy = neighbourD.GetCopy();
					removeThisFromNeighbours(neighBourDCopy, path);
					neighBourDCopy.weight += neighbourD.weight;
					neighBourDCopy.before = path;
					availblePaths.Add(neighBourDCopy);

				}
				else
				{
					//the neighbouring path is already updated 
				}
			}
		}

		private void removeThisFromNeighbours(KPath neighbourCopy, KPath startingPath)
		{
			for(int i = neighbourCopy.neighbours.Count -1; i >= 0; i--)
			{
				KPath path = startingPath;
				while(path != null)
				{
					if (neighbourCopy.neighbours[i].x == path.x && neighbourCopy.neighbours[i].y == path.y)
					{
						neighbourCopy.neighbours.RemoveAt(i);
						break;
					}
					path = path.before;
				}
				
			}
		}

		KPath hprChooseTheBestPath(List<KPath> availblePaths, Vector2 destination)
		{
			KPath chosenPath = null;
			for (int i = 0; i < availblePaths.Count; i++)
			{
				//Debug.Log("Testing Path at  : " + availblePaths[i].vec2);
				var neighbour = availblePaths[i];
				//check whether this is a valid neighbour
				float lowestWeightSoFar = (chosenPath == null) ? float.MaxValue : chosenPath.weight + (chosenPath.vec2 - destination).magnitude;
				if (neighbour.weight + (neighbour.vec2 - destination).magnitude < lowestWeightSoFar)
				{/*
				if(chosenPath!=null)
					
				Debug.Log("This path was better than : " + chosenPath.vec2 + " because  " + 
					"cost ("+ (neighbour.weight + (neighbour.vec2 - destination).sqrMagnitude) + ") was less than " + lowestWeightSoFar

					);
					 * */
					chosenPath = neighbour;
				}
				else
				{

					//Debug.Log("This path was NOT better than : " + chosenPath.vec2 + " because  " + "cost (" + (neighbour.weight + (neighbour.vec2 - destination).sqrMagnitude) + ") was less than " + lowestWeightSoFar);
				}
			}
			//Debug.Log("Chosen Best Path : " + chosenPath.vec2);
			return chosenPath;
		}


		void whileLoop(KPath path, Vector2 pathDestination, PathObject a, PathObject b)
		{
			availablePaths.Clear();
			int whileBKP = 2000; // While loop breaking point
			bool pathFound = false;
			while (!pathFound && path != null)
			{
				if (whileBKP-- <= 0)
				{
					Debug.LogError(this + "While Loop Limit Reached");
					break;
				}
				//update the current path
				if (path.id != pathID)
				{
					//its an old path
				}
				//check whether at the destination
				if (hprIsDestinationReached(path, pathDestination) ||
					(a != null && b != null && PathObject.WillPhysicallyBeInteractable(a, path.vec2, b, pathDestination))
					)
				{
					//we found the path
					Log("we found the path " + path.vec2);
					while (path.before != null)
					{
						if (whileBKP-- <= 0)
						{

							Log("BKB Reached 2 ");
							break;
						}
						Log(path.vec2);
						path.before.after = path;
						path = path.before;
					}

					Log("Path finishes at " + path.vec2);
					break;
				}
				//check whether the current path is "deadend value"
				if (path.weight == float.MaxValue)
				{
					Log("Dead end");
					break;
				}
				//find the next possible routes
				//for each possible route calculate the score
				//choose the best possible 

				hprUpdateNeighbouringPaths(path, availablePaths);
				KPath nextPath = hprChooseTheBestPath(availablePaths, pathDestination);
				availablePaths.Remove(nextPath);


				if (nextPath == null)
				{
					//this is a dead end
					Log("Dead end");
					path.weight = float.MaxValue;
					path = path.before;
				}
				else
				{
					//nextPath.before = path; It is already updated during the update neighbouringpaths cycle
					path = nextPath;
				}

			}
			availablePaths.Clear();
		}
		KPath getNewPathSeed(Vector2 posBegin, Vector2 pathDestination)
		{

			//check whether it is valid position
			if ((posBegin.x < 0 || posBegin.x >= maxWidth || posBegin.y < 0 || posBegin.y >= maxHeight) ||
				(pathDestination.x < 0 || pathDestination.x >= maxWidth || pathDestination.y < 0 || pathDestination.y >= maxHeight)
				)
			{
				Debug.Log("getNewPathSeed Invalid position " + posBegin + " , " + pathDestination);
				return null;
			}
			pathID++; //update the path ID
			KPath path = paths[Mathf.RoundToInt(posBegin.x)][Mathf.RoundToInt(posBegin.y)];
			path.reset(pathID, 0, null, null);
			return path;
		}

		public KPath getPath(Vector2 posBegin, Vector2 pathDestination)
		{
			var path = getNewPathSeed(posBegin, pathDestination);
			whileLoop(path, pathDestination, null, null);
			int BKP = 100;
			//Debug.Log("getPathWithPathObjects " );

			{
				var iterateP = path.after;
				while (iterateP != null)
				{
					if (true)
					{
						var cell = cells[iterateP.x][iterateP.y];
						///Debug.Log("Path " + cell.isOccupied);
					}
					iterateP = iterateP.after;
				}
			}

			while (path.before != null)
			{
				if (BKP-- <= 0)
				{
					Debug.Log("BKP 3 Reached");
					break;
				}
				//Print the path backward by cells
				Debug.Log("Path " + path.x + " " + path.y);
				path.before.after = path;
				path = path.before;
			}
			if (path.x == (int)posBegin.x && path.y == (int)posBegin.y)
			{
				path = path.after;
				//if (path != null) Debug.Log("returned path after " + path.x + " " + path.y);
			}

			return path;
		}
		public KPath getPathWithPathObjects(PathObject tarveler, PathObject target)
		{
			Debug.Log("getPathWithPathObjects() from " + tarveler + " " + tarveler.PositionInt + " to " + target + " " + target.PositionInt);
			var path = getNewPathSeed(tarveler.PositionInt, target.PositionInt);
			whileLoop(path, target.PositionInt, tarveler, target);
			int BKP = 100;
			//Debug.Log("getPathWithPathObjects " );

			{
				var iterateP = path.after;
				while (iterateP != null)
				{
					if (true)
					{
						var cell = cells[iterateP.x][iterateP.y];
						//Debug.Log("Path " + cell.isOccupied);
					}
					iterateP = iterateP.after;
				}
			}

			while (path.before != null)
			{
				if (BKP-- <= 0)
				{
					Debug.Log("BKP 3 Reached");
					break;
				}
				//Print the path backward by cells
				Debug.Log("Path " + path.x + " " + path.y);
				path.before.after = path;
				path = path.before;
			}
			if (path.x == (int)tarveler.X && path.y == (int)tarveler.Y)
			{
				path = path.after;
				//if(path!=null)Debug.Log("returned path after " + path.x + " " + path.y);
			}

			return path;
		}
	}

}
