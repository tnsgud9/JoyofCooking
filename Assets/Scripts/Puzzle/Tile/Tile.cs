using UnityEngine;

public class Tile
{
    
    public Block block = null;
    private Transform transform;
    public int X, Y;

    public Tile(Transform tile, int y,int x)
    {
        Y = y;
        X = x;
        transform = tile;
    }
    public Tile(Transform tile, Block block, int y,int x)
    {
        Y = y;
        X = x;
        transform = tile;
        this.block = block;
    }

    public void SetBlock(GameObject block)
    {
        this.block = block.GetComponent<Block>();
        this.block.Set(this,transform,true);
    }

    public void SetBlock(Block block)
    {
        this.block = block;
        block.Set(this,transform,true);
    }
}