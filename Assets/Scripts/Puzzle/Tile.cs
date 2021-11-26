﻿using UnityEngine;

public class Tile
{
    
    public Block block = null;
    private Transform transform;
    public int X, Y;

    public Tile(GameObject tile, int y,int x)
    {
        Y = y;
        X = x;
        transform = tile.transform;
    }

    public void SetBlock(GameObject block)
    {
        this.block = block.GetComponent<Block>();
        block.transform.position = transform.position;
        block.SetActive(true);
    }
}