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
    public Tile tile = null;
    public BlockType type = BlockType.none;
    private Animator _anim;
    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;
    private GameObject fullSprite;
    private static readonly int destroyAnim = Animator.StringToHash("destroy");
    
    private void Awake()
    {
        leftSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rightSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        fullSprite = transform.GetChild(2).GetComponent<GameObject>();
        _anim = GetComponent<Animator>();
    }
    public void Set(Tile tile, Transform trans, bool active)
    {
        this.tile = tile;
        transform.position = trans.position;
        this.gameObject.SetActive(active);
    }
    public void ChangeRandomType()
    {
        type = (BlockType)Random.Range(1, 5); // TODO: Fixed 5 value
        leftSprite.sprite = SuperManager.Instance.blockBundle.GetSprite(type.ToString(), "left");
        rightSprite.sprite = SuperManager.Instance.blockBundle.GetSprite(type.ToString(), "right");
    }

    public void Cut()
    {
        _anim.SetTrigger(destroyAnim);
        tile = null;
    }

    public void AnimEndCallback()
    {
        this.gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        tile = null;
    }
} 