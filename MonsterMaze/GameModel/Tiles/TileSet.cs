using System.Collections.Generic;
using System.Linq;
using System;

namespace MonsterMaze.GameModel.Tiles
{
	public class TileSet
	{
		private List<Tile> _tiles;
		public List<Tile> Tiles
		{
			get { return _tiles; }
			private set { _tiles = value; }
		}

		private Dictionary<Point, int> _index;
		private List<Func<Tile, Tile, bool>> _collisionRules = new List<Func<Tile, Tile, bool>>();
		public TileSet(IEnumerable<Tile> tiles)
		{
			Tiles = tiles.ToList();
			_index = Index();
		}

		public Tile this[int x, int y]
		{
			get{
				var point = new Point(x, y);
				if (_index.ContainsKey(point) && Tiles[_index[point]].IsActive)
					return Tiles[_index[point]];
				return null;
			}
		}
		public Tile this[int x, int y, int z]
		{
			get
			{
				var point = new Point(x, y, z);
				if (_index.ContainsKey(point) && Tiles[_index[point]].IsActive)
					return Tiles[_index[point]];
				return null;
			}
		}

		public IEnumerable<Tile> GetStack(int x, int y)
		{
			return _index.Where(i => i.Key.X == x && i.Key.Y == y).Select(i=> Tiles[i.Value]);
		}

		public Tile this[Point p]
		{
			get {
				if (_index.ContainsKey(p) && Tiles[_index[p]].IsActive)
					return Tiles[_index[p]];
				return null;
			}
		} 

		public void Update()
		{
			_index = Index();
		}

		private Dictionary<Point, int> Index()
		{			
			var index = new Dictionary<Point, int>();
			for (var i = 0; i < Tiles.Count(); i++)
				if (Tiles[i].IsActive)
					index.Add(Tiles[i].Location, i);

			return index;
		}

		public void AddCollisionRules(params Func<Tile, Tile, bool>[] rules)
		{
			_collisionRules.AddRange(rules);
		}

		public CollisionInfo CollisionDetection(Tile mobile, Point destination)
		{
			var target = this[destination];
			var collisionInfo = new CollisionInfo{
				Tile = mobile,
				Vector = mobile.Location.NormalizedVector(destination)};
			
			if (target != null)	
			{
				collisionInfo.Collision = _collisionRules.All(r => r(mobile, target));
				collisionInfo.Object = target;
			}
			
			
			if (collisionInfo.Tile != null && collisionInfo.Object != null && collisionInfo.Object != collisionInfo.Tile)
			{
				if (collisionInfo.Tile.OnCollision != null)
					collisionInfo.Messages.AddRange(collisionInfo.Tile.GetCollisionResponse(collisionInfo));

				if (collisionInfo.Object.OnCollision != null)
					collisionInfo.Messages.AddRange(collisionInfo.Object.GetCollisionResponse(collisionInfo));
			}
			return collisionInfo;            
		}
	}
}