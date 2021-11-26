using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class PuzzleManager : DestoryableSingleton<PuzzleManager>,Manager
{
    private GameManager gameManager;
    [Header("TileMap Components")]
    public GameObject tempTileSet;
    private Tile[,] tempTileMap;
    public GameObject tileSet;
    private Tile[,] tileMap;
    [Header("Block Components")]
    public GameObject blockPrefab;
    public List<GameObject> blocks;
    public Queue<GameObject> disableBlocks;
    


    private void Awake()
    {
        Initialize();
    }
    
    private void OnEnable()
    {
        
        SetupTileMap();
        SetupTempTileMap();
        CreateBlocks();
        DeployBlocks();
    }

    private void OnDisable()
    {
        DestroyBlocks();
        
    }

    public void Initialize()
    {
        gameManager = GameManager.Instance;
        tileMap = new Tile[Constants.TILESIZE, Constants.TILESIZE];
        tempTileMap = new Tile[Constants.TILESIZE, Constants.TILESIZE];
        tempTileSet = GameObject.Find("@TileSets Temp");
        tileSet = GameObject.Find("@TileSets");
        disableBlocks = new Queue<GameObject>();
        Debug.Log("Done: TileMap Init");
    }

    #region TilesComponents
    public void SetupTileMap()
    {
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                GameObject tile = tileSet.transform.GetChild(i).GetChild(j).gameObject;
                tileMap[i, j] = new Tile(tile, i, j);
            }
        }
    }
    
    private void SetupTempTileMap()
    {
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                GameObject tempTile = tempTileSet.transform.GetChild(i).GetChild(j).gameObject;
                tempTileMap[i, j] = new Tile(tempTile, i, j);
            }
        }
    }
    #endregion
    
    #region BlockComponents

    public void CreateBlocks()
    {
        for (int i = 0; i < Constants.CREATEBLOCKCOUNT; i++)
        {
            GameObject block = Instantiate(blockPrefab);
            block.SetActive(false);
            blocks.Add(block);
            disableBlocks.Enqueue(block);
        }
    }

    public void DeployBlocks()
    {
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                tileMap[i, j].SetBlock(disableBlocks.Dequeue());
                tileMap[i, j].block.ChangeRandomType();
            }
        }
    }

    public void DestroyBlocks()
    {
        foreach (GameObject block in blocks)
        {
            Destroy(block);
        }
        blocks.Clear();
        disableBlocks.Clear();
    }

    public List<Block> PuzzleMatchingBFS(Block targetBlock)
    {
        List<Block> matches = new List<Block>();
        matches.Add(targetBlock);
        Queue<Block> queue = new Queue<Block>();
        queue.Enqueue(targetBlock);
        bool[,] visit = new bool[Constants.TILESIZE, Constants.TILESIZE];
        
        while (queue.Count > 0)
        {
            Block block = queue.Dequeue();
            if (visit[block.Y, block.X]) continue;
            visit[block.Y, block.X] = true;
            if (block.X + 1 < 7 && tileMap[block.Y, block.X + 1].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[block.Y, block.X + 1].block);
                matches.Add(tileMap[block.Y, block.X + 1].block);
            }

            if (block.X - 1 >= 0 && tileMap[block.Y, block.X - 1].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[block.Y, block.X - 1].block);
                matches.Add(tileMap[block.Y, block.X - 1].block);
            }

            if (block.Y + 1 < 7 && tileMap[block.Y + 1, block.X].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[block.Y + 1, block.X].block);
                matches.Add(tileMap[block.Y + 1, block.X].block);
            }

            if (block.Y - 1 >= 0 && tileMap[block.Y - 1, block.X].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[block.Y - 1, block.X].block);
                matches.Add(tileMap[block.Y - 1, block.X].block);
            }
        }
        return matches;
    }

    #endregion
    
}
