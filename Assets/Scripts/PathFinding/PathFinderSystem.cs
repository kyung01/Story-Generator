using GameEnums;
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
			cells[x][y].weight = weight;
		}
		public void addCellWeight(int x, int y, float weight)
		{
			cells[x][y].weight += weight;
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

		private void hprAddCell(List<KPath> availablePaths, KPath path, KPath neighbour)
		{
			float distanceWeight = (neighbour.vec2 - path.vec2).sqrMagnitude;

			if(neighbour.id == pathID)
			{
				return;
			}

			neighbour.reset(pathID,
				//path.weight +
				//cells[path.x][path.y].Weight +
				cells[neighbour.x][neighbour.y].Weight 
				+ distanceWeight
				, null, null);

			//var neighBourCopy = neighbour.GetCopy();
			//removeThisFromNeighbours(neighBourCopy, path);
			neighbour.weight += path.weight;
			neighbour.before = path;
			availablePaths.Add(neighbour);
		}

		void hprUpdateNeighbouringPaths(KPath path, List<KPath> availablePaths)
		{

			for (int i = 0; i < path.neighbours.Count; i++)
			{
				var neighbour = path.neighbours[i];
				var neighbourCell = cells[neighbour.x][neighbour.y];
				if ( neighbour.id != pathID && ! neighbourCell.isOccupied)
				{
					
					hprAddCell(availablePaths, path, neighbour);
					
				}
				else
				{
					//the neighbouring path is already updated 
				}
			}

			for (int i = 0; i < path.neighboursDiagonal.Count; i++)
			{
				var neighbour_willMoveTo = path.neighboursDiagonal[i];
				var neighbourCell = cells[neighbour_willMoveTo.x][neighbour_willMoveTo.y];
				int moveX = neighbour_willMoveTo.x - path.x;
				int moveY = neighbour_willMoveTo.y - path.y;
				var neighbourCell_corner_X = cells[neighbour_willMoveTo.x - moveX][neighbour_willMoveTo.y];
				var neighbourCell_corner_Y = cells[neighbour_willMoveTo.x][neighbour_willMoveTo.y - moveY];

				if (
					( neighbour_willMoveTo.id != pathID) 
					&& !neighbourCell.isOccupied &&
					!neighbourCell_corner_X.isOccupied && !neighbourCell_corner_Y.isOccupied)
				{
					//Normally path -> the neighbouring path has distance of 1 or sqrt(2) but sometimes, a teleporter can be created
					//And teleporter can be used to shrink the distance between two points

					hprAddCell(availablePaths, path, neighbour_willMoveTo);
					

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
				var copysNeighbourX = neighbourCopy.neighbours[i];
				while (path != null)
				{
					if (neighbourCopy.neighbours[i].x == path.x && neighbourCopy.neighbours[i].y == path.y)
					{
						neighbourCopy.neighbours.RemoveAt(i);
						break;
					}
					path = null;
					//path = path.before;
				}
				foreach(var startingPathsNeighbour in startingPath.neighbours)
				{
					if(copysNeighbourX == startingPathsNeighbour)
					{
						neighbourCopy.neighbours.Remove(copysNeighbourX);
						break;
					}

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
				{
					chosenPath = neighbour;
				}
				else
				{
				}
			}
			return chosenPath;
		}

		void writeDebugLog(object obj)
		{
			this.debugLog.Add((string)obj);
		}
		void printDebugLogIntoUnity()
		{
			for(int i = 0; i< debugLog.Count; i++)
			{
				Debug.Log(this.debugLog[i]);
			}
		}
		void whileLoop(KPath path, Vector2 pathDestination, PathObject a, PathObject b)
		{
			writeDebugLog("beginning a while loop");
			availablePaths.Clear();
			int whileBKP = 5000; // While loop breaking point
			bool pathFound = false;
			while (!pathFound && path != null)
			{
				if (whileBKP-- <= 0)
				{
					printDebugLogIntoUnity();
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

				/*
				 * Order in which things must work for it to work properly
				 *			1. Check appropriate nearby cells then add it to the list
				 *			2. Select the best cell to go
				 *			3. Take that cell
				 */

				hprUpdateNeighbouringPaths(path, availablePaths);
				KPath nextPath = hprChooseTheBestPath(availablePaths, pathDestination);
				try
				{
					writeDebugLog("number of availablePaths after lookup " + availablePaths.Count + " " + nextPath.x + " " + nextPath.y);

				}
				catch
				{
					Debug.LogError(availablePaths);

				}
				availablePaths.Remove(nextPath);


				if (nextPath == null)
				{
					writeDebugLog("Dead end reached");
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
			path.reset(pathID, cells[path.x][path.y].weight, null, null);
			return path;
		}
		List<string> debugLog = new List<string>();
		void startDebugLog()
		{
			debugLog = new List<string>();
		}
		public KPath getPath(Vector2 posBegin, Vector2 pathDestination)
		{
			var path = getNewPathSeed(posBegin, pathDestination);
			startDebugLog();
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
