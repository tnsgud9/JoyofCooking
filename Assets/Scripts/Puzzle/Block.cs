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
        _anim = GetComponent<Animator>();
    }
    public void ChangeRandomType()
    {
        type = (BlockType)Random.Range(1, 5); // TODO: Fixed 5 value
        leftSprite.sprite = SuperManager.Instance.blockBundle.GetSprite(type.ToString(), "left");
        rightSprite.sprite = SuperManager.Instance.blockBundle.GetSprite(type.ToString(), "right");
    }

    public void CutHandler()
    {
        _anim.SetTrigger(destroyAnim);
        X = 0;
        Y = 0;
    }

    public void AnimEndCallback()
    {
        this.gameObject.SetActive(false);
    }
    
    

}    /*



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
 */ 