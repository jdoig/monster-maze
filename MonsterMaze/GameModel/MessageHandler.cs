using System;
using System.Linq;
using MonsterMaze.GameModel.Tiles;

namespace MonsterMaze.GameModel
{
	public class MessageHandler
	{        
		private readonly TileSet _mobiles;

		public MessageHandler(TileSet mobiles)
		{
			_mobiles = mobiles;
		}

		public void Resolve(Message message)
		{
			switch (message.MessageTarget)
			{
				case MessageTarget.Mobile:
					{
						var target = _mobiles.WithName(message.Name);
							switch (message.Type)
							{
								case (GameConstants.MOVEMENT):
									if (target != null)
										target.MoveTo(message.To.ToPoint());
									break;
								case (GameConstants.ACTIVE):
									if (target != null)
										target.IsActive = Convert.ToBoolean(message.To);
									break;
								case (GameConstants.SPLAT):
									ResolveReaction(message, 'b');
									break;
								case (GameConstants.CRASH):
									ResolveReaction(message, 'h');
									break;
							}
					}
					break;

				case MessageTarget.Game:
					{
						switch (message.Type)
						{
							case(GameConstants.NEXT_LEVEL):
								//Load next level;
								break;
						}
						switch (message.Type)
						{
							case (GameConstants.LOAD_LEVEL):
								//Load level X;
								break;
						}
					}
					break;
			}
		}

		private void ResolveReaction(Message message, char reationType)
		{
			if (message.To != GameConstants.NULL)
			{
				var point = message.To.ToPoint();
				var stack = _mobiles.GetStack(point.X, point.Y);
				var offset = new Point(0, 0, stack.Count());
				var tile = new Tile { IsTraversible = true, Token = reationType, Location = message.To.ToPoint() + offset };
				_mobiles.Tiles.Add(tile);
			}
			else
			{
				var point = message.From.ToPoint();
				var stack = _mobiles.GetStack(point.X, point.Y);
				_mobiles.Tiles.Remove(stack.Last());
			}
		}
	}
}