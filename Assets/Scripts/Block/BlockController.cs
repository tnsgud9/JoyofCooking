using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    none,lettuce,pimento,tofu,carrot
}

public class BlockController : MonoBehaviour
{
    public BlockType type = BlockType.none;
    
    private Transform reachPos;
    private Animator _anim;
    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;
    private GameObject fullSprite;
    private static readonly int destroyAnim = Animator.StringToHash("destroy");

    public void Start()
    {
        leftSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rightSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        fullSprite = transform.GetChild(2).GetComponent<GameObject>();
        

        reachPos = GameManager.Instance.RemainIngredientPos;
        _anim = GetComponent<Animator>();
        
        leftSprite.sprite = MainManager.Instance.blockBundle.GetSprite(type.ToString(), "left");
        rightSprite.sprite = MainManager.Instance.blockBundle.GetSprite(type.ToString(), "right");
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

    public void SelectBlock()
    {
        // BFS Searching Function
        // if Matching Rule true
        // call func Cut()
        if (true)
        {
            Cut();
        }
    }
    public void Cut()
    {
        _anim.SetTrigger(destroyAnim);
        // if anim finish -> active ture full sprites

    }
    public void AnimEndCallback()
    {
        //배열에서 내용 제거 관련 func
        Destroy(this.gameObject); // or object pooling
        
    }
}
