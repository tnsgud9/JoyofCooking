using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Camera _camera;
    public bool touchAvailable = true;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        ScreenTouchDown();
    }
    
    // debug PC Input
    void ScreenTouchDown()
    {
        if (Input.GetMouseButtonDown(0) && touchAvailable)
        {
            RaycastHit2D hit =
                Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), transform.forward, 100f);
            if (  hit.collider.gameObject.CompareTag("Block"))
            {
                Debug.Log("is Block hit");
                hit.collider.gameObject.GetComponent<Block>().SelectBlock();
            }
            
        }
    }
}
