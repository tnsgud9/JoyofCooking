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
    private Collider2D collider;
    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;
    private ParticleSystem particle;
    private GameObject fullSprite;
    private static readonly int destroyAnim = Animator.StringToHash("destroy");
    
    private void Awake()
    {
        leftSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rightSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        fullSprite = transform.GetChild(2).GetComponent<GameObject>();
        particle = transform.GetChild(3).GetComponent<ParticleSystem>();
        collider = GetComponent<Collider2D>();
        _anim = GetComponent<Animator>();
    }

    public void Set(Tile tile, Transform trans)
    {
        this.tile = tile;
        transform.position = trans.position;
        this.gameObject.SetActive(true);
    }

    public void Set(Tile tile, Transform trans, bool active)
    {
        this.tile = tile;
        transform.position = trans.position;
        this.gameObject.SetActive(active);
    }

    public void Move(Tile tile, Transform trans)
    {
        this.tile = tile;
        collider.enabled = false;
        StartCoroutine(Collection.MoveToPosition(this.transform, trans.position, 0.15f,
            () => { collider.enabled = false;}));
    }
    public void Move(Transform trans)
    {
        collider.enabled = false;
        StartCoroutine(Collection.MoveToPosition(this.transform, trans.position, 0.15f,
            () => { collider.enabled = false;}));
    }

    public void ChangeRandomType()
    {
        type = (BlockType)Random.Range(1, 5); // TODO: Fixed 5 value
        leftSprite.sprite = SuperManager.Instance.blockBundle.GetSprite(type.ToString(), "left");
        rightSprite.sprite = SuperManager.Instance.blockBundle.GetSprite(type.ToString(), "right");
    }

    public void PlayParticle()
    {
        particle.Play();
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