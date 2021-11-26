using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class BlocksManager : DestoryableSingleton<BlocksManager>, Manager
{
    public TilesManager tilesManager;
    public GameObject blockPrefab;
    public List<GameObject> blockList; //생성된 모든 블록 관리.
    private Queue<Block> disableBlocks;
    private Block[,] blockMap;
    //private Stack<Block>[] disableBlocks;
    

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        BlockGenerate();
        BlockDeploy();
    }

    private void OnDisable()
    {
        foreach (GameObject block in blockList)
        {
            Destroy(block);
        }
    }

    public void Initialize()
    {
        blockMap = new Block[Constants.TILESIZE, Constants.TILESIZE];
        tilesManager = TilesManager.Instance;;
        disableBlocks = new Queue<Block>();
    }

    public void BlockGenerate()
    {
        for (int i = 0; i < Constants.CREATEBLOCKCOUNT; i++)
        {
            GameObject b = Instantiate(blockPrefab);
            b.SetActive(false);
            blockList.Add(b);
            disableBlocks.Enqueue(b.GetComponent<Block>());
        }
    }

    public void BlockDeploy()
    {
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                if (!blockMap[i,j])
                {
                    Block block = disableBlocks.Dequeue();
                    blockMap[i, j] = block;
                    block.Y = i;
                    block.X = j;
                    block.transform.position = tilesManager.TileMap[i, j].position;
                    block.gameObject.SetActive(true);
                }
            }
        }
    }
    
    public void BlockGenerateed()
    {
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                GameObject block = Instantiate(
                    blockPrefab, 
                    tilesManager.TileMap[i, j].transform.position,
                    tilesManager.TileMap[i, j].transform.rotation
                    );
                //Blocks[i, j] = block.GetComponent<Block>();
            }
        }
    }

    public void SelectBlock(Block block)
    {
        List<Block> blockMatches = PuzzleMatchingBFS(block);
        if (blockMatches.Count >= Constants.MATCHCOUNT)
        {
            foreach (Block e in blockMatches)
            {
                //e.Cut();
            }
        }
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
            if (block.X + 1 < 7 && blockMap[block.Y, block.X + 1].type == targetBlock.type)
            {
                queue.Enqueue(blockMap[block.Y, block.X + 1]);
                matches.Add(blockMap[block.Y, block.X + 1]);
            }

            if (block.X - 1 >= 0 && blockMap[block.Y, block.X - 1].type == targetBlock.type)
            {
                queue.Enqueue(blockMap[block.Y, block.X - 1]);
                matches.Add(blockMap[block.Y, block.X - 1]);
            }

            if (block.Y + 1 < 7 && blockMap[block.Y + 1, block.X].type == targetBlock.type)
            {
                queue.Enqueue(blockMap[block.Y + 1, block.X]);
                matches.Add(blockMap[block.Y + 1, block.X]);
            }

            if (block.Y - 1 >= 0 && blockMap[block.Y - 1, block.X].type == targetBlock.type)
            {
                queue.Enqueue(blockMap[block.Y - 1, block.X]);
                matches.Add(blockMap[block.Y - 1, block.X]);
            }
        }
        return matches;
    }

    
}