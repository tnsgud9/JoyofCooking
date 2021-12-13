using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PuzzleScore : MonoBehaviour
{
    public int score  = 0;

    public void Add(int score)
    {
        this.score += score;
    }
    public void AddMatchCount(int count) => this.score += Constants.BASICSCORE * count;
}
