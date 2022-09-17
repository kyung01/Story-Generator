
using System.Collections;
using System.Collections.Generic;
using StoryGenerator.NTerrain;
using StoryGenerator.World.Things.Actors;
using UnityEngine;

using GameEnums;
namespace StoryGenerator.World
{
	#region helperClass
	
	public class ThingXY {
		public Thing thing;
		public int xOld, yOld;
		public int x, y;
		public ThingXY(Thing thing, int x, int y)
		{
			this.thing = thing;
			this.x = x;
			this.y = y;
			xOld = x;
			yOld = y;
		}
		public bool IsPositionUpdated()
		{
			int xNew = Mathf.RoundToInt(thing.X);
			int yNew = Mathf.RoundToInt(thing.Y);
			if (this.x == xNew && this.y == yNew) return false;
			xOld = x;
			yOld = y;
			x = xNew;
			y = yNew;
			return true;
		}
	}

	#endregion

	static public class WorldStatic {
		static public void Raise(
			this List<StoryGenerator.World.World.DEL_ON_THING_MOVED> list, 
			World world, Thing thing, int xBefore, int yBefore, int xNew, int yNew)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				//Debug.Log("WorldStatic -> Raising DelOnThingMoved " + i + " / " +  list.Count);
				list[i](world, thing, xBefore, yBefore, xNew, yNew);
			}

			/*
			for (int i = 0; i < list.Count; i++)
			{
				list[i](world, thing, xBefore, yBefore, xNew, yNew);
			}
			 * */
		}
	}

	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class World
	{
		public delegate void DEL_ON_THING_MOVED(World world, Thing thing, int xBefore, int yBefore, int xNew, int yNew);
		public List<DEL_ON_THING_MOVED> OnThingMoved = new List<DEL_ON_THING_MOVED>();

		const int WEIGHT_DOOR = 3;
		const int WEIGHT_AVOID_ANIMAL = 3;

		public delegate void DEL_THING_ADDED(Thing thing);
		public List<DEL_THING_ADDED> OnThingAdded = new List<DEL_THING_ADDED>();

		public int width = 100;
		public int height = 100;

		public PathFinder.PathFinderSystem pathFinder = new PathFinder.PathFinderSystem();
		public NTerrain.TerrainSystem terrain;
		public ZoneOrganizer zoneOrganizer;

		public List<Thing> allThings = new List<Thing>();
		public List<Thing>[] things;
		public List<Thing>[] thingsMoving;

		public List<Team> teams = new List<Team>();
		Team playerTeam;

		System.DateTime time;
		public System.DateTime Time {  get { return this.time; } }

		#region property


		#endregion

		#region getter setter

		public Team PlayerTeam { get { return this.playerTeam; } }


		#endregion
		
		#region Handlers

		void hprInitThingsIndex(ref List<Thing>[] things)
		{
			things = new List<Thing>[width * height];
			//structures = new List<Structure>[width * height];

			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{
					int index = i + j * width;
					things[index] = new List<Thing>();
					//structures[index] = new List<Structure>();
				}
		}

		
		bool hprIsWithinRange(float n, float minInclusive, float maxExclusive)
		{
			return n >= minInclusive && n < maxExclusive;
		}

		bool hprIsSameVec2(Vector2 a, Vector2 b)
		{
			int aX = Mathf.RoundToInt(a.x);
			int aY = Mathf.RoundToInt(a.y);
			int bX = Mathf.RoundToInt(b.x);
			int bY = Mathf.RoundToInt(b.y);
			return aX == bX && aY == bY;
		}

		Vector2 hprGetApprorpiateRandomPosition()
		{
			float x = 0, y = 0;
			bool isAppropriateLocationFound = false;
			while (!isAppropriateLocationFound)
			{
				x = Random.Range(0, width);
				y = Random.Range(0, height);
				var terrainType = terrain.pieces[(int)x + (int)y * width].Type;
				if (terrainType == Piece.KType.MOUNTAIN || terrainType == Piece.KType.WATER_DEEP || terrainType == Piece.KType.WATER_SHALLOW)
				{
					continue;
				}
				break;
			}
			return new Vector2(x, y);
		}

		void hdrStructurePositionChangedAdd(Thing thing, int x, int y)
		{
			Frame s = (Frame)thing;
			for (int w = 0; w < s.Width; w++)
			{
				for (int h = 0; h < s.Height; h++)
				{
					things[x + w + (y + h) * width].Add(thing);

				}
			}
		}

		void hdrPosIdxChg_Structure(Thing thing, int xBefore, int yBefore, int xNew, int yNew)
		{
			Frame s = (Frame)thing;
			for (int w = 0; w < s.Width; w++)
			{
				for (int h = 0; h < s.Height; h++)
				{
					things[xBefore + w + (yBefore + h) * width].Remove(thing);

				}
			}
			hdrStructurePositionChangedAdd(thing, xNew, yNew);
		}

		void hdrPosIdxChg_ThingsMoving(Thing thing, int xBefore, int yBefore, int xNew, int yNew)
		{
			//hdrAnimalMovedSoShouldWeightOnMap
			pathFinder.addCellWeightInt(xBefore, yBefore, -WEIGHT_AVOID_ANIMAL);
			pathFinder.addCellWeightInt(xNew, yNew, WEIGHT_AVOID_ANIMAL);
			//Debug.Log("Hdr Thing's position is changed " + thing + xBefore+ " "  + yBefore + " -> " +xNew + " " + yNew);
			thingsMoving[xBefore + yBefore * width].Remove(thing);
			thingsMoving[xNew + yNew * width].Add(thing);

		}

		private void hdrPosIdxChg_All(Thing thing, int xBefore, int yBefore, int xNew, int yNew)
		{
			OnThingMoved.Raise(this, thing, xBefore, yBefore, xNew, yNew);
			//this.zoneOrganizer.ThingMovedFrom(this, thing, xBefore, yBefore);
			//this.zoneOrganizer.ThingMovedTo(this, thing, xNew, yNew);

		}

		void hdrPosIdxChg_Things(Thing thing, int xBefore, int yBefore, int xNew, int yNew)
		{
			//Debug.Log("Hdr Thing's position is changed " + thing + xBefore+ " "  + yBefore + " -> " +xNew + " " + yNew);
			things[xBefore + yBefore * width].Remove(thing);
			things[xNew + yNew * width].Add(thing);

		}

		
		#endregion

		public World()
		{
			playerTeam = new Team();
			teams.Add(playerTeam);
			terrain = new NTerrain.TerrainSystem();
			zoneOrganizer = new ZoneOrganizer(this);
			time = new System.DateTime(2000, 1, 1);

			Init0_DefaultVariables();
			Init1_Terrain();
			//Init2_Animals();
			//Init3_TestingGround();
		}

		void Init0_DefaultVariables()
		{
			pathFinder.Init(width, height);
			hprInitThingsIndex(ref things);
			hprInitThingsIndex(ref thingsMoving);
		}
		
		void Init1_Terrain()
		{
			float chanceToSpawn = 10;
			//initialize the world 
			terrain.Init(width, height, probToBeMountainGround : 0, probClay:0);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					if (!terrain.GetPieceAt(i, j).IsWalkable)
					{
						pathFinder.setCellOccupied(i, j, true);
					}
				}
			}

			for (int i = 0; i < width; i++) for (int j = 0; j < height; j++)
				{
					if (terrain.GetPieceAt(i, j).Type == Piece.KType.MOUNTAIN)
					{
						//mountain is here
						continue;
					}
					var t = terrain.pieces[i + j * width];
					Thing thing = null;
					if (Random.Range(0, 100) > chanceToSpawn) continue;

					switch (t.Type)
					{
						case Piece.KType.FERTILE:
						case Piece.KType.DIRT:
							//grass bush or tree
							Thing[] vegitarians = new Thing[] {
								ThingSheet.GetGrass(), ThingSheet.GetBush(),ThingSheet.GetRock()};
							thing = vegitarians[Random.Range(0, vegitarians.Length)];
							break;
						case Piece.KType.ROCKY:
							thing = ThingSheet.GetRock();
							break;
						case Piece.KType.CLAY:
							thing = ThingSheet.GetReed();
							break;
						case Piece.KType.MOUNTAIN:
						case Piece.KType.WATER_SHALLOW:
						case Piece.KType.WATER_DEEP:
							break;
						default:
							break;
					}
					if (thing == null) continue;

					var obj = thing;
					obj.SetPosition(i, j);

					AddThingAndInit(obj);
					//things[i+j*width] 
				}

		}

		void Init2_Animals()
		{
			int numRabbit = 0;
			int numBear = 0;
			int numHumans = 1;

			for (int i = 0; i < numRabbit; i++)
			{
				Vector2 randomPos = hprGetApprorpiateRandomPosition();
				Thing rabbit = ThingSheet.Rabbit();
				rabbit.SetPosition(randomPos.x, randomPos.y);
				AddThingAndInit(rabbit);
				//allThings.Add(rabbit);
				//things[(int)randomPos.x + (int)randomPos.y * width].Add(rabbit);
				//thingsToKeepTracking.Add(new ThingXY( rabbit,(int)randomPos.x, (int)randomPos.y));

			}

			for (int i = 0; i < numBear; i++)
			{

				Vector2 randomPos = hprGetApprorpiateRandomPosition();

				Thing bear = ThingSheet.GetBear();
				bear.SetPosition(randomPos);
				AddThingAndInit(bear);
				//allThings.Add(bear);
				//things[(int)randomPos.x + (int)randomPos.y * width].Add(bear);
				//thingsToKeepTracking.Add(new ThingXY(bear, (int)randomPos.x, (int)randomPos.y));

			}
			for (int i = 0; i < numHumans; i++)
			{

				Vector2 randomPos = hprGetApprorpiateRandomPosition();

				Thing Human = (ActorBase) ThingSheet.Human();
				Human.SetPosition(randomPos);
				AddThingAndInit(Human);
				//allThings.Add(Human);
				//things[(int)randomPos.x + (int)randomPos.y * width].Add(Human);
				//thingsToKeepTracking.Add(new ThingXY(Human, (int)randomPos.x, (int)randomPos.y));

				playerTeam.AddThing(Human, Team.ThingRole.HOWLER);
				playerTeam.WorkManager.AddWorker(new Worker((ActorBase)Human));

			}
		}

		public void LoadTestingSceneaerio()
		{
			zoneOrganizer.BuildHouseZone(0, 0, 11, 11);
			int numHumans = 3;
			for (int i = 0; i < numHumans; i++)
			{
				Thing Human = (ActorBase)ThingSheet.Human();
				Human.SetPosition(new Vector2(5+i,5));
				AddThingAndInit(Human);
			}

			AddThingAndInit(ThingSheet.GetBed().SetFacingDirection(Direction.DOWN).SetPosition(1, 1));
			AddThingAndInit(ThingSheet.GetBed().SetFacingDirection(Direction.DOWN).SetPosition(9, 9));
		}

		//Private methods
		#region Private methods



		#endregion

		//Public methods
		/// <summary>
		/// Add thing to the world and initiate it
		/// </summary>
		public void AddThingAndInit(Thing thing)
		{
			thing.Init(this);
			this.allThings.Add(thing);
			//this.things[thing.X_INT + width * thing.Y_INT].Add(thing);

			thing.OnPositionIndexChanged.Add(hdrPosIdxChg_All);
			
			if (thing is Frame)
			{
				switch (thing.Category) {
					case ThingCategory.WALL:
						pathFinder.setCellOccupied(thing.X_INT, thing.Y_INT, true);
						break;
					case ThingCategory.DOOR:
						pathFinder.addCellWeightInt(thing.X_INT, thing.Y_INT, WEIGHT_DOOR);
						break;
					case ThingCategory.ROOF:
						break;
					default:
						Debug.Log("World AddThingAndInit Error : Cannot find cateogry of the structure");
						break;
				}
			}
			
			if(thing is Structure)
			{
				Debug.Log("Adding a structure");
				var structure = ((Structure)thing);
				var collisionMap = structure.CAIModel.GetCollisionMap((ThingWithPhysicalPresence) thing);
				var avoidanceMap = structure.CAIModel.GetAvoidanceMap((ThingWithPhysicalPresence) thing);

				foreach (var c in collisionMap)
				{
					Debug.Log("Collision Map occupied at " +c);
					pathFinder.setCellOccupied((int)c.x, (int)c.y, true);
				}
				foreach (var a in avoidanceMap)
				{
					pathFinder.addCellWeight( (int)a.x,  (int)a.y, a.z);
				}

			}

			if (thing is Frame )
			{
				//Structure/frame do not change their position  

				//hdrStructurePositionChangedAdd(thing, Mathf.RoundToInt(thing.X), Mathf.RoundToInt(thing.Y));
				//thing.OnPositionIndexChanged.Add(hdrPosIdxChg_Structure);
			}
			else
			{
				things[Mathf.RoundToInt(thing.X) + Mathf.RoundToInt(thing.Y) * width].Add(thing);
				thing.OnPositionIndexChanged.Add(hdrPosIdxChg_Things);

			}
			if (thing.moduleBody != null)
			{
				thingsMoving[Mathf.RoundToInt(thing.X) + Mathf.RoundToInt(thing.Y) * width].Add(thing);
				pathFinder.addCellWeightInt(thing.X_INT, thing.Y_INT, WEIGHT_AVOID_ANIMAL);

				//thing.OnPositionIndexChanged.Add(hdrAnimalMovedSoShouldWeightOnMap);
				thing.OnPositionIndexChanged.Add(hdrPosIdxChg_ThingsMoving);
			}
			OnThingAdded.Raise(thing);
		}

		public bool IsFrameAt(int x, int y)
		{
			var things = GetThingsAt(x, y);
			foreach (Thing t in things)
			{
				if (t is Frame)
				{
					var frame = (Frame)t;
					if (frame.Category == ThingCategory.ROOF)
					{
						//Roof is not considered as a structure that blocks building of another structure
						continue;
					}
					if (frame.IsInstalled)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsRoofAt(int x, int y)
		{
			var things = GetThingsAt(x, y);
			foreach (Thing t in things)
			{
				if (t.Category == ThingCategory.ROOF)
				{
					if (((Frame)t).IsInstalled)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void EmptySpotAndOccupy(int x, int y)
		{
			this.pathFinder.setCellOccupied(x, y, true);
			EmptySpot(x, y);

		}
		public void EmptySpot(int x, int y)
		{
			//Debug.Log("ClearspotForConstruction");
			var things = GetThingsAt(x, y);
			int xMin = x, yMin = y, xMax = x, yMax = y;
			//bool isFoundAnEmptySpot = false;
			//int emptyX = 0, emptyY = 0;

			List<Vector2> availablePositions = new List<Vector2>();

			while (availablePositions.Count == 0)
			{
				availablePositions.Clear();
				
				for (int positionX = xMin; positionX <= xMax; positionX++)
				{
					for (int positionY = yMin; positionY <= yMax; positionY++)
					{
						if (positionX == x && positionY == y)
						{
							continue;
						}
						if (!IsWalkableAt(positionX, positionY))
						{
							continue;
						}
						if (IsFrameAt(positionX, positionY))
						{
							continue;
						}
						availablePositions.Insert(Random.Range(0, availablePositions.Count + 1), new Vector2(positionX, positionY));
						
					}
				}
				
				
				xMin = Mathf.Max(0, xMin - 1);
				xMax = Mathf.Min(this.width - 1, xMax + 1);
				yMin = Mathf.Max(0, yMin - 1);
				yMax = Mathf.Min(this.height - 1, yMax + 1);
			}

			int availablePositionIndex = 0;
			for (int i = 0; i < things.Count; i++)
			{
				var e = availablePositions[availablePositionIndex++ % availablePositions.Count];

				   //Debug.Log(i+ "/" + things.Count +" Setting position of " + things[i] + " to " + new Vector2(emptyX, emptyY));
				   things[i].SetPosition((int)e.x,(int)e.y);
			}
			

		}
		List<Item> sortItems(List<Thing> things)
		{
			List<Item> items = new List<Item>();
			int n = 0;
			foreach (var t in things) { if (t is Item) items.Add((Item)t); }
			return items;
		}
		public List<Thing>	GetThingsAt(int x, int y)
		{
			List<Thing> list = new List<Thing>();
			foreach (var t in things[x + width * y])
			{
				list.Add(t);
			}
			return list;
		}

		public List<Thing>	GetThingsMovingAt(int x, int y)
		{
			List<Thing> list = new List<Thing>();
			foreach (var t in thingsMoving[x + width * y])
			{
				list.Add(t);
			}
			return list;
		}
				
		public List<Thing>	GetEdibleThings(Thing thing)
		{
			return new List<Thing>();
		}

		public List<Thing>	GetSightableThings(Thing thing, float sight)
		{
			Debug.Log("GetSightableThings "+ thing.XY + " " + sight);
			List<Thing> things = new List<Thing>();
			float xMin = Mathf.RoundToInt( thing.X - sight);
			float yMin =  Mathf.RoundToInt(thing.Y - sight);
			float xMax =  Mathf.RoundToInt(thing.X + sight);
			float yMax = Mathf.RoundToInt(thing.Y + sight);
			for(float i = xMin; i < xMax;i++)for(float j = yMin;j < yMax; j++)
				{
					if (!(hprIsWithinRange(i, 0, width) && hprIsWithinRange(j, 0, height)))
					{
						//Wrong coordiante
						continue;
					}
					//Vector2 sightedPosition = new Vector2(i, j);
					//bool canThingSeeThisPosition = TestLOS(thing, sightedPosition);
					things.AddRange(GetThingsAt((int)i, (int)j));
				}
			return things;
		}

		public float	GetThingSpeed(Thing thing)
		{
			if (thing.Category == ThingCategory.RABBIT)
			{
				return 7f;
			}
			if (thing.Category == ThingCategory.BEAR)
			{
				return 5f;
			}
			if (thing.Category == ThingCategory.HUMAN)
			{
				return 4.0f;
			}
			return 3;
		}
		
		public bool TestLOS(Thing thing, Thing otherThing)
		{
			return TestLOS(thing, otherThing.XY);
		}

		public bool TestLOS(Thing thing, Vector2 positon)
		{
			if (hprIsSameVec2(thing.XY, positon)) return true;

			var dir =( thing.XY - positon).normalized;
			float longerDirAxis = Mathf.Max(dir.x, dir.y);
			dir *= 1.0f / longerDirAxis; //make dir increase at least one when added
			var testingPosition = positon;
			while(hprIsSameVec2(thing.XY, testingPosition))
			{
				var piece = terrain.GetPieceAt(Mathf.RoundToInt(testingPosition.x), Mathf.RoundToInt(testingPosition.y));
				if (!piece.IsSightable)
				{
					return false;
				}
				testingPosition += dir;

			}
			return true;


		}


		public bool IsWalkableAt(int x_INT, int y_INT)
		{
			return terrain.IsEmptyAt(x_INT, y_INT) && !this.IsFrameAt(x_INT, y_INT) && !pathFinder.isCellOccupied(x_INT, y_INT);
		}


		public virtual void Update( float timeElapsed)
		{
			time = time.AddSeconds(timeElapsed*100);

			zoneOrganizer.Update(this, timeElapsed);
			for (int i = 0; i < allThings.Count; i++)
			{
				allThings[i].Update(this, timeElapsed);
			}
			/*
			for (int i = 0; i < thingsToKeepTracking.Count; i++)
			{
				var t = thingsToKeepTracking[i];
				if (t.IsPositionUpdated())
				{
					things[t.xOld + t.yOld * width].Remove(t.thing);
					things[t.x + t.y * width].Add(t.thing);
				}
			}
			 * */
			for (int i = 0; i< teams.Count; i++)
			{
				teams[i].Update(this, timeElapsed);
			}
		}

	}
}