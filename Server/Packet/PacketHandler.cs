﻿using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		C_Move movePacket = (C_Move)packet;
		ClientSession clientSession = (ClientSession)session;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.Push(room.HandleMove, player, movePacket);
	}

    public static void C_SyncHandler(PacketSession session, IMessage packet)
    {
		C_Sync syncPacket = (C_Sync)packet;
		ClientSession clientSession = (ClientSession)session;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.Push(room.HandleSync, player, syncPacket);
    }

    public static void C_LoginHandler(PacketSession session, IMessage packet)
    {
		C_Login loginPacket = (C_Login)packet;
		ClientSession clientSession = (ClientSession)session;

		GameRoom room = RoomManager.Instance.Find(1);

		Player myPlayer = ObjectManager.Instance.Add<Player>();
		{
			clientSession.MyPlayer = myPlayer;
			myPlayer.Room = room;
			myPlayer.UserInfo = loginPacket.UserInfo;
			myPlayer.MoveInfo = new MoveInfo();
			myPlayer.MoveInfo.State = CreatureState.Idle;
			myPlayer.MoveInfo.DirX = 0;
			myPlayer.MoveInfo.DirZ = 0;
			myPlayer.MoveInfo.InputBit = 0;
			myPlayer.PosInfo.PosX = 0;
			myPlayer.PosInfo.PosY = 0;
			myPlayer.PosInfo.PosX = 0;
			myPlayer.Session = clientSession;
		}
		
		room.Push(room.EnterGame, myPlayer);
	}
}
