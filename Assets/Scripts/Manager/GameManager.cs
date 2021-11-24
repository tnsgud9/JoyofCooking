using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


public class GameManager : DestoryableSingleton<GameManager>
{
    public TilesController tilesController;
    public BlocksController blocksController;
    private void Awake()
    {
    }

}

