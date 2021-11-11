using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Animator _anim;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        ScreenTouchDown();
    }
    
    // debug PC Input
    void ScreenTouchDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit =
                Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), transform.forward, 100f);
            if (hit.collider.gameObject.CompareTag("Block"))
            {
                Debug.Log("is Block hit");
                hit.collider.gameObject.GetComponent<BlockController>().SelectBlock();
            }
            
        }
    }
}
