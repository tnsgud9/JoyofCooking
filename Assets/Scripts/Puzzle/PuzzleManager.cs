using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class PuzzleManager : DestoryableSingleton<PuzzleManager>,Manager
{
    private GameManager gameManager;
    private AudioSource audio;
    [Header("TileMap Components")]
    public GameObject tempTileSet;
    private Tile[,] tempTileMap;
    public GameObject tileSet;
    private Tile[,] tileMap;
    [Header("Block Components")]
    public GameObject blockPrefab;
    public List<GameObject> blocks;
    public Stack<GameObject>[] blockStack;
    public Queue<GameObject> disableBlocks;

    #region Unity Functions

    
    private void Update()
    {
        Debug.Log(disableBlocks.Count);
    }

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
        audio = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        tileMap = new Tile[Constants.TILESIZE, Constants.TILESIZE];
        tempTileMap = new Tile[Constants.TILESIZE, Constants.TILESIZE];
        tempTileSet = GameObject.Find("@TileSets Temp");
        tileSet = GameObject.Find("@TileSets");
        disableBlocks = new Queue<GameObject>();
        blockStack = new Stack<GameObject>[Constants.TILESIZE];
        for (int i = 0; i < Constants.TILESIZE; i++)
            blockStack[i] = new Stack<GameObject>();
        Debug.Log("Done: TileMap Init");
    }
    
    #endregion
    
    #region TilesComponents
    public void SetupTileMap()
    {
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                Transform tile = tileSet.transform.GetChild(i).GetChild(j);
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
                Transform tempTile = tempTileSet.transform.GetChild(i).GetChild(j);
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

    public List<Tile> PuzzleMatchingBFS(Block targetBlock)
    {
        List<Tile> matches = new List<Tile>();
        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(targetBlock.tile);
        bool[,] visit = new bool[Constants.TILESIZE, Constants.TILESIZE];

        while (queue.Count > 0)
        {
            Tile tile = queue.Dequeue();
            if(visit[tile.Y,tile.X]) continue;
            visit[tile.Y, tile.X] = true;
            matches.Add(tileMap[tile.Y, tile.X]);

            if (tile.X + 1 < Constants.TILESIZE 
                && tileMap[tile.Y, tile.X + 1].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[tile.Y, tile.X + 1]);
            }
            if (tile.X - 1 >= 0 
                && tileMap[tile.Y, tile.X - 1].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[tile.Y, tile.X - 1]);
            }
            if (tile.Y + 1 < Constants.TILESIZE 
                && tileMap[tile.Y + 1, tile.X].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[tile.Y + 1, tile.X]);
            }
            if (tile.Y - 1 >= 0 
                && tileMap[tile.Y - 1, tile.X].block.type == targetBlock.type)
            {
                queue.Enqueue(tileMap[tile.Y - 1, tile.X]);
            }
        }
        return matches;
    }
    public void BlockRelocation()
    {
        for (int i = Constants.TILESIZE-1; i >= 0 ; i--)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                if (!tileMap[i, j].block)
                {
                    for (int k = i; k >= 0; k--)
                    {
                        if (tileMap[k, j].block)
                        {
                            tileMap[i, j].SetBlock(tileMap[k, j].block);
                            tileMap[k, j].block = null;
                            break;
                        }
                    }
                    
                }

                if (!tileMap[i, j].block)
                {
                    tileMap[i, j].SetBlock(disableBlocks.Dequeue());
                    tileMap[i, j].block.ChangeRandomType();
                    //TODO : 이전에 사용된 게임 오브젝트 풀링으로 사용시 좌표가 초기화 되지 않는 오류 FIX 필요
                }
            }
        }
    }
    #endregion

    #region Camera Events

    public void SelectBlock(GameObject target)
    {
        List<Tile> matchTiles = PuzzleMatchingBFS(target.GetComponent<Block>());
        if (matchTiles.Count >= Constants.MATCHCOUNT)
        {
            audio.Play();
            foreach (Tile tile in matchTiles)
            {
                tile.block.Cut();
                disableBlocks.Enqueue(tile.block.gameObject);
                tile.block = null;
            }
            //재배치 관련 함수
            // Block 쪽에서 이동하는 함수를 구현.
        }
        //callback
        BlockRelocation();
        
    }
    #endregion
    
}
