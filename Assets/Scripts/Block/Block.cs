using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BlockType
{
    none,lettuce,pimento,tofu,carrot
}

public class Block : MonoBehaviour
{
    public int X, Y;
    public bool isDestroy = false;
    public BlockType type = BlockType.none;
    private Transform reachPos;
    private Animator _anim;
    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;
    private GameObject fullSprite;
    private AudioSource _audioSource;
    private static readonly int destroyAnim = Animator.StringToHash("destroy");

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        leftSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rightSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        fullSprite = transform.GetChild(2).GetComponent<GameObject>();

        reachPos = GameManager.Instance.remainIngredientPos;
        _anim = GetComponent<Animator>();
        
        leftSprite.sprite = MainManager.Instance.blockBundle.GetSprite(type.ToString(), "left");
        rightSprite.sprite = MainManager.Instance.blockBundle.GetSprite(type.ToString(), "right");
        

    }


    public void Init(BlockType type, int y, int x)
    {
        
        this.type = type;
        Y = y;
        X = x;
        leftSprite.sprite = MainManager.Instance.blockBundle.GetSprite(this.type.ToString(), "left");
        rightSprite.sprite = MainManager.Instance.blockBundle.GetSprite(this.type.ToString(), "right");
        switch (type)
        {
            case BlockType.none:
                Debug.LogWarning("Undefined BlockTypes");
                Destroy(this.gameObject);
                break;
            case BlockType.lettuce:
                break;
            case BlockType.pimento:
                break;
            case BlockType.tofu:
                break;
            case BlockType.carrot:
                break;
        }
    }

    public List<Block> PuzzleMatchingBFS(int y, int x)
    {
        List<Block> matches = new List<Block>();
        matches.Add(this);
        bool[,] visit = new bool[7, 7];
        Queue<Block> queue = new Queue<Block>();
        queue.Enqueue(this);
        
        Block[,] blockmaps = GameManager.Instance.blockmaps;
        while (queue.Count > 0)
        {
            Block block = queue.Dequeue();
            if (visit[block.Y, block.X]) continue;
            visit[block.Y, block.X] = true;
            if (block.X + 1 < 7 && blockmaps[block.Y, block.X + 1].type == this.type)
            {
                queue.Enqueue(blockmaps[block.Y, block.X + 1]);
                matches.Add(blockmaps[block.Y, block.X + 1]);
            }
            if (block.X - 1 >= 0 && blockmaps[block.Y, block.X - 1].type == this.type)
            {
                queue.Enqueue(blockmaps[block.Y, block.X - 1]);
                matches.Add(blockmaps[block.Y, block.X - 1]);
            }
            if (block.Y + 1 < 7 && blockmaps[block.Y + 1, block.X].type == this.type)
            {
                queue.Enqueue(blockmaps[block.Y + 1, block.X]);
                matches.Add(blockmaps[block.Y + 1, block.X]);
            }
            if (block.Y - 1 >= 0 && blockmaps[block.Y - 1, block.X].type == this.type)
            {
                queue.Enqueue(blockmaps[block.Y - 1, block.X]);
                matches.Add(blockmaps[block.Y - 1, block.X]);
            }
        }

        if (matches.Count > 2)
        {
            return matches;
        }
        return null;
    }
    public void SelectBlock()
    {
        
        // BFS Searching Function
        // if Matching Rule true
        // call func Cut()
        List<Block> matches = PuzzleMatchingBFS(Y, X);
        if (matches != null)
        {
            foreach (Block block in matches)
            {
                block.Cut();
            }
            GameManager.Instance.Relocation();
            //Invoke("ReLocation",0.15f);
        }
    }
    private void ReLocation()
    {
        GameManager.Instance.Relocation();
    }
    public void Cut()
    {
        isDestroy = true;
        GameManager.Instance.disableBlocks.Enqueue(this.gameObject);
        _audioSource.Play();
        _anim.SetTrigger(destroyAnim);
        // if anim finish -> active ture full sprites

    }
    
    public void AnimEndCallback()
    {
        //배열에서 내용 제거 관련 func
       this.gameObject.SetActive(false); // or object pooling

    }
    
}
