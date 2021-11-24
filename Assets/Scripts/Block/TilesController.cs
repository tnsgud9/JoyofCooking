using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TilesController : DestoryableSingleton<TilesController>
{
    public GameObject block;
    public GameObject[,] TileMap;
    public GameObject[,] TempTileMap;


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        SetupTileMap();
        SetupTempTileMap();
    }

    private void Initialize()
    {
        TileMap = new GameObject[Constants.TILESIZE, Constants.TILESIZE];
        TempTileMap = new GameObject[Constants.TILESIZE, Constants.TILESIZE];
    }

    public void SetupTileMap()
    {
        GameObject tilemap = GameObject.Find("@TileSets");
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                TileMap[i, j] = tilemap.transform.GetChild(i).GetChild(j).gameObject;
            }
        }

    }
    public void SetupTempTileMap()
    {
        GameObject tempTileMap = GameObject.Find("@TileSets Temp");
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                TempTileMap[i, j] = tempTileMap.transform.GetChild(i).GetChild(j).gameObject;
            }
        }
    }
}