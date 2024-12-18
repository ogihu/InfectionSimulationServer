﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
	public class RoomManager
	{
		public static RoomManager Instance { get; } = new RoomManager();

		object _lock = new object();
		Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
		int _roomId = 1;

		public GameRoom Add()
		{
			GameRoom gameRoom = new GameRoom();

			lock (_lock)
			{
				gameRoom.RoomId = _roomId;
				_rooms.Add(_roomId, gameRoom);
				_roomId++;
                Program.TickRoom(gameRoom, 50);
            }

			return gameRoom;
		}

		public bool Remove(int roomId)
		{
			lock (_lock)
			{
				Program.StopTickRoom(_rooms[roomId]);
				return _rooms.Remove(roomId);
			}
		}

		public GameRoom Find(int roomId)
		{
			lock (_lock)
			{
				GameRoom room = null;
				if (_rooms.TryGetValue(roomId, out room))
					return room;

				return null;
			}
		}
	}
}
