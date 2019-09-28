using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MazeCfg
{
	public int SizeX;
	public int SizeY;

	public int RoomSize;
	public int RoomExtraSize;
	public int RoomGenTries;

	// 每个房间的门数量
	public int RoomDoorCnt;

	public Tilefg[] Tiles;
	public Itemfg[] Items;
	public MonsterCfg[] Monsters;
	public NpcCfg[] Npcs;

}
