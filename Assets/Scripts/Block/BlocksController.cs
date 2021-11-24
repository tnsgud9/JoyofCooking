using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class BlocksController : DestoryableSingleton<BlocksController>
{
    public List<GameObject> blockList;
    public GameObject[,] Blocks;
    public Stack<Block>[] DisableBlocks;

    private void Awake()
    {
        Blocks = new GameObject[Constants.TILESIZE, Constants.TILESIZE];
        DisableBlocks = new Stack<Block>[Constants.TILESIZE];
    }

    private void Start()
    {
        BlockGenerate();
    }

    public void BlockGenerate()
    {
        
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                TileMap[i, j] = tilemap.transform.GetChild(i).GetChild(j).gameObject;
            }
        }
    }
}
