using System;
using System.Linq;
using MonsterMaze.GameModel.Tiles;

namespace MonsterMaze.GameModel.Behaviour
{
	public static class MonsterAi
	{
		public static Func<Tile,TileSet, Func<Tile, Message>> ChasePlayer = (p,s) => c => ChaseTarget(p, c, s);

		private static Message ChaseTarget(Tile target, Tile chaser, TileSet statics)
		{
			var movementVector = chaser.Location.NormalizedVector(target.Location);

			var bestMove = new[]{	
				chaser.Location + new Point(0, movementVector.Y), //vertical
			    chaser.Location + new Point(movementVector.X, 0)} //horizontal 
					.OrderBy(m => m.Distance(target.Location)) // order on distance from player
					.FirstOrDefault(m => !statics.CollisionDetection(chaser, m).Collision); //pick first where there is no collision

			return bestMove != default(Point) 
				? chaser.BuildMoveMessage(bestMove) 
				: null;
		}
	}
}