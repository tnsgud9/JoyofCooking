using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileController : MonoBehaviour
{
    public GameObject block;
    public GameObject TempTilemaps;

    public void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                // 나중에 추가로 타일을 변경하는 경우가 생길 떄 이부분에 해당 이벤트에 관련된 코드를 적으면 된다.
                GameManager.Instance.tilemaps[i, j].SetActive(true);
                GameObject t = Instantiate(block,GameManager.Instance.tilemaps[i, j].transform.position, GameManager.Instance.tilemaps[i, j].transform.rotation);
                t.GetComponent<Block>().Init((BlockType)Random.Range(1,5),i,j);
                GameManager.Instance.blockmaps[i, j] = t.GetComponent<Block>();
            }
        }
    }

    public void BlockRelocation(Block[,] blockmaps)
    {
        
        Stack<Block>[] stacks = new Stack<Block>[7];
        for (int i = 0; i < 7; i++)
        {
            stacks[i] = new Stack<Block>();
            for (int j = 0; j < 7; j++)
            {
                if(!blockmaps[i,j].isDestroy)
                    stacks[i].Push(blockmaps[i,j]);
            }

            int tempCnt = 7;
            while (stacks[i].Count < 7)
            {
                GameObject t = GameManager.Instance.disableBlocks.Dequeue();
                // TODO: Stack Control 구현 필요
                stacks[i].Push(GameManager.Instance.disableBlocks.Dequeue().GetComponent<Block>());
            }
        }

        Debug.Log("ASd");
    }
    
    
}