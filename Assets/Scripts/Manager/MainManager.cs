using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.Serialization;

public class MainManager : Singleton<MainManager>, Manager
{
    [Header("Resources Setting")]
    public SpriteLibraryAsset blockBundle;
    
    [Header("Components Settings")]
    public CameraController CameraController;

    private void Awake()
    {
        blockBundle = Resources.Load<SpriteLibraryAsset>("BlockAssets");
    }
}
