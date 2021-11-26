using UnityEngine;

public class BlockItem
{
    BlockItem(GameObject block, int x, int y)
    {
        X = x;
        Y = y;
        
    }
    public GameObject Block;
    public int X, Y;
}