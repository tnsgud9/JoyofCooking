using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class PuzzleManager : DestoryableSingleton<PuzzleManager>,Manager
{
    
    //TOOD : Need to REFACTORING
    private GameManager gameManager;
    [Header("GameManage Components")]
    private AudioSource popAudio;
    public AudioClip popCorrectAudio;
    public AudioClip popNormalAudio;
    public AudioSource backgroundAudio;
    public AudioClip normalAudio;
    public AudioClip hurryAudio;
    public AudioClip PlayOverAudio;
    public PuzzleTimer timer;
    public PuzzleScore puzzleScore;
    public VideoPlayer charMotion;
    public VideoClip charNormal;
    public VideoClip charHurry;
    public Text scoreText;
    // Intro
    public GameObject introReady;
    // GameOverPopUp
    public GameObject gameOverPopup;
    // Shuffle
    public GameObject ShuffleObj;
    //Bonus section
    public Text bonusCountText;
    public Block BonusBlock;
    public int BonusRemainCount=0;
    private int BonusCount;
    public AudioSource bonusAudio;
    
    
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
        StartCoroutine(PlayStart());
    }

    private void OnDisable()
    {
        DestroyBlocks();
        BonusBlock.ChangeRandomType();
    }

    public void Initialize()
    {
        gameOverPopup.SetActive(false);
        charMotion.clip = charNormal;
        scoreText.text = $"Score : {puzzleScore.score}";
        BonusCount = Constants.STARTMATCHCOUNT;
        ShuffleObj.SetActive(false);
        backgroundAudio.clip = normalAudio;
        popAudio = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        tileMap = new Tile[Constants.TILESIZE, Constants.TILESIZE];
        tileSet = GameObject.Find("@TileSets");
        BonusBlock = GameObject.Find("@BonusBlock").GetComponent<Block>();
        disableBlocks = new Queue<GameObject>();
        blockStack = new Stack<GameObject>[Constants.TILESIZE];
        for (int i = 0; i < Constants.TILESIZE; i++)
            blockStack[i] = new Stack<GameObject>();
        Debug.Log("Done: TileMap Init");
    }
    #endregion
    #region GameManager Functions
    public IEnumerator PlayStart()
    {
        gameOverPopup.SetActive(false);
        puzzleScore.score = 0;
        introReady.SetActive(true);
        charMotion.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(introReady.GetComponent<AudioSource>().clip.length);
        introReady.SetActive(false);
        DeployBlocks();
        timer.TimeStart();
        backgroundAudio.Play();
        SetBonus();
    }

    public void PlayRestart()
    {
        StartCoroutine(PlayStart());
    }

    public void PlayPause()
    {
        CameraController.Instance.touchAvailable = false;
        Time.timeScale = 0f;
        backgroundAudio.Pause();
        //timer.TimePause();
    }

    public void PlayResume()
    {
        CameraController.Instance.touchAvailable = true;
        Time.timeScale = 1f;
        backgroundAudio.Play();
    }

    public void HurryTime()
    {
        //TODO: refactoring
        charMotion.clip = charHurry;
        backgroundAudio.clip = hurryAudio;
        backgroundAudio.Play();
    }

    public void SetBonus()
    {
        BonusBlock.ChangeRandomType();
        BonusCount = Constants.STARTMATCHCOUNT;
        BonusRemainCount = BonusCount;
        //BonusRemainCount = BonusCount.Dequeue();
        bonusCountText.text = BonusRemainCount.ToString();
    }
    
    

    
    public void GameOver()
    {
        Debug.Log("======== Game Over ========");
        backgroundAudio.clip = PlayOverAudio;
        backgroundAudio.Play();
        backgroundAudio.loop = false;
        charMotion.Stop();
        charMotion.gameObject.GetComponent<SpriteRenderer>().color = Constants.BACKGROUNDCOLOR;
        CameraController.Instance.enabled = false;
        gameOverPopup.SetActive(true);
        
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

    public IEnumerator BlockShuffle()
    {
        ShuffleObj.SetActive(true);
        timer.TimePause();
        CameraController.Instance.touchAvailable = false;
        //Animation Event;
        yield return new WaitForSeconds(ShuffleObj.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        ShuffleObj.SetActive(false);
        do 
        {
            for (int i = 0; i < Constants.SHUFFLECOUNT; i++)
            {
                tileMap[Random.Range(0, Constants.TILESIZE), Random.Range(0, Constants.TILESIZE)].block.ChangeRandomType();
                tileMap[Random.Range(0, Constants.TILESIZE), Random.Range(0, Constants.TILESIZE)].block.PlayParticle();
            }
        } while (BlockRelationCheck() < Constants.MATCHCOUNT);
        CameraController.Instance.touchAvailable = true; //TODO: Need to Camera Control func refactoring
        timer.TimeStart();
    }
    #endregion
    #region Camera Events
    public void SelectBlock(GameObject target)
    {
        popAudio.clip = popNormalAudio;
        List<Tile> matchTiles = PuzzleMatchingBFS(target.GetComponent<Block>());
        if (matchTiles.Count >= Constants.MATCHCOUNT)
        {
            //add score
            puzzleScore.AddMatchCount(matchTiles.Count);
            scoreText.text = $"Score : {puzzleScore.score}";
            
            foreach (Tile tile in matchTiles)
            {
                tile.block.Cut();
                disableBlocks.Enqueue(tile.block.gameObject);
                tile.ClearBlock();
            }
            if (target.GetComponent<Block>().type == BonusBlock.type)
            {
                popAudio.clip = popCorrectAudio;
                //TODO: Need to move SCORE FUNC Refactoring 
                BonusRemainCount -= matchTiles.Count;
                bonusCountText.text = BonusRemainCount.ToString();
                
                //Bouns Count Check
                if (BonusRemainCount <= 0)
                {
                    // 안좋은 코드 인듯??
                    BonusCount += Constants.INCREASEMATCHCOUNT;
                    BonusRemainCount = BonusCount;
                    bonusCountText.text = BonusRemainCount.ToString();
                    BonusBlock.ChangeRandomType();
                    BonusBlock.PlayParticle();
                    bonusAudio.Play();
                    timer.TimeAdd(Constants.BONUSINCREASETIME);
                    
                }
            }
            popAudio.Play();
        }
        //callback
        BlockRelocation();
        if (BlockRelationCheck() <= 2)
        {
            Debug.Log("Puzzle Shake!!!");
            StartCoroutine(BlockShuffle());
        }

    }
    #endregion
}
