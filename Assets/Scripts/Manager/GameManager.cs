using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


public class GameManager : DestoryableSingleton<GameManager>
{
    public TilesManager tilesManager;
    public BlocksManager blocksController;
    private void Awake()
    {
    }

}

