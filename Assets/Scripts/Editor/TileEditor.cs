using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(TileController))]
public class TileEditor : Editor
{
    /*
    private TileController _tileController;
    [SerializeField] private bool[][] _tileSet;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _tileController = target as TileController;
        
        GUILayout.Label ("Tile Generator");
        GUILayout.Label ("Set 7x7 puzzle generator shape");
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < 7; i++)
        {
            EditorGUILayout.BeginVertical();
            for (int j = 0; j < 7; j++)
            {
                _tileSet[i][j] = EditorGUILayout.Toggle(_tileSet[i][j]);
                if (_tileSet[i][j])
                    _tileController.TileMapActive[i][j] = true;
                else
                    _tileController.TileMapActive[i][j] = false;

            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
    */
}