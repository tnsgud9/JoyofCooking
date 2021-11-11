using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.Serialization;

public class MainManager : Singleton<MainManager>, Manager
{
    public SpriteLibraryAsset blockBundle;

    private void Awake()
    {
        blockBundle = Resources.Load<SpriteLibraryAsset>("BlockAssets");
    }
}
