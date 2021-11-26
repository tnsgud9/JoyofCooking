using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.Serialization;

public class SuperManager : Singleton<SuperManager>, Manager
{
    [Header("Resources Setting")]
    public SpriteLibraryAsset blockBundle;

    [Header("Components Settings")]
    public CameraController cameraController;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        blockBundle = Resources.Load<SpriteLibraryAsset>("BlockAssets");
    }
}
