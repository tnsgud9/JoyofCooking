using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class PuzzleManager : DestoryableSingleton<PuzzleManager>,Manager
{
    private GameManager gameManager;
    [Header("GameManage Components")]
    private AudioSource popAudio;
    public AudioSource backgroundAudio;
    public Timer timer;
    
    [Header("TileMap Components")]
    public GameObject tileSet;
    private Tile[,] tileMap;
    [Header("Block Components")]
    public GameObject blockPrefab;
    public List<GameObject> blocks;
    public Stack<GameObject>[] blockStack;
    public Queue<GameObject> disableBlocks;

    #region Unity Functions
    private void Awake()
    {
        Initialize();
    }
    
    private void OnEnable()
    {
        
        SetupTileMap();
        CreateBlocks();
        //DeployBlocks();
    }

    private void Start()
    {
        PlayStart();
    }

    private void OnDisable()
    {
        DestroyBlocks();
        
    }

    public void Initialize()
    {
        popAudio = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        tileMap = new Tile[Constants.TILESIZE, Constants.TILESIZE];
        tileSet = GameObject.Find("@TileSets");
        disableBlocks = new Queue<GameObject>();
        blockStack = new Stack<GameObject>[Constants.TILESIZE];
        for (int i = 0; i < Constants.TILESIZE; i++)
            blockStack[i] = new Stack<GameObject>();
        Debug.Log("Done: TileMap Init");
    }
    
    #endregion

    #region GameManager Functions
    public void PlayStart()
    {
        DeployBlocks();
        timer.TimeStart();
        backgroundAudio.Play();
    }

    public void PlayPause()
    {
        Time.timeScale = 0f;
        //timer.TimePause();
        //backgroundAudio.Pause();
    }

    public void PlayResume()
    {
        Time.timeScale = 1f;
        //backgroundAudio.Play();
    }

    public void GameOver()
    {
        Debug.Log("======== Game Over ========");
        CameraController.Instance.enabled = false;
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
                tileMap[i, j].ChangeRandomBlock();
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
                            tileMap[i, j].MoveBlock(tileMap[k, j].block);
                            tileMap[k, j].ClearBlock();
                            break;
                        }
                    }
                    
                }

                if (!tileMap[i, j].block)
                {
                    tileMap[i, j].SetBlock(disableBlocks.Dequeue());
                    tileMap[i, j].ChangeRandomBlock();
                }
            }
        }
    }

    public int BlockRelationCheck()
    {
        int maxRelation = Int32.MinValue;
        for (int i = 0; i < Constants.TILESIZE; i++)
        {
            for (int j = 0; j < Constants.TILESIZE; j++)
            {
                int relation = 1;
                if (j - 1 >= 0 && tileMap[i, j].block.type == tileMap[i, j - 1].block.type)
                    relation++;
                if (j + 1 < Constants.TILESIZE && tileMap[i, j].block.type == tileMap[i, j + 1].block.type)
                    relation++;
                if (i - 1 >= 0 && tileMap[i, j].block.type == tileMap[i - 1, j].block.type)
                    relation++;
                if (i + 1 < Constants.TILESIZE && tileMap[i, j].block.type == tileMap[i + 1, j].block.type)
                    relation++;
                maxRelation = maxRelation > relation ? maxRelation : relation;
            }
        }
        return maxRelation;
    }
    #endregion

    #region Camera Events
    public void SelectBlock(GameObject target)
    {
        List<Tile> matchTiles = PuzzleMatchingBFS(target.GetComponent<Block>());
        if (matchTiles.Count >= Constants.MATCHCOUNT)
        {
            popAudio.Play();
            foreach (Tile tile in matchTiles)
            {
                
                tile.block.Cut();
                disableBlocks.Enqueue(tile.block.gameObject);
                tile.ClearBlock();
            }
            //재배치 관련 함수
            // Block 쪽에서 이동하는 함수를 구현.
        }
        //callback
        BlockRelocation();
        if (BlockRelationCheck() <= 2)
        {
            Debug.Log("Puzzle Shake!!!");
            // Time Pause;
            //Shake Event;
        }

    }
    #endregion
    
}
