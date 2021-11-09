using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public AnimationClip clip;
    private Transform reachPos;
    private Animation _animation;

    public void Start()
    {
        _animation = GetComponent<Animation>();
        _animation.clip = clip;
        _animation.Play();
    }

    public void Cut()
    {
        
    }
}
