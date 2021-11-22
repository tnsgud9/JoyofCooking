using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


[Serializable]
public class TilemapY
{
    public GameObject[] tileMapX;
}

public class GameManager : DestoryableSingleton<GameManager>
{
    private GameObject tilemap;
    public GameObject[,] tilemaps;
    public Queue<GameObject> disableBlocks = new Queue<GameObject>();
    public Block[,] blockmaps;
    public Transform remainIngredientPos;
    public CameraController CameraController;

    private void Awake()
    {
        TileMapInit();
    }


    void TileMapInit()
    {
        tilemaps = new GameObject[7, 7];
        blockmaps = new Block[7, 7];
        tilemap = GameObject.Find("@TileSets");
        CameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();

        if (tilemap)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    tilemaps[i, j] = tilemap.transform.GetChild(i).GetChild(j).gameObject;
                }
            }
        }
    }

    public void Relocation()
    {
        Debug.Log("Relocation Call");
        tilemap.GetComponent<TileController>().BlockRelocation(blockmaps);
    }
    
}

