using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class TilesManager : DestoryableSingleton<TilesManager>,Manager
{
    public BlocksManager blocksManager;
    
    //Block들에 대한 좌표를 관리한다.
    public Transform[,] TileMap;
    public Transform[,] TempTileMap;


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        SetupTileMap();
        SetupTempTileMap();
    }

    public void Initialize()
    {
        TileMap = new Transform[Constants.TILESIZE, Constants.TILESIZE];
        TempTileMap = new Transform[Constants.TILESIZE, Constants.TILESIZE];
        blocksManager = BlocksManager.Instance;
    }

    public void SetupTileMap()
    {
        Transform tilemap = GameObject.Find("@TileSets").transform;
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                TileMap[i, j] = tilemap.GetChild(i).GetChild(j);
            }
        }

    }
    public void SetupTempTileMap()
    {
        Transform tempTileMap = GameObject.Find("@TileSets Temp").transform;
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                
                TempTileMap[i, j] = tempTileMap.GetChild(i).GetChild(j);
            }
        }
    }
}